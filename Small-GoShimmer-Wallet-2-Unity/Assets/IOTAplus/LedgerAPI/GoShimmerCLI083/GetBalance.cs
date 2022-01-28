using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IOTAplus.LedgerAPI.Wallet;


namespace IOTAplus.LedgerAPI.GoShimmerCLI083
{
	public class GetBalance
	{
		// The processParameter is the text passed to the CLI as a single string, containing one or more parameters.
		private const string PROCESS_PARAMETER = "balance";

		// Because we're parsing text output from the CLI, we need to specify some markers, to parse the text correctly.
		
		// Lines beginning with these strings are not important to the parser.
		private readonly string[] _throwAwayLines = { "-----", "FETCH", "IOTA " };
		
		// We watch for headings of the two returned tables of coin-balances and owned NFTs.
		private const string BALANCES_HEADER = "AVAIL";
		private const string NFT_HEADER      = "OWNED";
		private const string FIELD_NAMES     = "STATU";

		private readonly GoShimmerCLI083 _api;
		private readonly IWallet         _wallet;

		internal GetBalance (GoShimmerCLI083 api, IWallet wallet)
		{
			_api    = api;
			_wallet = wallet;
		}

		public IEnumerator Execute (Action onSuccess = null, Action onFailure = null)
		{
			if (_api != null) yield return _api.StartCoroutine (_api.StartProcess (PROCESS_PARAMETER, ParseBalanceOutput));
			
			// TODO: Check for success condition.
			onSuccess?.Invoke ();
		}
		
		private void ParseBalanceOutput (string balanceOutput)
		{ 
/*
			>>> Example output for "Balance" command >>>
			 
			IOTA 2.0 DevNet CLI -Wallet 0.2
      Fetching balance...

      Available Token Balances

      STATUS  BALANCE                COLOR                                           TOKEN NAME
      ------  ---------------        --------------------------------------------    -------------------------
      [ OK ]  949600 I               IOTA                                            IOTA
      [ OK ]  49900 EGF              AuaBGxsdWXV72PsCgtKgDiFhcVCHRLAfgyhFH6QY1bbr    EdwardCoin

      Owned NFTs (Governance Controlled Aliases)

      STATUS  NFT ID (ALIAS ID)                             BALANCE            COLOR                                           TOKEN NAME
      ------  --------------------------------------------  ---------------    --------------------------------------------    -------------------------
      [ OK ]  jzk4JDY2XHdH4iZq9gUP3Q7GwdLDtQkAjxBPAoxakVpk  100 I              IOTA                                            IOTA
      [ OK ]  eC3DMLPWjqJQR5LMMs55vniGwbyRX87HPYYjiTYdSvq7  100 I              IOTA                                            IOTA
      [ OK ]  oWXV7Ufgyh3dZayW5ozQcdqEN5s4oLwqrPrm8NUwxXVR  100 I              IOTA                                            IOTA
*/

			// The parser works with two lists in succession. The active list is referenced by workingList.
			var workingList = new List<Dictionary<string, string>> ();

			// For each list created, headings are derived from the output and stored in headings.
			List<string> headings = new List<string> ();

			// This foreach splits the output into an array of lines, to parse them one-by-one.
			foreach (var line in balanceOutput.Split (new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				// This parser will compare the first five characters of the line to determine appropriate actions.
				var lineStart = line.Substring (0, 5).ToUpper();
				
				// Skip lines the parser doesn't need.
				if (_throwAwayLines.Contains (lineStart)) continue;

				switch (lineStart)
				{
					case BALANCES_HEADER:
						workingList = _wallet.CoinBalances;
						workingList.Clear ();
						continue;
					case NFT_HEADER:
						workingList = _wallet.OwnedNFTs;
						workingList.Clear ();
						continue;
				}

				// Split the line at each tab, into an array of strings, discarding empty strings.
				var words = Regex.Split (line, @"\t").Where (s => s != String.Empty).ToArray<string> ();
				
				// Use these lines to log each line after splitting.
				//string log = "Entry:\n";
				//foreach (var t in words) { log = string.Concat (log, t, "\n"); }
				//Debug.Log(log);

				if (CheckForHeadings (lineStart, headings, words)) continue;

				ParseBalanceEntry (words, headings, workingList);
			}
		}

		/// <summary>
		/// Assigns entries in a string array it to the appropriate data structure in the wallet. Works for coins and NFTs.
		/// </summary>
		/// <param name="words">Array of strings, derived from a line of the CLI output</param>
		/// <param name="headings">Array of strings representing the headings for the current mode (coin or NFT)</param>
		/// <param name="workingList">Data structure in the wallet, depending on mode (coin or NFT)</param>
		private void ParseBalanceEntry (string[] words, List<string> headings, List<Dictionary<string, string>> workingList)
		{
			// This version of the CLI seems to incorrectly display some NFTs in the output, so we check that the split
			// line has the same number of entries as the previously parsed headings. If not, skip the line.
			if (words.Length < headings.Count) return;
			
			// Add a new, blank entry to the end of workingList.
			workingList.Add (new Dictionary<string, string> ());

			// The line data will match the order of the headings, which we use as the keys in the dictionary.
			for (int i = 0; i < words.Length; i++) workingList.Last ().Add (headings[i], words[i]);
		}

		private bool CheckForHeadings (string lineStart, List<string> headings, string[] words)
		{
			// Extract the heading line of the table, to use as field names in the dictionary.
			if (string.Equals (lineStart, FIELD_NAMES))
			{
				headings.Clear ();
				foreach (var s in words) headings.Add (s);
				return true;
			}

			return false;
		}
	}
}
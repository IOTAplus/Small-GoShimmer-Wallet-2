using System.Collections.Generic;
using UnityEngine;
using IOTAplus.LedgerAPI.Wallet;


namespace IOTAplus.LedgerAPI.DummyLedger
{
	/// <summary>
	/// DummyLedger is a fully operational API that conforms to IOTAplus.LedgerAPI.ILedgerAPI. Its purpose is to allow
	/// developers to test their Unity projects without sending off commands to the DevNet or live network. This will
	/// help isolate any code-structure problems that might not be obvious if there is a secondary communication problem. 
	/// </summary>
	public class DummyLedger : LedgerAPIBase
	{
		private IWallet _wallet;

		private void Start ()
		{
			_wallet = GetComponent<IWallet> ();
			if (_wallet == null) Debug.Log ("Dummy Ledger: No wallet found.");
		}

		public override void GetBalance ()
		{
			Debug.Log ("DummyLedger.GetBalance");
			var result = new List<Dictionary<string, string>> ()
			{
				new Dictionary<string, string> ()
				{
					{ "STATUS", "[FINE]" },
					{ "BALANCE", "123456.789" },
					{ "COLOR", "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGH" },
					{ "NAME", "DummyCoin" }
				},
				new Dictionary<string, string> ()
				{
					{ "STATUS", "[GOOD]" },
					{ "BALANCE", "1000" },
					{ "COLOR", "ZYXWVUTSRQPONMLKJIHGFEDCBA9876543210hgfedcba" },
					{ "NAME", "EdwardCoin" }
				}
			};

			_wallet.SetCoinBalances (result);
			
			//TODO Add dummy NFT data.
			
			_OnGetBalanceSuccess?.Invoke ();
		}
	}
}
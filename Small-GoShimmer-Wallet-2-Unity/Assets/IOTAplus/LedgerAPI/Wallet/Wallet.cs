using System.Collections.Generic;
using System.Linq;
using IOTAplus.Utilities;
using UnityEngine;

namespace IOTAplus.LedgerAPI.Wallet
{
	/// <summary>
	/// Wallet contains only wallet-data and methods to read, set or reset that data.
	/// It must be placed on the same GameObject as the IOTAplus Ledger API.
	/// At this time, there should only be one Wallet in the game.
	/// </summary>
	[DisallowMultipleComponent]
	public class Wallet : MonoBehaviour, IWallet
	{
		protected List<Dictionary<string, string>> _tokenBalances;
		protected List<Dictionary<string, string>> _ownedNfTs;

		public List<Dictionary<string, string>> TokenBalances => _tokenBalances;

		public List<Dictionary<string, string>> OwnedNFTs => _ownedNfTs;
		
		public string TokenBalancesJson => Json.ListDictionaryToJson (_tokenBalances);
		public string OwnedNFTsJson    => Json.ListDictionaryToJson (_ownedNfTs);
		
		private const string BALANCE_KEY   = "BALANCE";
		private const string COLORHASH_KEY = "COLOR";
		
		public Wallet ()
		{
			_tokenBalances = new List<Dictionary<string, string>> ();
			_ownedNfTs    = new List<Dictionary<string, string>> ();
		}

		public bool SetTokenBalances (List<Dictionary<string, string>> newTokenBalances)
		{
			// TODO: Validation checks for coin balances.
			_tokenBalances = newTokenBalances;
			return true;
		}

		public bool SetOwnedNfts (List<Dictionary<string, string>> newOwnedNfts)
		{
			// TODO: Validation checks for owned NFTs.
			_ownedNfTs = newOwnedNfts;
			return true;
		}

		public bool CheckForSufficientBalance (string color, float amount)
		{
			string balance = "";
			foreach (Dictionary<string,string> d in _tokenBalances)
			{
				if (d.ContainsValue (color))
					balance = d[BALANCE_KEY];
			}

			if (balance == "") return false;
			Debug.Log ("balance = " + balance);
			balance = balance.Split (' ')[0];
			float b = float.Parse (balance);
			Debug.Log ("b = " + b);
			return b >= amount;
		}

		public string GetColorHash (string tokenName)
		{
			var token = _tokenBalances.FirstOrDefault (d => d.ContainsValue (tokenName));

			return token?[COLORHASH_KEY];
		}
	}
}
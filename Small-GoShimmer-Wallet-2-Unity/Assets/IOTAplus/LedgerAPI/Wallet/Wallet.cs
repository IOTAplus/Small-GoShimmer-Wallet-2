using System.Collections.Generic;
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
		protected List<Dictionary<string, string>> _coinBalances;
		protected List<Dictionary<string, string>> _ownedNfTs;

		public List<Dictionary<string, string>> CoinBalances => _coinBalances;

		public List<Dictionary<string, string>> OwnedNFTs => _ownedNfTs;
		
		public string CoinBalancesJson => Json.ListDictionaryToJson (_coinBalances);
		public string OwnedNFTsJson    => Json.ListDictionaryToJson (_ownedNfTs);
		
		private const string BALANCE_KEY = "BALANCE";

		public Wallet ()
		{
			_coinBalances = new List<Dictionary<string, string>> ();
			_ownedNfTs    = new List<Dictionary<string, string>> ();
		}

		public bool SetCoinBalances (List<Dictionary<string, string>> newCoinBalances)
		{
			// TODO: Validation checks for coin balances.
			_coinBalances = newCoinBalances;
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
			foreach (Dictionary<string,string> d in _coinBalances)
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
	}
}
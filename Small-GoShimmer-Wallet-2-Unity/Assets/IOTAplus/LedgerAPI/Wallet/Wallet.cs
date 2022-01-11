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
	}
}
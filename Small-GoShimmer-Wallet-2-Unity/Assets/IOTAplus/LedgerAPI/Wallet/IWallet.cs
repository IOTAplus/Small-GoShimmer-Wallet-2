using System.Collections.Generic;

namespace IOTAplus.LedgerAPI.Wallet
{
	/// <summary>
	/// IWallet is an Interface for a crypto-wallet. It is the basis for all wallet-API interactions
	/// in the IOTAplus namespace.
	/// </summary>
	public interface IWallet
	{
		List <Dictionary<string, string>> CoinBalances { get; }
		List <Dictionary<string, string>> OwnedNFTs    { get; }

		string CoinBalancesJson { get; }
		string OwnedNFTsJson    { get; }

		bool SetCoinBalances (List<Dictionary<string, string>> newCoinBalances);
		bool SetOwnedNfts    (List<Dictionary<string, string>> newOwnedNfts);
	}
}
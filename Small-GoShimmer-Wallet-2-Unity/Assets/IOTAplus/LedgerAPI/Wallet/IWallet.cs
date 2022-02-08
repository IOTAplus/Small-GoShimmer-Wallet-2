using System.Collections.Generic;

namespace IOTAplus.LedgerAPI.WalletAPI
{
	/// <summary>
	/// IWallet is an Interface for a crypto-wallet. It is the basis for all wallet-API interactions
	/// in the IOTAplus namespace.
	/// </summary>
	public interface IWallet
	{
		List <Dictionary<string, string>> TokenBalances { get; }
		List <Dictionary<string, string>> OwnedNFTs    { get; }

		string TokenBalancesJson { get; }
		string OwnedNFTsJson    { get; }
		
		bool SetTokenBalances (List<Dictionary<string, string>> newTokenBalances);
		bool SetOwnedNfts    (List<Dictionary<string, string>> newOwnedNfts);

		/// <summary>
		/// Checks if the LOCAL CACHED wallet data has at least the specified amount of the specified color token.
		/// Consider performing a GetBalance before calling this method. 
		/// </summary>
		/// <param name="color">The color of the token to check.</param>
		/// <param name="amount">The minimum balance required for the method to return TRUE</param>
		/// <returns>TRUE if balance equals or exceed specified amount; FALSE otherwise.</returns>
		bool CheckForSufficientBalance (string color, float amount);

		string GetColorHash (string tokenName);
	}
}
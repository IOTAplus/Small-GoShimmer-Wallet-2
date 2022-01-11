using System;

namespace IOTAplus.LedgerAPI
{
	/// <summary>
	/// ILedgerAPI is the interface for all APIs developed under the IOTAplus namespace.
	/// It contains all available transactions, commands and events for the APIs.
	/// </summary>
	public interface ILedgerAPI
	{
		/// <summary>
		/// Obtains the balances of all coins and metadata for all NFTs in the wallet
		/// </summary>
		void GetBalance ();
		event Action OnGetBalanceSuccess;
		event Action OnGetBalanceFailure;
	}
}
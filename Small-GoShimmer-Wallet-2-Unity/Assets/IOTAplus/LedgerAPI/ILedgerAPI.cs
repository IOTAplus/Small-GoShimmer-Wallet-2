using System;

namespace IOTAplus.LedgerAPI
{
	/// <summary>
	/// ILedgerAPI is the interface for all APIs developed under the IOTAplus namespace.
	/// It contains all available transactions, commands and events for the APIs.
	/// </summary>
	public interface ILedgerAPI
	{
		
	#region GetBalance
		/// <summary>
		/// Obtains the balances of all coins and metadata for all NFTs in the wallet
		/// </summary>
		void GetBalance ();
		event Action OnGetBalanceSuccess;
		event Action OnGetBalanceFailure;
	#endregion

	#region SendToken
		/// <summary>
		/// Sends tokens to another wallet.
		/// </summary>
		/// <param name="amountToSend">The number of tokens to send.</param>
		/// <param name="colorToSend">The "color" of the token to send.</param>
		/// <param name="destinationAddress">The address of the destination wallet.</param>
		void SendToken (float amountToSend, string colorToSend, string destinationAddress);
		event Action OnSendTokenSuccess;
		event Action OnSendTokenFailure;
	#endregion

	}
}
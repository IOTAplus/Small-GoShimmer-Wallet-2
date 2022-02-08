using System;
using UnityEngine;


namespace IOTAplus.LedgerAPI
{
	/// <summary>
	/// LedgerAPIBase is an abstract class that implements ILedgerAPI and inherits from MonoBehaviour for
	/// more-convenient compatibility with Unity, which doesn't always play well with interfaces.
	/// Developers can create fields, properties and other references of type ILedgerAPI or LedgerAPIBase,
	/// depending on needs.
	/// 
	/// For consistency, all concrete implementations of ILedgerAPI should inherit from this LedgerAPIBase
	/// rather than implementing ILedgerAPI directly. 
	/// </summary>
	[RequireComponent(typeof(WalletAPI.Wallet))]
	[DisallowMultipleComponent]
	public abstract class LedgerAPIBase : MonoBehaviour, ILedgerAPI
	{
		
	#region GetBalance
		public virtual void GetBalance () {}

		protected    Action _OnGetBalanceSuccess;
		public event Action  OnGetBalanceSuccess
		{
			add    => _OnGetBalanceSuccess += value;
			remove => _OnGetBalanceSuccess -= value;
		}

		protected    Action _OnGetBalanceFailure;
		public event Action  OnGetBalanceFailure
		{
			add    => _OnGetBalanceFailure += value;
			remove => _OnGetBalanceFailure -= value;
		}
	#endregion

	#region SendToken
		public virtual void SendToken (float amountToSend, string colorToSend, string destinationAddress) {}

		protected Action _OnSendTokenSuccess;
		public event Action OnSendTokenSuccess
		{
			add    => _OnSendTokenSuccess += value;
			remove => _OnSendTokenSuccess -= value;
		}

		protected Action _OnSendTokenFailure;
		public event Action OnSendTokenFailure
		{
			add    => _OnSendTokenFailure += value;
			remove => _OnSendTokenFailure -= value;
		}
	#endregion
		
	}
}
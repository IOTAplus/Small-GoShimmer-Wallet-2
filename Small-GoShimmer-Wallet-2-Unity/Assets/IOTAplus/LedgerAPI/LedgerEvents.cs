using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace IOTAplus.LedgerAPI
{
	/// <summary>
	/// LedgerEvents is used by Unity designers who wish to wire up Ledger API commands and events from within the Editor.
	/// Place this component on the same component as the Ledger API and wire any GameObject's interaction event to
	/// the commands on this Component, and and GameObject's Component's Method to the provided UnityEvents
	/// in the Inspector.
	///
	/// This is a separate component so that, if a developer wishes, the Ledger API can be removed from the GameObject
	/// and replaced by another API, without losing all the in-editor connections.
	/// </summary>
	[DisallowMultipleComponent]
	public class LedgerEvents : MonoBehaviour
	{
		protected LedgerAPIBase _passthrough;
		
		[SerializeField]
		protected UnityEvent _OnGetBalanceSuccess;

		[SerializeField]
		protected UnityEvent _OnGetBalanceFailure;
		
		private void Awake ()
		{
			_passthrough = GetComponent<LedgerAPIBase> ();
		}

		private void OnEnable ()
		{
			_passthrough.OnGetBalanceSuccess += HandleGetBalanceSuccess;
			_passthrough.OnGetBalanceFailure += HandleGetBalanceFailure;
		}

		private void OnDisable ()
		{
			_passthrough.OnGetBalanceSuccess -= HandleGetBalanceSuccess;
			_passthrough.OnGetBalanceFailure -= HandleGetBalanceFailure;
		}

		void HandleGetBalanceSuccess () => _OnGetBalanceSuccess?.Invoke ();
		void HandleGetBalanceFailure () => _OnGetBalanceFailure?.Invoke ();
		
	
		public void GetBalance () => _passthrough.GetBalance ();
	}
}
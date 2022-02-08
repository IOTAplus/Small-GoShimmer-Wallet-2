using System;
using System.Collections;
using IOTAplus.LedgerAPI.WalletAPI;

namespace IOTAplus.LedgerAPI.GoShimmerCLI083
{
	
	public class SendToken
	{
		private readonly GoShimmerCLI083 _api;
		private readonly IWallet         _wallet;

		// We have leading an trailing spaces for these constants for cleaner concatenation code.
		private const string PROCESS_PARAMETER = " send-funds ";
		private const string AMOUNT_PARAMETER  = " -amount ";
		private const string DEST_PARAMETER    = " -dest-addr ";
		private const string COLOR_PARAMETER   = " -color ";

		private bool _success = true;		

		internal SendToken (GoShimmerCLI083 api, IWallet wallet)
		{
			_api    = api;
			_wallet = wallet;
		}
		
		
		public IEnumerator Execute (float amountToSend, string colorToSend, string destinationAddress, Action onSuccess = null, Action onFailure = null)
		{
			if (_api != null)
			{
				string parameters = string.Concat
				(
						PROCESS_PARAMETER,
						AMOUNT_PARAMETER,
						amountToSend.ToString (),
						DEST_PARAMETER,
						destinationAddress,
						COLOR_PARAMETER,
						colorToSend
				);

				yield return _api.StartCoroutine (_api.StartProcess (parameters, ParseBalanceOutput));

				if (_success) onSuccess?.Invoke ();
				else onFailure?.Invoke ();
			}

		}

		protected void ParseBalanceOutput (string output)
		{
			// TODO: Parse for "[DONE]" or failure
			_success = true;
		}
	}
}
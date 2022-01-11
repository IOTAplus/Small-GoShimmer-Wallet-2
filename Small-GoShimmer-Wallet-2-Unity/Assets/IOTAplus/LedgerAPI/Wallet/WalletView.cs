using UnityEngine;
using UnityEngine.UI;

namespace IOTAplus.LedgerAPI.Wallet
{
	/// <summary>
	/// WalletView is a simple set of display methods to show wallet balances on-screen.
	/// It's a temporary class for development purposes and is unlikely to be kept in the final API.
	/// </summary>
	public class WalletView : MonoBehaviour
	{
		private IWallet       _wallet;
		private LedgerAPIBase _ledger;

		private void Awake ()
		{
			_wallet = FindObjectOfType<Wallet> ();
			if (_wallet == null) Debug.LogError ("Cannot find wallet.");

			_ledger = FindObjectOfType<LedgerAPIBase> ();
			if (_ledger == null) Debug.LogError ("Cannot find Ledger API.");
		}

		/// <summary>
		/// WalletCoinBalanaceJson sets the text of a Text component to a JSON representation of
		/// a wallet's coin balance. The Text component must be on the same GameObject.
		/// The wallet must be of type IOTAplus.LedgerAPI.Wallet.Wallet.
		/// </summary>
		public void WalletCoinBalanceJson ()
		{
			Debug.Log ("Setting Balance in UI.");
			if (_wallet == null)
				_wallet = FindObjectOfType<Wallet> ();

			var display = GetComponent<Text> ();
			
			if (display != null)
				display.text = _wallet.CoinBalancesJson;
			else
				Debug.Log (string.Concat("Display Text Not Found.\n", _wallet.CoinBalancesJson));
		}
	}
}
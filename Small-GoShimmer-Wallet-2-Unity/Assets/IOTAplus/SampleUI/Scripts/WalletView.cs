using UnityEngine;
using UnityEngine.UI;
using IOTAplus.LedgerAPI.WalletAPI;

namespace IOTAplus.LedgerAPI.SampleUI
{
	/// <summary>
	/// WalletView is a simple set of display methods to show wallet balances on-screen.
	/// </summary>
	public class WalletView : MonoBehaviour
	{
		private Wallet        _wallet;
		private LedgerAPIBase _ledger;
		private Text          _display;

		private void Awake ()
		{
			_wallet = FindObjectOfType<WalletAPI.Wallet> ();
			if (_wallet == null) Debug.LogError ("Cannot find wallet.");

			_ledger = FindObjectOfType<LedgerAPIBase> ();
			if (_ledger == null) Debug.LogError ("Cannot find Ledger API.");

			_display = GetComponent<Text> ();
			if (_display == null) Debug.LogError ("Cannot find Text component.");
		}

		
		// These Json methods set the text of a Text component to a JSON representation
		// of a wallet's data. The Text component must be on the same GameObject.
		// The wallet must be of type IOTAplus.LedgerAPI.Wallet.Wallet.
		
		public void WalletCoinBalanceJson ()
		{
			_display.text = _wallet.TokenBalancesJson;
		}

		public void WalletOwnedNFTsJson ()
		{
			_display.text = _wallet.OwnedNFTsJson;
		}
	}
}
using UnityEngine;
using UnityEngine.UI;
using IOTAplus.LedgerAPI;
using IOTAplus.LedgerAPI.WalletAPI;

namespace IOTAplus.SampleUI
{
	/// <summary>
	/// A sample custom script to send tokens to another wallet, using UI InputFields and connecting
	/// to the Ledger API through the LedgerEvents bridge. 
	/// </summary>
	public class SendToken : MonoBehaviour
	{
		[SerializeField] protected Dropdown   _drpColor;
		[SerializeField] protected InputField _amount;
		[SerializeField] protected InputField _color;
		[SerializeField] protected InputField _destAddr;

		protected LedgerEvents _api;
		protected Wallet       _wallet;

		protected const string DROPDOWN_CAPTION = "Select Token";

		private void Awake ()
		{
			_api    = FindObjectOfType<LedgerEvents> ();
			_wallet = FindObjectOfType<Wallet> ();
		}

		private void OnEnable ()
		{
			PopulateDropDown ();
		}

		private void PopulateDropDown ()
		{
			_drpColor.options.Clear ();
			_drpColor.value            = 0;

			foreach (var token in _wallet.TokenBalances)
			{
				_drpColor.options.Add (new Dropdown.OptionData(token["TOKEN NAME"]));
			}
		}

		public void Send ()
		{
			string colorToSend = _drpColor.options[_drpColor.value].text;
			
			Debug.Log (string.Concat ("Sending Token: ", colorToSend));
			if (float.TryParse (_amount.text, out float a))
			{
				if (_wallet.CheckForSufficientBalance (colorToSend, a))
				{
					_api.SendToken (a, _wallet.GetColorHash (colorToSend), _destAddr.text);
				}
				else
				{
					// Insufficient balance.
				}
			}
		}
	}
}
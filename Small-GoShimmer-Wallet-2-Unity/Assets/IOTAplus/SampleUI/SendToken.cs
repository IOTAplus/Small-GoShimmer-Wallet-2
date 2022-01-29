using IOTAplus.LedgerAPI;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A sample custom script to send tokens to another wallet, using UI InputFields and connecting
/// to the Ledger API through the LedgerEvents bridge. 
/// </summary>
public class SendToken : MonoBehaviour
{
	[SerializeField] private InputField _amount;
	[SerializeField] private InputField _color;
	[SerializeField] private InputField _destAddr;

	private LedgerEvents                     _api;
	private IOTAplus.LedgerAPI.Wallet.Wallet _wallet;

	private void Awake ()
	{
		_api    = FindObjectOfType<LedgerEvents> ();
		_wallet = FindObjectOfType<IOTAplus.LedgerAPI.Wallet.Wallet> ();
	}

	public void Send ()
	{
		if (float.TryParse (_amount.text, out float a))
		{
			if (_wallet.CheckForSufficientBalance (_color.text, a))
			{
				_api.SendToken (a, _color.text, _destAddr.text);
			}
			else
			{
				// Insufficient balance.
			}
		}
	}
}

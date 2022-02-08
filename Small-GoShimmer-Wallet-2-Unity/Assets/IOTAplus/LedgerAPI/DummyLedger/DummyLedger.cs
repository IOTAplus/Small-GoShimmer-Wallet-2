using System.Collections.Generic;
using UnityEngine;
using IOTAplus.LedgerAPI.WalletAPI;


namespace IOTAplus.LedgerAPI.DummyLedger
{
	/// <summary>
	/// DummyLedger is a fully operational API that conforms to IOTAplus.LedgerAPI.ILedgerAPI. Its purpose is to allow
	/// developers to test their Unity projects without sending off commands to the DevNet or live network. This will
	/// help isolate any code-structure problems that might not be obvious if there is a secondary communication problem. 
	/// </summary>
	public class DummyLedger : LedgerAPIBase
	{
		private IWallet _wallet;

		private void Start ()
		{
			_wallet = GetComponent<IWallet> ();
			if (_wallet == null) Debug.Log ("Dummy Ledger: No wallet found.");
		}

		public override void GetBalance ()
		{
			Debug.Log ("DummyLedger.GetBalance");
			var coinBalances = new List<Dictionary<string, string>> ()
			{
				new Dictionary<string, string> ()
				{
					{ "STATUS", "[FINE]" },
					{ "BALANCE", "123456.789" },
					{ "COLOR", "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGH" },
					{ "NAME", "DummyCoin" }
				},
				new Dictionary<string, string> ()
				{
					{ "STATUS", "[GOOD]" },
					{ "BALANCE", "1000" },
					{ "COLOR", "ZYXWVUTSRQPONMLKJIHGFEDCBA9876543210hgfedcba" },
					{ "NAME", "EdwardCoin" }
				}
			};

			_wallet.SetTokenBalances (coinBalances);

			var OwnedNFTs = new List<Dictionary<string, string>> ()
			{
					new Dictionary<string, string> ()
					{
							{ "STATUS", "[ OK ]" },
							{ "NFT ID (ALIAS ID)", "oWXs55vniGwbh5Fg73yJWdpKSWXCJMkvTzUHpgA7tfTF" },
							{ "BALANCE", "14 I" },
							{ "COLOR", "IOTA" },
							{ "TOKEN NAME", "IOTA" }
					},
					new Dictionary<string, string> ()
					{
							{ "STATUS", "[ OK ]" },
							{ "NFT ID (ALIAS ID)", "qEZFqHefW2aoi8CW3axgk6kueWGpGdH4iZq9gUP3Q7wr" },
							{ "BALANCE", "105 I" },
							{ "COLOR", "IOTA" },
							{ "TOKEN NAME", "IOTA" }
					},
					new Dictionary<string, string> ()
					{
							{ "STATUS", "[ OK ]" },
							{ "NFT ID (ALIAS ID)", "jzk4JDY2XHJQR5LMMX87HPYYjiTYdSvq74oPAoxakVpk" },
							{ "BALANCE", "83 I" },
							{ "COLOR", "IOTA" },
							{ "TOKEN NAME", "IOTA" }
					},
			};

			_wallet.SetOwnedNfts (OwnedNFTs);
			
			_OnGetBalanceSuccess?.Invoke ();
		}

		public override void SendToken (float amountToSend, string colorToSend, string destinationAddress)
		{
			_OnSendTokenSuccess?.Invoke ();
		}
	}
}
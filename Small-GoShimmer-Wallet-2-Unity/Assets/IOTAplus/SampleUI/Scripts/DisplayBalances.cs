using System;
using System.Collections.Generic;
using IOTAplus.LedgerAPI.WalletAPI;
using UnityEngine;
using UnityEngine.Serialization;

namespace IOTAplus.SampleUI
{
	public class DisplayBalances : MonoBehaviour
	{
		[SerializeField]
		protected Transform _listingParent;
		
		[FormerlySerializedAs ("TokenListing")]
		[SerializeField]
		protected TokenListing _tokenListing;

		protected Wallet           _wallet;
		protected List<GameObject> _listings = new List<GameObject> ();

		protected const string NAME_LABEL   = "TOKEN NAME";
		protected const string STATUS_LABEL = "STATUS";
		protected const string PENDING_STATUS  = "[PEND]";


		private void Awake ()
		{
			_wallet = FindObjectOfType<Wallet> ();
		}

		public void UpdateOnScreenTokenBalances ()
		{
			foreach (var l in _listings) GameObject.Destroy (l);
			
			_listings.Clear ();

			foreach (var token in _wallet.TokenBalances)
			{
				var newListing = Instantiate (_tokenListing, _listingParent);
				_listings.Add (newListing.gameObject);
				newListing.name              = String.Concat (_tokenListing.name, " - ", token[NAME_LABEL]);
				newListing.tokenName.text    = token[NAME_LABEL];
				newListing.tokenBalance.text = String.Format("{0:#,###0}", float.Parse (token["BALANCE"].Split (' ')[0]));
			}
		}
	}
}

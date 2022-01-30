using System;
using System.Collections.Generic;
using IOTAplus.LedgerAPI.Wallet;
using UnityEngine;
using UnityEngine.Serialization;

namespace IOTAplus.SampleUI
{
	public class DisplayBalances : MonoBehaviour
	{
		[SerializeField]
		private Transform _listingParent;
		
		[FormerlySerializedAs ("TokenListing")]
		[SerializeField]
		private TokenListing _tokenListing;

		private Wallet           _wallet;
		private List<GameObject> _listings = new List<GameObject> ();

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
				newListing.name              = token["TOKEN NAME"];
				newListing.tokenName.text    = token["TOKEN NAME"];
				newListing.tokenBalance.text = String.Format("{0:#,###0}", float.Parse (token["BALANCE"].Split (' ')[0]));
			}
		}
	}
}

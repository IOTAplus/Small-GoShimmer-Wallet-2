namespace IOTAplus.LedgerAPI.WalletAPI
{
	[System.Serializable]
	public class NftData
	{
		public NftData ()
		{
			Properties = new Properties ();
		}

		public string     Title;
		public string     Type;
		public Properties Properties;
	}

	[System.Serializable]
	public class Properties
	{
		public Properties ()
		{
			name        = new NftDatum ();
			description = new NftDatum ();
			image       = new NftDatum ();
		}

		public NftDatum name, description, image;
	}

	[System.Serializable]
	public class NftDatum
	{
		public string type;
		public string description;
	}
}
using System;

namespace IOTAplus.LedgerAPI
{
	public interface INft
	{
		string NftId        { get; set; }
		string NftBalance   { get; set; }
		string NftTokenName { get; set; }
		string Color        { get; set; }
		string Balance      { get; set; }
		string TokenName    { get; set; }

		string NftTitle            { get; set; }
		string NftDescription      { get; set; }
		string NftIDataURLImage    { get; set; }
		string NftImageTitle       { get; set; }
		string NftImageDescription { get; set; }

		string NftIDString { get; set; }
		Byte[] AssetInNFT  { get; set; }

		bool IsNFT              { get; set; }
		bool withdrawAssetinNFT { get; set; }
	}
}
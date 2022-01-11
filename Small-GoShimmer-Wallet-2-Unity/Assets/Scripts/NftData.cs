using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
		nftName        = new NFTName ();
		nftDescription = new NFTDescription ();
		nftImage       = new NftImage ();
	}

	[FormerlySerializedAs ("Name")]
	public NFTName nftName;

	[FormerlySerializedAs ("NFTDescription")]
	public NFTDescription nftDescription;

	[FormerlySerializedAs ("Image")]
	public NftImage nftImage;
}

[System.Serializable]
public class NFTName
{
    [FormerlySerializedAs ("nftType")]
    public string nftType;
    public string Description;
}

[System.Serializable]
public class NFTDescription
{
    [FormerlySerializedAs ("nftType")]
    public string nftType;
    public string Description;
}

[System.Serializable]
public class NftImage
{
    [FormerlySerializedAs ("Type")]
    public string nftType;
    public string Description;
}

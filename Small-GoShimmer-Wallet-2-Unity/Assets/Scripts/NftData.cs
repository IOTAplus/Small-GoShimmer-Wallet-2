using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NftData
{
    public NftData()
    {
        Properties = new Properties();
    }
    public string Title;
    public string Type;
    public Properties Properties;
}

[System.Serializable]
public class Name
{
    public string Type;
    public string Description;
}

[System.Serializable]
public class NFTDescription
{
    public string Type;
    public string Description;
}

[System.Serializable]
public class Image
{
    public string Type;
    public string Description;
}

[System.Serializable]
public class Properties
{
    public Properties()
    {
        Name = new Name();
        NFTDescription = new NFTDescription();
        Image = new Image();
    }
    public Name Name;
    public NFTDescription NFTDescription;
    public Image Image;
}





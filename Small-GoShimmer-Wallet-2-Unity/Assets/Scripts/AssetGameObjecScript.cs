using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetGameObjecScript : MonoBehaviour
{
    public Text nftId;
    public Text nftBalance;
    public Text nftTokenName;
    public Text color;
    public Text balance;
    public Text tokenName;

    public Text NFTTitle;
    public Text NFTDescription;
    public Text NFTIDataURLImage;

    public string nftIDString;

    public InputField nftAmountToSend;
    public InputField nftAddressWhereToSend;
    public InputField anmountToSend;
    public InputField addressWhereToSend;

    public GameObject AssetInNFT;
    public GameObject ImageGameobject;

    public Renderer imageRenderer;

    public bool isNFT = false;
    public bool withdrawAssetinNFT = false;
    public Toggle toggle;

    public GameObject assetInNFT;

    Wallet wallet;

    // Start is called before the first frame update
    void Start()
    {
        wallet = GameObject.Find("Scripts").GetComponent<Wallet>();
    }

    // Update is called once per frame
    public void SendToken()
    {
        if (isNFT == false)
        { 
                wallet.StartProcess("send-funds -amount " + Convert.ToUInt32(anmountToSend.text) + " -dest-addr " + addressWhereToSend.text + " -color " + color.text);
        }

        if (isNFT == true)
        {
            if (withdrawAssetinNFT == true)
            {

                print(nftIDString);
                print(addressWhereToSend.text);
                print(color.text);

                wallet.StartProcess("withdraw-from-nft -amount " + Convert.ToUInt32(anmountToSend.text) + " -id " + nftIDString + " -dest-addr " + addressWhereToSend.text + " -color " + color.text); //withdraw funds from an nft

                // wallet.StartProcess("transfer-nft" + " -dest-addr " + nftAddressWhereToSend.text + " -id " + nftId.text);

            }
            else
            {
                //var balanceSplit = Regex.Split(nftBalance.text, @" ").Where(s => s != String.Empty).ToArray<string>();
                //for (int i = 0; i < balanceSplit.Length; i++)
                //{
                //    print($"at index {i} is word {balanceSplit[i]}");
                //    //if (words[i] == " ") { words[i].; }
                //}
                //wallet.StartProcess("send-funds -amount " + Convert.ToUInt32(balanceSplit) + " -dest-addr " + nftAddressWhereToSend.text + " -color " + nftId.text);

                wallet.StartProcess("transfer-nft" + " -dest-addr " + nftAddressWhereToSend.text + " -id " + nftId.text); //transfer the ownership of an nft
            }

        }
    }

    public void SendFunds()
    {
        wallet.StartProcess("send-funds -amount " + Convert.ToUInt32(anmountToSend.text) + " -dest-addr " + addressWhereToSend.text + " -color " + color.text);
    }

    public void DepositToNFT()
    {
        wallet.StartProcess("deposit-to-nft -amount " + Convert.ToUInt32(anmountToSend.text) + " -id " + addressWhereToSend.text + " -color " + color.text); //withdraw funds from an nft
    }


    public void SweepNFTOwnedNFTs()
    {
        wallet.StartProcess("sweep-nft-owned-nfts -id "+ nftId.text);
    }

    public void DestroyNFT()
    {
        wallet.StartProcess("destroy-nft -id " + nftId.text);
    }

    public GameObject CreateNewAssetInNFTGameobject()
    {
        var newObject = Instantiate(assetInNFT, assetInNFT.transform.parent);
        newObject.SetActive(true);
        return newObject;
    }

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = nftId.text;
    }

    public void CopyToClipboardColor()
    {
        GUIUtility.systemCopyBuffer = color.text;
    }

    public void ImageDisplay()
    {
       StartCoroutine(GetTexture());
        imageRenderer.material.color = Color.red;

    }
    IEnumerator GetTexture()
    {
       string MediaUrl="https://s20.directupload.net/images/210713/y25sow7c.jpg";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            ImageGameobject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    

            imageRenderer.material.color = Color.green;
  
    }
}

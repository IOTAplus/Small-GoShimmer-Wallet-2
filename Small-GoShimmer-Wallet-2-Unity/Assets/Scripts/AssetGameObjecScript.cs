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

    public Text NFTImageTitle;
    public Text NFTImageDescription;

    public string nftIDString;

    public InputField nftAmountToSend;
    public InputField nftAddressWhereToSend;
    public InputField anmountToSend;
    public InputField addressWhereToSend;

    public GameObject AssetInNFT;
    public SpriteRenderer ImageGameobject;

    public Renderer imageRenderer;

    public bool isNFT = false;
    public bool withdrawAssetinNFT = false;
    public Toggle toggle;

    public GameObject assetInNFT;

    Wallet wallet;

    public float correctedHeight;
    public float correctedWidth;


    [SerializeField]
    private UniGifImage m_uniGifImage;

    private bool m_mutex;

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

    public void CopyToClipboard(Text text)
    {
        GUIUtility.systemCopyBuffer = text.text;//nftId.text;
    }

    public void CopyToClipboardColor()
    {
        GUIUtility.systemCopyBuffer = color.text;
    }

    public void ImageDisplay()
    {
       //StartCoroutine(GetTexture());
       imageRenderer.material.color = Color.red;
        try
        {
            if (m_mutex || m_uniGifImage == null || string.IsNullOrEmpty(NFTIDataURLImage.text))
            {
                return;
            }

            m_mutex = true;
            StartCoroutine(ViewGifCoroutine());
        }
        catch
        {
            StartCoroutine(GetNFTImage(NFTIDataURLImage.text));
        }
      
    }
  

    private IEnumerator ViewGifCoroutine()
    {
        yield return StartCoroutine(m_uniGifImage.SetGifFromUrlCoroutine(NFTIDataURLImage.text));
        m_mutex = false;
    }

    private IEnumerator GetNFTImage(string url)
    {
        // string url = "


        NFTImageTitle.text = NFTTitle.text;
        NFTImageDescription.text = NFTDescription.text;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture2D myTexture = DownloadHandlerTexture.GetContent(www);

        print("Width: " + myTexture.width + " High: " + myTexture.height);
        float maxWidth = 460;
        float maxHigh = 460;
        //int widthCorrection = maxWidth / myTexture.width ;
        float highCorrection = maxHigh / myTexture.height;
        float widthCorrection = maxWidth / myTexture.width;
        
        Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
        Sprite spriteToUse = Sprite.Create(myTexture, rec, new Vector2(0.5f, 0.5f), 100);

        ImageGameobject.sprite = spriteToUse;
                
        ImageGameobject.GetComponent<RectTransform>().localScale = new Vector3(highCorrection * 20, 20 * highCorrection);


        //correctedWidth = myTexture.width * highCorrection;
        //correctedHeight = myTexture.height * highCorrection;

        print("HeighCorrection: "+ highCorrection+" CorrectedWidth: " + correctedWidth + " CorrectedHigh: " + correctedHeight);

        if (widthCorrection < 1)
        {
            //correctedWidth = myTexture.width * widthCorrection;
            //correctedHeight = myTexture.height * widthCorrection;
            print("Widthcorrection: " + widthCorrection + " CorrectedWidth: " + correctedWidth + " CorrectedHigh: " + correctedHeight);
            ImageGameobject.GetComponent<RectTransform>().localScale = new Vector3(widthCorrection * 20, 20 * widthCorrection);
        }

    }
}

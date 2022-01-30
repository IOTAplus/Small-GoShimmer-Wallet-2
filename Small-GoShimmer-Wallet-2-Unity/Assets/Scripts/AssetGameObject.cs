using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetGameObject : MonoBehaviour
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

    public GameObject NFTImageGameobject;

    public bool isNFT = false;
    public bool withdrawAssetinNFT = false;
    public Toggle toggle;

    public GameObject assetInNFT;

    TestWallet wallet;

    public float correctedHeight;
    public float correctedWidth;

    public GameObject  RawImage;

    [SerializeField] private UniGifImage m_uniGifImage;

    private bool m_mutex;

    void Start()
    {
        wallet = GameObject.Find("Scripts").GetComponent<TestWallet>();
    }

    public void SendFunds()
    {
        if (isNFT == false)
        {
            wallet.StartProcess("send-funds -amount " + Convert.ToUInt32(anmountToSend.text) + " -dest-addr " + addressWhereToSend.text + " -color " + color.text);
        }
    }

    public void WithdrawFromNFT ()
    {
	    print (nftIDString);
	    print (addressWhereToSend.text);
	    print (color.text);

	    wallet.StartProcess ("withdraw-from-nft -amount " + Convert.ToUInt32 (anmountToSend.text) + " -id " +
	                         nftIDString                  + " -dest-addr " + addressWhereToSend.text + " -color " +
	                         color.text); //withdraw funds from an nft

	    // wallet.StartProcess("transfer-nft" + " -dest-addr " + nftAddressWhereToSend.text + " -id " + nftId.text);
    }

    public void TransferNFT ()
    {
	    //var balanceSplit = Regex.Split(nftBalance.text, @" ").Where(s => s != String.Empty).ToArray<string>();
	    //for (int i = 0; i < balanceSplit.Length; i++)
	    //{
	    //    print($"at index {i} is word {balanceSplit[i]}");
	    //    //if (words[i] == " ") { words[i].; }
	    //}
	    //wallet.StartProcess("send-funds -amount " + Convert.ToUInt32(balanceSplit) + " -dest-addr " + nftAddressWhereToSend.text + " -color " + nftId.text);

	    wallet.StartProcess ("transfer-nft" + " -dest-addr " + nftAddressWhereToSend.text + " -id " +
	                         nftId.text); //transfer the ownership of an nft
    }

    public void DepositToNFT ()
    {
	    // Assets to NFT
	    wallet.StartProcess ("deposit-to-nft -amount " + Convert.ToUInt32 (anmountToSend.text) + " -id " +
	                         addressWhereToSend.text   + " -color " + color.text); //withdraw funds from an nft
    }

    public void SweepNFTownedFunds ()
    {
	    wallet.StartProcess ("sweep-nft-owned-funds -id " + nftId.text);
    }

    public void SweepNFTOwnedNFTs ()
    {
	    wallet.StartProcess ("sweep-nft-owned-nfts -id " + nftId.text);
    }

    public void ConsolidateFunds ()
    {
	    wallet.StartProcess ("consolidate-funds");

    }

    public void DestroyNFT ()
    {
	    wallet.StartProcess ("destroy-nft -id " + nftId.text);
    }

    public GameObject CreateNewAssetInNFTGameobject ()
    {
	    var newObject = Instantiate (assetInNFT, assetInNFT.transform.parent);
	    newObject.SetActive (true);
	    return newObject;
    }

    public void CopyToClipboard (Text text)
    {
	    string s = text.text;
	    GUIUtility.systemCopyBuffer = s;
    }

    public void CopyToClipboardColor ()
    {
	    GUIUtility.systemCopyBuffer = color.text;
    }

    public void ImageDisplay ()
    {
	    NFTImageTitle.text       = NFTTitle.text;
	    NFTImageDescription.text = NFTDescription.text;

	    MethodExtension.gif = true;
	    RawImage.SetActive (true);
	    ImageGameobject.sprite = null;

	    if (m_mutex || m_uniGifImage == null || string.IsNullOrEmpty (NFTIDataURLImage.text))
	    {
		    return;
	    }

	    m_mutex = true;
	    StartCoroutine (ViewGifCoroutine ());
    }

    private IEnumerator ViewGifCoroutine ()
    {
	    IEnumerator x;
	    yield return StartCoroutine (m_uniGifImage.SetGifFromUrlCoroutine (NFTIDataURLImage.text));

	    m_mutex = false;

	    float height = RawImage.GetComponent<RectTransform> ().rect.height;
	    float width  = RawImage.GetComponent<RectTransform> ().rect.width;

	    Resize (height, width, 90, 130, Convert.ToSingle (0.25), RawImage);

	    //print("Height: " + height+" Width: "+width);
	    if (MethodExtension.gif == false)
	    {
		    RawImage.SetActive (false);
		    StartCoroutine (GetNFTImage ());
	    }
    }

    private IEnumerator GetNFTImage ()
    {
	    // string url = "
	    print ("GET NFT ÌMAGE STARTED");

	    UnityWebRequest www = UnityWebRequestTexture.GetTexture (NFTIDataURLImage.text);
	    yield return www.SendWebRequest ();

	    Texture2D myTexture = DownloadHandlerTexture.GetContent (www);

	    //int widthCorrection = maxWidth / myTexture.width ;

	    Rect   rec         = new Rect (0, 0, myTexture.width, myTexture.height);
	    Sprite spriteToUse = Sprite.Create (myTexture, rec, new Vector2 (0.5f, 0.5f), 100);

	    ImageGameobject.sprite = spriteToUse;

	    Resize (myTexture.height, myTexture.width, 90, 130, 20, NFTImageGameobject);

	    //print("HeighCorrection: "+ highCorrection+" CorrectedWidth: " + correctedWidth + " CorrectedHigh: " + correctedHeight);
    }

    public void Resize (float height, float width, float maxHeight, float maxWidth, float currentScale,
	    GameObject            gameObject)
    {
	    float highCorrection  = 300 / height;
	    float widthCorrection = 450 / width;

	    print ("Width: " + width + " High: " + height + " CurrentScale: " + currentScale + " + HighCorrection: " +
	           highCorrection);

	    gameObject.GetComponent<RectTransform> ().localScale =
		    new Vector3 (currentScale * highCorrection, currentScale * highCorrection);

	    if (widthCorrection < 1)
	    {
		    print ("Width: " + width + " CurrentScale: " + currentScale + " + WitdhCorrection: " + widthCorrection);

		    gameObject.GetComponent<RectTransform> ().localScale =
			    new Vector3 (currentScale * widthCorrection, currentScale * widthCorrection);
	    }
    }
}
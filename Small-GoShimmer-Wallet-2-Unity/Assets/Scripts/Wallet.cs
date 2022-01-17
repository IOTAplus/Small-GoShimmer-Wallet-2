using UnityEngine;
using System.Diagnostics;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Collections;

[Serializable]
public class Wallet : MonoBehaviour
{
	//public Color schwarz;
	//public Color grau;
	public Text responseText;

	public GameObject scrollRectGameobject;

	public InputField tokenName;
	public InputField symbol;
	public InputField nftDescriptionInputfield;
	public InputField nftURL;
	public InputField amount;

	// public InputField colorToSend;
	// public InputField amountToSend;
	// public InputField AddressWhereToSend;
	public InputField colorToNFT;

	public GameObject assetGameobject;
	public GameObject nfttGameobject;
	public GameObject nftTitleGameobject;
	public GameObject assetInNfttGameobject;

	public List<GameObject> gameobjectList;

	public NftData nftData;
	NFTName        m_nftNftName;
	NFTDescription nftDescription;
	NftImage       m_nftNftImage;
	Properties     nftProperties;

	string     manaNodeUrl = "http://nodes.nectar.iota.cafe";
	public int index;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    private string path = Application.streamingAssetsPath + "/cli-wallet-mac";
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private string path = Application.streamingAssetsPath + "/cli-wallet-win.exe";
#endif
#if UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
	private string path = Application.streamingAssetsPath + "/cli-wallet-linux";
#endif

	public void Start ()
	{
		nftData        = new NftData ();
		m_nftNftName   = new NFTName ();
		nftDescription = new NFTDescription ();
		m_nftNftImage  = new NftImage ();
		nftProperties  = new Properties ();
		gameobjectList = new List<GameObject> ();
		InitWallet ();
		GetBalance ();
	}

	void Awake ()
	{
		StartCoroutine (FixScrollRects ());
	}

	public IEnumerator FixScrollRects ()
	{
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();

		foreach (var scrollRect in scrollRectGameobject.GetComponentsInChildren<ScrollRect> ())
		{
			scrollRect.SetValue (0);
		}
	}

	public NftData GetImmutableData (string nftAddress)
	{
		var NFTAddress = nftAddress;

		string    url                    = "http://goshimmer.maikpiel.de:8080/ledgerstate/addresses/" + NFTAddress;
		WebClient client                 = new WebClient ();
		string    immutableNFTTextBase64 = client.DownloadString (url);

		immutableNFTTextBase64 =
				immutableNFTTextBase64.Substring (immutableNFTTextBase64.IndexOf ("immutableData\":") + 16);

		immutableNFTTextBase64 = immutableNFTTextBase64.Substring (0, immutableNFTTextBase64.IndexOf ("\""));

		byte[] data                   = Convert.FromBase64String (immutableNFTTextBase64);
		string encodedImmutableString = Encoding.UTF8.GetString (data);
		print (encodedImmutableString);

		NftData nftData = JsonUtility.FromJson<NftData> (encodedImmutableString);
		print (nftData.Properties.nftImage.Description);
		return nftData;
	}

	public void InitWallet ()
	{
		StartProcess ("init");
	}

	public void RequestIotas ()
	{
		StartProcess ("request-funds");
	}

	public void GetBalance ()
	{
		StartProcess ("balance");
		string balance = responseText.text;
		print (balance);

		bool inNftLines = false;

		foreach (var gameObject in gameobjectList)
		{
			Destroy (gameObject);
		}

		gameobjectList = new List<GameObject> ();


		foreach (var line in balance.Split (new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
		{
			// skip lines which has no value
			if (line.StartsWith ("IOTA")   || line.StartsWith ("Fetching") || line.StartsWith ("Available") ||
			    line.StartsWith ("STATUS") || line.StartsWith ("-"))
				continue;

			// check if we reached the nft part
			if (line.StartsWith ("Owned NFT"))
			{
				inNftLines = true;
				var newNFTTitleObject = Instantiate (nftTitleGameobject, assetGameobject.transform.parent);
				newNFTTitleObject.SetActive (true);
				gameobjectList.Add (newNFTTitleObject);
				continue;
			}

			if (inNftLines == false)
			{
				//Console.WriteLine("hi");
				var words = Regex.Split (line, @"\t").Where (s => s != String.Empty).ToArray<string> ();

				// string[] words = line.Split('\t', (char)StringSplitOptions.RemoveEmptyEntries);

				var newObject = Instantiate (assetGameobject, assetGameobject.transform.parent);
				newObject.SetActive (true);
				gameobjectList.Add (newObject);
				newObject.gameObject.GetComponent<AssetGameObject> ().color.text     = words[2];
				newObject.gameObject.GetComponent<AssetGameObject> ().balance.text   = words[1];
				newObject.gameObject.GetComponent<AssetGameObject> ().tokenName.text = words[3];

				Console.WriteLine ("balance " + words[1] + " color " + words[2] + " name " + words[3]);
			}

			if (inNftLines == true)
			{
				// if line starts with space it is the buggy line

				if (line.StartsWith ("[ OK ]") || line.StartsWith ("[PEND]"))
				{
					//Console.WriteLine("hi");
					var words = Regex.Split (line, @"\t").Where (s => s != String.Empty).ToArray<string> ();

					//Console.WriteLine("balance " + words[2] + " color " + words[1] + " name " + words[3]);
					var newObject = Instantiate (nfttGameobject, assetGameobject.transform.parent);
					newObject.SetActive (true);
					gameobjectList.Add (newObject);
					newObject.gameObject.GetComponent<AssetGameObject> ().nftId.text         = words[1];
					newObject.gameObject.GetComponent<AssetGameObject> ().nftBalance.text    = words[2];
					newObject.gameObject.GetComponent<AssetGameObject> ().nftTokenName.text  = words[3];
					newObject.gameObject.GetComponent<AssetGameObject> ().isNFT              = true;
					newObject.gameObject.GetComponent<AssetGameObject> ().withdrawAssetinNFT = false;

					newObject.gameObject.GetComponent<AssetGameObject> ().nftAmountToSend.GetComponent<InputField> ().placeholder
							.GetComponent<Text> ().text = "all";

					newObject.gameObject.GetComponent<AssetGameObject> ().nftAmountToSend.GetComponent<InputField> ().readOnly =
							true;

					nftData = new NftData ();
					nftData = GetImmutableData (words[1]);
					
					newObject.gameObject.GetComponent<AssetGameObject> ().NFTTitle.text = nftData.Properties.nftName.Description;

					newObject.gameObject.GetComponent<AssetGameObject> ().NFTDescription.text =
							nftData.Properties.nftDescription.Description;

					newObject.gameObject.GetComponent<AssetGameObject> ().NFTIDataURLImage.text =
							nftData.Properties.nftImage.Description;
				}

				if (line.StartsWith ("\t"))
				{
					print ("-------------------hi----------------------" + "\n" + line);

					var words = Regex.Split (line, @"\t").Where (s => s != String.Empty).ToArray<string> ();

					//Console.WriteLine("balance " + words[0] + " color " + words[1] + " name " + words[2]);
					//print("Amount of object in list: "+(gameobjectList.Count - 1).ToString());


					// for (int i = 0 ; i < (gameobjectList.Count-1); i++) {
					//   print(i.ToString());
					// try
					//{
					//  if (gameobjectList[i].gameObject.GetComponent<AssetGameObject>().withdrawAssetinNFT == false)
					// {
					//    index = i;
					//    print("Index found maching == false: "+index.ToString());
					//}
					//}
					//catch { };

					//}

					var newObject2 = gameobjectList[gameobjectList.Count - 1].GetComponent<AssetGameObject> ()
							.CreateNewAssetInNFTGameobject (); //Instantiate(nfttGameobject, assetGameobject.transform.parent);

					//print("Number of objects in generated object list: " + (gameobjectList.Count - 1).ToString());

					//var newObject2 = Instantiate(assetInNfttGameobject, newObject.transform.parent);
					//newObject2.SetActive(true);


					//newObject2.GetComponent<AssetGameObject>().AssetInNFT.SetActive(true);

					string nftid = gameobjectList[gameobjectList.Count - 1].GetComponent<AssetGameObject> ().nftId.text;

					//gameobjectList.Add(newObject);
					newObject2.gameObject.GetComponent<AssetGameObject> ().isNFT              = true;
					newObject2.gameObject.GetComponent<AssetGameObject> ().withdrawAssetinNFT = true;
					newObject2.gameObject.GetComponent<AssetGameObject> ().nftIDString        = nftid;
					newObject2.gameObject.GetComponent<AssetGameObject> ().color.text         = words[1];
					newObject2.gameObject.GetComponent<AssetGameObject> ().balance.text       = words[0];
					newObject2.gameObject.GetComponent<AssetGameObject> ().tokenName.text     = words[2];

					//newObject.gameObject.GetComponent<AssetGameObject>().amountToSend.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "all";
					//newObject.gameObject.GetComponent<AssetGameObject>().amountToSend.GetComponent<InputField>().readOnly = true;
				}
			}
		}
	}

	public void CreateAsset ()
	{
		tokenName.text = tokenName.text.Replace (" ", "");

		if (tokenName.text.Length > 19)
		{
			tokenName.text = tokenName.text.Substring (0, 20);
		}

		;

		StartProcess ("create-asset -name " + tokenName.text + " -symbol " + symbol.text + " -amount " +
		              Convert.ToUInt32 (amount.text));
	}

	public void CreateNFT ()
	{
		nftData.Title                                 = "NFT";
		nftData.Properties.nftName.Description        = tokenName.text;
		nftData.Properties.nftDescription.Description = nftDescriptionInputfield.text;
		nftData.Properties.nftImage.Description       = nftURL.text;

		string saveFile   = Application.persistentDataPath + "/nftData.json";
		string jsonString = JsonUtility.ToJson (nftData);
		print (jsonString);

		// Write JSON to file.
		File.WriteAllText (saveFile, jsonString);

		string x = "create-nft -initial-amount " + Convert.ToUInt32 (amount.text) + " -immutable-data " + saveFile +
		           " -color "                    + colorToNFT.text;

		StartProcess (x);
	}

	// public void SendToken()
	//{
	//  StartProcess("send-funds -amount " + amountToSend.text + " -dest-addr " + AddressWhereToSend.text + " -color " + colorToSend.text);
	//}

	public void GenerateNewAddress ()
	{
		StartProcess ("address -new ");
	}

	//public void GetUnspendAddress()
	//{
	//   StartProcess("address -listunspent ");
	//   spendAddressTitel.color = grau;
	//   unspendAddressTitel.color = schwarz;
	//}

	public void LastAddress ()
	{
		StartProcess ("address -receive");
		string lastAddress = responseText.text;

		lastAddress                 = lastAddress.Substring (lastAddress.IndexOf ("Latest Receive Address: ") + 24);
		lastAddress                 = lastAddress.Substring (0, 45);
		GUIUtility.systemCopyBuffer = lastAddress;
	}

	public void ListAddress ()
	{
		StartProcess ("address -list");
	}

	public void GetSpendAddress ()
	{
		StartProcess ("address -listspent ");

		//  spendAddressTitel.color = schwarz;
		//  unspendAddressTitel.color = grau;

	}

	public void ServerStatus ()
	{
		string strCmdText;
		strCmdText = "server-status";
		Process.Start ("Assets/dll/cli-wallet.exe", strCmdText);
	}

	public void GameObjectOFF (GameObject gameObject)
	{
		gameObject.SetActive (false);
	}

	public void GameObjectON (GameObject gameObject)
	{
		gameObject.SetActive (true);
	}

	public void StartProcess (string order)
	{
		print (order);
		var process = new Process ();
		process.StartInfo.FileName  = path;
		process.StartInfo.Arguments = order;

		process.StartInfo.UseShellExecute        = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError  = true;
		process.StartInfo.CreateNoWindow         = true;

		process.Start ();

		//* Read the output (or the error)
		string err = process.StandardError.ReadToEnd ();

		responseText.text = err;
		print (err);

		string output = process.StandardOutput.ReadToEnd ();

		print (output);

		responseText.text = output;
		process.WaitForExit ();
	}
}
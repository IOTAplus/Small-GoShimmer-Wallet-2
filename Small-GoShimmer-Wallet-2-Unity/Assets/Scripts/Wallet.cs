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
    // Start is called before the first frame update
    //public Color schwarz;
    //public Color grau;
    public Text responseText;
    //public Text respondAddress;
    //public Text unspendAddressTitel;
    //public Text spendAddressTitel;

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
    Name nftName;
    NFTDescription nftDescription;
    Image nftImage;
    Properties nftProperties;

    string manaNodeUrl= "http://nodes.nectar.iota.cafe";
    public int index;

#if UNITY_STANDALONE_OSX
    private string path = Application.streamingAssetsPath + "/cli-wallet-mac";
#endif

#if UNITY_STANDALONE_WIN
    private string path = Application.streamingAssetsPath + "/cli-wallet-win.exe";
#endif
#if UNITY_STANDALONE_LIN
    private string path = Application.streamingAssetsPath + "/cli-wallet-lin.";
#endif

    public void Start()
    {
      
        nftData = new NftData();
        nftName = new Name();
        nftDescription = new NFTDescription();
        nftImage = new Image();
        nftProperties = new Properties();
        gameobjectList = new List<GameObject>();  
        InitWallet();
        GetBalance();
    }
    void Awake()
    {
        StartCoroutine(FixScrollRects());
    }

    public IEnumerator FixScrollRects()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (var scrollRect in scrollRectGameobject.GetComponentsInChildren<ScrollRect>())
        {
            scrollRect.SetValue(0);
        }
    }
    public NftData GetImmutableData(string nftAddress)
    {
        var NFTAddress = nftAddress;
        
        var url = "http://goshimmer.maikpiel.de:8080/ledgerstate/addresses/"+NFTAddress;
        WebClient client = new WebClient();
        string immutableNFTTextBase64 = client.DownloadString(url);
        immutableNFTTextBase64 = immutableNFTTextBase64.Substring(immutableNFTTextBase64.IndexOf("immutableData\":") + 16);
        immutableNFTTextBase64 = immutableNFTTextBase64.Substring(0, immutableNFTTextBase64.IndexOf("\""));

        byte[] data = Convert.FromBase64String(immutableNFTTextBase64);
        string encodedImmutableString = Encoding.UTF8.GetString(data);
        print(encodedImmutableString);

        NftData nftData = JsonUtility.FromJson<NftData>(encodedImmutableString);
        print(nftData.Properties.Image.Description);
        return nftData;
    }

    public void InitWallet()
    {
        StartProcess("init");
    }


    public void RequestIotas()
    {
        StartProcess("request-funds");
    }
    public void GetBalance()
    {
        StartProcess("balance");
        string balance = responseText.text;
        print(balance);
        
        bool inNftLines = false;
        foreach (var gameObject in gameobjectList) { Destroy(gameObject); }
        gameobjectList = new List<GameObject>();


        foreach (var line in balance.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            
            // skip lines which has no value
            if (line.StartsWith("IOTA") || line.StartsWith("Fetching") || line.StartsWith("Available") || line.StartsWith("STATUS") || line.StartsWith("-"))
                continue;

            // check if we reached the nft part
            if (line.StartsWith("Owned NFT"))
            {
                inNftLines = true;
                var newNFTTitleObject = Instantiate(nftTitleGameobject, assetGameobject.transform.parent);
                newNFTTitleObject.SetActive(true);
                gameobjectList.Add(newNFTTitleObject);
                continue;
            }

            if (inNftLines == false)
            {
                //Console.WriteLine("hi");
               var words = Regex.Split(line,@"\t").Where(s => s != String.Empty).ToArray<string>();
               // string[] words = line.Split('\t', (char)StringSplitOptions.RemoveEmptyEntries);

          
                var newObject = Instantiate(assetGameobject, assetGameobject.transform.parent);
                newObject.SetActive(true);
                gameobjectList.Add(newObject);
                newObject.gameObject.GetComponent<AssetGameObjecScript>().color.text = words[2] ;
                newObject.gameObject.GetComponent<AssetGameObjecScript>().balance.text = words[1];
                newObject.gameObject.GetComponent<AssetGameObjecScript>().tokenName.text = words[3];

                Console.WriteLine("balance " + words[1] + " color " + words[2] + " name " + words[3]);
            }

            if (inNftLines == true)
            {
                // if line starts with space it is the buggy line

                if (line.StartsWith("[ OK ]") || line.StartsWith("[PEND]"))
                {
                    //Console.WriteLine("hi");
                    var words = Regex.Split(line, @"\t").Where(s => s != String.Empty).ToArray<string>();
                    //Console.WriteLine("balance " + words[2] + " color " + words[1] + " name " + words[3]);
                    var newObject = Instantiate(nfttGameobject, assetGameobject.transform.parent);
                    newObject.SetActive(true);
                    gameobjectList.Add(newObject);
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().nftId.text = words[1];
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().nftBalance.text = words[2];
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().nftTokenName.text = words[3];
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().isNFT = true;
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().withdrawAssetinNFT = false;

                    newObject.gameObject.GetComponent<AssetGameObjecScript>().nftAmountToSend.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "all";
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().nftAmountToSend.GetComponent<InputField>().readOnly = true;

                    nftData = new NftData();
                    nftData = GetImmutableData(words[1]);
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().NFTTitle.text = nftData.Properties.Name.Description;
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().NFTDescription.text = nftData.Properties.NFTDescription.Description;
                    newObject.gameObject.GetComponent<AssetGameObjecScript>().NFTIDataURLImage.text = nftData.Properties.Image.Description;


                }
                if (line.StartsWith("\t"))
                {
                    print("-------------------hi----------------------"+"\n"+line);

                    var words = Regex.Split(line, @"\t").Where(s => s != String.Empty).ToArray<string>();
                    //Console.WriteLine("balance " + words[0] + " color " + words[1] + " name " + words[2]);
                    //print("Amount of object in list: "+(gameobjectList.Count - 1).ToString());


                   // for (int i = 0 ; i < (gameobjectList.Count-1); i++) {
                     //   print(i.ToString());
                       // try
                        //{
                          //  if (gameobjectList[i].gameObject.GetComponent<AssetGameObjecScript>().withdrawAssetinNFT == false)
                           // {
                            //    index = i;
                            //    print("Index found maching == false: "+index.ToString());
                            //}
                        //}
                        //catch { };
                       
                    //}

                    var newObject2 = gameobjectList[gameobjectList.Count - 1].GetComponent<AssetGameObjecScript>().CreateNewAssetInNFTGameobject(); //Instantiate(nfttGameobject, assetGameobject.transform.parent);
                                                                                                                                                    //print("Number of objects in generated object list: " + (gameobjectList.Count - 1).ToString());

                    //var newObject2 = Instantiate(assetInNfttGameobject, newObject.transform.parent);
                    //newObject2.SetActive(true);


                    //newObject2.GetComponent<AssetGameObjecScript>().AssetInNFT.SetActive(true);

                    string nftid = gameobjectList[gameobjectList.Count - 1].GetComponent<AssetGameObjecScript>().nftId.text;
                   
                    //gameobjectList.Add(newObject);
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().isNFT = true;
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().withdrawAssetinNFT = true;
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().nftIDString = nftid ;
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().color.text = words[1];
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().balance.text = words[0];
                    newObject2.gameObject.GetComponent<AssetGameObjecScript>().tokenName.text = words[2];

                    //newObject.gameObject.GetComponent<AssetGameObjecScript>().amountToSend.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "all";
                    //newObject.gameObject.GetComponent<AssetGameObjecScript>().amountToSend.GetComponent<InputField>().readOnly = true;

                }
            }
            else
            {
            }
            
           
        }
    }


    public void CreateAsset()
    {
        tokenName.text = tokenName.text.Replace(" ", "");
        if (tokenName.text.Length > 19) { tokenName.text = tokenName.text.Substring(0, 20); };
        StartProcess("create-asset -name " + tokenName.text + " -symbol " + symbol.text + " -amount " + Convert.ToUInt32(amount.text));
    }

    public void CreateNFT()
    {
        nftData.Title = "NFT";
        nftData.Properties.Name.Description = tokenName.text;
        nftData.Properties.NFTDescription.Description = nftDescriptionInputfield.text;
        nftData.Properties.Image.Description = nftURL.text;

        string saveFile = Application.persistentDataPath + "/nftData.json";
        string jsonString = JsonUtility.ToJson(nftData);
        print(jsonString);
        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);

        string x = "create-nft -initial-amount " + Convert.ToUInt32(amount.text) + " -immutable-data " + saveFile + " -color " + colorToNFT.text;

        StartProcess(x);
    }

  

    // public void SendToken()
    //{
    //  StartProcess("send-funds -amount " + amountToSend.text + " -dest-addr " + AddressWhereToSend.text + " -color " + colorToSend.text);
    //}
    public void GenerateNewAddress()
    {
        StartProcess("address -new ");
    }
    //public void GetUnspendAddress()
    //{
     //   StartProcess("address -listunspent ");
     //   spendAddressTitel.color = grau;
     //   unspendAddressTitel.color = schwarz;
    //}
    public void LastAddress()
    {
        StartProcess("address -receive");
        string lastAddress = responseText.text;

        lastAddress = lastAddress.Substring(lastAddress.IndexOf("Latest Receive Address: ")+24);

        GUIUtility.systemCopyBuffer = lastAddress;

    }
    public void ListAddress()
    {
        StartProcess("address -list");
    }

    public void GetSpendAddress()
    {
        StartProcess("address -listspent ");
      //  spendAddressTitel.color = schwarz;
      //  unspendAddressTitel.color = grau;

    }
    public void ServerStatus()
    {
        string strCmdText;
        strCmdText = "server-status";
        Process.Start("Assets/dll/cli-wallet.exe", strCmdText);
    }

    public void GameObjectOFF(GameObject gameObject)
    {
       
        gameObject.SetActive(false);
        Canvas.ForceUpdateCanvases();
    }

    public void GameObjectON(GameObject gameObject)
    {
        gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }

    public void StartProcess(string order)
    {
        Process process = new Process();
        process.StartInfo.FileName = path;
        process.StartInfo.Arguments = order;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        //* Read the output (or the error)
        string err = process.StandardError.ReadToEnd();
        responseText.text = err;
        print(err);
        string output = process.StandardOutput.ReadToEnd();
        print(output);

        responseText.text = output;
        process.WaitForExit();
    }

}


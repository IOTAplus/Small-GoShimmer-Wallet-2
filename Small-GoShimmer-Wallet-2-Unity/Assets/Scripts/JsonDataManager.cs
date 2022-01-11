using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonDataManager : MonoBehaviour
{
    // Create a field for the save file.
    [SerializeField]
    private string saveFile = Application.persistentDataPath + "/nftData.json";

    public NftData nftData;

    public void ReadFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Work with JSON
            nftData = JsonUtility.FromJson<NftData>(fileContents);
            print(nftData.Title);
        }
    }

    public void WriteFile()
    {
        // Work with JSON
        string jsonString = JsonUtility.ToJson(nftData);
        print(jsonString);
        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }
}
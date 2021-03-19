using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public static class FileNameGetter
{
    public static readonly string SaveFolderLocation = Application.persistentDataPath + "/Worlds/";
    public static void Init()
    {
        if (!Directory.Exists(SaveFolderLocation))
        {
            Directory.CreateDirectory(SaveFolderLocation);
        }
    }
}
public class WorldInfoDisplay : MonoBehaviour
{
    public GameObject Info;
    public Transform Parent;
    public List<string> GetFileNames = new List<string>();
    void Start()
    {
        string[] files = Directory.GetFiles(FileNameGetter.SaveFolderLocation);
        foreach (string file in files)
        {
            GetFileNames.Add(Path.GetFileName(file));
        }
        for(int i = 0; i < GetFileNames.Count; ++i)
        {
            GetFileNames[i] = GetFileNames[i].Replace(".txt", "");
            string LoadString = SaveLoadSystem.Load(GetFileNames[i]);
            if (LoadString != null)
            {
                Debug.Log("Loading");
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(LoadString);
                GameObject clone = Instantiate(Info, Parent);
                clone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = saveFile.WorldName + "\nSeed: " + saveFile.Seed;
            
            }
        
        }
    }
}

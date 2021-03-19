using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    LevelLoad m_levelLoader;
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
                clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = saveFile.WorldName + "\nSeed: " + saveFile.Seed + "\n"+ saveFile.ModeName;
                clone.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { SendData(saveFile); });
            }
        
        }
    }
    public void SendData(SaveFile _file)
    {
        m_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        m_levelLoader.Seed = _file.Seed;
        m_levelLoader.WorldName = _file.WorldName;
        m_levelLoader.ModeName = _file.ModeName;
        m_levelLoader.LoadLevel(1);
        if (_file.ModeName == "FreeMode")
            m_levelLoader.FreeMode = true;
        if (_file.ModeName == "ScoreMode")
            m_levelLoader.ScoreMode = true;
        if (_file.ModeName == "ExitMode")
            m_levelLoader.ExitMode = true;
    }
}

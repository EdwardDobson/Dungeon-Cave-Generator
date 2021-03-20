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
    public List<Texture2D> Screenshots = new List<Texture2D>();
    FileManager m_fileManager;
    LevelLoad m_levelLoader;
    void Start()
    {
        string[] files = Directory.GetDirectories(FileNameGetter.SaveFolderLocation);
        foreach (string file in files)
        {
            string[] subfiles = Directory.GetFiles(file, "*.txt");
            foreach (string subfile in subfiles)
            {
                GetFileNames.Add(Path.GetFileName(subfile));
            }
        }
        foreach (string file in files)
        {
            string[] subfiles = Directory.GetFiles(file, "*.png");
            foreach (string subfile in subfiles)
            {
                byte[] byteArray = File.ReadAllBytes(subfile);
                Texture2D texture = new Texture2D(500 , 500);
                texture.LoadImage(byteArray);
                Screenshots.Add(texture);
            }
        }
        for (int i = 0; i < GetFileNames.Count; ++i)
        {
            GetFileNames[i] = GetFileNames[i].Replace(".txt", "");
            string LoadString = SaveLoadSystem.Load(GetFileNames[i]);
            if (LoadString != null)
            {
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(LoadString);
                GameObject clone = Instantiate(Info);
                clone.transform.SetParent(Parent);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = saveFile.WorldName + "\nSeed: " + saveFile.Seed + "\n"+ saveFile.ModeName;
                clone.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { SendData(saveFile); });
                clone.transform.GetChild(0).GetComponent<Button>().image.sprite = Sprite.Create(Screenshots[i],new Rect(0,0, Screenshots[i].width, Screenshots[i].height),new Vector2());
            }
        
        }
    }
    public void SendData(SaveFile _file)
    {
        m_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        m_levelLoader.WorldName = _file.WorldName;
        m_levelLoader.Seed = _file.Seed;
        m_levelLoader.ModeName = _file.ModeName;
        m_levelLoader.LoadLevel(1);
        gameObject.SetActive(false);
    }
}

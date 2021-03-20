using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    public static readonly string SaveFolderLocation = Application.persistentDataPath + "/Worlds/";
    public static void Init()
    {
        if(!Directory.Exists(SaveFolderLocation))
        {
            Directory.CreateDirectory(SaveFolderLocation);
        }
    }
    public static void Save(string _saveString,string _worldName)
    {
        if (!Directory.Exists(SaveFolderLocation + _worldName ))
        {
            Directory.CreateDirectory(SaveFolderLocation + _worldName);
        }
        if (Directory.Exists(SaveFolderLocation + _worldName + "/"))
        {
            File.WriteAllText(SaveFolderLocation + _worldName + "/" + _worldName + ".txt", _saveString);
        }
    }
    public static string Load(string _worldName)
    {
        if (File.Exists(SaveFolderLocation + _worldName + "/" + _worldName + ".txt"))
        {
            string saveString = File.ReadAllText(SaveFolderLocation + _worldName + "/" + _worldName + ".txt");
      
            return saveString;
        }
        else
        {
            return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
/// <summary>
/// Houses all of the variables needed to create a world
/// </summary>
public class WorldMaker : MonoBehaviour
{
    public string WorldName;
    public int Seed;
    public TMP_InputField SeedField;
    public TMP_InputField WorldNameField;
    public TextAsset WorldNameFile;
    string[] m_worldNames;
    public string RandomWorldName;
    private void Start()
    {
        SeedField.characterLimit = 9;
       // WorldNameFile = (TextAsset)Resources.Load("WorldNames");
     //   m_worldNames = WorldNameFile.text.Split('\n');
    }

    public void ClearVariables()
    {
        WorldName = "";
        Seed = 0;
     
    }
    /// <summary>
    /// Used to create a template
    /// </summary>
    public void SetRandomValues()
    {
        Seed = UnityEngine.Random.Range(0, int.MaxValue);
        SeedField.text = Seed.ToString();
    }
    public void SetSeed()
    { 
       
        bool result = int.TryParse(SeedField.text, out Seed);
        if (result)
            Seed = int.Parse(SeedField.text);

    }
    public void SetWorldName()
    {
        WorldName = WorldNameField.text;
    }
    public void SendWorldInfo()
    {
<<<<<<< HEAD
        LevelLoad m_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        string[] dirNames = Directory.GetDirectories(FileNameGetter.SaveFolderLocation);
        if (WorldName == "")
        {
            WorldName = "World " + (dirNames.Length + 1);
            Debug.Log("World Name is: " + WorldName);
        }
        if(Seed == 0)
        {
            Seed = UnityEngine.Random.Range(0, int.MaxValue);
        }
        m_levelLoader.Seed = Seed;
        m_levelLoader.WorldName = WorldName;

        if (DungeonSizeX == 0 && DungeonSizeY == 0)
        {
            DungeonSizeX = UnityEngine.Random.Range(20, 1000); 
            DungeonSizeY = UnityEngine.Random.Range(20, 1000); 
        }
        if (SquareRoomAmount == 0)
        {
            SquareRoomAmount = UnityEngine.Random.Range(1, (DungeonSizeX+ DungeonSizeY)/5);
        }
        if (CircleRoomAmount == 0)
        {
            CircleRoomAmount = UnityEngine.Random.Range(1, (DungeonSizeX + DungeonSizeY) / 5);
        }
        if (TShapeRoomAmount == 0)
        {
            TShapeRoomAmount = UnityEngine.Random.Range(1, (DungeonSizeX + DungeonSizeY) / 5);
        }
        if (LShapeRoomAmount == 0)
        {
            LShapeRoomAmount = UnityEngine.Random.Range(1, (DungeonSizeX + DungeonSizeY) / 5);
        }
        if (DiamondRoomAmount == 0)
        {
            DiamondRoomAmount = UnityEngine.Random.Range(1, (DungeonSizeX + DungeonSizeY) / 5);
        }
        m_levelLoader.SquareRoomAmount = SquareRoomAmount;
        m_levelLoader.CircleRoomAmount = CircleRoomAmount;
        m_levelLoader.TShapeRoomAmount = TShapeRoomAmount;
        m_levelLoader.LShapeRoomAmount = LShapeRoomAmount;
        m_levelLoader.LShapeRoomAmount = DiamondRoomAmount;
        m_levelLoader.DungeonSizeX = DungeonSizeX;
        m_levelLoader.DungeonSizeY = DungeonSizeY;
}
=======
        GameObject.Find("LevelLoader").GetComponent<LevelLoad>().Seed = Seed;
        GameObject.Find("LevelLoader").GetComponent<LevelLoad>().WorldName = WorldName;
    }
>>>>>>> parent of e7e673c (Update)
}

using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
[System.Serializable]
public struct DataToSave
{
    public int PosX;
    public int PosY;
    public int PosZ;
    public int ID;
    public bool IsNull;
    public bool IsPlacedTile;
}
[System.Serializable]
public class SaveFile
{
    public int Seed;
    public bool SeedSet;
    public List<DataToSave> DataPacks = new List<DataToSave>();
}
public class FileManager : MonoBehaviour
{
    public List<DataToSave> TilesToSave = new List<DataToSave>();
    public List<DataToSave> TilesToLoad = new List<DataToSave>();
    public List<Vector3Int> PacksToRemove = new List<Vector3Int>();
    public string SavePath;
    public SaveFile Save = new SaveFile();
    [SerializeField]
    public Dictionary<Vector3Int, CustomTile> PlacedOnTiles;
    public GameObject Dungeon;
    BuildDungeon m_dungeon;
    public TextMeshProUGUI DebugText;
    private void Awake()
    {
        SavePath = Application.persistentDataPath + "/save.dat";
        Save.DataPacks = new List<DataToSave>();
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
    }
    private void Start()
    {
      LoadJson();
        //      LoadFromDisk();
    }
    public void Input(DataToSave _item)
    {
        if (Save.SeedSet)
            TilesToSave.Add(_item);
    }
    public void SaveJson()
    {
        SaveFile newFile = new SaveFile
        {
            Seed = 123123,
            SeedSet = true,
            DataPacks = TilesToSave
        };
        string saveString = JsonUtility.ToJson(newFile, true);
        File.WriteAllText(Application.persistentDataPath + "/save.txt", saveString);
    }
    public void LoadJson()
    {
        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            string loadString = File.ReadAllText(Application.persistentDataPath + "/save.txt");
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(loadString);
            Random.InitState(saveFile.Seed);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            TilePlacer(saveFile);

        }
        else
        {
            Random.InitState(123123);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            SaveJson();
        }
    }
    public void SaveToDisk()
    {

        for (int i = 0; i < TilesToSave.Count; ++i)
        {
            if (Save.DataPacks.All(t => !t.Equals(TilesToSave[i])))
                Save.DataPacks.Add(TilesToSave[i]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SavePath);
        bf.Serialize(file, Save);
        file.Close();
        Debug.Log("Saving Seed: " + Save.Seed);
        TilesToSave.Clear();
    }
    public void LoadFromDisk()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SavePath, FileMode.Open);
            Save = (SaveFile)bf.Deserialize(file);
            file.Close();
            Random.InitState(Save.Seed);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            TilePlacer(Save);
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(SavePath);
            bf.Serialize(file, Save);
            file.Close();
            Random.InitState(123123);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
        }
    }
    public void AddChangedTiles(int _index, SaveFile _file)
    {
        DataToSave temp = new DataToSave();
        temp.PosX = _file.DataPacks[_index].PosX;
        temp.PosY = _file.DataPacks[_index].PosY;
        temp.PosZ = _file.DataPacks[_index].PosZ;
        temp.IsNull = _file.DataPacks[_index].IsNull;
        temp.ID = _file.DataPacks[_index].ID;
        temp.IsPlacedTile = _file.DataPacks[_index].IsPlacedTile;
        TilesToLoad.Add(temp);
    }
    public void TilePlacer(SaveFile _file)
    {
        for (int i = 0; i < _file.DataPacks.Count; ++i)
        {
            Vector3Int tempPosI = new Vector3Int(_file.DataPacks[i].PosX, _file.DataPacks[i].PosY, _file.DataPacks[i].PosZ);
            for (int wall = 0; wall < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++wall)
            {
                if (_file.DataPacks[i].ID == TileManager.GetTileHolder(TileType.Wall).Tiles[wall].ID)
                {
                    if (!_file.DataPacks[i].IsNull)
                        TileManager.PlaceTile(tempPosI, 0, WallGen.GetTilemap(), WallGen.GetTilemap(), TileManager.GetTileHolder(TileType.Wall).Tiles[wall], DictionaryType.Walls);
                    else
                    {
                        TileManager.RemoveTilePiece(tempPosI, WallGen.GetTilemap());
                    }
                }
            }
            for (int floor = 0; floor < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++floor)
            {
                if (_file.DataPacks[i].ID == TileManager.GetTileHolder(TileType.Floor).Tiles[floor].ID)
                {
                    TileManager.PlaceTile(tempPosI, 0, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Floor).Tiles[floor], DictionaryType.Floor);
                    if (_file.DataPacks[i].IsPlacedTile)
                    {
                        if (!PlacedOnTiles.ContainsKey(tempPosI))
                            PlacedOnTiles.Add(tempPosI, TileManager.GetTileHolder(TileType.Floor).Tiles[floor]);
                    }
                }
            }
            for (int path = 0; path < TileManager.GetTileHolder(TileType.Path).Tiles.Count; ++path)
            {
                if (_file.DataPacks[i].ID == TileManager.GetTileHolder(TileType.Path).Tiles[path].ID)
                {
                    TileManager.PlaceTile(tempPosI, 0, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Path).Tiles[path], DictionaryType.Floor);
                }
            }
        }
    }
    public void TileSetter()
    {
        for (int i = 0; i < TilesToLoad.Count; ++i)
        {

            Vector3Int tempPosI = new Vector3Int(TilesToLoad[i].PosX, TilesToLoad[i].PosY, TilesToLoad[i].PosZ);

            if (TileManager.GetTileDictionaryFloor().ContainsKey(tempPosI))
            {
                for (int b = 0; b < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++b)
                {
                    if (TilesToLoad[i].ID == TileManager.GetTileHolder(TileType.Floor).Tiles[b].ID)
                    {
                        if (!TilesToLoad[i].IsPlacedTile)
                            TileManager.PlaceTile(tempPosI, 0, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Floor).Tiles[b], DictionaryType.Floor);
                        if (TilesToLoad[i].IsPlacedTile)
                        {
                            if (!PlacedOnTiles.ContainsKey(tempPosI))
                                PlacedOnTiles.Add(tempPosI, TileManager.GetTileHolder(TileType.Floor).Tiles[b]);
                            for (int a = 0; a < TilesToLoad.Count; ++a)
                            {
                                for (int c = 0; c < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++c)
                                {
                                    if (TilesToLoad[a].ID == TileManager.GetTileHolder(TileType.Wall).Tiles[c].ID)
                                    {
                                        TileManager.PlaceTile(tempPosI, 0, DungeonUtility.GetTilemap(), WallGen.GetTilemap(), TileManager.GetTileHolder(TileType.Wall).Tiles[c], DictionaryType.Walls);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (TileManager.GetTileDictionaryWalls().ContainsKey(tempPosI))
            {
                if (TilesToLoad[i].IsNull)
                {
                    WallGen.GetTilemap().SetTile(tempPosI, null);
                }
                for (int a = 0; a < TilesToLoad.Count; ++a)
                {
                    Vector3Int tempPosA = new Vector3Int(TilesToLoad[a].PosX, TilesToLoad[a].PosY, TilesToLoad[a].PosZ);
                    if (!TilesToLoad[a].IsNull && tempPosA == tempPosI)
                    {
                        for (int b = 0; b < TileManager.GetTileHolder(TileType.Path).Tiles.Count; ++b)
                        {
                            if (TilesToLoad[a].ID == TileManager.GetTileHolder(TileType.Path).Tiles[b].ID)
                            {
                                TileManager.PlaceTile(tempPosA, 0, WallGen.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Path).Tiles[b], DictionaryType.Floor);
                            }
                        }
                        for (int b = 0; b < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++b)
                        {
                            if (TilesToLoad[a].ID == TileManager.GetTileHolder(TileType.Floor).Tiles[b].ID)
                            {
                                TileManager.PlaceTile(tempPosA, 0, WallGen.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Floor).Tiles[b], DictionaryType.Floor);
                            }
                        }
                    }
                }
            }
        }
    }
}

using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct ItemInventorySave
{
    public ItemInventory InventorySlot;
}
[System.Serializable]
public struct DataToSave
{
    public Vector2Int Position;
    public int ID;
    public bool IsNull;
    public bool IsPlacedTile;
}
[System.Serializable]
public class SaveFile
{
    public string WorldName;
    public int Seed;
    public bool SeedSet;
    public string ModeName;
    public Vector2 PlayerPosition;
    public List<DataToSave> DataPacks = new List<DataToSave>();
    public List<ItemInventorySave> ItemPacks = new List<ItemInventorySave>();
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
    public TextMeshProUGUI DebugText;
    public int SetSeed;
    public string WorldName;
    public string ModeName;
    private void Awake()
    {
        SavePath = Application.persistentDataPath + "/save.dat";
        Save.DataPacks = new List<DataToSave>();
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        SaveLoadSystem.Init();
        if (GameObject.Find("LevelLoader") != null)
        {
            SetSeed = GameObject.Find("LevelLoader").GetComponent<LevelLoad>().Seed;
            WorldName = GameObject.Find("LevelLoader").GetComponent<LevelLoad>().WorldName;
            ModeName = GameObject.Find("LevelLoader").GetComponent<LevelLoad>().ModeName;
        }
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
        if (!File.Exists(SaveLoadSystem.SaveFolderLocation + WorldName + "/" + WorldName + ".txt"))
        {
            SaveFile newFile = new SaveFile
            {
                Seed = SetSeed,
                SeedSet = true,

                WorldName = WorldName,
                ModeName = ModeName,
                PlayerPosition = GameObject.Find("Player").transform.position
            };
            for (int i = 0; i < TilesToSave.Count; ++i)
            {
                if (newFile.DataPacks.All(t => !t.Equals(TilesToSave[i])))
                {
                    newFile.DataPacks.Add(TilesToSave[i]);
                }
            }
            SceenshotTaker.TakeScreenShot_static(64, 64);
            string saveString = JsonUtility.ToJson(newFile);
            SaveLoadSystem.Save(saveString, WorldName);
        }
        if (File.Exists(SaveLoadSystem.SaveFolderLocation + WorldName + "/" + WorldName + ".txt"))
        {

            string LoadString = SaveLoadSystem.Load(WorldName);

            if (LoadString != null)
            {
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(LoadString);
                for (int i = 0; i < TilesToSave.Count; ++i)
                {
                    if (saveFile.DataPacks.All(t => !t.Equals(TilesToSave[i])))
                    {
                        saveFile.DataPacks.Add(TilesToSave[i]);
                    }
                }
                InventoryBackpack backpack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
                for (int i = 0; i < backpack.Storage.Count; ++i)
                {
                    ItemInventorySave invSave = new ItemInventorySave();
                    invSave.InventorySlot = backpack.Storage[i];
                    if (saveFile.ItemPacks.All(t => t.InventorySlot.ID != backpack.Storage[i].ID))
                        saveFile.ItemPacks.Add(invSave);
                }
                //Clears the json file incase the backpack amount doesn't match
                if (saveFile.ItemPacks.Count != backpack.Storage.Count)
                {
                    saveFile.ItemPacks.Clear();
                    SaveLoadSystem.Save("", WorldName);
                }
                for (int i = 0; i < backpack.Storage.Count; ++i)
                {
                    ItemInventorySave invSave = new ItemInventorySave();
                    invSave.InventorySlot = backpack.Storage[i];
                    if (saveFile.ItemPacks.All(t => t.InventorySlot.ID != backpack.Storage[i].ID))
                        saveFile.ItemPacks.Add(invSave);
                }
                SceenshotTaker.TakeScreenShot_static(64, 64);
                saveFile.PlayerPosition = GameObject.Find("Player").transform.position;
                string saveString = JsonUtility.ToJson(saveFile);
                SaveLoadSystem.Save(saveString, WorldName);

            }
        }

    }
    public void LoadJson()
    {
        string LoadString = SaveLoadSystem.Load(WorldName);
        if (LoadString != null)
        {
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(LoadString);
            Random.InitState(saveFile.Seed);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            InventoryBackpack backpack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
            for (int i = 0; i < saveFile.ItemPacks.Count; ++i)
            {
                if (backpack.Storage.All(t => t.ID != saveFile.ItemPacks[i].InventorySlot.ID))
                    backpack.Storage.Add(saveFile.ItemPacks[i].InventorySlot);
            }
            for (int i = 0; i < backpack.Storage.Count; ++i)
            {
                for (int a = 0; a < TileManager.AllTiles.Count; ++a)
                {
                    if (backpack.Storage[i].ID == TileManager.AllTiles[a].ID)
                    {
                        for (int b = 0; b < backpack.Storage[i].Items.Count; ++b)
                        {
                            backpack.Storage[i].Items[b] = TileManager.AllTiles[a].Item;
                            backpack.Storage[i].Items[b].ItemID = TileManager.AllTiles[a].Item.ItemID;
                        }
                    }
                }
            }

            TilesToSave = saveFile.DataPacks;
            TilePlacer(saveFile, clone.transform.GetChild(0).GetComponent<Tilemap>(), clone.transform.GetChild(1).GetComponent<Tilemap>());
            GameObject.Find("Player").transform.position = saveFile.PlayerPosition;
            if (saveFile.ModeName == "FreeMode")
                GameObject.Find("GameManager").GetComponent<GameManager>().FreeMode = true;
            if (saveFile.ModeName == "ScoreMode")
                GameObject.Find("GameManager").GetComponent<GameManager>().ScoreMode = true;
            if (saveFile.ModeName == "ExitMode")
                GameObject.Find("GameManager").GetComponent<GameManager>().ExitMode = true;
        }
        else
        {
            if (SetSeed == 0)
                Random.InitState(123123);
            else
            {
                Random.InitState(SetSeed);
            }
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            if (FloorGen.GetFloorPositions().Count > 0)
            {
                Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
                Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                GameObject.Find("Player").transform.position = positionReadjusted;
            }
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
            //      TilePlacer(Save);
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
        temp.IsNull = _file.DataPacks[_index].IsNull;
        temp.ID = _file.DataPacks[_index].ID;
        temp.IsPlacedTile = _file.DataPacks[_index].IsPlacedTile;
        TilesToLoad.Add(temp);
    }
    public void TilePlacer(SaveFile _file, Tilemap _floorMap, Tilemap _wallMap)
    {
        for (int i = 0; i < _file.DataPacks.Count; ++i)
        {

            Vector3Int tempPosI = new Vector3Int(_file.DataPacks[i].Position.x, _file.DataPacks[i].Position.y, 0);
            for (int wall = 0; wall < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++wall)
            {
                if (_file.DataPacks[i].ID == TileManager.GetTileHolder(TileType.Wall).Tiles[wall].ID)
                {
                    if (!_file.DataPacks[i].IsNull)
                        TileManager.PlaceTile(tempPosI, 0, _wallMap, _wallMap, TileManager.GetTileHolder(TileType.Wall).Tiles[wall], DictionaryType.Walls);
                    else
                    {
                        TileManager.RemoveTilePiece(tempPosI, _wallMap);
                    }
                }
            }
            for (int floor = 0; floor < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++floor)
            {
                if (_file.DataPacks[i].ID == TileManager.GetTileHolder(TileType.Floor).Tiles[floor].ID)
                {
                    TileManager.PlaceTile(tempPosI, 0, _floorMap, _floorMap, TileManager.GetTileHolder(TileType.Floor).Tiles[floor], DictionaryType.Floor);
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
                    TileManager.PlaceTile(tempPosI, 0, _floorMap, _floorMap, TileManager.GetTileHolder(TileType.Path).Tiles[path], DictionaryType.Floor);
                }
            }
        }
    }
    public void TileSetter()
    {
        for (int i = 0; i < TilesToLoad.Count; ++i)
        {

            Vector3Int tempPosI = new Vector3Int(TilesToLoad[i].Position.x, TilesToLoad[i].Position.y, 0);

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
                    Vector3Int tempPosA = new Vector3Int(TilesToLoad[a].Position.x, TilesToLoad[a].Position.y, 0);
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

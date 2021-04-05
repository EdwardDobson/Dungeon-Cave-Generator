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
public class ItemInventorySave
{
    public ItemInventory InventorySlot;
    public int SlotNumber;
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
    public int SquareRoomAmount;
    public int CircleRoomAmount;
    public int TShapeRoomAmount;
    public int LShapeRoomAmount;
    public int DiamondRoomAmount;
    public int XDimension;
    public int YDimension;
    public Vector2 PlayerPosition;
    public int DigPower;
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
    public int SquareRoomAmount;
    public int CircleRoomAmount;
    public int TShapeRoomAmount;
    public int LShapeRoomAmount;
    public int DiamondRoomAmount;
    public int XDimension;
    public int YDimension;

    public List<GameObject> Slots = new List<GameObject>();
    private void Awake()
    {
        SavePath = Application.persistentDataPath + "/save.dat";
        Save.DataPacks = new List<DataToSave>();
        Save.ItemPacks = new List<ItemInventorySave>();
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        SaveLoadSystem.Init();
        LevelLoad m_levelLoad = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        if (m_levelLoad.gameObject != null)
        {
            SetSeed = m_levelLoad.Seed;
            WorldName = m_levelLoad.WorldName;
            ModeName = m_levelLoad.ModeName;
            SquareRoomAmount = m_levelLoad.SquareRoomAmount;
            CircleRoomAmount = m_levelLoad.CircleRoomAmount;
            TShapeRoomAmount = m_levelLoad.TShapeRoomAmount;
            LShapeRoomAmount = m_levelLoad.LShapeRoomAmount;
            DiamondRoomAmount = m_levelLoad.DiamondRoomAmount;
            XDimension = m_levelLoad.DungeonSizeX;
            YDimension = m_levelLoad.DungeonSizeY;
        }
    }
    private void Start()
    {
        LoadJson();
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
            Save.Seed = SetSeed;
            Save.SeedSet = true;
            Save.WorldName = WorldName;
            Save.ModeName = ModeName;
            Save.PlayerPosition = GameObject.Find("Player").transform.position;
            Save.SquareRoomAmount = SquareRoomAmount;
            Save.CircleRoomAmount = CircleRoomAmount;
            Save.TShapeRoomAmount = TShapeRoomAmount;
            Save.LShapeRoomAmount = LShapeRoomAmount;
            Save.DiamondRoomAmount = DiamondRoomAmount;
            Save.XDimension = XDimension;
            Save.YDimension = YDimension;
            for (int i = 0; i < TilesToSave.Count; ++i)
            {
                if (Save.DataPacks.All(t => !t.Equals(TilesToSave[i])))
                {
                    Save.DataPacks.Add(TilesToSave[i]);
                }
            }
            SceenshotTaker.TakeScreenShot_static(256, 256);
            string saveString = JsonUtility.ToJson(Save, true);
            SaveLoadSystem.Save(saveString, WorldName);
        }
        if (File.Exists(SaveLoadSystem.SaveFolderLocation + WorldName + "/" + WorldName + ".txt"))
        {
            string LoadString = SaveLoadSystem.Load(WorldName);

            if (LoadString != null)
            {
                Save = JsonUtility.FromJson<SaveFile>(LoadString);
                for (int i = 0; i < TilesToSave.Count; ++i)
                {
                    if (Save.DataPacks.All(t => !t.Equals(TilesToSave[i])))
                    {
                        Save.DataPacks.Add(TilesToSave[i]);
                    }
                }
                InventoryBackpack backpack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
                Save.ItemPacks.Clear();
                SaveLoadSystem.Save("", WorldName);
                for (int i = 0; i < backpack.Storage.Count; ++i)
                {
                    ItemInventorySave invSave = new ItemInventorySave();
                    invSave.InventorySlot = backpack.Storage[i];
                    if (Save.ItemPacks.All(t => t.InventorySlot.ID != backpack.Storage[i].ID))
                        Save.ItemPacks.Add(invSave);
                }
                for (int i = 0; i < backpack.Display.CombindSlots.Count; ++i)
                {
                    for (int a = 0; a < Save.ItemPacks.Count; ++a)
                    {
                        if (backpack.Display.CombindSlots[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile != null)
                        {
                            if (backpack.Display.CombindSlots[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.ItemID == Save.ItemPacks[a].InventorySlot.ID)
                            {
                                Save.ItemPacks[a].SlotNumber = backpack.Display.CombindSlots[i].transform.GetChild(0).GetComponent<HoldCustomTile>().SlotID;
                            }
                        }
                    }
                }
                //Clears the json file incase the backpack amount doesn't match
                if (Save.ItemPacks.Count != backpack.Storage.Count)
                {
                    Save.ItemPacks.Clear();
                    SaveLoadSystem.Save("", WorldName);
                }
                for (int i = 0; i < backpack.Storage.Count; ++i)
                {
                    ItemInventorySave invSave = new ItemInventorySave();
                    invSave.InventorySlot = backpack.Storage[i];
                    if (Save.ItemPacks.All(t => t.InventorySlot.ID != backpack.Storage[i].ID))
                        Save.ItemPacks.Add(invSave);
                }
                SceenshotTaker.TakeScreenShot_static(256, 256);
                Save.PlayerPosition = GameObject.Find("Player").transform.position;
                Save.DigPower = GameObject.Find("Player").GetComponent<DigTier>().CurrentDigTier;
                string saveString = JsonUtility.ToJson(Save, true);
                SaveLoadSystem.Save(saveString, WorldName);
                if (Save.ModeName == "FreeMode")
                    GameObject.Find("GameManager").GetComponent<GameManager>().FreeMode = true;
                if (Save.ModeName == "ScoreMode")
                    GameObject.Find("GameManager").GetComponent<GameManager>().ScoreMode = true;
                if (Save.ModeName == "ExitMode")
                    GameObject.Find("GameManager").GetComponent<GameManager>().ExitMode = true;
            }
        }
    }
    public void LoadJson()
    {
        string LoadString = SaveLoadSystem.Load(WorldName);
        if (LoadString != null)
        {
            Save = JsonUtility.FromJson<SaveFile>(LoadString);
            Random.InitState(Save.Seed);
            GameObject clone = Instantiate(Dungeon);
            clone.transform.position = new Vector3(0, 0, 0);
            clone.transform.SetParent(transform);
            SquareRoomAmount = Save.SquareRoomAmount;
            CircleRoomAmount = Save.CircleRoomAmount;
            TShapeRoomAmount = Save.TShapeRoomAmount;
            LShapeRoomAmount = Save.LShapeRoomAmount;
            DiamondRoomAmount = Save.DiamondRoomAmount;
            XDimension = Save.XDimension;
            YDimension = Save.YDimension;
            if(Save.DigPower > 1)
           GameObject.Find("Player").GetComponent<DigTier>().CurrentDigTier = Save.DigPower;
            clone.transform.GetChild(0).GetComponent<BuildDungeon>().Build();
            InventoryBackpack backpack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
            for (int i = 0; i < Save.ItemPacks.Count; ++i)
            {
                if (backpack.Storage.All(t => t.ID != Save.ItemPacks[i].InventorySlot.ID))
                    backpack.Storage.Add(Save.ItemPacks[i].InventorySlot);
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
            TilesToSave = Save.DataPacks;
            TilePlacer(Save, clone.transform.GetChild(0).GetComponent<Tilemap>(), clone.transform.GetChild(1).GetComponent<Tilemap>());
            GameObject.Find("Player").transform.position = Save.PlayerPosition;
            GameObject.Find("Player").GetComponent<PlayerMovement>().SetPlayerPlaced(true);
            if (Save.ModeName == "FreeMode")
                GameObject.Find("GameManager").GetComponent<GameManager>().FreeMode = true;
            if (Save.ModeName == "ScoreMode")
                GameObject.Find("GameManager").GetComponent<GameManager>().ScoreMode = true;
            if (Save.ModeName == "ExitMode")
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
                GameObject.Find("Player").GetComponent<PlayerMovement>().SetPlayerPlaced(true);
                FloorGen.GetFloorPositions().Remove(position);
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
                        TileManager.GetTileDictionaryWalls().Remove(tempPosI);
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
}

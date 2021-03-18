using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    public List<DataToSave> DataPacks = new List<DataToSave>();
    public int Seed;
    public bool SeedSet;
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
    private void Awake()
    {
        SavePath = Application.persistentDataPath + "/save.dat";
        Save.DataPacks = new List<DataToSave>();
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        LoadFromDisk();

    }
    public void Input(DataToSave _item)
    {
        if (Save.SeedSet)
        {
                TilesToSave.Add(_item);

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
            Debug.Log("Loading");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SavePath, FileMode.Open);
            Save = (SaveFile)bf.Deserialize(file);
            file.Close();
            Debug.Log("Seed: " + Save.Seed);
            if (Save.Seed == 0)
            {
                Random.InitState(12432523);
            }
            else
            {
                Random.InitState(Save.Seed);
            }
            for (int i = 0; i < Save.DataPacks.Count; ++i)
                AddChangedTiles(i);

        }
    }
    public void AddChangedTiles(int _index)
    {
        DataToSave temp = new DataToSave();
        temp.PosX = Save.DataPacks[_index].PosX;
        temp.PosY = Save.DataPacks[_index].PosY;
        temp.PosZ = Save.DataPacks[_index].PosZ;
        temp.IsNull = Save.DataPacks[_index].IsNull;
        temp.ID = Save.DataPacks[_index].ID;
        temp.IsPlacedTile = Save.DataPacks[_index].IsPlacedTile;
        TilesToLoad.Add(temp);

    }
    public void TilePlacer()
    {
        for (int i = 0; i < TilesToLoad.Count; ++i)
        {
            Vector3Int tempPosI = new Vector3Int(TilesToLoad[i].PosX, TilesToLoad[i].PosY, TilesToLoad[i].PosZ);
            for (int wall = 0; wall < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++wall)
            {
                if (TilesToLoad[i].ID == TileManager.GetTileHolder(TileType.Wall).Tiles[wall].ID)
                {
                    if(!TilesToLoad[i].IsNull)
                    TileManager.PlaceTile(tempPosI, 0, WallGen.GetTilemap(), WallGen.GetTilemap(), TileManager.GetTileHolder(TileType.Wall).Tiles[wall], DictionaryType.Walls);
                    else
                    {
                        TileManager.RemoveTilePiece(tempPosI, WallGen.GetTilemap());
                    }
                }
            }
            for (int floor = 0; floor < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++floor)
            {
                if (TilesToLoad[i].ID == TileManager.GetTileHolder(TileType.Floor).Tiles[floor].ID)
                {
                    TileManager.PlaceTile(tempPosI, 0, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Floor).Tiles[floor], DictionaryType.Floor);
                    if (TilesToLoad[i].IsPlacedTile)
                    {
                        if (!PlacedOnTiles.ContainsKey(tempPosI))
                            PlacedOnTiles.Add(tempPosI, TileManager.GetTileHolder(TileType.Floor).Tiles[floor]);
                    }
                }
            }
            for (int path = 0; path < TileManager.GetTileHolder(TileType.Path).Tiles.Count; ++path)
            {
                if (TilesToLoad[i].ID == TileManager.GetTileHolder(TileType.Path).Tiles[path].ID)
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

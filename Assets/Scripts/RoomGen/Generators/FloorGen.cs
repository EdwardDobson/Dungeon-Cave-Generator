using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DungeonGeneration
{
    public class FloorGen
    {
        static List<Vector3Int> m_floorPositions = new List<Vector3Int>();
        public static void FillFloor()
        {
            TileHolder tileHolder = TileManager.GetTileHolder(TileType.Floor);
            Vector3Int temp = new Vector3Int();
   
            for (int i = 0; i < WallGen.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < WallGen.GetWallDimensions().y; ++a)
                {
                    float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                    List<CustomTile> tilesWithinRange = new List<CustomTile>();
                    tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                    int tempTileIndex;

                    TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, Random.Range(0, tilesWithinRange.Count), false, TileType.Floor, DungeonUtility.GetTilemap());
                    tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                    TileManager.FillDictionary(new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0), tilesWithinRange, tempTileIndex, DungeonUtility.GetTilemap());

                    TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0), tilesWithinRange[tempTileIndex]);
                    temp = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0);
                    if (DungeonUtility.GetTilemap().GetTile(temp).name.Contains("Floor"))
                        m_floorPositions.Add(temp);
                }
            }
        }
        public static List<Vector3Int> GetFloorPositions()
        {
            return m_floorPositions;
        }
        public static void AddFloorPositions(Vector3Int _pos)
        {
            m_floorPositions.Add(_pos);
        }
    }
}

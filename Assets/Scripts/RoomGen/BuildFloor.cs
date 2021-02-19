using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildFloor
    {
        public static void FillFloor()
        {
            TileHolder tileHolder = TileManager.GetTileHolder(TileType.Floor);
            Vector3Int temp = new Vector3Int();
   
            for (int i = 0; i < DungeonUtility.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < DungeonUtility.GetWallDimensions().y; ++a)
                {
                    float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                    List<CustomTile> tilesWithinRange = new List<CustomTile>();
                    tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                    int tempTileIndex;

                    TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, Random.Range(0, tilesWithinRange.Count), false, TileType.Floor, DungeonUtility.GetTilemap());
                    tempTileIndex = Random.Range(0, tilesWithinRange.Count);

                    Vector3Int pos = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0);
                    TileData td = new TileData();
                    td.CustomTile = tilesWithinRange[tempTileIndex];
                    td.TileBase = DungeonUtility.GetTilemap().GetTile(pos);
                    if(!TileManager.GetTileDictionary().ContainsKey(pos))
                    TileManager.FillDictionary(pos, td);

                    TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0), tilesWithinRange[tempTileIndex]);
                    temp = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 0);
                    if (DungeonUtility.GetTilemap().GetTile(temp).name.Contains("Floor"))
                        DungeonUtility.AddFloorPositions(temp);
                }
            }
        }
    }
}

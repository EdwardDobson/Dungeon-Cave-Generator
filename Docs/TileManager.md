# Overview

[Current Features](./Features.md) 

The Tile Manager handles all of the tile loading and placing.

### Tile Placing

There are two separate tile maps one of the walls and the other for the floor.
The walls tilemap has collision this helps with player interaction.

### Tile Dictionaries

There are two dictionaries that store a tiles position within the tilemap
and it's custom tile.
<pre><code>
public static void FillDictionary(Vector3Int _pos, CustomTile _customTile, Tilemap _map, DictionaryType _dirType)
        {
            TileData td = new TileData();
            td.CustomTile = _customTile;
            td.TileBase = _map.GetTile(_pos);
            switch (_dirType)
            {
                case DictionaryType.Walls:
                    if (!m_tileDatasWalls.ContainsKey(_pos))
                        m_tileDatasWalls.Add(_pos, td);
                    break;
                case DictionaryType.Floor:
                    if (!m_tileDatasFloor.ContainsKey(_pos))
                        m_tileDatasFloor.Add(_pos, td);
                    break;
            }
        }
<pre><code>
### Tile Loading

By using the resources folder I load in all of the tiles that I have created.
These then get organised into lists based on their type.


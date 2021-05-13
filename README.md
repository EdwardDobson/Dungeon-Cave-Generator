# MCompProjectGit
DUNGEON GENERATOR

This project was developed for my final masters project to answer the question,  “Can you create a random generated dungeon that is 
interactable and easily expanded?”

This project is built using C# and the Unity Game Engine.

All of the assets and scripts were created by me.

GENERATION

Both the walls and the room generation use Unity Tilemaps this allows for easy tile creation. I expanded this tile system to allow for custom varibles. This allows for custom varibles for tile health and speed.

SAVING

To save all of the tile data needed I use a combination of scriptable objects and JSON. The scriptable objects allow me to make all of the different tile types found throughout the project. The JSON is used to save all of the dungeon values such as size and room amounts, as well as all modifed/placed tiles.

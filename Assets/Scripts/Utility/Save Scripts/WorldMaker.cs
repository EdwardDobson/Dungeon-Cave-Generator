using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public int SquareRoomAmount;
    public int CircleRoomAmount;
    public int TShapeRoomAmount;
    public int LShapeRoomAmount;
    public int DiamondRoomAmount;
    public int DungeonSizeX;
    public int DungeonSizeY;
    public Slider[] RoomAmountSliders;
    private void Start()
    {
        SeedField.characterLimit = 9;
        // WorldNameFile = (TextAsset)Resources.Load("WorldNames");
        //   m_worldNames = WorldNameFile.text.Split('\n');
        RoomAmountSliders[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Square Room Amount\n" + SquareRoomAmount;
        RoomAmountSliders[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Circle Room Amount\n" + CircleRoomAmount;
        RoomAmountSliders[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "TShape Room Amount\n" + TShapeRoomAmount;
        RoomAmountSliders[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "LShape Room Amount\n" + LShapeRoomAmount;
        RoomAmountSliders[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Diamond Room Amount\n" + DiamondRoomAmount;
    }
    public void ClearVariables()
    {
        WorldName = "";
        Seed = 0;
         SquareRoomAmount = 0;
        CircleRoomAmount = 0;
        TShapeRoomAmount = 0;
        LShapeRoomAmount = 0;
        DiamondRoomAmount = 0;
        DungeonSizeX = 0;
        DungeonSizeY = 0;
        RoomAmountSliders[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Square Room Amount\n" + SquareRoomAmount;
        RoomAmountSliders[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Circle Room Amount\n" + CircleRoomAmount;
        RoomAmountSliders[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "TShape Room Amount\n" + TShapeRoomAmount;
        RoomAmountSliders[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "LShape Room Amount\n" + LShapeRoomAmount;
        RoomAmountSliders[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Diamond Room Amount\n" + DiamondRoomAmount;
        RoomAmountSliders[5].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Dungeon X Value\n" + DungeonSizeX;
        RoomAmountSliders[6].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Dungeon Y Value\n" + DungeonSizeY;
        foreach (Slider s in RoomAmountSliders)
        {
            s.value = 0;
        }
    }
    public void SetRoomAmount(int _index)
    {
        switch(_index)
        {
            case 0:
                SquareRoomAmount = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Square Room Amount\n" + SquareRoomAmount;
                break;
            case 1:
                CircleRoomAmount = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Circle Room Amount\n" + CircleRoomAmount;
                break;
            case 2:
                TShapeRoomAmount = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "TShape Room Amount\n" + TShapeRoomAmount;
                break;
            case 3:
                LShapeRoomAmount = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "LShape Room Amount\n" + LShapeRoomAmount;
                break;
            case 4:
                DiamondRoomAmount = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Diamond Room Amount\n" + DiamondRoomAmount;
                break;
            case 5:
                DungeonSizeX = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Dungeon X Value\n" + DungeonSizeX;
                break;
            case 6:
                DungeonSizeY = (int)RoomAmountSliders[_index].value;
                RoomAmountSliders[_index].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Dungeon Y Value\n" + DungeonSizeY;
                break;
        }

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
        LevelLoad m_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        m_levelLoader.Seed = Seed;
        m_levelLoader.WorldName = WorldName;
        if(SquareRoomAmount == 0 && CircleRoomAmount == 0 && TShapeRoomAmount == 0 && LShapeRoomAmount == 0 && DiamondRoomAmount ==0)
        {
            SquareRoomAmount = 25;
        }
        if(DungeonSizeX == 0 && DungeonSizeY == 0)
        {
            DungeonSizeX = 150;
            DungeonSizeY = 150;
        }
        m_levelLoader.SquareRoomAmount = SquareRoomAmount;
        m_levelLoader.CircleRoomAmount = CircleRoomAmount;
        m_levelLoader.TShapeRoomAmount = TShapeRoomAmount;
        m_levelLoader.LShapeRoomAmount = LShapeRoomAmount;
        m_levelLoader.LShapeRoomAmount = DiamondRoomAmount;
        m_levelLoader.DungeonSizeX = DungeonSizeX;
        m_levelLoader.DungeonSizeY = DungeonSizeY;
}
}

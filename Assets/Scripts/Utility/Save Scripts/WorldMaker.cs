using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject.Find("LevelLoader").GetComponent<LevelLoad>().Seed = Seed;
        GameObject.Find("LevelLoader").GetComponent<LevelLoad>().WorldName = WorldName;
    }
}

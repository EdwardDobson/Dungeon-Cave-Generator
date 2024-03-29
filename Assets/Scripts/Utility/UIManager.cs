using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    bool m_paused;
    public Button MainMenu;
    LevelLoad m_levelLoad;
    FileManager m_fileManager;
    Minimap m_minimap;
    void Start()
    {
        if(GameObject.Find("LevelLoader") != null)
        m_levelLoad = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        m_fileManager = GameObject.Find("SaveHolder").GetComponent<FileManager>();
        m_minimap = GameObject.Find("HUD").GetComponent<Minimap>();
        MainMenu.onClick.AddListener(LevelLoad);
    }
    void LevelLoad()
    {
        FloorGen.GetFloorPositions().Clear();
        FloorGen.GetFloorTilePositions().Clear();
        m_fileManager.SaveJson();
        m_levelLoad.LoadLevel(0);
        m_levelLoad.FreeMode = false;
        m_levelLoad.ScoreMode = false;
        m_levelLoad.ExitMode = false;
        m_levelLoad.Seed = 0;
        m_levelLoad.WorldName = "";
        Time.timeScale = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!m_paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    void Pause()
    {
        Time.timeScale = 0;
        m_paused = true;
        transform.GetChild(0).gameObject.SetActive(true);
        m_minimap.ShowSmallMap();
    }
   public void Resume()
    {
        Time.timeScale = 1;
        m_paused = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public bool GetPausedState()
    {
        return m_paused;
    }
}

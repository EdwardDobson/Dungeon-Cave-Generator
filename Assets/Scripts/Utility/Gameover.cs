using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    public Button Restart;
    public Button MainMenu;
    public Button Quit;
    LevelLoad m_levelLoad;
    GameManager m_manager;
    void Start()
    {
        m_levelLoad = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Restart.onClick.AddListener(ReloadLevel);
        Quit.onClick.AddListener(QuitGame);
        MainMenu.onClick.AddListener(LoadMainMenu);
    }
    void ReloadLevel()
    {
        Time.timeScale = 1;
        FloorGen.GetFloorPositions().Clear();
        FloorGen.GetFloorTilePositions().Clear();
        m_levelLoad.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        if(m_manager.ScoreMode)
        {
            m_manager.Player.GetComponent<Scoring>().CurrentScore = 0;
        }
        if (m_manager.ExitMode)
        {
            m_manager.Player.transform.position = new Vector3(0, 0, 0);
        }
    }
    void LoadMainMenu()
    {
        m_levelLoad.LoadLevel(0);
        FloorGen.GetFloorPositions().Clear();
        FloorGen.GetFloorTilePositions().Clear();
        Time.timeScale = 1;
    }
    void QuitGame()
    {
        Application.Quit();
    }
}

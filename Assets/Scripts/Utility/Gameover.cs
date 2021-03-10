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
    void Start()
    {
        m_levelLoad = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        Restart.onClick.AddListener(ReloadLevel);
        Quit.onClick.AddListener(QuitGame);
        MainMenu.onClick.AddListener(LoadMainMenu);
        Time.timeScale = 0;
    }
    void ReloadLevel()
    {
        m_levelLoad.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        FloorGen.GetFloorPositions().Clear();
        FloorGen.GetFloorTilePositions().Clear();
        Time.timeScale = 1;
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

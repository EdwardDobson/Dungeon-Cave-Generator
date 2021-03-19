using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void SetGameMode(string _mode)
    {
        LevelLoad levelLoad = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
        levelLoad.LoadLevel(1);
        Time.timeScale = 1;
        switch (_mode)
        {
            case "FreeMode":
                levelLoad.FreeMode = true;
                levelLoad.ScoreMode = false;
                levelLoad.ExitMode = false;
                levelLoad.ModeName = "FreeMode";
                break;
            case "ScoreMode":
                levelLoad.ScoreMode = true;
                levelLoad.FreeMode = false;
                levelLoad.ExitMode = false;
                levelLoad.ModeName = "ScoreMode";
                break;
            case "ExitMode":
                levelLoad.ExitMode = true;
                levelLoad.ScoreMode = false;
                levelLoad.FreeMode = false;
                levelLoad.ModeName = "ExitMode";
                break;
        }
    }
}

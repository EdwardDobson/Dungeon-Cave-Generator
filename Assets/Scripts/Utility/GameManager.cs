using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool Creative;
    public bool FindMode;
    public GameObject CreativeCanvas;
    public GameObject SurvivalCanvas;
    public GameObject ControlsInfoObj;
    public GameObject WinScreen;
    public Button Button;
    public Button FindModeButton;
    public float Timer;
    public bool SetTimer;

    public bool FreeMode;
    public bool ScoreMode;
    public bool ExitMode;

    public GameObject Player;
    public GameObject EndPoint;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI PlayerPosText;
    public TextMeshProUGUI EndPosText;
    public TextMeshProUGUI ScoreNeededText;
    BuildDungeon m_dungeon;
    [SerializeField]
    int m_totalScoreNeeded;
    [SerializeField]
    int m_totalScore;
    public DamagedTiles DamagedTiles;
    private void Start()
    {
        if (Creative)
        {
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Survival";
        }
        else
        {
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Creative";
        }
        m_dungeon = GameObject.Find("Map").GetComponent<BuildDungeon>();
        if(GameObject.Find("LevelLoader")!= null)
        {
            LevelLoad temp = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
             FreeMode = temp.FreeMode;
            ScoreMode = temp.ScoreMode;
            ExitMode = temp.ExitMode;
        }
        DisableObjs();
        SetTimer = false;
    }
    void DisableObjs()
    {
        if(FreeMode)
        {
            TimerText.transform.parent.gameObject.SetActive(false);
            GameObject.Find("ScoreBackground").gameObject.SetActive(false);
            EndPosText.transform.parent.gameObject.SetActive(false);
            ScoreNeededText.transform.parent.gameObject.SetActive(false);
        }
        if(ScoreMode)
        {
            TimerText.transform.parent.gameObject.SetActive(true);
            GameObject.Find("ScoreBackground").gameObject.SetActive(true);
            EndPosText.transform.parent.gameObject.SetActive(false);
            Button.gameObject.SetActive(false);
            ScoreNeededText.transform.parent.gameObject.SetActive(true);
        }
        if(ExitMode)
        {
            TimerText.transform.parent.gameObject.SetActive(true);
            GameObject.Find("ScoreBackground").gameObject.SetActive(false);
            Button.gameObject.SetActive(false);
            ScoreNeededText.transform.parent.gameObject.SetActive(false);
        }

    }
    public void SetCreative()
    {
        if(Creative)
        {
            Creative = false;
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Survival";
        }
        else
        {
            Creative = true;
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Creative";
        }
    }
    public void WinState()
    {
        if(m_dungeon.ScoresPlaced)
        {
            for (int i = 0; i < m_dungeon.Scores.Count; ++i)
            {
                m_totalScore += m_dungeon.Scores[i].GetComponent<ScorePoint>().ScoreWorth;
            }
            m_dungeon.ScoresPlaced = false;
        }

        m_totalScoreNeeded = m_totalScore / 4;
        ScoreNeededText.text = "Score Needed: " + m_totalScoreNeeded;
        if (Player.GetComponent<Scoring>().CurrentScore >= m_totalScoreNeeded)
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ExitWin()
    {
        Vector3Int PlayerPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y,0);
        Vector3Int EndPos = new Vector3Int((int)EndPoint.transform.position.x, (int)EndPoint.transform.position.y, 0);
        if (PlayerPos == EndPos)
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void SetFindMode()
    {
        if(FindMode)
        {
            FindMode = false;
            FindModeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Activate Find Mode";
        }
        else
        {
            FindMode = true;
            FindModeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deactivate Find Mode";
        }
    }
    private void Update()
    {
        DamagedTiles.RemoveDamagedTiles();
        if(ScoreMode || ExitMode)
        {
            if(ScoreMode)
            WinState();
            if (ExitMode)
                ExitWin();
            if (!SetTimer && Player.GetComponent<PlayerMovement>().GetPlayerPlaced() && EndPoint.GetComponent<EndGoal>().GetEndPointSet())
            {
                float distance = Vector2.Distance(Player.transform.position, EndPoint.transform.position);
                Timer = distance * 2;
                TimerText.text = "Time: " + Timer;
                SetTimer = true;
            }
            if (SetTimer)
            {
                Timer -= Time.deltaTime;
                TimerText.text = "Time: " + (int)Timer;
                if (Timer <= 0)
                {
                    Destroy(Player);
                    Time.timeScale = 0;
                    
                }
            }
        }
        if (EndPoint != null && Player != null)
        {
            PlayerPosText.text = "Player\nX: " + (int)Player.transform.position.x + "\nY: " + (int)Player.transform.position.y;
            EndPosText.text = "End Door\nX: " + (int)EndPoint.transform.position.x + "\nY: " + (int)EndPoint.transform.position.y;
        }

        if (Creative)
        {
            CreativeCanvas.SetActive(true);
            SurvivalCanvas.SetActive(false);
        }
        else
        {
            CreativeCanvas.SetActive(false);
            SurvivalCanvas.SetActive(true);
        }
    }
    public void SetFreeMode(bool _state)
    {
        FreeMode = _state;
    }
    public void HideControlsDescription()
    {
        if(ControlsInfoObj.activeSelf)
        {
            ControlsInfoObj.SetActive(false);
        }
        else
        {
            ControlsInfoObj.SetActive(true);
        }
    }
    public void ResetTime()
    {
        Time.timeScale = 1;
    }
}

using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool Creative;
    public bool FindMode;
    public GameObject CreativeCanvas;
    public GameObject SurvivalCanvas;
    public GameObject ControlsInfoObj;
    public GameObject WinScreen;
    public GameObject GameoverScreen;
    public GameObject ScoreBackground;

    public GameObject CreativeInventory;
    public GameObject SurvivalInventory;
    public bool CanPerformAction;

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
    public GameObject ScorePrefab;
    public List<GameObject> Scores;
    [SerializeField]
    int m_scoreAmount;
    bool m_scoresPlaced;
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

        if (GameObject.Find("LevelLoader") != null)
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
        if (FreeMode)
        {
            if (TimerText.transform.parent.gameObject.activeSelf)
                TimerText.transform.parent.gameObject.SetActive(false);
            if (ScoreBackground.activeSelf)
                ScoreBackground.SetActive(false);
            if (EndPosText.transform.parent.gameObject.activeSelf)
                EndPosText.transform.parent.gameObject.SetActive(false);
            if (ScoreNeededText.transform.parent.gameObject.activeSelf)
                ScoreNeededText.transform.parent.gameObject.SetActive(false);
        }
        if (ScoreMode)
        {
            if (!TimerText.transform.parent.gameObject.activeSelf)
                TimerText.transform.parent.gameObject.SetActive(true);
            if (!ScoreBackground.activeSelf)
                ScoreBackground.SetActive(true);
            if (EndPosText.transform.parent.gameObject.activeSelf)
                EndPosText.transform.parent.gameObject.SetActive(false);
            if (Button.gameObject.activeSelf)
                Button.gameObject.SetActive(false);
            if (!ScoreNeededText.transform.parent.gameObject.activeSelf)
                ScoreNeededText.transform.parent.gameObject.SetActive(true);
        }
        if (ExitMode)
        {
            if (!TimerText.transform.parent.gameObject.activeSelf)
                TimerText.transform.parent.gameObject.SetActive(true);
            if (ScoreBackground.activeSelf)
                ScoreBackground.SetActive(false);
            if (Button.gameObject.activeSelf)
                Button.gameObject.SetActive(false);
            if (ScoreNeededText.transform.parent.gameObject.activeSelf)
                ScoreNeededText.transform.parent.gameObject.SetActive(false);
        }

    }
    public void SetCreative()
    {
        if (Creative)
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
        if (m_dungeon == null)
            m_dungeon = GameObject.Find("Map").GetComponent<BuildDungeon>();
        //  m_fileManager.TileSetter();
        if (ScoreMode && !m_scoresPlaced)
        {
            m_scoreAmount = FloorGen.GetFloorPositions().Count / 4;
            for (int i = 0; i < m_scoreAmount; ++i)
            {
                if (FloorGen.GetFloorPositions().Count > 1)
                {
                    Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
                    Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                    FloorGen.GetFloorPositions().Remove(position);
                    GameObject scoreClone = Instantiate(ScorePrefab, positionReadjusted, Quaternion.identity, transform);
                    scoreClone.GetComponent<ScorePoint>().ScoreWorth = Random.Range(1, 25);
                    Scores.Add(scoreClone);
                }
            }
            for (int i = 0; i < Scores.Count; ++i)
            {
                m_totalScore += Scores[i].GetComponent<ScorePoint>().ScoreWorth;
            }
            m_scoresPlaced = true;
        }

        if (m_scoresPlaced)
        {
            m_totalScoreNeeded = m_totalScore / 2;
            ScoreNeededText.text = "Score Needed: " + m_totalScoreNeeded;
            if (Player.GetComponent<Scoring>().CurrentScore >= m_totalScoreNeeded)
            {
                WinScreen.SetActive(true);
            }
        }
    }
    public void ExitWin()
    {
        Vector3Int PlayerPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y, 0);
        Vector3Int EndPos = new Vector3Int((int)EndPoint.transform.position.x, (int)EndPoint.transform.position.y, 0);
        if (PlayerPos == EndPos)
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void SetFindMode()
    {
        if (FindMode)
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
        DisableObjs();
        DamagedTiles.RemoveDamagedTiles();
        CanPerformActionFunction();
        if (ScoreMode || ExitMode)
        {
            if (ScoreMode)
                WinState();
            if (ExitMode && Player.GetComponent<PlayerMovement>().GetPlayerPlaced() && EndPoint.GetComponent<EndGoal>().GetEndPointSet())
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
                if (Timer <= 0 && !WinScreen.activeSelf)
                {
                    Timer = 0;
                    GameoverScreen.SetActive(true);
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
        if (ControlsInfoObj.activeSelf)
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
    void CanPerformActionFunction()
    {
        if (!CreativeInventory.activeSelf && !SurvivalInventory.activeSelf)
        {
            CanPerformAction = true;
        }
        else
        {
            CanPerformAction = false;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}

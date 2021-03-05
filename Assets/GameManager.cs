using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool Creative;
    public bool FindMode;
    public GameObject CreativeCanvas;
    public GameObject SurvivalCanvas;
    public Button Button;
    public Button FindModeButton;
    public float Timer;
    public bool SetTimer;
    public GameObject Player;
    public GameObject EndPoint;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI PlayerPosText;
    public TextMeshProUGUI EndPosText;
    BuildDungeon m_dungeon;
    [SerializeField]
    int m_totalScoreNeeded;
    [SerializeField]
    int m_totalScore;
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
        if(Player.GetComponent<Scoring>().CurrentScore >= m_totalScoreNeeded)
        {
            Debug.Log("Player wins");
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
        WinState();
        if (!SetTimer && Player.GetComponent<PlayerMovement>().GetPlayerPlaced() && EndPoint.GetComponent<EndGoal>().GetEndPointSet())
        {
            float distance = Vector2.Distance(Player.transform.position, EndPoint.transform.position);
            Timer = distance * 2;
            TimerText.text = "Time: " + Timer;
            SetTimer = true;
        }
        if (EndPoint != null && Player != null)
        {
            PlayerPosText.text = "X: " +(int)Player.transform.position.x + "\nY: " + (int)Player.transform.position.y;
            EndPosText.text = "End Door\nX: " + (int)EndPoint.transform.position.x + "\nY: " + (int)EndPoint.transform.position.y;
        }
 
        if (SetTimer && FindMode)
        {
            Timer -= Time.deltaTime;
            TimerText.text = "Time: " + (int)Timer;
            if (Timer <= 0)
            {
                Destroy(Player);
                Time.timeScale = 0;
            }
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public float CurrentScore;
    public TextMeshProUGUI ScoreText;
    private void Start()
    {
        ScoreText.text = "Score: " + CurrentScore;
    }
    public void IncreaseScore(float _amount)
    {
        CurrentScore += _amount;
        ScoreText.text = "Score: " + CurrentScore;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Score"))
        {

            IncreaseScore(collision.GetComponent<ScorePoint>().ScoreWorth);
            ScoreText.text = "Score: " + CurrentScore;
            Destroy(collision.gameObject);
        }
    }
}

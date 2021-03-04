using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool Creative;
    public GameObject CreativeCanvas;
   public GameObject SurvivalCanvas;
    public Button Button;
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
    private void Update()
    {
        if(Creative)
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

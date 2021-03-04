using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool Creative;
    public GameObject CreativeCanvas;
   public GameObject SurvivalCanvas;
    public void SetCreative()
    {
        if(Creative)
        {
            Creative = false;
        }
        else
        Creative = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisplayScroll : MonoBehaviour
{
    public Scrollbar Bar;

    void Update()
    {
        if(Time.timeScale <= 0)
        {
            float d = Input.GetAxis("Mouse ScrollWheel");
            Bar.value += d;
            if (Bar.value <= 0)
                Bar.value = 0;
            if(Bar.value >= 1)
                Bar.value = 1;
        }
  
    }
}

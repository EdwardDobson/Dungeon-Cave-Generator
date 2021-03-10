using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject SmallMap;
    public GameObject LargeMap;
    bool m_mapOpen;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (m_mapOpen)
            {
                ShowSmallMap();
            }
            else
            {
                ShowLargeMap();
            }
        }
    }
    void ShowSmallMap()
    {
        m_mapOpen = false;
        SmallMap.gameObject.SetActive(true);
        LargeMap.gameObject.SetActive(false);
    }
    void ShowLargeMap()
    {
        m_mapOpen = true;
        LargeMap.gameObject.SetActive(true);
        SmallMap.gameObject.SetActive(false);
    }
}

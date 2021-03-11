using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject SmallMap;
    public GameObject LargeMap;
    public GameObject CreativeInventory;
    public GameObject Inventory;
    bool m_mapOpen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
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
        if (CreativeInventory.activeSelf || Inventory.activeSelf)
        {
            m_mapOpen = false;
            SmallMap.gameObject.SetActive(true);
            LargeMap.gameObject.SetActive(false);

        }
        if (!CreativeInventory.activeSelf && !Inventory.activeSelf)
        {
            Time.timeScale = 1;
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

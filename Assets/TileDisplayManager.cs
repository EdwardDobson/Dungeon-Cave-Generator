using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class TileDisplayManager : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image SlotPrefab;
    public PlaceTile PTile;
    public Transform Parent;
    public Transform StartPoint;
    public GameObject TextInfo;
    public Image TileImage;
    PlaceTile m_pTile;
    public CustomTile TileToSwitch;
    public GameObject TileDisplay;
    bool m_tileDisplayOpen;
    string m_damageInfo;
    string m_healthInfo;
    string m_speedInfo;
    void Start()
    {
        m_pTile = GameObject.Find("Player").GetComponent<PlaceTile>();
        for (int i = 0; i < PTile.GetCustomTiles().Count; ++i)
        {
            GameObject temp = Instantiate(SlotPrefab.gameObject);
            temp.transform.SetParent(Parent);
  
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<HoldCustomTile>().CustomTile = PTile.GetCustomTiles()[i];
            temp.GetComponent<Image>().color = PTile.GetCustomTiles()[i].TileColour;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_tileDisplayOpen)
            {
                TileDisplay.SetActive(true);
                m_tileDisplayOpen = true;
                Time.timeScale = 0;
            }
            else
            {
                TileDisplay.SetActive(false);
                m_tileDisplayOpen = false;
                Time.timeScale = 1;
            }
               
        }
        if(TileToSwitch != null)
        {
            if(Input.GetMouseButtonDown(1))
            {
                TileToSwitch = null;
            }
            TileImage.transform.position = Input.mousePosition;
        }
        else
        {
            TileImage.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData _data)
    {
        if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>() != null && _data.pointerCurrentRaycast.gameObject.name.Contains("SlotPrefab") )
        {
            TileToSwitch = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            TileImage.gameObject.SetActive(true);
            TileImage.GetComponent<Image>().color = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile.TileColour;
        }
        if (_data.pointerCurrentRaycast.gameObject.name.Contains("Slot "))
        {
            if(TileToSwitch != null && _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != TileToSwitch)
            {
                _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile = TileToSwitch;
                m_pTile.RefreshTileImages();
                TileToSwitch = null;
                TileImage.gameObject.SetActive(false);
            }
        }
    }
    public void OnPointerEnter(PointerEventData _data)
    {
        if(_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>() != null)
        {
            HoldCustomTile hCustomTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>();
            TextInfo.SetActive(true);
            TextInfo.transform.position = new Vector3(_data.pointerCurrentRaycast.gameObject.transform.position.x+175, _data.pointerCurrentRaycast.gameObject.transform.position.y,0);
            string defaultInfo = hCustomTile.CustomTile.name + "\nType: " +
               hCustomTile.CustomTile.Type.ToString() + "\n";
            if (hCustomTile.CustomTile.Damage > 0)
            {
                m_damageInfo = "Damage: " + hCustomTile.CustomTile.Damage.ToString() + "\n";
            }
            else
                m_damageInfo =null;
            if (hCustomTile.CustomTile.Speed > 0)
            {
                m_speedInfo = "Speed: " + hCustomTile.CustomTile.Speed.ToString() + "\n";
            }
            else
                m_speedInfo = null;
            if (hCustomTile.CustomTile.Health > 0)
            {
                m_healthInfo = "Health: " + hCustomTile.CustomTile.Health.ToString() +"\n";
            }
            else
                m_healthInfo = null;
            TextInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = defaultInfo + m_speedInfo + m_damageInfo + m_healthInfo;
        }
    }
    public void OnPointerExit(PointerEventData _data)
    {
        TextInfo.SetActive(false);
    }
}

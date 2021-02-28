using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class TileDisplayManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    TileType m_tempType;
    public TMP_InputField InputField;
    Vector3 m_scale = new Vector3(1, 1, 1);
    void Start()
    {
        m_pTile = GameObject.Find("Player").GetComponent<PlaceTile>();
        SwitchTab(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
                TileImage.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

        }
        if (TileToSwitch != null)
        {
            if (Input.GetMouseButtonDown(1))
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

    public void SwitchTab(int _index)
    {
        foreach (Transform g in Parent)
        {
            Destroy(g.gameObject);
        }
        switch (_index)
        {
            case 1:
                m_tempType = TileType.Wall;
                break;
            case 2:
                m_tempType = TileType.Floor;
                break;
            case 3:
                m_tempType = TileType.Door;
                break;
            case 4:
                m_tempType = TileType.Path;
                break;
        }
        if (_index > 0)
        {
            for (int i = 0; i < PTile.GetCustomTiles().Count; ++i)
            {
                if (PTile.GetCustomTiles()[i].Type == m_tempType)
                {
                    GameObject temp = Instantiate(SlotPrefab.gameObject);
                    temp.transform.SetParent(Parent);
                    temp.transform.localScale = m_scale;
                    temp.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = PTile.GetCustomTiles()[i];
                    temp.transform.GetChild(0).GetComponent<Image>().color = PTile.GetCustomTiles()[i].TileColour;
                    temp.transform.GetChild(0).GetComponent<Image>().sprite = PTile.GetCustomTiles()[i].DisplaySprite;
                }
            }
        }
        else if (_index <= 0)
        {
            for (int i = 0; i < PTile.GetCustomTiles().Count; ++i)
            {
                GameObject temp = Instantiate(SlotPrefab.gameObject);
                temp.transform.SetParent(Parent);
                temp.transform.localScale = m_scale;
                temp.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = PTile.GetCustomTiles()[i];
                temp.transform.GetChild(0).GetComponent<Image>().color = PTile.GetCustomTiles()[i].TileColour;
                temp.transform.GetChild(0).GetComponent<Image>().sprite = PTile.GetCustomTiles()[i].DisplaySprite;
            }
        }

    }
    public void Search()
    {
        string searchField = InputField.text;
        foreach (Transform g in Parent)
        {
            Destroy(g.gameObject);
        }
        for (int i = 0; i < PTile.GetCustomTiles().Count; ++i)
        {
            if (PTile.GetCustomTiles()[i].name.Contains(searchField) || PTile.GetCustomTiles()[i].Type.ToString().Contains(searchField))
            {
                GameObject temp = Instantiate(SlotPrefab.gameObject);
                temp.transform.SetParent(Parent);
                temp.transform.localScale = m_scale;
                temp.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = PTile.GetCustomTiles()[i];
                temp.transform.GetChild(0).GetComponent<Image>().color = PTile.GetCustomTiles()[i].TileColour;
                temp.transform.GetChild(0).GetComponent<Image>().sprite = PTile.GetCustomTiles()[i].DisplaySprite;
            }
            if(searchField != "")
            {
                for (int a = 0; a < PTile.GetCustomTiles()[i].Attributes.Length; ++a)
                {
                    if (PTile.GetCustomTiles()[i].Attributes[a].ToString().Contains(searchField))
                    {
                        GameObject temp = Instantiate(SlotPrefab.gameObject);
                        temp.transform.SetParent(Parent);
                        temp.transform.localScale = m_scale;
                        temp.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = PTile.GetCustomTiles()[i];
                        temp.transform.GetChild(0).GetComponent<Image>().color = PTile.GetCustomTiles()[i].TileColour;
                        temp.transform.GetChild(0).GetComponent<Image>().sprite = PTile.GetCustomTiles()[i].DisplaySprite;
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData _data)
    {
        if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>() != null && _data.pointerCurrentRaycast.gameObject.name.Contains("SlotPrefab"))
        {
            TileToSwitch = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            TileImage.gameObject.SetActive(true);
            TileImage.transform.GetChild(0).GetComponent<Image>().color = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile.TileColour;
            TileImage.transform.GetChild(0).GetComponent<Image>().sprite = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile.DisplaySprite;
        }
        if (_data.pointerCurrentRaycast.gameObject.name.Contains("Slot "))
        {
            if (TileToSwitch != null && _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != TileToSwitch)
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
        if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>() != null)
        {
            HoldCustomTile hCustomTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>();
            TextInfo.SetActive(true);
            float xPos = _data.pointerCurrentRaycast.gameObject.transform.parent.position.x + _data.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>().rect.width;
            Vector2 pos = new Vector2(xPos, _data.pointerCurrentRaycast.gameObject.transform.position.y);
            TextInfo.transform.position = pos;
            string defaultInfo = hCustomTile.CustomTile.name + "\nType: " +
               hCustomTile.CustomTile.Type.ToString() + "\n";
            if (hCustomTile.CustomTile.Damage > 0)
            {
                m_damageInfo = "Damage: " + hCustomTile.CustomTile.Damage.ToString() + "\n";
            }
            else
                m_damageInfo = null;
            if (hCustomTile.CustomTile.Speed > 0)
            {
                m_speedInfo = "Speed: " + hCustomTile.CustomTile.Speed.ToString() + "\n";
            }
            else
                m_speedInfo = null;
            if (hCustomTile.CustomTile.Health > 0)
            {
                m_healthInfo = "Health: " + hCustomTile.CustomTile.Health.ToString() + "\n";
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

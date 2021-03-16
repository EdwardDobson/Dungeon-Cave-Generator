using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public GameObject TileSlotHolder;
    public List<GameObject> Slots;
    public TextMeshProUGUI BlockInfo;
    public TextMeshProUGUI InventoryTitle;
    bool m_tileDisplayOpen;
    string m_damageInfo;
    string m_healthInfo;
    string m_speedInfo;
    TileType m_tempType;
    public TMP_InputField InputField;
    Vector3 m_scale = new Vector3(1, 1, 1);
    int m_index;
    [SerializeField]
    CustomTile m_tileToPlace;
    int m_tabIndex;
    UIManager m_manager;
    GameManager m_gameManager;
    void Start()
    {
        m_pTile = GameObject.Find("Player").GetComponent<PlaceTile>();
        m_manager = GameObject.Find("PauseMenu").GetComponent<UIManager>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if(!m_manager.GetPausedState())
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (TileDisplay.activeSelf)
                {
                    SwitchTab(m_tabIndex);
                    TileDisplay.SetActive(false);
                    Time.timeScale = 1;
                }
                else if (!TileDisplay.activeSelf)
                {
                    TileDisplay.SetActive(true);
                    SwitchTab(m_tabIndex);
                    Time.timeScale = 0;
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
            SwitchBlockWithInput();
            Scroll();
            HighlightSlot();
            m_tileToPlace = Slots[m_index].GetComponent<HoldCustomTile>().CustomTile;
            if(m_gameManager.CanPerformAction)
            PTile.PlaceTileClick(m_tileToPlace);
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
            case 0:
                m_tabIndex = 0;
                InventoryTitle.text = "Blocks";
                break;
            case 1:
                m_tempType = TileType.Wall;
                m_tabIndex = 1;
                InventoryTitle.text = "Walls";
                break;
            case 2:
                m_tempType = TileType.Floor;
                m_tabIndex = 2;
                InventoryTitle.text = "Floors";
                break;
            case 3:
                m_tempType = TileType.Door;
                m_tabIndex = 3;
                InventoryTitle.text = "Doors";
                break;
            case 4:
                m_tempType = TileType.Path;
                m_tabIndex = 4;
                InventoryTitle.text = "Paths";
                break;
        }
        if (_index > 0)
        {
            for (int i = 0; i < PTile.GetCustomTiles().Count; ++i)
            {
                if(_index < 5)
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
            TextInfo.SetActive(false);
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
            if(!_data.pointerCurrentRaycast.gameObject.name.Contains("Slot "))
            {
                HoldCustomTile hCustomTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>();
                TextInfo.SetActive(true);
                float xPos = _data.pointerCurrentRaycast.gameObject.transform.parent.position.x + _data.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>().rect.width;
                Vector2 pos = new Vector2(xPos, _data.pointerCurrentRaycast.gameObject.transform.position.y);
                TextInfo.transform.position = pos;
                string defaultInfo = hCustomTile.CustomTile.TileName + "\nType: " +
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
                TextInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = defaultInfo + "ID: " + hCustomTile.CustomTile.ID + "\n" + m_speedInfo + m_damageInfo + m_healthInfo;
            }
         
        }
    }
    public void OnPointerExit(PointerEventData _data)
    {
        TextInfo.SetActive(false);
    }
    void SwitchBlockWithInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_index = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_index = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_index = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_index = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            m_index = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            m_index = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            m_index = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            m_index = 8;
        }
    }
    void Scroll()
    {
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            m_index++;
            if (m_index > TileSlotHolder.transform.childCount - 1)
            {
                m_index = 0;
            }
        }
        else if (d < 0f)
        {
            m_index--;
            if (m_index < 0)
            {
                m_index = TileSlotHolder.transform.childCount - 1;
            }
        }
    }
    void HighlightSlot()
    {
        for(int i = 0;i < Slots.Count; ++i)
        {
            if(i == m_index)
            {
                Slots[i].GetComponent<Image>().color = Slots[i].GetComponent<HoldCustomTile>().CustomTile.TileColour;
                BlockInfo.GetComponent<TextMeshProUGUI>().text = Slots[i].GetComponent<HoldCustomTile>().CustomTile.TileName + "\nType: " + Slots[i].GetComponent<HoldCustomTile>().CustomTile.Type.ToString();
            }
            else
            {
                Slots[i].GetComponent<Image>().color = new Color(Slots[i].GetComponent<HoldCustomTile>().CustomTile.TileColour.r, Slots[i].GetComponent<HoldCustomTile>().CustomTile.TileColour.g, Slots[i].GetComponent<HoldCustomTile>().CustomTile.TileColour.b, 0.5f);
            }
        }
    }
}

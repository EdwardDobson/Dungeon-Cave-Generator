using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    InventoryBackpack m_inventoryBackPack;
    public GameObject SlotPrefab;

    Vector3 m_scale = new Vector3(1, 1, 1);
    public Transform Parent;

    [SerializeField]
    List<GameObject> Slots = new List<GameObject>();

    public HotBarScrolling HotBar;
    public GameObject StorageHolder;
    public Image TileImage;
    public CustomTile TileToSwitch;
    public CustomTile ChosenTile;
    public CustomTile TransitTile;
    public CustomTile EndTile;
    public CustomTile SwappingTile;
    public CustomTile ShiftSwapTile;
    [SerializeField]
    GameObject m_clickedObj;
    Color SlotColour = new Color(195, 195, 195);

    string m_damageInfo;
    string m_healthInfo;
    string m_speedInfo;
    public GameObject TextInfo;
    void Start()
    {
        m_inventoryBackPack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
        HotBar = GetComponent<HotBarScrolling>();
        int amountOfSlots = m_inventoryBackPack.StorageCapacity - HotBar.HotBarSize;
        for (int i = 0; i < amountOfSlots; ++i)
        {
            GameObject temp = Instantiate(SlotPrefab.gameObject);
            temp.transform.SetParent(Parent);
            temp.transform.localScale = m_scale;
            Slots.Add(temp);
        }
        StorageHolder.SetActive(false);

    }
    void Update()
    {
        if (TileImage.gameObject.activeSelf)
        {
            TileImage.transform.position = Input.mousePosition;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (StorageHolder.activeSelf)
            {
                StorageHolder.SetActive(false);
                Time.timeScale = 1;
            }
            else if (!StorageHolder.activeSelf)
            {
                StorageHolder.SetActive(true);
                Time.timeScale = 0;
            }
            DisplayCount();
        }
        if (StorageHolder.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                ShiftClickTile();
            }
        }
        if (!StorageHolder.activeSelf)
        {
            TextInfo.gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData _data)
    {
        if (_data.pointerCurrentRaycast.gameObject.name.Contains("SlotPrefab"))
        {
            SetEndTile(_data.pointerCurrentRaycast.gameObject);
            if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != null && TransitTile != null)
            {
                SwappingTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            }
           
            ShiftSwapTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            m_clickedObj = _data.pointerCurrentRaycast.gameObject;
            if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != null && _data.pointerCurrentRaycast.gameObject.transform.parent.parent.name != "Hotbar")
            {
                HoldCustomTile hCustomTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>();
                float xPos = _data.pointerCurrentRaycast.gameObject.transform.parent.position.x + _data.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>().rect.width;
                Vector2 pos = new Vector2(xPos, _data.pointerCurrentRaycast.gameObject.transform.position.y);
                TextPopup(hCustomTile.CustomTile, pos);
            }
             
        }

    }
    public void OnPointerExit(PointerEventData _data)
    {
        ShiftSwapTile = null;
        m_clickedObj = null;
        TextInfo.SetActive(false);
    }
    public void AddToSlot(CustomTile _tile)
    {
        if (HotBar.SlotsHotbar.All(i => i.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile != null))
        {
            for (int a = 0; a < Slots.Count; ++a)
            {
                if (Slots[a].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
                {
                    Slots[a].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = _tile;
                    Slots[a].transform.GetChild(0).GetComponent<Image>().sprite = _tile.DisplaySprite;
                    Slots[a].transform.GetChild(0).GetComponent<Image>().color = _tile.TileColour;
                    break;
                }
            }
        }
        for (int i = 0; i < HotBar.SlotsHotbar.Count; ++i)
        {
            if (HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
            {
                HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = _tile;
                HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<Image>().sprite = _tile.DisplaySprite;
                HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<Image>().color = _tile.TileColour;
                break;
            }

        }
    }
    void DisplayCount()
    {
        for (int i = 0; i < m_inventoryBackPack.Storage.Count; ++i)
        {
            for (int a = 0; a < m_inventoryBackPack.Storage[i].Items.Count; ++a)
            {
                for (int b = 0; b < Slots.Count; ++b)
                {
                    if (Slots[b].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == m_inventoryBackPack.Storage[i].Items[a])
                    {
                        Slots[b].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_inventoryBackPack.Storage[i].Items.Count.ToString();
                    }
                }
            }
        }
    }
    public void OnPointerClick(PointerEventData _data)
    {
        if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>() != null && _data.pointerCurrentRaycast.gameObject.name.Contains("SlotPrefab"))
        {
            SetTransitTile();
            if (TransitTile == null && _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != null)
            {
                PickTile(_data.pointerCurrentRaycast.gameObject);

            }
            if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile == null)
            {
                PlaceEndTile(_data.pointerCurrentRaycast.gameObject);
            }
            else
            {
                SwapTile(_data.pointerCurrentRaycast.gameObject);
            }
        }
        if (_data.pointerCurrentRaycast.gameObject.name.Contains("Bin"))
        {
            m_inventoryBackPack.ClearStorage(ChosenTile);
            ChosenTile = null;
            TileImage.gameObject.SetActive(false);
        }
    }
    void ShiftClickTile()
    {
        if (ShiftSwapTile != null)
        {
            m_clickedObj.GetComponent<Image>().sprite = null;
            m_clickedObj.GetComponent<HoldCustomTile>().CustomTile = null;
            m_clickedObj.GetComponent<Image>().color = SlotColour;

            if (m_clickedObj.transform.parent.parent.name.Contains("Content"))
            {
                for (int i = 0; i < HotBar.SlotsHotbar.Count; ++i)
                {
                    if (HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
                    {
                        HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = ShiftSwapTile;
                        HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<Image>().sprite = ShiftSwapTile.DisplaySprite;
                        HotBar.SlotsHotbar[i].transform.GetChild(0).GetComponent<Image>().color = ShiftSwapTile.TileColour;
                        HotBar.SlotsHotbar[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_clickedObj.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text;

                        ShiftSwapTile = null;
                        ChosenTile = null;
                        EndTile = null;
                        break;
                    }
                }
            }
            if (m_clickedObj.transform.parent.parent.name.Contains("Hotbar"))
            {
                for (int i = 0; i < Slots.Count; ++i)
                {
                    if (Slots[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
                    {
                        Slots[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = ShiftSwapTile;
                        Slots[i].transform.GetChild(0).GetComponent<Image>().sprite = ShiftSwapTile.DisplaySprite;
                        Slots[i].transform.GetChild(0).GetComponent<Image>().color = ShiftSwapTile.TileColour;
                        Slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_clickedObj.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                        ShiftSwapTile = null;
                        ChosenTile = null;
                        EndTile = null;
                        break;
                    }
                }
            }
            m_clickedObj.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
    }
    void PickTile(GameObject _chosenTile)
    {
        ChosenTile = _chosenTile.GetComponent<HoldCustomTile>().CustomTile;
        _chosenTile.GetComponent<HoldCustomTile>().CustomTile = null;
        _chosenTile.GetComponent<Image>().sprite = null;
        _chosenTile.GetComponent<Image>().color = SlotColour;
        _chosenTile.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

        TileImage.gameObject.SetActive(true);
        TileImage.transform.GetChild(0).GetComponent<Image>().sprite = ChosenTile.DisplaySprite;
        TileImage.transform.GetChild(0).GetComponent<Image>().color = ChosenTile.TileColour;
    }

    void SetTransitTile()
    {
        TransitTile = ChosenTile;
    }
    void SetEndTile(GameObject _exitTile)
    {
        EndTile = _exitTile.GetComponent<HoldCustomTile>().CustomTile;
    }
    void SwapTile(GameObject _otherTile)
    {
        ChosenTile = _otherTile.GetComponent<HoldCustomTile>().CustomTile;
        _otherTile.GetComponent<HoldCustomTile>().CustomTile = TransitTile;
        TileImage.transform.GetChild(0).GetComponent<Image>().sprite = ChosenTile.DisplaySprite;
        TileImage.transform.GetChild(0).GetComponent<Image>().color = ChosenTile.TileColour;
        _otherTile.GetComponent<Image>().color = _otherTile.GetComponent<HoldCustomTile>().CustomTile.TileColour;
        _otherTile.GetComponent<Image>().sprite = _otherTile.GetComponent<HoldCustomTile>().CustomTile.DisplaySprite;
        DisplayCount();
    }
    void PlaceEndTile(GameObject _finalTile)
    {
        if (TransitTile != null)
        {
            _finalTile.GetComponent<HoldCustomTile>().CustomTile = TransitTile;
            _finalTile.GetComponent<Image>().sprite = TransitTile.DisplaySprite;
            _finalTile.GetComponent<Image>().color = TransitTile.TileColour;
            for (int i = 0; i < m_inventoryBackPack.Storage.Count; ++i)
            {
                if (m_inventoryBackPack.Storage[i].Items.Contains(_finalTile.GetComponent<HoldCustomTile>().CustomTile))
                {
                    _finalTile.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_inventoryBackPack.Storage[i].Items.Count.ToString();
                }
            }
            TileImage.gameObject.SetActive(false);
            Vector2 pos = new Vector2(Input.mousePosition.x + 100, Input.mousePosition.y);
            TextPopup(TransitTile, pos);
               ShiftSwapTile = ChosenTile;
            TransitTile = null;
            ChosenTile = null;
        }
    }
    void TextPopup(CustomTile _data, Vector2 _pos)
    {
        TextInfo.SetActive(true);
  
        TextInfo.transform.position = _pos;
        string defaultInfo = _data.TileName + "\nType: " +
           _data.Type.ToString() + "\n";
        if (_data.Damage > 0)
        {
            m_damageInfo = "Damage: " + TransitTile.Damage.ToString() + "\n";
        }
        else
            m_damageInfo = null;
        if (_data.Speed > 0)
        {
            m_speedInfo = "Speed: " + _data.Speed.ToString() + "\n";
        }
        else
            m_speedInfo = null;
        if (_data.Health > 0)
        {
            m_healthInfo = "Health: " + _data.Health.ToString() + "\n";
        }
        else
            m_healthInfo = null;
        TextInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = defaultInfo + "ID: " + _data.ID + "\n" + m_speedInfo + m_damageInfo + m_healthInfo;
    }
}

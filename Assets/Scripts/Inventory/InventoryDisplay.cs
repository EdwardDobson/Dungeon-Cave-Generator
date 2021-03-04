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
    int m_slotFullindex;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StorageHolder.activeSelf)
                StorageHolder.SetActive(false);
            else StorageHolder.SetActive(true);
            DisplayCount();
        }
        if (StorageHolder.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                ShiftClickTile();
            }
        }

    }
    public void OnPointerEnter(PointerEventData _data)
    {
        if (_data.pointerCurrentRaycast.gameObject.name.Contains("SlotPrefab"))
        {
            SetEndTile(_data.pointerCurrentRaycast.gameObject);
            if (_data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile != null && TransitTile != null)
                SwappingTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            ShiftSwapTile = _data.pointerCurrentRaycast.gameObject.GetComponent<HoldCustomTile>().CustomTile;
            m_clickedObj = _data.pointerCurrentRaycast.gameObject;
        }

    }
    public void OnPointerExit(PointerEventData _data)
    {
        ShiftSwapTile = null;
        m_clickedObj = null;
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
            ShiftSwapTile = ChosenTile;
            TransitTile = null;
            ChosenTile = null;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HotBarScrolling : MonoBehaviour
{
    int m_index;
    public Transform HotBar;
    [SerializeField]
    List<GameObject> m_hotBarImages = new List<GameObject>();
    Color m_slotColour = new Color(195, 195, 195);
    [SerializeField]
    public List<GameObject> SlotsHotbar = new List<GameObject>();
    Vector3 m_scale = new Vector3(1, 1, 1);
    public Transform ParentHotBar;
    public GameObject SlotPrefabHotBar;
    public InventoryBackpack InventoryBackpack;
    public int HotBarSize;
    public TextMeshProUGUI BlockInfo;
    bool m_filledHotBar;
    void Start()
    {
   
      

    }
    void Update()
    {
        if (m_filledHotBar)
        {
            Scroll();
            HighLightSelected();
            SwitchBlockWithInput();
        }
     
    }
    public CustomTile TileToPlace()
    {
        return SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile;
    }
    void Scroll()
    {
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            m_index++;
            if (m_index > HotBar.childCount - 1)
            {
                m_index = 0;
            }
        }
        else if (d < 0f)
        {
            m_index--;
            if (m_index < 0)
            {
                m_index = HotBar.childCount - 1;
            }
        }
    }

    public void UpdateCountDisplay(int _count)
    {
        SlotsHotbar[m_index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _count.ToString();
        if (_count <= 0)
        {
            m_hotBarImages[m_index].GetComponent<Image>().color = new Color(m_slotColour.r, m_slotColour.g, m_slotColour.b, 1f);
            m_hotBarImages[m_index].GetComponent<Image>().sprite = null;
            SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile = null;
        }
    }
    void HighLightSelected()
    {
        if (SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile != null)
        {
            m_hotBarImages[m_index].GetComponent<Image>().color = SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.TileColour;
            if (SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.CanBePlaced)
                BlockInfo.GetComponent<TextMeshProUGUI>().text = SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.TileName + "\nType: " + SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Type.ToString();
            if (!SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.CanBePlaced)
            {
                string subName;
                if(SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.name.Contains("("))
                {
                    subName = SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.name.Substring(0, SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.name.IndexOf("("));
                }
                else
                {
                    subName = SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item.name;
                }
                BlockInfo.GetComponent<TextMeshProUGUI>().text = subName;
            }
            BlockInfo.transform.parent.gameObject.SetActive(true);
            for (int a = 0; a < InventoryBackpack.Storage.Count; ++a)
            {
                if (InventoryBackpack.Storage[a].Items.Contains(SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item))
                    SlotsHotbar[m_index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryBackpack.Storage[a].Items.Count.ToString();
            }
        }

        if (SlotsHotbar[m_index].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
        {
            m_hotBarImages[m_index].GetComponent<Image>().color = new Color(m_slotColour.r, m_slotColour.g, m_slotColour.b, 1f);
            SlotsHotbar[m_index].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            BlockInfo.GetComponent<TextMeshProUGUI>().text = "";
            BlockInfo.transform.parent.gameObject.SetActive(false);
        }
        for (int i = 0; i < m_hotBarImages.Count; ++i)
        {
            if (i != m_index)
            {
                if (SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile != null)
                {
                    m_hotBarImages[i].GetComponent<Image>().color = new Color(SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.TileColour.r, SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.TileColour.g, SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.TileColour.b, 0.5f);
                    for (int a = 0; a < InventoryBackpack.Storage.Count; ++a)
                    {
                        if (InventoryBackpack.Storage[a].Items.Contains(SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile.Item))
                            SlotsHotbar[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryBackpack.Storage[a].Items.Count.ToString();
                    }
                }
                if (SlotsHotbar[i].transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null)
                {
                    m_hotBarImages[i].GetComponent<Image>().color = new Color(m_slotColour.r, m_slotColour.g, m_slotColour.b, 0.5f);
                    SlotsHotbar[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }
    }
    public void FillHotBar()
    {
        m_filledHotBar = false;
        for (int i = 0; i < HotBarSize; ++i)
        {
            GameObject temp = Instantiate(SlotPrefabHotBar.gameObject);
            temp.transform.SetParent(ParentHotBar);
            temp.transform.localScale = m_scale;
            if(!SlotsHotbar.Contains(temp) && SlotsHotbar.Count < HotBarSize)
            SlotsHotbar.Add(temp);
        }
        for (int i = 0; i < HotBar.childCount; ++i)
        {
            m_hotBarImages.Add(HotBar.GetChild(i).GetChild(0).gameObject);
        }
        m_filledHotBar =true;
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigTier : MonoBehaviour
{
    Dig m_dig;
    public int CurrentDigTier;
    InventoryBackpack m_backpack;
    public int MaxAmountNeeded;
    public List<Item> TierItems;
    public TextMeshProUGUI UpgradeCost;
    public TextMeshProUGUI CurrentDigPower;
    public GameObject UpgradeButton;
    void Start()
    {
        m_dig = GetComponent<Dig>();
        m_backpack = GetComponent<InventoryBackpack>();
        for(int i = 0; i < CurrentDigTier -1; ++i)
        {
            TierItems.RemoveAt(0);
        }
        CurrentDigPower.text = "Dig Power: " + CurrentDigTier;
        if(TierItems.Count > 0)
        UpgradeCost.text = "Cost: " + MaxAmountNeeded + " " + TierItems[0].Name + "s";
        else
        {
            UpgradeCost.text = "MAX LEVEL REACHED";
            UpgradeButton.SetActive(false);
        }
    }
    void Update()
    {
        SetDigStrength();
    }
    public void UpDigTier()
    {
        if (TierItems.Count > 0)
        {
            for (int i = 0; i < m_backpack.Storage.Count; ++i)
            {
                for (int a = 0; a < m_backpack.Storage[i].Items.Count; ++a)
                {
                    if (m_backpack.Storage[i].Items[a].Name == TierItems[0].Name)
                    {
                        if (m_backpack.Storage[i].Items.Count >= MaxAmountNeeded)
                        {
                            CurrentDigTier++;

                            TierItems.RemoveAt(0);
                            m_backpack.RemoveMultipleItems(MaxAmountNeeded, m_backpack.Storage[i].Items);
                            m_backpack.Display.UpdateCountDisplaySlot();
                            CurrentDigPower.text = "Dig Power: " + CurrentDigTier;
                            if(TierItems.Count > 0)
                            UpgradeCost.text = "Cost: " + MaxAmountNeeded + " " + TierItems[0].Name + "s";
                            else
                            {
                                UpgradeCost.text = "MAX LEVEL REACHED";
                                UpgradeButton.SetActive(false);
                            }
                            break;
                        }
                    }
                }
            }
        }

    }
    public void SetDigStrength()
    {
        switch (CurrentDigTier)
        {
            case 1:
                m_dig.DigDamage = 2;
                break;
            case 2:
                m_dig.DigDamage = 3;
                break;
            case 3:
                m_dig.DigDamage = 4;
                break;
            case 4:
                m_dig.DigDamage = 5;
                break;
            case 5:
                m_dig.DigDamage = 6;
                break;
            case 6:
                m_dig.DigDamage = 7;
                break;
        }
    }
}

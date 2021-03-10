using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolSwitch : MonoBehaviour
{
    PlayerAttack m_pAttack;
    Dig m_dig;
    public Image ToolIcon;
    bool m_usingWeapon;
    public Sprite[] Icons;
    void Start()
    {
        m_pAttack = GetComponent<PlayerAttack>();
        m_dig = GetComponent<Dig>();
        ToolIcon.sprite = Icons[0];
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_usingWeapon)
                m_usingWeapon = false;
            else m_usingWeapon = true;
        }
        if (!m_usingWeapon)
        {
            m_dig.CanDig = true;
            m_pAttack.CanAttack = false;
            ToolIcon.sprite = Icons[0];
        }
        if (m_usingWeapon)
        {
            m_pAttack.CanAttack = true;
            m_dig.CanDig = false;
            ToolIcon.sprite = Icons[1];
        }
  
    }
}

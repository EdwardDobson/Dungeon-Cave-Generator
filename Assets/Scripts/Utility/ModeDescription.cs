using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModeDescription : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public GameObject DescriptionBox;
    TextMeshProUGUI m_descriptionText;
    public string[] Descriptions;
    public Transform[] DescriptionPositions;
    void Start()
    {
        m_descriptionText = DescriptionBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData _data)
    {
        if(_data.pointerCurrentRaycast.gameObject.transform.parent.name == "Free Mode")
        {
            DescriptionBox.SetActive(true);
            m_descriptionText.text = Descriptions[0];
            DescriptionBox.transform.position = DescriptionPositions[0].position;
        }
        if (_data.pointerCurrentRaycast.gameObject.transform.parent.name == "Exit Mode")
        {
            DescriptionBox.SetActive(true);
            m_descriptionText.text = Descriptions[1];
            DescriptionBox.transform.position = DescriptionPositions[1].position;
        }
        if (_data.pointerCurrentRaycast.gameObject.transform.parent.name == "Score Mode")
        {
            DescriptionBox.SetActive(true);
            m_descriptionText.text = Descriptions[2];
            DescriptionBox.transform.position = DescriptionPositions[2].position;
        }
    }
    public void OnPointerExit(PointerEventData _data)
    {
        DescriptionBox.SetActive(false);
        m_descriptionText.text = "";
    }
}

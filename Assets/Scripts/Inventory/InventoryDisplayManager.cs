using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(false);
            else transform.GetChild(0).gameObject.SetActive(true);

        }
    }
}

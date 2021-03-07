using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    bool m_paused;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!m_paused)
            {
                Pause();
            }
            else
            {
                Resume();
             
            }
        }
    }
    void Pause()
    {
        Time.timeScale = 0;
        m_paused = true;
    }
    void Resume()
    {
        Time.timeScale = 1;
        m_paused = false;
    }
}

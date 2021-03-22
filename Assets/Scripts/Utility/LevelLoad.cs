using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoad : MonoBehaviour
{
    static LevelLoad m_instance;
    public GameObject LoadingBar;
    public bool FreeMode;
    public bool ScoreMode;
    public bool ExitMode;
    public int Seed;
    public string WorldName;
    public string ModeName;
    private void Start()
    {
        DontDestroyOnLoad(this);

        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevel(int _index)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && LoadingBar == null)
        {
            LoadingBar = GameObject.Find("Loading Canvas").transform.GetChild(0).gameObject;
        }
        if (LoadingBar != null)
        {
            LoadingBar.SetActive(true);
        }
        StartCoroutine(LoadLevelAsync(_index));
    }
    IEnumerator LoadLevelAsync(int _index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_index);
        while (!asyncLoad.isDone)
        {
            if (LoadingBar != null)
            {
                LoadingBar.transform.GetChild(0).GetComponent<Slider>().value = asyncLoad.progress * 100;
                LoadingBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = asyncLoad.progress * 100 + " %";
            }
            yield return null;
        }
    }
    public void SetFreeMode(bool _state)
    {
        FreeMode = _state;
    }
    public void SetScoreMode(bool _state)
    {
        ScoreMode = _state;
    }
    public void SetExitMode(bool _state)
    {
        ExitMode = _state;
    }
}

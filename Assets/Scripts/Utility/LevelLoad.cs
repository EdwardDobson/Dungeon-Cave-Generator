using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoad : MonoBehaviour
{
    public GameObject LoadingBar;
    public bool FreeMode;
    public bool ScoreMode;
    public bool ExitMode;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadLevel()
    {
        LoadingBar.SetActive(true);
        StartCoroutine(LoadLevelAsync());
    }
    IEnumerator LoadLevelAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            LoadingBar.transform.GetChild(0).GetComponent<Slider>().value = asyncLoad.progress * 100;
            LoadingBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = asyncLoad.progress * 100 + " %";
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

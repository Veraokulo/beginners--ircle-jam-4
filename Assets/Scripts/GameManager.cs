using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Player Player;
    public Gravity Gravity;
    public Text keysInfo;

    public Text message;
    public Image filterImage;
    private Coroutine _filterCoroutine;
    private Coroutine _messageCoroutine;
    public GameObject GameOverMenu;
    public GameObject PauseMenu;
    public GameObject VictoryMenu;
    private bool isPaused;

    private void Start()
    {
        CheckIDs<Key>();
        CheckIDs<Gate>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void CheckIDs<T>() where T : LinkedID
    {
        var objects = FindObjectsOfType<T>();
        if (objects.Count(_ => _.id == 0) > 0)
            Debug.LogError(typeof(T).Name + " has no ID");
        if (objects.Select(_ => _.id).Distinct().Count() != objects.Length)
            Debug.LogError(typeof(T).Name + "s have duplicate value");
    }

    public void UpdateKeysInfo()
    {
        keysInfo.text = "Keys: " + string.Join(", ", GameManager.Instance.Player.Keys);
    }

    public void ShowMessage(string messageText, float time = 1f)
    {
        if (_messageCoroutine != null)
            StopCoroutine(_messageCoroutine);
        _messageCoroutine = StartCoroutine(ShowMessageCoroutine(messageText, time));
    }

    public IEnumerator ShowMessageCoroutine(string messageText, float time)
    {
        message.enabled = true;
        message.text = messageText;
        yield return new WaitForSeconds(time);
        message.enabled = false;
        yield return null;
    }

    public void SetColorFilter(Color color, float time = 0.1f)
    {
        if (_filterCoroutine != null)
            StopCoroutine(_filterCoroutine);
        _filterCoroutine = StartCoroutine(SetColorFilterCoroutine(color, time));
    }

    private IEnumerator SetColorFilterCoroutine(Color color, float time)
    {
        color.a = 0.1f;
        filterImage.color = color;
        filterImage.enabled = true;
        yield return new WaitForSeconds(time);
        filterImage.enabled = false;
        yield return null;
    }

    public void GameOver()
    {
        GameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void Victory()
    {
        VictoryMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
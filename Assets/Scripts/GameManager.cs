using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Text keysInfo;

    public Text message;
    public Image filterImage;
    private Coroutine _filterCoroutine;
    private Coroutine _messageCoroutine;


    private void Start()
    {
        CheckIDs<Key>();
        CheckIDs<Gate>();
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
        keysInfo.text = "Keys: " + string.Join(", ", Player.Instance.Keys);
    }

    public void ShowMessage(string messageText, float time = 1f)
    {
        if(_messageCoroutine!=null)
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
        if(_filterCoroutine!=null)
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
        throw new System.NotImplementedException();
    }
}
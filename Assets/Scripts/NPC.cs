using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected bool isPlayerNear;
    [TextArea] public string DialogText = "sample text";
    protected bool isDialogueEnd;
    protected Coroutine _dialogueDelayCoroutine;
    public float dialogueDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            if (!isDialogueEnd)
            {
                if (_dialogueDelayCoroutine == null)
                {
                    _dialogueDelayCoroutine = StartCoroutine(SetDialogueEndWithDelay());
                    GameManager.Instance.ShowMessage(DialogText, dialogueDuration);
                }
            }
            else
            {
                isPlayerNear = true;
            }
        }
    }

    private IEnumerator SetDialogueEndWithDelay()
    {
        yield return new WaitForSeconds(dialogueDuration);
        isDialogueEnd = true;
            isPlayerNear = true;
        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isPlayerNear = false;
        }
    }
}

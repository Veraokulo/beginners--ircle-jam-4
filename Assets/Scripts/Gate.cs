using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Gate : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public float openValue = 5f;
    public bool isOpened = false;
    private bool isPlayerNear = false;

    private void Start()
    {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerNear)
        {
            if (!isOpened)
            {
                Open();
            }
        }
    }

    private void Open()
    {
        left.transform.position -= left.transform.right * openValue;
        right.transform.position += right.transform.right * openValue;
        isOpened = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var q))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var q))
        {
            isPlayerNear = false;
        }
    }
}
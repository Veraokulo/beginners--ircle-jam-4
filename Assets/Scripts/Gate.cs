using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Gate : LinkedID
{
    public GameObject left;
    public GameObject right;
    public float openValue = 5f;
    public bool isOpened = false;
    private bool isPlayerNear = false;
    private Collider _colliderToDisable;
    private Player _player;

    private void Start()
    {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;
        _colliderToDisable = GetComponents<Collider>().First(_ => !_.isTrigger);
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerNear)
        {
            if (!isOpened)
            {
                if (_player.Keys.Contains(id))
                {
                    Open();
                    _player.Keys.Remove(id);
                    GameManager.Instance.UpdateKeysInfo();
                }
                else
                {
                    GameManager.Instance.ShowMessage("You do not have Key №" + id, 3f);
                }
            }
        }
    }

    private void Open()
    {
        left.transform.position -= left.transform.right * openValue;
        right.transform.position += right.transform.right * openValue;
        isOpened = true;
        _colliderToDisable.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isPlayerNear = false;
        }
    }
}
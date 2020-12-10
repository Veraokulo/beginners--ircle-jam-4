using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private bool _isPlayerInOpenSpace;
    [Min(0)] public float oxygenConsumptionPerSecond;

    void FixedUpdate()
    {
        if (_isPlayerInOpenSpace)
        {
            Player.Instance.TakeOxygen(oxygenConsumptionPerSecond * Time.fixedDeltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var q))
        {
            _isPlayerInOpenSpace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var q))
        {
            _isPlayerInOpenSpace = false;
        }
    }
}
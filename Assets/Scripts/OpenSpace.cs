using UnityEngine;

public class OpenSpace : MonoBehaviour
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
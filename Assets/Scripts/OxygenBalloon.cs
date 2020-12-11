using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OxygenBalloon : MonoBehaviour
{
    public float balloonValue = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            GameManager.Instance.Player.AddOxygen(balloonValue);
        }
    }
}
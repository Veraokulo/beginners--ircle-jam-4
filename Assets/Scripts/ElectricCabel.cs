using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ElectricCabel : MonoBehaviour
{
    public float DPS;
    private Collider _collider;
    private bool isIntersecting = false;


    private void FixedUpdate()
    {
        if (isIntersecting)
        {
            GameManager.Instance.Player.TakeDamage(DPS * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isIntersecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isIntersecting = false;
        }
    }
}
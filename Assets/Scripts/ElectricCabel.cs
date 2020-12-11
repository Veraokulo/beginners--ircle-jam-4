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
            Player.Instance.TakeDamage(DPS * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player.Instance.gameObject)
        {
            isIntersecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player.Instance.gameObject)
        {
            isIntersecting = false;
        }
    }
}
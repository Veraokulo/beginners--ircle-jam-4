using UnityEngine;

public class ElectricCabel : MonoBehaviour
{
    public float DPS;
    private Collider _collider;
    private Player _player;
    private bool isIntersecting = false;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (isIntersecting)
        {
            _player.TakeDamage(DPS * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(typeof(Player),out var q))
        {
            isIntersecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(typeof(Player),out var q))
        {
            isIntersecting = false;
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : LinkedID
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var q))
        {
            Player.Instance.Keys.Add(id);
            GameManager.Instance.UpdateKeysInfo();
            GameManager.Instance.ShowMessage("You picked up key № " + id);
            Destroy(gameObject);
        }
    }
}
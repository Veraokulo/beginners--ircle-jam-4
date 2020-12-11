using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : LinkedID
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            GameManager.Instance.Player.Keys.Add(id);
            GameManager.Instance.UpdateKeysInfo();
            GameManager.Instance.ShowMessage("You picked up key № " + id);
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class CameraFolower : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    void LateUpdate()
    {
        var playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, -50);
        var playerRotation = player.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerRotation.z));
    }
}
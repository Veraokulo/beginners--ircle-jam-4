using UnityEngine;

public class CameraFolower : MonoBehaviour
{
    public GameObject player;
    [Range(0,1)]
    public float smoothness = 0.5f;

    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    void FixedUpdate()
    {
        var playerPosition = player.transform.position;
        transform.position = 
            Vector3.Slerp(transform.position, new Vector3(playerPosition.x, playerPosition.y, -50),smoothness);
        var playerRotation = player.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(new Vector3(0, 0, playerRotation.z)),smoothness);
    }
}
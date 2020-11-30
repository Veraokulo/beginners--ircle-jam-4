using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraFolower : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void LateUpdate()
    {
        var playerPosition = player.transform.position;
        playerPosition.z = transform.position.z;
        transform.position = playerPosition;
    }
}

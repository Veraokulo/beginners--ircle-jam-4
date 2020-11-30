using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    public float horizontal;
    public float speed;
    public bool jump;


    public float vertical;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal")*speed;
        //jump = Input.GetKeyDown("Jump");
    }

    private void FixedUpdate()
    {
        var direction = new Vector3(horizontal, 0, 0);
        rb.AddForce(direction);
    }
}

using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed;
    public float jumpHeight;
    private float gravityValue = -9.81f;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        controller.Move(move * (Time.deltaTime * speed));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        
        if (Input.GetButton("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}

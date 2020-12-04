using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public enum GravityMode
    {
        None,
        FromCenter,
        ToCenter
    }

    private float _distanceToTheGround;

    private Rigidbody _rb;

    [Min(0)] public float gravity = 10f;
    public GravityMode gravityMode = GravityMode.FromCenter;

    private bool isChangingGravityDirection;
    public bool jump;
    public float jumpForce = 20f;
    [Range(0, 15)] public float rotationSpeed = 10f;
    [Min(0)] public float speed = 30f;

    private bool IsGrounded => Physics.Raycast(transform.position, -transform.up, out var hit,
        _distanceToTheGround + 0.01f); //HACk: DO NOT DELETE HIT!!!

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _distanceToTheGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gravityMode == GravityMode.FromCenter)
                gravityMode = GravityMode.ToCenter;
            if (gravityMode == GravityMode.ToCenter)
                gravityMode = GravityMode.FromCenter;

            isChangingGravityDirection = true;
        }

        if (Input.GetKeyDown(KeyCode.N)) gravityMode = GravityMode.None;

        if (Input.GetButtonDown("Jump") && IsGrounded) jump = true;
    }

    private void FixedUpdate()
    {
        var transform1 = transform;

        Vector3 upDirection;
        switch (gravityMode)
        {
            case GravityMode.FromCenter:
                upDirection = -transform1.position.normalized;
                break;
            case GravityMode.ToCenter:
                upDirection = transform1.position.normalized;
                break;
            default:
                upDirection = Vector3.zero;
                break;
        }

        #region Rotation

        if (gravityMode != GravityMode.None)
        {
            var newZRotation = gravityMode == GravityMode.FromCenter ?  180f : 360f;
            newZRotation -= Mathf.Atan2(transform1.position.x, transform1.position.y) * Mathf.Rad2Deg;
            var newRotation = Quaternion.Euler(0, 0, newZRotation);

            if (isChangingGravityDirection)
            {
                transform1.rotation = Quaternion.Slerp(transform1.rotation, newRotation, Time.fixedDeltaTime * rotationSpeed);
                isChangingGravityDirection = !IsGrounded;
            }
            else
            {
                transform1.rotation = newRotation;
            }
        }

        #endregion


        #region Velocity change

        
        var oldVerticalVelocity = Vector3.zero;
        if (gravityMode != GravityMode.None)
        {
            oldVerticalVelocity = Vector3.Project(_rb.velocity, upDirection);
        } 

        switch (gravityMode)
        {
            case GravityMode.FromCenter:
            case GravityMode.ToCenter:
                oldVerticalVelocity += -upDirection * gravity;
                break;
            case GravityMode.None:
                oldVerticalVelocity += transform1.up * (Input.GetAxis("Vertical") * speed);
                break;
        }

        if (jump)
        {
            oldVerticalVelocity += upDirection * jumpForce;
            jump = false;
        }

        var horizontal = transform1.right * (Input.GetAxis("Horizontal") * speed);
        _rb.velocity = horizontal + oldVerticalVelocity;

        #endregion
    }
}
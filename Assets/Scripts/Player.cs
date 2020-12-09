using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public enum GravityMode
    {
        None,
        FromCenter,
        ToCenter
    }

    private Rigidbody _rb;
    public GameObject graphics;
    public Animator animator;

    [Min(0)] public float gravity = 3f;
    public GravityMode gravityMode = GravityMode.FromCenter;
    [Range(0, 15)] public float rotationSpeed = 10f;


    private bool _isChangingGravityDirection;
    private bool _jump;
    [Min(0)] public float jumpForce = 50f;
    [Min(0)] public float speed = 30f;

    private float _distanceToTheGround;

    private bool IsGrounded =>
        Physics.Raycast(transform.position, -transform.up, out var hit,
            _distanceToTheGround + 0.05f);

    private bool isResized;
    public float resizingScale = 2f;

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

            _isChangingGravityDirection = true;
        }

        if (Input.GetKeyDown(KeyCode.N)) gravityMode = GravityMode.None;

        if (Input.GetButtonDown("Jump") && IsGrounded) _jump = true;
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            graphics.transform.localRotation = Quaternion.Euler(0, Input.GetAxisRaw("Horizontal") < 0 ? -90 : 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.localScale *= isResized ? resizingScale : 1/resizingScale;
            isResized = !isResized;
        }
    }


    private void FixedUpdate()
    {
        Vector3 upDirection;
        switch (gravityMode)
        {
            case GravityMode.FromCenter:
                upDirection = -transform.position.normalized;
                break;
            case GravityMode.ToCenter:
                upDirection = transform.position.normalized;
                break;
            default:
                upDirection = transform.up;
                break;
        }

        var leftDirection = (Vector3) Vector2.Perpendicular(upDirection);


        #region Rotation

        if (gravityMode != GravityMode.None)
        {
            var newZRotation = gravityMode == GravityMode.FromCenter ? 180f : 360f;
            newZRotation -= Mathf.Atan2(transform.position.x, transform.position.y) * Mathf.Rad2Deg;
            var newRotation = Quaternion.Euler(0, 0, newZRotation);

            if (_isChangingGravityDirection)
            {
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * rotationSpeed);
                _isChangingGravityDirection = !IsGrounded;
            }
            else
            {
                transform.rotation = newRotation;
            }
        }

        #endregion

        #region Velocity change

        var oldVerticalVelocity = Vector3.zero;
        if (gravityMode != GravityMode.None) oldVerticalVelocity = Vector3.Project(_rb.velocity, upDirection);

        var newVerticalVelocity = Vector3.zero;
        switch (gravityMode)
        {
            case GravityMode.FromCenter:
            case GravityMode.ToCenter:
                newVerticalVelocity = -upDirection * gravity;
                break;
            case GravityMode.None:
                newVerticalVelocity = transform.up * (Input.GetAxis("Vertical") * speed);
                break;
        }

        if (_jump)
        {
            newVerticalVelocity += upDirection * jumpForce;
            _jump = false;
            animator.SetTrigger("jump");
        }

        animator.SetBool("isGrounded", IsGrounded);

        var horizontal = leftDirection * -(Input.GetAxis("Horizontal") * speed);
        animator.SetFloat("speed", horizontal.magnitude);
        animator.SetBool("isZeroGravity", gravityMode == GravityMode.None);

        _rb.velocity = horizontal + newVerticalVelocity + oldVerticalVelocity;

        #endregion
    }
}
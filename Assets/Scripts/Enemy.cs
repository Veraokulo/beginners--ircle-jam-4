using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public const string TAG = "Enemy";
    public float Health = 100f;
    public GameObject loot;
    public float speed = 30f;
    private Rigidbody _rb;
    public HealthBar healthBar;
    public float attackRadius;
    public float DPS = 20f;
    private bool isPlayerNear;
    [TextArea] public string DialogText = "sample text";
    private bool isDialogueEnd;
    private Coroutine _dialogueDelayCoroutine;
    public float dialogueDuration = 3f;
    public bool isBoss;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (loot != null)
            loot.SetActive(false);
    }

    private void FixedUpdate()
    {
        var vectorToPlayer = GameManager.Instance.Player.transform.position - transform.position;
        if (isPlayerNear && vectorToPlayer.magnitude < attackRadius)
        {
            GameManager.Instance.Player.TakeDamage(DPS * Time.fixedDeltaTime);
        }

        Vector3 upDirection;
        var gravity = GameManager.Instance.Gravity.Bodies[_rb];
        switch (gravity)
        {
            case Gravity.GravityMode.FromCenter:
                upDirection = -transform.position.normalized;
                break;
            case Gravity.GravityMode.ToCenter:
                upDirection = transform.position.normalized;
                break;
            default:
                upDirection = transform.up;
                break;
        }

        var leftDirection = (Vector3) Vector2.Perpendicular(upDirection);

        #region Rotation

        if (gravity != Gravity.GravityMode.None)
        {
            transform.rotation = Quaternion.Euler(0, 0,
                Vector3.SignedAngle(Vector3.up, upDirection, Vector3.forward));
        }

        #endregion


        #region Velocity change

        Vector3 vertical;
        if (gravity == Gravity.GravityMode.None)
            vertical = isPlayerNear ? Vector3.Project(vectorToPlayer.normalized, upDirection) * speed : Vector3.zero;
        else
            vertical = Vector3.Project(_rb.velocity, upDirection);

        var horizontal =
            isPlayerNear ? Vector3.Project(vectorToPlayer.normalized, leftDirection) * speed : Vector3.zero;

        _rb.velocity = horizontal + vertical;

        #endregion
    }


    public void TakeDamage(float damage)
    {
        Health -= damage;
        healthBar.SetHealth(Health);
        GameManager.Instance.SetColorFilter(Color.red);
        if (Health <= 0)
        {
            if (isBoss)
                GameManager.Instance.Victory();
            Die();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            if (!isDialogueEnd)
            {
                if (_dialogueDelayCoroutine == null)
                {
                    _dialogueDelayCoroutine = StartCoroutine(SetDialogueEndWithDelay());
                    GameManager.Instance.ShowMessage(DialogText, dialogueDuration);
                }
            }
            else
            {
                isPlayerNear = true;
            }
        }
    }

    private IEnumerator SetDialogueEndWithDelay()
    {
        yield return new WaitForSeconds(dialogueDuration);
        isDialogueEnd = true;
        if ((GameManager.Instance.Player.transform.position - transform.position).magnitude < 15f)
            isPlayerNear = true;
        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player.gameObject)
        {
            isPlayerNear = false;
        }
    }

    private void Die()
    {
        GameManager.Instance.Gravity.Bodies.Remove(_rb);
        Destroy(gameObject);
        if (loot != null)
        {
            var gObj = Instantiate(loot, transform.position, transform.rotation);
            gObj.SetActive(true);
        }

        Debug.Log(gameObject.name + " DIED!!!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
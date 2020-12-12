using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : NPC
{
    public const string Tag = "Enemy";
    public float health = 100f;
    public GameObject loot;
    public float speed = 30f;
    private Rigidbody _rb;
    public HealthBar healthBar;
    public float attackRadius;
    public float dps = 20f;
    public bool isBoss;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health);
            healthBar.SetHealth(health);
        }

        if (loot != null)
            loot.SetActive(false);
    }

    private void FixedUpdate()
    {
        var vectorToPlayer = GameManager.Instance.Player.transform.position - transform.position;
        if (isPlayerNear && vectorToPlayer.magnitude < attackRadius)
        {
            GameManager.Instance.Player.TakeDamage(dps * Time.fixedDeltaTime);
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
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.Gravity.Bodies.Remove(_rb);
        Destroy(gameObject);
        if (loot != null)
        {
            var gObj = Instantiate(loot, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            gObj.transform.localScale = Vector3.one * 0.3f;
            gObj.SetActive(true);
        }

        if (isBoss)
            GameManager.Instance.Victory();

        Debug.Log(gameObject.name + " DIED!!!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
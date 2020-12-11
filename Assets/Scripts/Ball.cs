using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Ball : MonoBehaviour
{
    public float damage;
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Enemy.TAG))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}

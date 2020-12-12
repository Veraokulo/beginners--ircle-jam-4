using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public float forceToCenter = 8f;
    public float shootVelocity = 20f;
    private Plane _plane;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    public float maxRadius = 5f;
    [Range(0,1)]
    public float smoothness = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && !other.CompareTag(Enemy.Tag) && !other.CompareTag(Player.Tag))
        {
            rigidbodies.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rigidbodies.Remove(other.attachedRigidbody);
    }


    public void Start()
    {
        _plane = new Plane(Vector3.forward, Vector3.zero);
    }

    public void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var localP = hitPoint - transform.parent.position;
            if (localP.magnitude > maxRadius)
                localP *= maxRadius / localP.magnitude;
            transform.position = Vector3.Slerp(transform.position,GameManager.Instance.Player.transform.position + localP,smoothness);
        }

        if (Input.GetMouseButton(0))
        {
            foreach (var rb in rigidbodies)
            {
                rb.velocity = (transform.position - rb.transform.position) * forceToCenter;
            }
        }

        if (Input.GetMouseButton(1))
        {
            for (var i = rigidbodies.Count - 1; i >= 0; i--)
            {
                rigidbodies[i].velocity = (rigidbodies[i].transform.position - transform.parent.position).normalized * shootVelocity;
                rigidbodies.RemoveAt(i);
            }
        }
    }
}
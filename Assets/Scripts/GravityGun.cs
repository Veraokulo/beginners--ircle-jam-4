﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public float forceToCenter = 8f;
    public float forceToShoot = 20f;
    private Plane _plane;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && !other.CompareTag(Enemy.TAG))
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

    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var localP = hitPoint - transform.parent.position;
            var q = Mathf.Atan2(localP.y, localP.x) * Mathf.Rad2Deg;
            transform.parent.rotation = Quaternion.Euler(0, 0, q);
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
            foreach (var rb in rigidbodies)
            {
                rb.AddForce((rb.transform.position - transform.parent.parent.position) * forceToShoot);
            }
        }
    }
}
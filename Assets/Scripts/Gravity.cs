using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public enum GravityMode
    {
        None,
        FromCenter,
        ToCenter
    }

    public GravityMode globalGravityMode = GravityMode.FromCenter;
    public Dictionary<Rigidbody, GravityMode> Bodies;
    [Min(0)] public float gravityForce = 3f;

    private void Start()
    {
        GameManager.Instance.Gravity = this;
        Bodies = FindObjectsOfType<Rigidbody>().ToDictionary(rb => rb, _ => globalGravityMode);

        var bodiesToInvert = Bodies.Where(_ => _.Key.useGravity).ToDictionary(x=>x.Key,x=>x.Value);
        //TEMPORARY HACK
        foreach (var key in bodiesToInvert.Keys)
        {
            Bodies[key] = GravityMode.ToCenter;
        }
    }

    private void FixedUpdate()
    {
        foreach (var body in Bodies)
        {
            if (body.Value == GravityMode.FromCenter)
                body.Key.AddForce(body.Key.transform.position.normalized * gravityForce, ForceMode.VelocityChange);
            if (body.Value == GravityMode.ToCenter)
                body.Key.AddForce(-body.Key.transform.position.normalized * gravityForce, ForceMode.VelocityChange);
        }
    }

    public void ChangeGlobalGravity(GravityMode gm, bool overrideAll = false)
    {
        foreach (var rb in Bodies.Where(_ => _.Value == globalGravityMode || overrideAll))
        {
            Bodies[rb.Key] = gm;
        }

        globalGravityMode = gm;
    }
}
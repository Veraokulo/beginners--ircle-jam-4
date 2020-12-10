using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public static Gravity Instance;

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
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Bodies = FindObjectsOfType<Rigidbody>().ToDictionary(rb => rb, _ => globalGravityMode);

        
        //TEMPORARY HACK
        foreach (var body in Bodies.Where(_ => _.Key.useGravity))
        {
            Bodies[body.Key] = GravityMode.ToCenter;
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
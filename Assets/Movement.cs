using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody body;
    private Vector3 sideForceVec;
    private Vector3 forwardForceVec;
    private Vector3 upForceVec;

    // Start is called before the first frame update
    void Start()
    {
        sideForceVec = new Vector3(10f, 0f, 0.0f);
        forwardForceVec = new Vector3(0.0f, 0.0f, 10.0f);
        upForceVec = new Vector3(0f, 5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left"))
        {
            body.AddForce(-sideForceVec);
        }

        if (Input.GetKey("right")) {
            body.AddForce(sideForceVec);
        }

        if (Input.GetKey("up"))
        {
            body.AddForce(forwardForceVec);
        }

        if (Input.GetKey("down"))
        {
            body.AddForce(-forwardForceVec);
        }

        if (Input.GetKey("space"))
        {
            body.AddForce(upForceVec);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    int limit = 9;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > limit)
            transform.position = new Vector3(-9, transform.position.y, transform.position.z);

        if (transform.position.x < -limit)
            transform.position = new Vector3(9, transform.position.y, transform.position.z);

        if (transform.position.z < -limit)
            transform.position = new Vector3(transform.position.x, transform.position.y, 9);

        if (transform.position.z > limit)
            transform.position = new Vector3(transform.position.x, transform.position.y, -9);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPortals : MonoBehaviour
{
    void Update ()
    {
        transform.Rotate (0, GameManager.Instance.Config.portalRotationSpeed*Time.deltaTime,0); //rotates 50 degrees per second around z axis
    }
}

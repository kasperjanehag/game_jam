using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPortals : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed;

    void Update ()
    {
        transform.Rotate (0, m_rotationSpeed*Time.deltaTime,0); //rotates 50 degrees per second around z axis
    }
}

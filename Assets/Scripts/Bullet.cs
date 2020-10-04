using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}

public class Bullet : MonoBehaviour
{
    public BulletType CurrentBulletType
    {
        get => m_currentType;
    }

    private Vector3 m_direction;
    Color colour;

    float damagePoints;

    private const float BULLET_SPEED = 8f;
    private Vector3 offsetVecForward = new Vector3(0.0f, 0.0f, 0.5f);
    private Vector3 offsetVecSide = new Vector3(0.5f, 0.0f, 0.0f);

    [SerializeField] private Material m_redMaterial;
    [SerializeField] private Material m_blueMaterial;
    private int bounce_counter = 0;

    private Rigidbody rb;
    private Vector3 lastFrameVelocity;

    void Start()
    {
        damagePoints = GameManager.Instance.Config.BulletDamage;
        rb = GetComponent<Rigidbody>();
    }
    
    private BulletType m_currentType = BulletType.None;

    public void Fire(Vector3 direction, BulletType bulletType)
    {
        m_direction = direction;
        m_currentType = bulletType;
        SetColor(m_currentType);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "portal1")
        {
            transform.position = GameObject.FindWithTag("portal2").transform.position + GameObject.FindWithTag("portal2").transform.forward;
            m_currentType = BulletType.Red;
            SetColor(m_currentType);
            return;
        }
        else if (collision.gameObject.tag == "portal2")
        {
            transform.position = GameObject.FindWithTag("portal1").transform.position + GameObject.FindWithTag("portal1").transform.forward;
            m_currentType = BulletType.Red;
            SetColor(m_currentType);
            return;
        }

        else if (collision.gameObject.tag == "portal3")
        {
            transform.position = GameObject.FindWithTag("portal4").transform.position + GameObject.FindWithTag("portal4").transform.forward;
            m_currentType = BulletType.Blue;
            SetColor(m_currentType);
            return;
        }

        else if (collision.gameObject.tag == "portal4")
        {
            transform.position = GameObject.FindWithTag("portal3").transform.position + GameObject.FindWithTag("portal3").transform.forward;
            m_currentType = BulletType.Blue;
            SetColor(m_currentType);
            return;
        }
        
        Bounce(collision.contacts[0].normal);
        
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(m_direction, collisionNormal);
        m_direction = direction;
        bounce_counter += 1;
        if (bounce_counter == GameManager.Instance.Config.BulletBounceDestroy) {
            Destroy(gameObject);
        }
        // rb.velocity = direction * -10000000*lastFrameVelocity.magnitude;
    }

        private void Update()
    {
        transform.position += m_direction * Time.deltaTime * GameManager.Instance.Config.BulletSpeed;
    }

    private void SetColor(BulletType type)
    {
        if (type == BulletType.Red)
        {
            gameObject.GetComponent<MeshRenderer>().material = m_redMaterial;
           // gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * 4f);
        }

        if (type == BulletType.Blue)
        {
            gameObject.GetComponent<MeshRenderer>().material = m_blueMaterial;
          //  gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.blue * 4f);
        }
        //
        // if (type == BulletType.Green)
        // {
        //     gameObject.GetComponent<MeshRenderer>().material = m_redMaterial;
        //    // gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green * 4f);
        // }
        //
        // if (type == BulletType.Yellow)
        // {
        //     gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //     //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow * 4f);
        // }
    }
}

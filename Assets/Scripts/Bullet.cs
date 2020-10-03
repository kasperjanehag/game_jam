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

    void Start()
    {
        damagePoints = GameManager.Instance.Config.BulletDamage;
    }
    
    private BulletType m_currentType = BulletType.None;

    public void Fire(Vector3 direction, BulletType bulletType)
    {
        m_direction = direction;
        m_currentType = bulletType;
        SetColor(m_currentType);
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(GameManager.Instance.Config.BulletDeathTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "portal1")
        {
            transform.position = GameObject.FindWithTag("portal2").transform.position + offsetVecForward;
            m_currentType = BulletType.Red;
            SetColor(m_currentType);
        }

        if (collision.gameObject.tag == "portal2")
        {
            transform.position = GameObject.FindWithTag("portal1").transform.position - offsetVecForward;
            m_currentType = BulletType.Green;
            SetColor(m_currentType);
        }

        if (collision.gameObject.tag == "portal3")
        {
            transform.position = GameObject.FindWithTag("portal4").transform.position + offsetVecSide;
            m_currentType = BulletType.Blue;
            SetColor(m_currentType);
        }

        if (collision.gameObject.tag == "portal4")
        {
            transform.position = GameObject.FindWithTag("portal3").transform.position - offsetVecSide;
            m_currentType = BulletType.Yellow;
            SetColor(m_currentType);
        }
    }

        private void Update()
    {
        transform.position += m_direction * Time.deltaTime * GameManager.Instance.Config.BulletSpeed;
    }

    private void SetColor(BulletType type)
    {
        if (type == BulletType.Red)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * 4f);
        }

        if (type == BulletType.Blue)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.blue * 4f);
        }

        if (type == BulletType.Green)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green * 4f);
        }

        if (type == BulletType.Yellow)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow * 4f);
        }
    }
}

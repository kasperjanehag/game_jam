using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 m_direction;

    private const float BULLET_SPEED = 8f;
    private Vector3 offsetVecForward = new Vector3(0.0f, 0.0f, 0.5f);
    private Vector3 offsetVecSide = new Vector3(0.5f, 0.0f, 0.0f);

    public void Fire(Vector3 direction)
    {
        m_direction = direction;
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
        }

        if (collision.gameObject.tag == "portal2")
        {
            transform.position = GameObject.FindWithTag("portal1").transform.position - offsetVecForward;
        }

        if (collision.gameObject.tag == "portal3")
        {
            transform.position = GameObject.FindWithTag("portal4").transform.position + offsetVecSide;
        }

        if (collision.gameObject.tag == "portal4")
        {
            transform.position = GameObject.FindWithTag("portal3").transform.position - offsetVecSide;
        }
    }


        private void Update()
    {
        transform.position += m_direction * Time.deltaTime * GameManager.Instance.Config.BulletSpeed;
    }
}

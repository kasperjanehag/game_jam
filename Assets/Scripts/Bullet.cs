using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 m_direction;

    private const float BULLET_SPEED = 8f;

    public void Fire(Vector3 direction)
    {
        m_direction = direction;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += m_direction * Time.deltaTime * BULLET_SPEED;
    }
}

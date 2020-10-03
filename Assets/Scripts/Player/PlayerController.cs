using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private float m_speed;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_shootDelay;

    private bool m_isShooting;

    void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveY);
        var moveValue = move * Time.deltaTime * m_speed;
        m_characterController.Move(moveValue);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetMouseButton(0) && !m_isShooting)
        {
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
    }

    private IEnumerator SpawnBulletAfterDelay()
    {
        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 3f, Quaternion.identity);
        bullet.GetComponent<Bullet>().Fire(transform.forward);
        yield return new WaitForSeconds(m_shootDelay);
        m_isShooting = false;
    }
}

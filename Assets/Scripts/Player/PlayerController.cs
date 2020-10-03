﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private float m_speed;
    [SerializeField] private GameObject m_bullet;

    private bool m_isShooting;

    void Start()
    {
        Cursor.visible = true;
    }

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

        Vector3 lookTarget = new Vector3();
        var ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            if (hit.transform.tag == "GroundPlane")
            {
                Debug.DrawLine(transform.position, hit.point);
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }

        if (Input.GetMouseButton(0) && !m_isShooting)
        {
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
    }

    private IEnumerator SpawnBulletAfterDelay()
    {
        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, Quaternion.identity);
        bullet.GetComponent<Bullet>().Fire(transform.forward);
        yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);
        m_isShooting = false;
    }
}

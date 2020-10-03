using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_jumpHeight = 3f;

    [SerializeField] private CameraShake cameraShake;

    private const float GRAVITY = -70f;
    private bool m_isShooting;
    private bool m_canMove;

    private Vector3 playerVelocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.visible = true;
        m_canMove = true;
    }

    void Update()
    {
        if(!m_canMove) return;


        isGrounded = m_characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveY);
        var moveValue = move * Time.deltaTime * GameManager.Instance.Config.PlayerSpeed;
        m_characterController.Move(moveValue);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        Vector3 lookTarget = new Vector3();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

        playerVelocity.y += GRAVITY * Time.deltaTime;
        m_characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);  
            cameraShake.shakeDuration = 0.1f;
        }
    }


    public void setCanMove(bool canMove)
    {
        m_canMove = canMove;
    }

    private IEnumerator SpawnBulletAfterDelay()
    {
        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, transform.rotation);
        bullet.GetComponent<Bullet>().Fire(transform.forward);
        yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);
        m_isShooting = false;
    }
}

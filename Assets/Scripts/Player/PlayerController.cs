using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_jumpHeight = 3f;
    [SerializeField] private bool m_isSecondPlayer;

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

        if (m_isSecondPlayer)
        {
            MoveSecondPlayer();
        }
        else
        {
            MoveFirstPlayer();
        }

        playerVelocity.y += GRAVITY * Time.deltaTime;
        m_characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void MoveFirstPlayer()
    {
        var moveX = Input.GetAxis("HorizontalP1");
        var moveY = Input.GetAxis("VerticalP1");

        Vector3 move = new Vector3(moveX, 0, moveY);
        var moveValue = move * Time.deltaTime * GameManager.Instance.Config.PlayerSpeed;
        m_characterController.Move(moveValue);

        float _angle = Mathf.Atan2(Input.GetAxis("Roll"), Input.GetAxis("Pitch")) * Mathf.Rad2Deg;
        if (new Vector2(Input.GetAxis("Roll"), Input.GetAxis("Pitch")) != Vector2.zero)
        {
            var _rot = Quaternion.AngleAxis(_angle, new Vector3(0, 1, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, _rot, 20 * Time.deltaTime);
        }

        var input = new Vector3(moveX, 0, moveY);

        if (!m_isShooting && Input.GetButtonDown("XboxRBPlayer1"))
        {
            Debug.Log("Player 1 fire!!!");
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
    }

    private void MoveSecondPlayer()
    {
        var moveX = Input.GetAxis("HorizontalP2");
        var moveY = Input.GetAxis("VerticalP2");

        Vector3 move = new Vector3(moveX, 0, moveY);
        var moveValue = move * Time.deltaTime * GameManager.Instance.Config.PlayerSpeed;
        m_characterController.Move(moveValue);

        float _angle = Mathf.Atan2(Input.GetAxis("RollP2"), Input.GetAxis("PitchP2")) * Mathf.Rad2Deg;
        if (new Vector2(Input.GetAxis("RollP2"), Input.GetAxis("PitchP2")) != Vector2.zero)
        {
            var _rot = Quaternion.AngleAxis(_angle, new Vector3(0, 1, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, _rot, 20 * Time.deltaTime);
        }

        var input = new Vector3(moveX, 0, moveY);
        if (!m_isShooting && Input.GetButtonDown("XboxRBPlayer2"))
        {
            Debug.Log("Player 2 fire!!!");
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
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
        Debug.Log("spawning bullet!!!");


        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, transform.rotation);
        bullet.GetComponent<Bullet>().Fire(transform.forward);
        yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);
        m_isShooting = false;
    }
}

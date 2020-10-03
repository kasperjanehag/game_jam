using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum PlayerColor
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}

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
    private PlayerColor m_currentColor = PlayerColor.None;

    private Vector3 playerVelocity;
    private bool isGrounded;
    private 

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

    public void setCanMove(bool canMove)
    {
        m_canMove = canMove;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
            cameraShake.shakeDuration = 0.1f;
        }
    }



    void OnControllerColliderHit(ControllerColliderHit collision)
    {

        if (collision.gameObject.tag == "portal1")
        {
            m_characterController.enabled = false;
            m_currentColor = PlayerColor.Red;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            var PortalTransform = GameObject.FindWithTag("portal2").transform;
            m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
            m_characterController.enabled = true;
        }

        if (collision.gameObject.tag == "portal2")
        {
            m_characterController.enabled = false;
            m_currentColor = PlayerColor.Green;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            var PortalTransform = GameObject.FindWithTag("portal1").transform;
            m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
            m_characterController.enabled = true;

        }

        if (collision.gameObject.tag == "portal3")
        {
            m_characterController.enabled = false;
            m_currentColor = PlayerColor.Blue;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            var PortalTransform = GameObject.FindWithTag("portal4").transform;
            m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
            m_characterController.enabled = true;

        }

        if (collision.gameObject.tag == "portal4")
        {
            m_characterController.enabled = false;
            m_currentColor = PlayerColor.Yellow;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            var PortalTransform = GameObject.FindWithTag("portal3").transform;
            m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
            m_characterController.enabled = true;
        }





    }

    private IEnumerator SpawnBulletAfterDelay()
    {
        Debug.Log("spawning bullet!!!");


        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, transform.rotation);
        bullet.GetComponent<Bullet>().Fire(transform.forward, (BulletType)m_currentColor);
        yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);
        m_isShooting = false;
    }
}

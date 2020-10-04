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
    enum HealthBarPosition {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    };

    [SerializeField] private GameObject m_gun;
    [SerializeField] private Material m_redMaterial;
    [SerializeField] private Material m_blueMaterial;

    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_jumpHeight = 3f;
    [SerializeField] private bool m_isSecondPlayer;

    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private Color m_color = Color.red;
    [SerializeField] private Texture m_healthIcon;
    [SerializeField] private bool m_horizontalHealthBar = true;
    [SerializeField] private HealthBarPosition m_healthBarPosition;

    [SerializeField] private int m_health;

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

    private void MoveFirstPlayer()
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

        if (!m_isShooting && Input.GetButton("XboxRBPlayer1"))
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
        if (collision.gameObject.tag == "bullet" && (int)collision.gameObject.GetComponent<Bullet>().CurrentBulletType != (int)m_currentColor )
        {
            Destroy(collision.gameObject);
            cameraShake.shakeDuration = 0.1f;
            m_health -= 1;

            if (m_health == 0)
            {
                m_canMove = false;
            }
        }
    }
    //
    // void OnControllerColliderHit(ControllerColliderHit collision)
    // {
    //     if (collision.gameObject.tag == "portal1")
    //     {
    //         m_characterController.enabled = false;
    //         m_currentColor = PlayerColor.Red;
    //         m_gun.GetComponent<MeshRenderer>().material = m_redMaterial;
    //
    //         var PortalTransform = GameObject.FindWithTag("portal2").transform;
    //         m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
    //         m_characterController.enabled = true;
    //     }
    //
    //     if (collision.gameObject.tag == "portal2")
    //     {
    //         m_characterController.enabled = false;
    //         m_currentColor = PlayerColor.Red;
    //         m_gun.GetComponent<MeshRenderer>().material = m_redMaterial;
    //         var PortalTransform = GameObject.FindWithTag("portal1").transform;
    //         m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
    //         m_characterController.enabled = true;
    //
    //     }
    //
    //     if (collision.gameObject.tag == "portal3")
    //     {
    //         m_characterController.enabled = false;
    //         m_currentColor = PlayerColor.Blue;
    //         m_gun.GetComponent<MeshRenderer>().material = m_blueMaterial;
    //         var PortalTransform = GameObject.FindWithTag("portal4").transform;
    //         m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
    //         m_characterController.enabled = true;
    //
    //     }
    //
    //     if (collision.gameObject.tag == "portal4")
    //     {
    //         m_characterController.enabled = false;
    //         m_currentColor = PlayerColor.Blue;
    //         m_gun.GetComponent<MeshRenderer>().material = m_blueMaterial;
    //         var PortalTransform = GameObject.FindWithTag("portal3").transform;
    //         m_characterController.transform.position = PortalTransform.position + PortalTransform.forward;
    //         m_characterController.enabled = true;
    //     }
    // }

    private IEnumerator SpawnBulletAfterDelay()
    {
        Debug.Log("spawning bullet!!!");

        var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, transform.rotation);
        bullet.GetComponent<Bullet>().Fire(transform.forward, (BulletType)m_currentColor);
        yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);
        m_isShooting = false;
    }
    void OnGUI()
    {
        if (m_healthIcon) {
            Vector2 healthBarDirection = new Vector2();
            Vector2 healthPosition = new Vector2();
            switch (m_healthBarPosition) {
                case HealthBarPosition.TopLeft:
                    healthPosition = new Vector2(10, 10);
                    if (m_horizontalHealthBar)
                    {
                        healthBarDirection = new Vector2(1, 0);
                    }
                    else {
                        healthBarDirection = new Vector2(0, 1);
                    }
                    break;
                case HealthBarPosition.TopRight:
                    healthPosition = new Vector2(Screen.width - 10 - m_healthIcon.width, 10);
                    if (m_horizontalHealthBar)
                    {
                        healthBarDirection = new Vector2(-1, 0);
                    }
                    else
                    {
                        healthBarDirection = new Vector2(0, 1);
                    }
                    break;
                case HealthBarPosition.BottomLeft:
                    healthPosition = new Vector2(10, Screen.height - 10 - m_healthIcon.height);
                    if (m_horizontalHealthBar)
                    {
                        healthBarDirection = new Vector2(1, 0);
                    }
                    else
                    {
                        healthBarDirection = new Vector2(0, -1);
                    }
                    break;
                case HealthBarPosition.BottomRight:
                    healthPosition = new Vector2(Screen.width - 10 - m_healthIcon.width, Screen.height - 10 - m_healthIcon.height);
                    if (m_horizontalHealthBar)
                    {
                        healthBarDirection = new Vector2(-1, 0);
                    }
                    else
                    {
                        healthBarDirection = new Vector2(0, -1);
                    }
                    break;
            }

            Vector2 hpIconPos = healthPosition;
            for (int i = 0; i < m_health; i++) {
                Vector2 iconSize = new Vector2(m_healthIcon.width, m_healthIcon.height);
                GUI.DrawTexture(new Rect(hpIconPos.x, hpIconPos.y, m_healthIcon.width, m_healthIcon.height), m_healthIcon, ScaleMode.ScaleToFit, true, 0, m_color, 0, 0);

                hpIconPos += healthBarDirection * iconSize * 0.6f /*+ 5 * healthBarDirection*/;
            }
        }
    }
}

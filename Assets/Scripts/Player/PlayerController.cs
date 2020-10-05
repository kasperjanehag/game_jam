using System;
using System.Collections;
using UnityEngine;

public enum TeamColor
{
    Blue,
    Pink,
}

public class PlayerController : MonoBehaviour
{
    enum HealthBarPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    };

    [SerializeField] private GameObject[] m_arrows;
    [SerializeField] private Material m_redMaterial;
    [SerializeField] private Material m_blueMaterial;

    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private GameObject m_playerHitParticleSystem;
    [SerializeField] private bool m_isSecondPlayer;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private Color m_color = Color.white;
    [SerializeField] private Texture m_healthIcon;
    [SerializeField] private bool m_horizontalHealthBar = true;
    [SerializeField] private HealthBarPosition m_healthBarPosition;

    [SerializeField] private int m_health;
    [SerializeField] private int m_bulletInMag = 3;
    [SerializeField] public TeamColor m_teamColor;

    [SerializeField] public KeyCode Forward;
    [SerializeField] public KeyCode Backward;
    [SerializeField] public KeyCode RotateLeft;
    [SerializeField] public KeyCode RotateRight;
    [SerializeField] public KeyCode Shoot;

    private const float GRAVITY = -70f;
    private bool m_isShooting;

    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isAlive = true;

    private float enemyShootDt = 0f;

    void Start()
    {
        Cursor.visible = true;
        m_health = GameManager.Instance.Config.PlayerHealth;
        StartCoroutine(ReloadBulletAfterDelay());
    }

    void Update()
    {
        if (!isAlive || !GameManager.Instance.HasStarted) return;

        isGrounded = m_characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        ManageControllerInput();

        if (!m_isSecondPlayer)
        {
            ManageKeyboardInput();
        }

        playerVelocity.y += GRAVITY * Time.deltaTime;
        m_characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void ManageControllerInput()
    {
        if (m_isSecondPlayer)
        {
            MoveSecondPlayerAI();
        }
        else
        {
            MoveFirstPlayer();
        }
    }

    private void MoveSecondPlayerAI()
    {
        // Find player pos
        //var otherPlayerPosition =

        //var vec = new Vector3() - transform.position;
        var obj = GameObject.FindGameObjectWithTag("PlayerOne");
        var vec = obj.transform.position - transform.position;
        vec.Normalize();

        transform.forward = vec;

        var move = vec * 2f;

        if (enemyShootDt > 1f)
        {
            enemyShootDt = 0f;
            ShootPlayer();
        }
        enemyShootDt += Time.deltaTime;
        m_characterController.Move(move * Time.deltaTime);
    }

    private void ShootPlayer()
    {
        //var obj = GameObject.FindGameObjectWithTag("PlayerOne");
        //var vec = obj.transform.position - transform.position;
        StartCoroutine(SpawnBulletAfterDelay());
    }

    private void ManageKeyboardInput()
    {
        // var moveValue = new Vector3(0,0,0);
        // if (Input.GetKey(Forward))
        // {
        //     moveValue = transform.forward * GameManager.Instance.Config.PlayerSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey(Backward))
        // {
        //     moveValue = -transform.forward * GameManager.Instance.Config.PlayerSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey(RotateRight))
        // {
        //     transform.Rotate(Vector3.up * 400f * Time.deltaTime);
        // }
        // if (Input.GetKey(RotateLeft))
        // {
        //     transform.Rotate(-Vector3.up * 400f * Time.deltaTime);
        // }
        // if (Input.GetKeyDown(Shoot) && !m_isShooting)
        // {
        //     m_isShooting = true;
        //     StartCoroutine(SpawnBulletAfterDelay());
        // }

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveY);
        var moveValue = move * Time.deltaTime * GameManager.Instance.Config.PlayerSpeed;

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

        m_characterController.Move(moveValue);

        if (Input.GetMouseButton(0) && !m_isShooting)
        {
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
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
            m_isShooting = true;
            StartCoroutine(SpawnBulletAfterDelay());
        }
    }

    public void moveCharacterToPos(Vector3 newPos)
    {
        m_characterController.enabled = false;
        m_characterController.transform.position = newPos;
        m_characterController.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet" && collision.gameObject.GetComponent<Bullet>().m_teamColor != m_teamColor)
        {
            Destroy(collision.gameObject);
            Instantiate(m_playerHitParticleSystem, transform.position, Quaternion.identity);
            cameraShake.shakeDuration = 0.1f;
            cameraShake.shakeAmount = 0.7f;
            cameraShake.shakeDuration = 0.2f;
            m_health -= 1;

            GameManager.Instance.SetHealth(m_health, !m_isSecondPlayer);
            if (m_health == 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        GameManager.Instance.SetGameOver(m_isSecondPlayer);
        isAlive = false;
    }

    private IEnumerator SpawnBulletAfterDelay()
    {
        if (m_bulletInMag >= 1)
        {
            //Debug.Log("bullet inmag" + m_bulletInMag);
            m_arrows[m_bulletInMag - 1].SetActive(false);

            var bullet = Instantiate(m_bullet, transform.position + transform.forward * 2f, transform.rotation);
            cameraShake.shakeAmount = 0.1f;
            cameraShake.shakeDuration = 0.1f;
            bullet.GetComponent<Bullet>().Fire(transform.forward, m_teamColor);
            AudioManager.Instance.PlaySound("LaserShot");
            m_bulletInMag -= 1;
            yield return new WaitForSeconds(GameManager.Instance.Config.ShootDelay);

        }
        m_isShooting = false;
    }

    private IEnumerator ReloadBulletAfterDelay()
    {
        while(true){
            m_bulletInMag = Math.Min(m_bulletInMag + 1,GameManager.Instance.Config.MaxBulletInMag);
            m_arrows[m_bulletInMag - 1].SetActive(true);
            yield return new WaitForSeconds(GameManager.Instance.Config.ReloadDelay);
        }
    }
    void OnGUI()
    {



        // if (m_healthIcon)
        // {
        //     Vector2 healthBarDirection = new Vector2();
        //     Vector2 healthPosition = new Vector2();
        //     switch (m_healthBarPosition)
        //     {
        //         case HealthBarPosition.TopLeft:
        //             healthPosition = new Vector2(10, 10);
        //             if (m_horizontalHealthBar)
        //             {
        //                 healthBarDirection = new Vector2(1, 0);
        //             }
        //             else
        //             {
        //                 healthBarDirection = new Vector2(0, 1);
        //             }
        //             break;
        //         case HealthBarPosition.TopRight:
        //             healthPosition = new Vector2(Screen.width - 10 - m_healthIcon.width, 10);
        //             if (m_horizontalHealthBar)
        //             {
        //                 healthBarDirection = new Vector2(-1, 0);
        //             }
        //             else
        //             {
        //                 healthBarDirection = new Vector2(0, 1);
        //             }
        //             break;
        //         case HealthBarPosition.BottomLeft:
        //             healthPosition = new Vector2(10, Screen.height - 10 - m_healthIcon.height);
        //             if (m_horizontalHealthBar)
        //             {
        //                 healthBarDirection = new Vector2(1, 0);
        //             }
        //             else
        //             {
        //                 healthBarDirection = new Vector2(0, -1);
        //             }
        //             break;
        //         case HealthBarPosition.BottomRight:
        //             healthPosition = new Vector2(Screen.width - 10 - m_healthIcon.width, Screen.height - 10 - m_healthIcon.height);
        //             if (m_horizontalHealthBar)
        //             {
        //                 healthBarDirection = new Vector2(-1, 0);
        //             }
        //             else
        //             {
        //                 healthBarDirection = new Vector2(0, -1);
        //             }
        //             break;
        //     }
        //
        //     Vector2 hpIconPos = healthPosition;
        //     for (int i = 0; i < m_health; i++)
        //     {
        //         Vector2 iconSize = new Vector2(m_healthIcon.width, m_healthIcon.height);
        //         GUI.DrawTexture(new Rect(hpIconPos.x, hpIconPos.y, m_healthIcon.width, m_healthIcon.height), m_healthIcon, ScaleMode.ScaleToFit, true, 0, m_color, 0, 0);
        //
        //         hpIconPos += healthBarDirection * iconSize * 0.6f /*+ 5 * healthBarDirection*/;
        //     }
        // }
    }
}

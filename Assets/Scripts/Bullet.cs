using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TeamColor m_teamColor;

    private Vector3 m_direction;
    Color colour;

    float damagePoints;

    private const float BULLET_SPEED = 8f;
    private int bounceCounter = 0;
    private Vector3 offsetVecForward = new Vector3(0.0f, 0.0f, 0.5f);
    private Vector3 offsetVecSide = new Vector3(0.5f, 0.0f, 0.0f);

    [SerializeField] private Material m_redMaterial;
    [SerializeField] private Material m_blueMaterial;
    


    void Start()
    {
        damagePoints = GameManager.Instance.Config.BulletDamage;
    }
    
    public void Fire(Vector3 direction, TeamColor teamColor)
    {
        m_direction = direction;
        m_teamColor = teamColor;
        SetColor(teamColor);
    }


    void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(m_direction, collisionNormal);
        m_direction = direction;
        bounceCounter += 1;
        if (bounceCounter == GameManager.Instance.Config.BulletBounceDestroy) {
            Destroy(gameObject);
        }
    }

        private void Update()
    {
        transform.position += m_direction * Time.deltaTime * GameManager.Instance.Config.BulletSpeed;
    }

    private void SetColor(TeamColor teamColor)
    {
        if (teamColor == TeamColor.Pink)
        {
            gameObject.GetComponent<MeshRenderer>().material = m_redMaterial;
        }

        if (teamColor == TeamColor.Blue)
        {
            gameObject.GetComponent<MeshRenderer>().material = m_blueMaterial;
        }
    }
}

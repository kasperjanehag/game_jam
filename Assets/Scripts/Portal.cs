using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] private TeamColor m_teamColor;
    [SerializeField] private GameObject[] m_otherPortals;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "bullet" && collider.gameObject.GetComponent<Bullet>().m_teamColor == m_teamColor)
        {
            collider.gameObject.transform.position = getOtherPortalPos();
        }
        if (collider.gameObject.tag == "Player" && m_teamColor == collider.gameObject.GetComponent<PlayerController>().m_teamColor)
        {
            AudioManager.Instance.PlaySound("Portal");
            PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
            pc.moveCharacterToPos(getOtherPortalPos());
        }
    }

    private Vector3 getOtherPortalPos() {
        Transform otherPortal = m_otherPortals[Random.Range(0, m_otherPortals.Length - 1)].transform;
        return otherPortal.position + otherPortal.forward;
    }


}

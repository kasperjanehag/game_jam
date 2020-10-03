using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageReceiver : MonoBehaviour,IEntity
{
    public float playerHP = 100;
    public void ApplyDamage(float points)
    {
        playerHP -= points;

        if (playerHP <= 0)
        {
            //Player is dead
            playerHP = 0;
        }
    }
}

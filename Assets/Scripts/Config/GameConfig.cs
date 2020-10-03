﻿  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Shooting")]
    public float ShootDelay = 0.5f;
    public float BulletSpeed = 10f;
    public float BulletDeathTime = 2f;

    public float BulletDamage = 50f;

    [Header("Enemy")]
    public float WaveInterval = 5f;
}

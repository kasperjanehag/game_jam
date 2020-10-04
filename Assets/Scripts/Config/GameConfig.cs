  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    public int PlayerHealth = 3;
    public float PlayerSpeed = 5f;
    public float PlayerGravity = -10f;

    [Header("Shooting")]
    public float ShootDelay = 0.5f;
    public float ReloadDelay = 3f;
    public float BulletSpeed = 10f;
    public float BulletDeathTime = 2f;
    public float BulletDamage = 50f;
    public int BulletBounceDestroy = 5;

    public int MaxBulletInMag = 5;
    

    [Header("Enemy")]
    public float WaveInterval = 2f;
    public int EnemiesPerWave = 4;
    public float AttackRange = 2f;
    public float EnemySpeed = 3f;

    [Header("Portal")]
    public float portalRotationSpeed = 10f;
}

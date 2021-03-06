﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.Experimental.AssetImporters;
// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
// [RequireComponent(typeof(Rigidbody))]

// public class Enemy : MonoBehaviour,IEntity
// {

//     public float attackDistance = 3f;
//     public float movementSpeed = 4f;
//     public float npcHP = 100;
//     //How much damage will npc deal to the player
//     public float npcDamage = 5;
//     public float attackRate = 0.5f;
//     public Transform firePoint;
//     public GameObject npcDeadPrefab;

//     [HideInInspector]
//     public Transform playerTransform;
//     [HideInInspector]
//     public EnemySpawner es;
//     UnityEngine.AI.NavMeshAgent agent;
//     float nextAttackTime = 0;
//     Rigidbody r;

//     // Start is called before the first frame update
//     void Start()
//     {
//         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//         agent.stoppingDistance = attackDistance;
//         agent.speed = GameManager.Instance.Config.EnemySpeed;
//         attackDistance = GameManager.Instance.Config.AttackRange;
//         r = GetComponent<Rigidbody>();
//         r.useGravity = false;
//         r.isKinematic = true; 
//     }

//     private void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.tag == "bullet")
//         {
//             Destroy(collision.gameObject);

//             if (collision.gameObject.GetComponent<Bullet>().CurrentBulletType == m_bulletType)
//             {
//                 Debug.Log("Bullet hit enemy!");
//                 Destroy(gameObject);

//                 IEntity npc = collision.gameObject.transform.GetComponent<IEntity>();
//                 if (npc != null)
//                 {
//                     npc.ApplyDamage(GameManager.Instance.Config.BulletDamage);
//                 }
//             }
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (agent.remainingDistance - attackDistance < 0.01f)
//         {
//             // Fire logic
//             if(Time.time > nextAttackTime)
//             {
//                 nextAttackTime = Time.time + attackRate;

//                 //Attack
//                 RaycastHit hit;
//                 if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
//                 {
//                     if (hit.transform.CompareTag("Player"))
//                     {
//                         Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

//                         IEntity player = hit.transform.GetComponent<IEntity>();
//                         player.ApplyDamage(npcDamage);
//                     }
//                 }
//             }
//         }
//         //Move towardst he player
//         agent.destination = playerTransform.position;
//         //Always look at player
//         transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
//         //Gradually reduce rigidbody velocity if the force was applied by the bullet
//         r.velocity *= 0.99f;
        
//     }

//     public void ApplyDamage(float points)
//     {
//         npcHP -= points;
//         if(npcHP <= 0)
//         {
//             //Destroy the NPC
//             // GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
//             //Slightly bounce the npc dead prefab up
//             // npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
//             // Destroy(npcDead, 10);
//             es.EnemyEliminated(this);
//             Destroy(gameObject);
//         }
//     }
    
// }

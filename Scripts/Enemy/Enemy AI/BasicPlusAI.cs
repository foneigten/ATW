using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.EnemyAi
{
    public class BasicPlusAI : RPG.Combat.Enemy
    {
        private float canAttackTime; // attack timer
        float waitTime = 0; // timer to wait before going back to spawn position
        


        private void Awake()
        {
            homePosition = new Vector2(transform.position.x, transform.position.y);
            enrageCooldown = timeSinceEnraged;
        }

        // Static enemy
        private void Update()
        {
            timer += Time.deltaTime;
            canAttackTime -= Time.deltaTime;
            enrageTime -= Time.deltaTime; // used in stats evolving

            RandomMovement();

            ChasePlayer();

        }

        private void ChasePlayer()
        {
            if (target != null)
            {
                waitTime = 2f;
                if (isEnraged && target.gameObject.GetComponent<PlayerStats>().currentHealth != 0)
                {
                    if (isEnraged)
                    {
                        timeSinceEnraged -= Time.deltaTime;
                        if (Vector2.Distance(transform.position, target.position) > chaseRadius)
                        {
                            if (timeSinceEnraged <= 0)
                            {
                                timeSinceEnraged = enrageCooldown;
                                isEnraged = false;
                            }
                        }
                        if (Vector2.Distance(transform.position, target.position) < chaseRadius)
                        {
                            timeSinceEnraged = enrageCooldown;
                        }
                    }

                    // move to the player
                    if (Vector2.Distance(transform.position, target.position) > (attackRadius - 0.1f))
                    {
                        if (enemyrb.bodyType != RigidbodyType2D.Static)
                        {
                            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                            ChangeAnim(temp - transform.position);
                            enemyrb.MovePosition(temp);
                        }
                    }

                    // attack the player if it is in the attack range
                    if (target != null && Vector2.Distance(transform.position, target.position) <= attackRadius && canAttackTime <= 0)
                    {
                        if (enemyrb.bodyType != RigidbodyType2D.Static)
                        {
                            Debug.Log(enemyName + " is attacking with fury");
                            target.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
                            canAttackTime = attackSpeed;
                        }
                    }
                }
            }
        }

        private void RandomMovement()
        {
            if (!isEnraged || target.gameObject.GetComponent<PlayerStats>().currentHealth == 0)
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {

                    if (timer > 0)
                    {
                        if (timer < newTarget)
                        {
                            if (enemyrb.bodyType != RigidbodyType2D.Static)
                            {
                                Vector3 temp = Vector3.MoveTowards(transform.position, randomPointTarget, moveSpeed * Time.deltaTime);
                                ChangeAnim(temp - transform.position);
                                enemyrb.MovePosition(temp);
                            }
                        }
                        else
                        {
                            NewTarget();
                            timer = 0;
                        }
                    }
                }
            }
        }

    }
}

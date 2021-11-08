using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;
using RPG.Combat;
using RPG.Stats;

namespace RPG.EnemyAi
{
    public class AdvancedAI : Enemy
    {
        private float canAttackTime; // attack timer
        public float waitTime = 3; // timer to wait before going back to spawn position

        private void Awake()
        {
            homePosition = new Vector2(transform.position.x, transform.position.y);

            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.tag = "Enemy";
            anim = GetComponent<Animator>();
            enemyrb = GetComponent<Rigidbody2D>();
            enemyLevel = GetComponent<EnemyBaseStats>().enemyLevel;

            health = GetComponent<EnemyBaseStats>().GetStat(Stat.Health);
            damage = GetComponent<EnemyBaseStats>().GetStat(Stat.damage);
            armor = GetComponent<EnemyBaseStats>().GetStat(Stat.armor);
            coinsRewarded = GetComponent<EnemyBaseStats>().GetStat(Stat.coinToReward);
            expToReward = GetComponent<EnemyBaseStats>().GetStat(Stat.expToReward);
        }

        private void Start() {
            enemyNameText.text = enemyName;
            enemyLevelText.text = "Level " + enemyLevel;

            NewTarget();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            canAttackTime -= Time.deltaTime;
            enrageTime -= Time.deltaTime; // used in stats evolving

            // Get back to the home position if the player get out of range or is dead.
            RandomMovement();

            // Start chasing if the player get in his range.     
            ChasePlayer();
        }

        private void ChasePlayer()
        {
            if(!isDead)
            {
                if (target != null)
                {
                    if (Vector2.Distance(transform.position, target.position) <= chaseRadius && target.gameObject.GetComponent<PlayerStats>().currentHealth != 0 || isEnraged)
                    {
                        if (isEnraged)
                        {
                            timeSinceEnraged -= Time.deltaTime;
                            if (timeSinceEnraged <= 0)
                            {
                                timeSinceEnraged = enrageCooldown;
                                isEnraged = false;
                            }
                        }

                        // move to the player
                        if (Vector2.Distance(transform.position, target.position) > (attackRadius - 0.1f))
                        {
                            waitTime = 0;
                            if (enemyrb.bodyType != RigidbodyType2D.Static)
                            {
                                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                                if (anim != null)
                                {
                                    ChangeAnim(temp - transform.position);
                                }
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
            
        }

        private void RandomMovement()
        {
            if(target == null)
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


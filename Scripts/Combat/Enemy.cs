using System;
using System.Collections;
using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.Combat
{
    // public enum EnemyState{
    //     idle,
    //     walk,
    //     chase,
    //     attack,
    //     stagger
    // }

    public class Enemy : MonoBehaviour
    {
        [HideInInspector]
        // public EnemyState currentState;

        public Rigidbody2D enemyrb;
        public Animator anim;
        public bool isDead = false;

        [Header("Variables for Trigger Action")]
        string action = "quest";
        [SerializeField] UnityEvent onTrigger;

        [Tooltip("Time after which the enemy respawn.")]
        private float deathTime = 30f;
        [Header("Chase variables")]
        public float chaseRadius;
        public float attackRadius;
        public bool isEnraged;
        public float timeSinceEnraged = 3f; // the amount of seconds the enemy move towards the player when isEnraged being out of chase range

        // [Header("Variables to enrage near monsters")]
        // [Tooltip("The radiuns in which we enrage near monsters")]
        // public bool hasEnragedMonsters = false;
        // public float enrageInRadius = 7f; // the radius in which the enemy enrages near monsters
        public float enrageCooldown;

        [Tooltip("The radius in which the enemy random moves.")]
        public float moveRadius;
        // [HideInInspector]
        public Vector3 homePosition;
        public Transform target = null; //get the position of the player for chase and attack purposes

        [Header("Enemy Stats")]
        public string enemyName;
        public int enemyLevel;
        public Text enemyNameText;
        public Text enemyLevelText;
        public float health;
        public float damage;
        public float armor;
        public float attackSpeed = 1f;
        public float moveSpeed = 6f;
        [Tooltip("Used to evolve the Dungeon bosses on some conditions.")]
        public float enrageTime = 0; //to be used in stats evolving

        [Header("Random Movement Variables")]
        // [HideInInspector]
        public float timer;
        [Tooltip("Time after which the enemy selects a new random direction")]
        public float newTarget;
        public Vector3 randomPointTarget;

        [Header("Enemy Rewards")]
        public float coinsRewarded;
        public float expToReward;


        private void Start() {
            // gameObject.layer = LayerMask.NameToLayer("Enemy");
            // gameObject.tag = "Enemy";
            // anim = GetComponent<Animator>();
            // enemyrb = GetComponent<Rigidbody2D>();
            // enemyLevel = GetComponent<EnemyBaseStats>().enemyLevel;
            
            // health = GetComponent<EnemyBaseStats>().GetStat(Stat.Health);
            // damage = GetComponent<EnemyBaseStats>().GetStat(Stat.damage);
            // armor = GetComponent<EnemyBaseStats>().GetStat(Stat.armor);
            // coinsRewarded = GetComponent<EnemyBaseStats>().GetStat(Stat.coinToReward);
            // expToReward = GetComponent<EnemyBaseStats>().GetStat(Stat.expToReward);

            // enemyNameText.text = enemyName;
            // enemyLevelText.text = "Level " + enemyLevel;
        }

        public void TakeDamage(float dmg)
        {
            isEnraged = true;   //enrage the enemy when take damage
            dmg -= armor;
            if(dmg <= 0)
            {
                health -= 0;
                Debug.Log(enemyName + " blocked all the damage.");
            }

            if(dmg > 0)
            {
                health -= dmg;
                Debug.Log(enemyName + " took " + dmg);
                if (health <= 0)
                {
                    StartCoroutine(EnemyDeathAndRespawn(deathTime));
                                                         
                    //TODO give gold coins(cooper < silver < gold)
                }
            }
        }

        //Make the enemyes respawn after death
        private IEnumerator EnemyDeathAndRespawn(float seconds)
        {
            target.GetComponent<PlayerStats>().GainExperience(expToReward);   // give experience points to player
            TriggerAction(action); //complete the objective of the quest if any.

            EnemyIsDead();

            //TODO drop some items around or drop a chest that contain items + coinsRewarded
            GetComponent<RandomDropper>().RandomDrop();

            yield return new WaitForSeconds(seconds);


            RespawnEnemy();
        }

        private void EnemyIsDead()
        {
            isDead = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            // enemyrb.bodyType = RigidbodyType2D.Static;
            enemyNameText.gameObject.SetActive(false);
            enemyLevelText.gameObject.SetActive(false);
        }
        private void RespawnEnemy()
        {
            transform.position = homePosition;
            // enemyrb.bodyType = RigidbodyType2D.Kinematic;
            health = GetComponent<EnemyBaseStats>().GetStat(Stat.Health);
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            enemyNameText.gameObject.SetActive(true);
            enemyLevelText.gameObject.SetActive(true);
            target = null;
            isDead = false;

        }

        // Generate random normalized direction
        public static Vector3 GetRandomDir()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        }

        protected void SetAnimFloat(Vector2 setVector)
        {
            anim.SetFloat("moveX", setVector.x);
            anim.SetFloat("moveY", setVector.y);
        }

        protected void ChangeAnim(Vector2 direction)
        {
            if (direction.x != direction.y)
            {
                if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if(direction.x > 0)
                    {
                        anim.SetFloat("lastMoveX", 1);
                        anim.SetFloat("lastMoveY", 0);
                        SetAnimFloat(Vector2.right);                    
                    }
                    else if(direction.x < 0)
                    {
                        anim.SetFloat("lastMoveX", -1);
                        anim.SetFloat("lastMoveY", 0);
                        SetAnimFloat(Vector2.left);                    
                    }
                    
                }
                else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                {
                    if (direction.y > 0)
                    {
                        anim.SetFloat("lastMoveX", 0);
                        anim.SetFloat("lastMoveY", 1);
                        SetAnimFloat(Vector2.up);                    
                    }
                    else if (direction.y < 0)
                    {
                        anim.SetFloat("lastMoveX", 0);
                        anim.SetFloat("lastMoveY", -1);
                        SetAnimFloat(Vector2.down);                    
                    }
                }
            }
            else if (direction.x == direction.y)
            {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", 0);               
            }
        }

        // Gets the new random move-to position in the Radius around the spawn point.
        protected void NewTarget()
        {
            float myX = homePosition.x;
            float myY = homePosition.y;

            float xPos = UnityEngine.Random.Range(myX - moveRadius, myX + moveRadius);
            float yPos = UnityEngine.Random.Range(myY - moveRadius, myY + moveRadius);

            randomPointTarget = new Vector2(xPos, yPos);
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            // the radius in which the enemy is chasing
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            // the radius in which the enemy is random moving
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(homePosition, moveRadius);
        }

        private void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == action)
            {
                onTrigger.Invoke();
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;
            else
            {
                Trigger(action);
            }
        }

    }
}
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private float speed = 2f;
        private Rigidbody2D rb;
        public Collider2D target = null;
        private float damage; //the damage variable

        [Header("Effects")]
        [SerializeField] GameObject hitEffect = null;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        // private void Start()
        // {
        //     rb = GetComponent<Rigidbody2D>();
        // }

        private void OnEnable()
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<RPG.Combat.Fighter>().currentWeapon != null)
            {
                damage = GameObject.FindGameObjectWithTag("Player").GetComponent<RPG.Combat.Fighter>().damage;
            }
        }

        // Update is called once per frame
        void Update()
        {
            RotateTowardTarget();
            MoveToTarget();
        }

        public void SetTarget(Collider2D target)
        {
            this.target = target;
        }


        private void RotateTowardTarget()
        {
            Vector2 direction = (target.transform.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
        }

        private void MoveToTarget()
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.transform.position, speed);
            rb.MovePosition(temp);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage(damage);

                if(hitEffect != null)
                {
                    Instantiate(hitEffect, target.transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
            
        }

    }

}

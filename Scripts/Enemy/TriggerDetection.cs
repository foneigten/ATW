using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.EnemyAi
{
    public class TriggerDetection : MonoBehaviour
    {
        private float targetTime = 3f;

        private void Update() {
            targetTime -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponentInParent<Enemy>().target = other.transform;
                targetTime = 3f;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (targetTime <= 0)
                {
                    GetComponentInParent<Enemy>().target = null;
                }
            }
        }
    }
}

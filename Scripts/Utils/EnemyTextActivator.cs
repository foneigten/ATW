using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTextActivator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.SetActive(true);
            }
            // other.transform.GetChild(0).gameObject.SetActive(true);
            // other.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    float health;
    [SerializeField] RectTransform foreground = null;
    [SerializeField] Canvas rootCanvas = null;

    private void Start() {
        // health = GetComponentInParent<Enemy>().health;
    }

    void Update()
    {
        if (Mathf.Approximately(GetFraction(), 0) || Mathf.Approximately(GetFraction(), 1))
        {
            rootCanvas.enabled = false;
            return;
        }

        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(GetFraction(), 1, 1);
    }

    public float GetFraction()
    {
        health = GetComponentInParent<Enemy>().health;
        return health / GetComponentInParent<EnemyBaseStats>().GetStat(Stat.Health);
    }
}

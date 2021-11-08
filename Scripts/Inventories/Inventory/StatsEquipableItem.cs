using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Combat;
using UnityEditor;
using System;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [Header("Weapon stats")]
        [SerializeField] public float attackSpeed = 1;
        [SerializeField] public bool bowAttack;
        [SerializeField] public bool staffAttack;
        [SerializeField] public Projectile projectile = null;


        [SerializeField] List<Modifier> additiveModifiers = new List<Modifier>();
        [SerializeField] List<Modifier> percentageModifiers = new List<Modifier>();

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform player, Collider2D target, float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, player.position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }

        Vector3 ChooseArrowDirection()
        {
            float temp = Mathf.Atan2(GameObject.FindWithTag("Player").GetComponent<Control.PlayerController>().anim.GetFloat("lastMoveY"), GameObject.FindWithTag("Player").GetComponent<Control.PlayerController>().anim.GetFloat("lastMoveX")) * Mathf.Rad2Deg;
            return new Vector3(0, 0, temp - 135);
        }

        #region InventoryItemEditor Changes

        string FormatAttribute(Modifier mod, bool percent)
        {
            if ((int)mod.value == 0.0f) return "";
            string percentString = percent ? "percent" : "point";
            string bonus = mod.value > 0.0f ? "<color=#8888ff>bonus</color>" : "<color=#ff8888>penalty</color>";
            return $"{Mathf.Abs((int)mod.value)} {percentString} {bonus} to {mod.stat}\n";
        }

        public override string GetDescription()
        {
            string result = GetRawDescription() + "\n";
            foreach (Modifier mod in additiveModifiers)
            {
                result += FormatAttribute(mod, false);
            }

            foreach (Modifier mod in percentageModifiers)
            {
                result += FormatAttribute(mod, true);
            }
            return result;
        }


#if UNITY_EDITOR


        void AddModifier(List<Modifier> modifierList)
        {
            SetUndo("Add Modifier");
            modifierList?.Add(new Modifier());
            Dirty();
        }

        void RemoveModifier(List<Modifier> modifierList, int index)
        {
            SetUndo("Remove Modifier");
            modifierList?.RemoveAt(index);
            Dirty();
        }

        void SetStat(List<Modifier> modifierList, int i, Stat stat)
        {
            if (modifierList[i].stat == stat) return;
            SetUndo("Change Modifier Stat");
            Modifier mod = modifierList[i];
            mod.stat = stat;
            modifierList[i] = mod;
            Dirty();
        }

        void SetValue(List<Modifier> modifierList, int i, float value)
        {
            if (modifierList[i].value == value) return;
            SetUndo("Change Modifier Value");
            Modifier mod = modifierList[i];
            mod.value = value;
            modifierList[i] = mod;
            Dirty();
        }

        bool drawStatsEquipableItemData = true;
        bool drawAdditive = true;
        bool drawPercentage = true;

        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            drawStatsEquipableItemData =
                EditorGUILayout.Foldout(drawStatsEquipableItemData, "StatsEquipableItemData", foldoutStyle);
            if (!drawStatsEquipableItemData) return;

            drawAdditive = EditorGUILayout.Foldout(drawAdditive, "Additive Modifiers");
            if (drawAdditive)
            {
                DrawModifierList(additiveModifiers);
            }
            drawPercentage = EditorGUILayout.Foldout(drawPercentage, "Percentage Modifiers");
            if (drawPercentage)
            {
                DrawModifierList(percentageModifiers);
            }

        }


        void DrawModifierList(List<Modifier> modifierList)
        {
            int modifierToDelete = -1;
            GUIContent statLabel = new GUIContent("Stat");
            for (int i = 0; i < modifierList.Count; i++)
            {
                Modifier modifier = modifierList[i];
                EditorGUILayout.BeginHorizontal();
                SetStat(modifierList, i, (Stat)EditorGUILayout.EnumPopup(statLabel, modifier.stat, IsStatSelectable, false));
                SetValue(modifierList, i, EditorGUILayout.IntSlider("Value", (int)modifier.value, -20, 500));
                if (GUILayout.Button("-"))
                {
                    modifierToDelete = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (modifierToDelete > -1)
            {
                RemoveModifier(modifierList, modifierToDelete);
            }

            if (GUILayout.Button("Add Modifier"))
            {
                AddModifier(modifierList);
            }
        }

        bool IsStatSelectable(Enum candidate)
        {
            Stat stat = (Stat)candidate;
            if (stat == Stat.expToReward || stat == Stat.ExperienceToLevelUp) return false;
            return true;
        }


#endif

        #endregion

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using static RPG.Dialogue.DialogueTrigger;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] public bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] ActionEnum onEnterAction;
        [SerializeField] ActionEnum onExitAction;
        [SerializeField] Condition condition;


        public Rect GetRect() 
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public ActionEnum GetOnEnterAction()
        {
            return onEnterAction;
        }

        public ActionEnum GetOnExitAction()
        {
            return onExitAction;
        }

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators);
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");

            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");

                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");

            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;

            EditorUtility.SetDirty(this);
        }

#endif
    }
}


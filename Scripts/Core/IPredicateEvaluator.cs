using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public enum PredicateEnum
    {
        None,
        HasQuest,
        HasQuestItem,
        CompletedQuest
    }

    public interface IPredicateEvaluator 
    {
        bool? Evaluate(PredicateEnum predicate, string[] parameters); //nullable boolean where if the function is not true or false it will be null.
        
    }

}
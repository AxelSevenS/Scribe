using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class EventMultiCondition {

        public EventCondition condition;
        public EventSubCondition[] subConditions;


        public bool Evaluate() {
            if (condition.conditionType == EventCondition.ConditionType.Always) return true;

            bool conditionsMet = condition.Evaluate();
            foreach (EventSubCondition subCondition in subConditions) {
                conditionsMet = subCondition.Evaluate(conditionsMet);
            }
            return conditionsMet;
        }
    }
    
}

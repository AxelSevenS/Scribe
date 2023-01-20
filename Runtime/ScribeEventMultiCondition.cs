using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class ScribeEventMultiCondition {

        public ScribeEventCondition condition;
        public ScribeEventSubCondition[] subConditions;


        public bool Evaluate() {
            if (condition.conditionType == ScribeEventCondition.ConditionType.Always) return true;

            bool conditionsMet = condition.Evaluate();
            foreach (ScribeEventSubCondition subCondition in subConditions) {
                conditionsMet = subCondition.Evaluate(conditionsMet);
            }
            return conditionsMet;
        }
    }
    
}

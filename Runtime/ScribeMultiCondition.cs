using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    public abstract class ScribeMultiCondition {
        public abstract bool Evaluate();
    }

    [System.Serializable]
    public class ScribeMultiCondition<TCondition> : ScribeMultiCondition where TCondition : ScribeCondition {

        public TCondition condition;
        public ScribeSubCondition<TCondition>[] subConditions;


        public override bool Evaluate() {
            if (condition.binaryModifier == ScribeCondition.BinaryModifier.Always) return true;

            bool conditionsMet = condition.Evaluate();
            foreach (ScribeSubCondition<TCondition> subCondition in subConditions) {
                conditionsMet = subCondition.Evaluate(conditionsMet);
            }
            return conditionsMet;
        }
    }
    
}

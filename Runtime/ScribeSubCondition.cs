using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    public abstract class ScribeSubCondition {

        public BinaryOperationType binaryOperation;


        public enum BinaryOperationType {
            And,
            Or
        }
    }

    [System.Serializable]
    public class ScribeSubCondition<TCondition> : ScribeSubCondition where TCondition : ScribeCondition {

        public TCondition condition;

        public bool MultiConditionEvaluate(bool left, bool right) {
            return (binaryOperation == BinaryOperationType.And) ? (left & right) : (left | right);
        } 
    }
}

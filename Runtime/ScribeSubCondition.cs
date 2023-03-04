using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public abstract class ScribeSubCondition {

        public BinaryOperationType binaryOperation;


        public bool MultiConditionEvaluate(bool left, bool right) {
            return (binaryOperation == BinaryOperationType.And) ? (left & right) : (left | right);
        } 

        public enum BinaryOperationType {
            And,
            Or
        }
    }

    [System.Serializable]
    public class ScribeSubCondition<TCondition> : ScribeSubCondition where TCondition : ScribeCondition, new() {

        public TCondition condition = new TCondition();
    }
}

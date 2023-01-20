using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class ScribeEventSubCondition {

        public BinaryOperationType binaryOperation;
        public ScribeEventCondition condition;



        public bool Evaluate(bool left) {

            bool right = condition.Evaluate();
            return (binaryOperation == BinaryOperationType.And) ? (left & right) : (left | right);
        }


        public enum BinaryOperationType {
            And,
            Or
        }
    }
}

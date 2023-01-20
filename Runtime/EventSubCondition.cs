using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class EventSubCondition {

        public BinaryOperationType binaryOperation;
        public EventCondition condition;



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

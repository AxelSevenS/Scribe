using System;

namespace Scribe {

    [Serializable]
    public class ScribeSubCondition<TCondition> where TCondition : ScribeCondition, new() {

        public TCondition condition = new();

        public BinaryOperationType binaryOperation;


        public bool MultiConditionEvaluate(bool left, bool right) {
            return (binaryOperation == BinaryOperationType.And) ? (left & right) : (left | right);
        } 

        public enum BinaryOperationType {
            And,
            Or
        }
    }
}

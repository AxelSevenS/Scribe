using System.Collections.Generic;

using UnityEngine;

// using SeleneGame.Core.UI;

namespace Scribe {

    [System.Serializable]
    public class ScribeEventCondition {

        public ConditionType conditionType;
        public ScribeFlags.FlagType flagType;
        public string flagName;
        public OperatorType operatorType;
        public int flagValue;



        public bool Evaluate() {

            if (conditionType == ConditionType.Always)
                return true;

            int flagValue = ScribeFlags.GetFlag(flagName, flagType == ScribeFlags.FlagType.TemporaryFlag);

            bool postOperation = false;
            switch (operatorType) {
                case OperatorType.Equals:
                    postOperation = flagValue == this.flagValue;
                    break;
                case OperatorType.NotEquals:
                    postOperation = flagValue != this.flagValue;
                    break;
                case OperatorType.GreaterThan:
                    postOperation = flagValue > this.flagValue;
                    break;
                case OperatorType.LessThan:
                    postOperation = flagValue < this.flagValue;
                    break;
                case OperatorType.GreaterThanOrEquals:
                    postOperation = flagValue >= this.flagValue;
                    break;
                case OperatorType.LessThanOrEquals:
                    postOperation = flagValue <= this.flagValue;
                    break;
            }

            return conditionType == ConditionType.If ? postOperation : !postOperation;
        }


        public enum ConditionType {
            If,
            IfNot,
            Always
        }

        public enum OperatorType {
            Equals,
            NotEquals,
            GreaterThan,
            LessThan,
            GreaterThanOrEquals,
            LessThanOrEquals
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class FlagCondition : ScribeCondition {

        [ScribeHideLabel]
        public ScribeFlags.FlagType flagType;

        public string flagName;

        [ScribeHideLabel]
        public FlagOperationType operationType;

        public int flagValue;

        

        public bool Evaluate() {

            if (binaryModifier == BinaryModifier.Always)
                return true;

            int flagValue = ScribeFlags.GetFlag(flagName, flagType == ScribeFlags.FlagType.TemporaryFlag);

            bool postOperation = false;
            switch (operationType) {
                case FlagOperationType.Equals:
                    postOperation = flagValue == this.flagValue;
                    break;
                case FlagOperationType.NotEquals:
                    postOperation = flagValue != this.flagValue;
                    break;
                case FlagOperationType.GreaterThan:
                    postOperation = flagValue > this.flagValue;
                    break;
                case FlagOperationType.LessThan:
                    postOperation = flagValue < this.flagValue;
                    break;
                case FlagOperationType.GreaterThanOrEquals:
                    postOperation = flagValue >= this.flagValue;
                    break;
                case FlagOperationType.LessThanOrEquals:
                    postOperation = flagValue <= this.flagValue;
                    break;
            }

            return binaryModifier == BinaryModifier.If ? postOperation : !postOperation;

        }



        public enum FlagOperationType {
            Equals,
            NotEquals,
            GreaterThan,
            LessThan,
            GreaterThanOrEquals,
            LessThanOrEquals
        }
    }
}
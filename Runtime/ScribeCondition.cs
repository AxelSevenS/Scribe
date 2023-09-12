using System;

namespace Scribe {

    [Serializable]
    public abstract class ScribeCondition {

        [ScribeHideLabel]
        public BinaryModifier binaryModifier;

        public enum BinaryModifier {
            If,
            IfNot,
            Always
        }
    }
}

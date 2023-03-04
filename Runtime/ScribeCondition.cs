using System;
using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
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

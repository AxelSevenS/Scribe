using System;
using System.Collections.Generic;

using UnityEngine;

// using SeleneGame.Core.UI;

namespace Scribe {

    [System.Serializable]
    public abstract class ScribeCondition {

        public BinaryModifier binaryModifier;

        public abstract bool Evaluate();

        public enum BinaryModifier {
            If,
            IfNot,
            Always
        }
    }

    [System.Serializable]
    public abstract class ScribeCondition<TConditionType> : ScribeCondition where TConditionType : System.Enum {

        public TConditionType conditionType = default;

    }
}

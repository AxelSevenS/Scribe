using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    public abstract class ScribeEvent {
        // protected abstract bool EvaluateConditions();
        
        // protected abstract void InvokeEvent();
    }

    [System.Serializable]
    public abstract class ScribeEvent<TEvent, TCondition> : ScribeEvent where TEvent : System.Enum where TCondition : ScribeCondition {

        public ScribeMultiCondition<TCondition> conditions;

        public TEvent eventType;



        // protected sealed override bool EvaluateConditions() => conditions.Evaluate();
    }

}
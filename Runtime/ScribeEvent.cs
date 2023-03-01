using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    public abstract class ScribeEvent {
        public abstract bool Evaluate();
        
        protected abstract void Invoke();
    }

    [System.Serializable]
    public abstract class ScribeEvent<TEvent, TCondition> : ScribeEvent where TEvent : System.Enum where TCondition : ScribeCondition {

        public ScribeMultiCondition<TCondition> conditions;

        public TEvent eventType;



        public sealed override bool Evaluate() => conditions.Evaluate();
    }

}
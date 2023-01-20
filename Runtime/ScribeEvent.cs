using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    public abstract class ScribeEvent {}

    [System.Serializable]
    public abstract class ScribeEvent<TEvent> : ScribeEvent where TEvent : System.Enum  {

        public ScribeEventMultiCondition conditions;

        public TEvent eventType;



        public bool Evaluate() => conditions.Evaluate();
        
        public abstract void Invoke(GameObject dialogueObject);
    }

}
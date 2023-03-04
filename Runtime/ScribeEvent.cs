using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public abstract class ScribeEvent {
    }

    [System.Serializable]
    public abstract class ScribeEvent<TCondition> : ScribeEvent where TCondition : ScribeCondition, new() {

        public ScribeMultiCondition<TCondition> conditions = new ScribeMultiCondition<TCondition>();

    }

}
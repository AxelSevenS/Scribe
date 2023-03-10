using System.Collections.Generic;

using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class ScribeEvent<TAction, TCondition> where TAction : ScribeAction, new() where TCondition : ScribeCondition, new() {

        public ScribeMultiCondition<TCondition> conditions = new ScribeMultiCondition<TCondition>();
        public List<TAction> actions = new List<TAction>();

    }

}
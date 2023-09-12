using System.Collections.Generic;

namespace Scribe {

    [System.Serializable]
    public class ScribeEvent<TAction, TCondition> where TAction : ScribeAction, new() where TCondition : ScribeCondition, new() {

        public ScribeMultiCondition<TCondition> conditions = new();
        public List<TAction> actions = new();

    }

}
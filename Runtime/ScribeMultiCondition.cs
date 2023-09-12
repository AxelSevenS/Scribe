using System;
using System.Collections.Generic;

namespace Scribe {

    [Serializable]
    public class ScribeMultiCondition<TCondition> where TCondition : ScribeCondition, new() {

        public TCondition condition = new();
        public List<ScribeSubCondition<TCondition>> subConditions = new();

    }
    
}

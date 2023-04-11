using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class ScribeMultiCondition<TCondition> where TCondition : ScribeCondition, new() {

        public TCondition condition = new TCondition();
        public List<ScribeSubCondition<TCondition>> subConditions = new List<ScribeSubCondition<TCondition>>();

    }
    
}

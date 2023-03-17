using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    internal interface IScribeMultiCondition {}

    [System.Serializable]
    public class ScribeMultiCondition<TCondition> : IScribeMultiCondition where TCondition : ScribeCondition, new() {

        public TCondition condition = new TCondition();
        public List<ScribeSubCondition<TCondition>> subConditions = new List<ScribeSubCondition<TCondition>>();

    }
    
}

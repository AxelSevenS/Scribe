using System;

using UnityEngine;

namespace Scribe {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ScribeFieldAttribute : PropertyAttribute {

        public int eventType;

        public ScribeFieldAttribute(int eventType = -1) {
            this.eventType = eventType;
        }
    }
}

using System;

using UnityEngine;

namespace Scribe {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ScribeEventDataAttribute : PropertyAttribute {

        public int eventType;

        public ScribeEventDataAttribute(int eventType) {
            this.eventType = eventType;
        }
    }
}

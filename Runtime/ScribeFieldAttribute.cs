using System;

using UnityEngine;

namespace Scribe {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ScribeFieldAttribute : PropertyAttribute {

        public string optionName;
        public int optionValue;

        public ScribeFieldAttribute(string optionName, int optionValue) {
            this.optionName = optionName;
            this.optionValue = optionValue;
        }
    }
}

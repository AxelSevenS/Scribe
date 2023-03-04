using System;

using UnityEngine;

namespace Scribe {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ScribeOptionAttribute : PropertyAttribute {

        public ScribeOptionAttribute() {
        }
    }
}

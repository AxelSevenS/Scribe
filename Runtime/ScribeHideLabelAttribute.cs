using System;

using UnityEngine;

namespace Scribe {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ScribeHideLabelAttribute : PropertyAttribute {

        public ScribeHideLabelAttribute() {
        }
    }
}

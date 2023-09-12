using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    [System.Serializable]
    public class EditFlagAction : ScribeAction {

        [ScribeHideLabel]
        [ScribeOption] 
        public FlagOperation eventType;

        
        [ScribeHideLabel]
        [ScribeField(nameof(eventType), (int)FlagOperation.SetFlag)]
        [ScribeField(nameof(eventType), (int)FlagOperation.RemoveFlag)]
        public ScribeFlags.FlagType editedFlagType;


        [ScribeField(nameof(eventType), (int)FlagOperation.SetFlag)]
        [ScribeField(nameof(eventType), (int)FlagOperation.RemoveFlag)]
        public string editedFlagName;


        [ScribeField(nameof(eventType), (int)FlagOperation.SetFlag)]
        public int editedFlagValue;
        


        public void Invoke() {
            switch (eventType) {
                case FlagOperation.SetFlag:
                    ScribeFlags.SetFlag(editedFlagName, editedFlagValue, editedFlagType == ScribeFlags.FlagType.TemporaryFlag);
                    break;
                case FlagOperation.RemoveFlag:
                    ScribeFlags.RemoveFlag(editedFlagName, editedFlagType == ScribeFlags.FlagType.TemporaryFlag);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public enum FlagOperation {
            SetFlag,
            RemoveFlag,
        }
    }

}
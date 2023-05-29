using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scribe {

    public static class ScribeFlags {
        
        public static Dictionary<string, int> flags = new Dictionary<string, int>();
        public static Dictionary<string, int> tempFlags = new Dictionary<string, int>();

        public static event Action<string, int, FlagType> OnFlagChanged;


        public static void SetFlag(string flagName, int flagValue, bool isTemporary = false) {
            (isTemporary ? tempFlags : flags)[flagName] = flagValue;
            OnFlagChanged?.Invoke(flagName, flagValue, isTemporary ? FlagType.TemporaryFlag : FlagType.GlobalFlag);
        }

        public static void RemoveFlag(string flagName, bool isTemporary = false) {
            (isTemporary ? tempFlags : flags).Remove(flagName);
        }

        public static int GetFlag(string flagName, bool isTemporary = false) {
            var selectedFlags = (isTemporary ? tempFlags : flags);
            if (selectedFlags.ContainsKey(flagName)) {
                return selectedFlags[flagName];
            } else {
                return 0;
            }
        }

        

        public enum FlagType {
            GlobalFlag,
            TemporaryFlag
        }
        
    }
}

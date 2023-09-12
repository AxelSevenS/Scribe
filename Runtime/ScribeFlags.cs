using System;
using System.Collections.Generic;

namespace Scribe {

    public static class ScribeFlags {
        
        public static Dictionary<string, int> flags = new();
        public static Dictionary<string, int> tempFlags = new();

        public static event Action<string, int, FlagType> OnFlagChanged;


        public static void SetFlag(string flagName, int flagValue, bool isTemporary = false) {
            (isTemporary ? tempFlags : flags)[flagName] = flagValue;
            OnFlagChanged?.Invoke(flagName, flagValue, isTemporary ? FlagType.TemporaryFlag : FlagType.GlobalFlag);
        }

        public static void RemoveFlag(string flagName, bool isTemporary = false) {
            (isTemporary ? tempFlags : flags).Remove(flagName);
        }

        public static int GetFlag(string flagName, bool isTemporary = false) {
            Dictionary<string, int> selectedFlags = isTemporary ? tempFlags : flags;
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

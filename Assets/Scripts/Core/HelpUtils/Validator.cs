using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace HelpUtils
{
    #if UNITY_EDITOR
    public static class Validator
    {
        public static bool IsPrefab(GameObject gameObject)
        {
            return PrefabUtility.IsPartOfAnyPrefab(gameObject);
        }

        public static void ValidatePrefabHasComponent<T>(ref GameObject prefab, string argumentName, int skipStackFrames = 0)
        {
            if (!IsPrefab(prefab) || !prefab.TryGetComponent(out T component))
            {
                prefab = null;
            }
            ValidateNotNull(prefab, argumentName, skipStackFrames + 1);
        }

        public static void ValidatePrefab<T>(ref T prefab, string argumentName, int skipStackFrames = 0) where T : Component
        {
            ValidateNotNull(prefab, argumentName, skipStackFrames + 1);
            if (prefab != null && !IsPrefab(prefab.gameObject))
            {
                prefab = null;
                ValidateNotNull(prefab, argumentName, skipStackFrames + 1);
            }
        }

        public static void ValidateNotNull(object obj, string argumentName, int skipStackFrames = 0)
        {
            if (obj == null)
            {
                StackFrame frame = new StackFrame(1 + skipStackFrames);
                string callerName = frame.GetMethod().DeclaringType.Name;
                UnityEngine.Debug.LogError($"{callerName}.{argumentName} is null!");
            }
        }

        public static void ValidateNotNullNotAPrefab(GameObject obj, string argumentName)
        {
            ValidateNotNull(obj, argumentName, 1);
            if (IsPrefab(obj))
            {
                StackFrame frame = new StackFrame(1);
                string callerName = frame.GetMethod().DeclaringType.Name;
                UnityEngine.Debug.LogError($"{callerName}.{argumentName} is a prefab!");
            }
        }

        public static void ValidateNoNulls(IEnumerable<object> objects, string argumentName)
        {
            foreach (object obj in objects)
            {
                ValidateNotNull(obj, argumentName, 1);
            }
        }

        public static void ValidateMinCountNoNulls(IEnumerable<object> objects, int minCount, string argumentName)
        {
            if (objects == null)
            {
                string callerName = new StackFrame(1).GetMethod().DeclaringType.Name;
                UnityEngine.Debug.LogError($"{callerName}.{argumentName} is null!");
            }
            else
            {
                int count = 0;
                foreach (object obj in objects)
                {
                    ValidateNotNull(obj, $"{argumentName}[{count++}]", 1);
                }
                if (count < minCount)
                {
                    string callerName = new StackFrame(1).GetMethod().DeclaringType.Name;
                    UnityEngine.Debug.LogError($"Not enough elements of {callerName}.{argumentName}! Current count is {count} but must be at least {minCount}!");
                }
            }
        }

        public static void ValidatePrefabs<T>(ref GameObject[] prefabs, string argumentName)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                ValidatePrefabHasComponent<T>(ref prefabs[i], $"{argumentName}[{i}]", 1);
            }
            ValidateNotNull(prefabs, argumentName, 1);
        }
    }
    #endif
}
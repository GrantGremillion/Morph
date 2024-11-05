using UnityEngine;

namespace Helpers
{
    public static class Print
    {
        public static void Log(bool debug, params object[] args)
        {
            if (debug)
            {
                Debug.Log(string.Join(" ", args));
            }
        }

        public static void LogError(bool debug, params object[] args)
        {
            if (debug)
            {
                Debug.LogError(string.Join(" ", args));
            }
        }
    }
}
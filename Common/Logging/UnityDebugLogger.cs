using HideItBobby.Properties;
using System;
using UnityEngine;

namespace HideItBobby.Common.Logging
{
    internal sealed class UnityDebugLogger : ILogger
    {
        public void Info(string text)
        {
            try
            {
                Debug.Log($"[{ModProperties.LongName}] {text}");
            }
            catch
            {
#if DEV
                throw;
#else
                //ignore
#endif
            }
        }

        public void Warning(string text)
        {
            try
            {
                Debug.LogWarning($"[{ModProperties.LongName}] {text}");
            }
            catch
            {
#if DEV
                throw;
#else
                //ignore
#endif
            }
        }

        public void Error(string text)
        {
            try
            {
                Debug.LogError($"[{ModProperties.LongName}] {text}");
            }
            catch
            {
#if DEV
                throw;
#else
                //ignore
#endif
            }
        }
        public void Error(string text, Exception e)
        {
            try
            {
                Debug.LogError($"[{ModProperties.LongName}] {text}");
                Debug.LogError(e);
            }
            catch
            {
#if DEV
                throw;
#else
                //ignore
#endif
            }
        }
    }
}
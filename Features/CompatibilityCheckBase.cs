using HideItBobby.Common.Logging;
using System;
using System.Threading;
using UnityEngine;

namespace HideItBobby.Features
{
    internal abstract class CompatibilityCheckBase : ICompatibilityCheck
    {
#if DEV || PREVIEW
        private const int CheckCountTreshold = 10;
#else
        private const int CheckCountTreshold = 3;
#endif
        private int _checkCount;
        public int CheckCount => _checkCount;

        private bool _isCompatible = true;
        public bool IsCompatible
        {
            get
            {
                if (_checkCount > CheckCountTreshold) return _isCompatible;
                Interlocked.Increment(ref _checkCount);
                try
                {
                    var isCompatible = CheckCompatibility();
                    _isCompatible = isCompatible;
                }
                catch (Exception e)
                {
                    _isCompatible = false;
                    Log.Error($"{GetType().Name}.{nameof(CheckCompatibility)} failed", e);
                }
                return _isCompatible;
            }
        }

        protected abstract bool CheckCompatibility();

        public void Reset() => Interlocked.Exchange(ref _checkCount, 0);
    }
}
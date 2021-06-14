using System;

namespace HideItBobby.Common
{
    internal interface IDisposableEx : IDisposable
    {
        bool IsDisposed { get; }
    }
}
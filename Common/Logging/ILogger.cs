using System;

namespace HideItBobby.Common.Logging
{
    internal interface ILogger
    {
        void Info(string text);
        void Warning(string text);
        void Error(string text);
        void Error(string text, Exception e);
    }
}
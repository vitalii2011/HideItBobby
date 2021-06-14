using HideItBobby.Settings;
using System;
using System.Globalization;
using System.IO;

namespace HideItBobby.Common.Logging
{
    internal sealed class SyncFileLogger : ILogger
    {
#if DEV
        private const int LOG_RETENTION_DAYS = 1;
#elif PREVIEW
        private const int LOG_RETENTION_DAYS = 30;
#else
        private const int LOG_RETENTION_DAYS = 7;
#endif

        public string FileName { get; }

        public SyncFileLogger()
        {
            FileName = Path.Combine(Paths.LogsDir, $"{DateTime.Now.ToString("yyyy-MM-dd_HHmm", CultureInfo.InvariantCulture)}.log");
            RemoveOldFiles();
        }

        private static void RemoveOldFiles()
        {
            try
            {
                var daysToKeep = TimeSpan.FromDays(LOG_RETENTION_DAYS);
                foreach (var file in Directory.GetFiles(Paths.LogsDir, "*.log", SearchOption.TopDirectoryOnly))
                {
                    var createDate = File.GetCreationTime(file);
                    if (DateTime.Now - createDate > daysToKeep) File.Delete(file);
                }
            }
            catch
            {
#if DEV
                throw;
#endif
            }
        }

        public void Info(string text)
        {
            try
            {
                File.AppendAllText(FileName, $"[INFO] {text}\n");
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
                File.AppendAllText(FileName, $"[WARNING] {text}\n");
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
                File.AppendAllText(FileName, $"[ERROR] {text}\n");
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
                File.AppendAllText(FileName, $"[ERROR] {text}\n");
                File.AppendAllText(FileName, $"{e}\n");
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
using System;

namespace Forms.Logging
{
    public interface ILogs
    {
        void Log(Exception ex);
    }
}
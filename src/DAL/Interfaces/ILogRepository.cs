using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ILogRepository
    {
        void LogUserAction(string userId, string action);
        IList<LogEntry> GetUserActions(string userId);
    }
}

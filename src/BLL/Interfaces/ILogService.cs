using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILogService
    {
        void LogUserAction(string userId, string action);
        IList<LogEntryDTO> GetUserActions(string userId);
    }
}

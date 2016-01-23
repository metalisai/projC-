using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using BLL.DTO;

namespace BLL
{
    public class LogService : ILogService
    {
        IRepositoryProvider _repos;

        public LogService(IRepositoryProvider repos)
        {
            _repos = repos;
        }

        public IList<LogEntryDTO> GetUserActions(string userId)
        {
            return _repos.LogRepository.GetUserActions(userId).Select( x => new LogEntryDTO(x)).ToList();
        }

        public void LogUserAction(string userId, string action)
        {
            _repos.LogRepository.LogUserAction(userId, action);
        }
    }
}

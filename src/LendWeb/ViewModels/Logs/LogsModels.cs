using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LendWeb.ViewModels.Logs
{
    public class LogsModel
    {
        public IList<LogEntryDTO> LogEntries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class LogEntryDTO
    {
        public DateTime Created { get; set; }
        public string Description { get; set; }

        public LogEntryDTO()
        {
                
        }

        public LogEntryDTO(Model.LogEntry logEntry)
        {
            Created = logEntry.Created;
            Description = logEntry.Description;
        }
    }
}

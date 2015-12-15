using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class LendingDTO
    {
        public DateTime LentAt { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public string OtherUserName { get; set; }
    }
}

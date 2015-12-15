using BLL.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILendingService
    {
        IList<LendObject> GetUserLendObjects(string userId);

        IList<LendingDTO> GetUserLendings(string userId);

        IList<LendingDTO> GetUserBorrowings(string userId);
    }
}

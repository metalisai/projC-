using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace DAL.Interfaces
{
    public interface ILendingRepository
    {
        void LendUserObject(string userId, Lending lending);
        void Remove(string userId, string lendingId);
        void MarkRetured(string userId, string lendingId);
    }
}

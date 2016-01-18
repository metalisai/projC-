using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace DAL.Interfaces
{
    public interface ILendingRepository
    {
        void Remove(string userId, string lendingId);
        // TODO: this method doesn,t really belong here, does it?
        void MarkRetured(string userId, string lendObjectId);
        IList<Lending> GetUserLendings(string userId);
        IList<Lending> GetUserBorrowings(string userId);
        bool AddUserLending(string userId, Lending lending);
        bool AddUserBorrowing(string userId, Lending lending);
        Lending GetUserLending(string userId, string lendingId);
        Lending GetUserBorrowing(string userId, string lendingId);
    }
}

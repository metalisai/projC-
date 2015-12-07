using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace DAL.Interfaces
{
    public interface ILendObjectRepository
    {
        void Add(string userId, LendObject lo);
        void Remove(string userId, string objectId);
        IList<LendObject> GetUserObjects(string userId);
    }
}

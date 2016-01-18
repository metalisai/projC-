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
        LendObject GetUserObject(string userId, string objectId);
        void SetLendObjectLending(string userId, string objectId, string lendingId);
        void AddImageToLendObject(string userId, string objectId, string fileName);
        void AddPropertyToLendObject(string userId, string objectId, object property);
    }
}

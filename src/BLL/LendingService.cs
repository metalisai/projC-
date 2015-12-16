using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using DAL.Interfaces;
using BLL.DTO;
using BLL.Interfaces;

namespace BLL
{
    public class LendingService : ILendingService
    {
        IRepositoryProvider _repos;

        public LendingService(IRepositoryProvider repos)
        {
            _repos = repos;
        }

        // TODO: use DTO
        public IList<LendObjectDTO> GetUserLendObjects(string userId)
        {
            return _repos.LendObjectRepository.GetUserObjects(userId).Select(
                x => new LendObjectDTO(x) {
                    Status = GetUserObject(userId, x.Id).CurrentLending == null ? LendObjectDTO.LendObjectStatus.Available : LendObjectDTO.LendObjectStatus.Lent
                }).ToList();
        }

        public IList<LendingDTO> GetUserLendings(string userId)
        {
            return _repos.LendingRepository.GetUserLendings(userId).Select(x => new LendingDTO()
            {
                ExpectedReturn = x.ExpectedReturn.Value,
                LentAt = x.LentAt,
                OtherUserName = _repos.UserRepository.FindById(x.OtherUser).UserName,
                ItemName = _repos.LendObjectRepository.GetUserObject(x.OtherUser, x.LendObjectId).Name,
                ItemId = x.LendObjectId
            }).ToList();
        }

        public IList<LendingDTO> GetUserBorrowings(string userId)
        {
            return _repos.LendingRepository.GetUserBorrowings(userId).Select(x => new LendingDTO()
            {
                ExpectedReturn = x.ExpectedReturn.Value,
                LentAt = x.LentAt,
                OtherUserName = _repos.UserRepository.FindById(x.OtherUser).UserName,
                ItemName = _repos.LendObjectRepository.GetUserObject(x.OtherUser, x.LendObjectId).Name,
                ItemId = x.LendObjectId
            }).ToList();
        }

        public void LendUserObject(string userId, string objectId, string otherUserName)
        {
            bool borrowerResult = false;
            User otherUser = _repos.UserRepository.FindByUserName(otherUserName);
            if (otherUser != null)
            {
                // basically deep clone with 1 field changed
                var borrowerLending = new Lending();
                borrowerLending.OtherUser = userId;
                borrowerLending.LentAt = DateTime.Now;
                borrowerLending.ExpectedReturn = DateTime.Now.AddDays(7);
                borrowerLending.Returned = null;
                borrowerLending.LendObjectId = objectId;

                borrowerResult = _repos.LendingRepository.AddUserBorrowing(otherUser.Id, borrowerLending);
            }
            if (borrowerResult)
            {
                var lending = new Lending
                {
                    OtherUser = otherUser.Id,
                    LentAt = DateTime.Now,
                    ExpectedReturn = DateTime.Now.AddDays(7),
                    Returned = null,
                    LendObjectId = objectId
                };
                bool result = _repos.LendingRepository.AddUserLending(userId, lending);
                if(result)
                {
                    _repos.LendObjectRepository.SetLendObjectLending(userId, objectId, lending.Id);
                }
            }
        }

        public void AddUserLendObject(string userId, LendObjectDTO lobject)
        {
            _repos.LendObjectRepository.Add(userId, lobject.ToLendObject());
        }

        public LendObjectDTO GetUserObject(string userId, string objectId)
        {
            var obj = _repos.LendObjectRepository.GetUserObject(userId, objectId);
            if (obj != null)
                return new LendObjectDTO(_repos.LendObjectRepository.GetUserObject(userId, objectId))
                {
                    Status = obj == null ? LendObjectDTO.LendObjectStatus.Available : LendObjectDTO.LendObjectStatus.Lent
                };
            else
                return null;
        }

        public void UserObjectReturned(string userId, string objectId)
        {
            _repos.LendingRepository.MarkRetured(userId, objectId);
        }
    }
}

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

        private string GetLendingStatus(string userId, Lending lending)
        {
            var obj = GetUserObject(userId, lending.LendObjectId);
            return obj.CurrentLending == lending.Id ? "Not returned" : "Returned";
        }

        private string GetBorrowingStatus(string userId, Lending lending)
        {
            var obj = GetUserObject(userId, lending.LendObjectId);
            return obj.CurrentBorrowing == lending.Id ? "Not returned" : "Returned";
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
                OtherUserName = x.OtherUser != null ? _repos.UserRepository.FindById(x.OtherUser).UserName: x.BorrowerName,
                OtherUserId = x.OtherUser,
                ItemName = _repos.LendObjectRepository.GetUserObject(userId, x.LendObjectId).Name,
                ItemId = x.LendObjectId,
                Status = GetLendingStatus(userId, x)
            }).ToList();
        }

        public IList<LendingDTO> GetUserBorrowings(string userId)
        {
            return _repos.LendingRepository.GetUserBorrowings(userId).Select(x => new LendingDTO()
            {
                ExpectedReturn = x.ExpectedReturn.Value,
                LentAt = x.LentAt,
                OtherUserName = _repos.UserRepository.FindById(x.OtherUser).UserName,
                OtherUserId = x.OtherUser,
                ItemName = _repos.LendObjectRepository.GetUserObject(x.OtherUser, x.LendObjectId).Name,
                ItemId = x.LendObjectId,
                Status = GetBorrowingStatus(x.OtherUser, x)
            }).ToList();
        }

        public void LendUserObject(string userId, string objectId, string otherUserName)
        {
            bool borrowerResult = false;
            Lending borrowerLending = null;

            User otherUser = _repos.UserRepository.FindByUserName(otherUserName);
            if (otherUser != null)
            {
                // basically deep clone with 1 field changed
                borrowerLending = new Lending()
                {
                    OtherUser = userId,
                    LentAt = DateTime.Now,
                    ExpectedReturn = DateTime.Now.AddDays(7),
                    Returned = null,
                    LendObjectId = objectId
                };

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
                    _repos.LendObjectRepository.SetLendObjectBorrowing(userId, objectId, borrowerLending.Id);
                }
            }
        }

        public void LendUserObjectToContact(string userId, string objectId, string otherUserName, string otherEmail)
        {
            var lending = new Lending
            {
                BorrowerName = otherUserName,
                BorrowerEmail = otherEmail,
                LentAt = DateTime.Now,
                ExpectedReturn = DateTime.Now.AddDays(7),
                Returned = null,
                LendObjectId = objectId
            };
            bool result = _repos.LendingRepository.AddUserLending(userId, lending);
            if (result)
            {
                _repos.LendObjectRepository.SetLendObjectLending(userId, objectId, lending.Id);
            }
        }

        public string AddUserLendObject(string userId, LendObjectDTO lobject)
        {
            return _repos.LendObjectRepository.Add(userId, lobject.ToLendObject());
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

        public void AddImageToLendObject(string userId, string objectId, string fileName)
        {
            _repos.LendObjectRepository.AddImageToLendObject(userId, objectId, fileName);
        }

        public void AddPropertyToLendObject(string userId, string objectId, object property)
        {
            _repos.LendObjectRepository.AddPropertyToLendObject(userId, objectId, property);
        }
    }
}

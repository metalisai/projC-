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
        public IList<LendObject> GetUserLendObjects(string userId)
        {
            return _repos.LendObjectRepository.GetUserObjects(userId);
        }

        public IList<LendingDTO> GetUserLendings(string userId)
        {
            return _repos.LendingRepository.GetUserLendings(userId).Select(x => new LendingDTO()
            {
                ExpectedReturn = x.ExpectedReturn.Value,
                LentAt = x.LentAt,
                OtherUserName = _repos.UserRepository.FindById(x.OtherUser).UserName
            }).ToList();
        }

        public IList<LendingDTO> GetUserBorrowings(string userId)
        {
            return _repos.LendingRepository.GetUserBorrowings(userId).Select(x => new LendingDTO()
            {
                ExpectedReturn = x.ExpectedReturn.Value,
                LentAt = x.LentAt,
                OtherUserName = _repos.UserRepository.FindById(x.OtherUser).UserName
            }).ToList();
        }
    }
}

using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public class UsersService : IUsersService
    {
        IRepositoryProvider _repos;

        public UsersService(IRepositoryProvider repos)
        {
            _repos = repos;
        }

        public bool UserWithNameExists(string username)
        {
            // TODO: should probably write a repo method for it
            return _repos.UserRepository.FindByUserName(username) != null;
        }

        public bool UserWithIdExists(string id)
        {
            // TODO: should probably write a repo method for it
            return _repos.UserRepository.FindById(id) != null;
        }

        public UserDTO FindUserByName(string username)
        {
            return new UserDTO(_repos.UserRepository.FindByUserName(username));
        }

        public UserDTO FindUserById(string id)
        {
            return new UserDTO(_repos.UserRepository.FindById(id));
        }
    }
}

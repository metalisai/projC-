using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUsersService
    {
        bool UserWithNameExists(string username);
        UserDTO FindUserByName(string username);
        bool UserWithIdExists(string id);
        UserDTO FindUserById(string id);
    }
}

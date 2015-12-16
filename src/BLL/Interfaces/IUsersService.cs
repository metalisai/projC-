using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUsersService
    {
        bool UserExists(string username);
        UserDTO FindUserByName(string username);
    }
}

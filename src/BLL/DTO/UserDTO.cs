using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}

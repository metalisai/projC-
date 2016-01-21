using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;

namespace LendWeb.ViewModels.Users
{
    public class UserPageModel
    {
        public UserDTO User { get; set; }
    }

    public class ObjectPageModel
    {
        public UserDTO User { get; set; }
        public LendObjectDTO Object { get; set; }
    }

    public class SearchPageModel
    {
        public string Search { get; set; }
        public IList<UserDTO> Users { get; set; }
    }
}

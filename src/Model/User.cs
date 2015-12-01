using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class User : IdentityUser
    {
        public List<LendObject> LendObjects { get; set; }
        public List<Lending> Lendings { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class User : IdentityUser
    {
        public List<LendObject> LendObjects { get; set; }
        public List<Lending> Lendings { get; set; }
    }

    public class User2:BaseEntity
    {
        public List<LendObject> LendObjects { get; set; }
        public List<Lending> Lendings { get; set; }
    }
}

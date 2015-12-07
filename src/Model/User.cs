using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class User : IdentityUser
    {
        public List<LendObject> LendObjects { get; set; } = new List<LendObject>();
        public List<Lending> Lendings { get; set; } = new List<Lending>();
        public List<Lending> Borrowings { get; set; } = new List<Lending>();
    }
}

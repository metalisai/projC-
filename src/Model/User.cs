using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Model
{
    public class User : IdentityUser
    {
        [InverseProperty("Lender")]
        public List<Lending> Lendings { get; set; }
        [InverseProperty("Borrower")]
        public List<Lending> Borrowings { get; set; }

        [InverseProperty("Owner")]
        public List<LendObject> OwnedObjects { get; set; }
    }
}

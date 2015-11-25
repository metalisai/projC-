using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class Lending
    {
        public int Id { get; set; }
        public DateTime LentAt { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public DateTime Returned { get; set; }

        //[ForeignKey("User")]
        public string LenderId { get; set; }
        public User Lender { get; set; }

        //[ForeignKey("User")]
        public string BorrowerId { get; set; }
        public User Borrower { get; set; }
    }
}

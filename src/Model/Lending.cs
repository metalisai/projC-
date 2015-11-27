using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class Lending : BaseEntity
    {
        public DateTime LentAt { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public DateTime Returned { get; set; }
    }
}

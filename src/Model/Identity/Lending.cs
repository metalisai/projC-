using System;

namespace Model
{
    public class Lending : BaseEntity
    {
        public DateTime LentAt { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public DateTime Returned { get; set; }
    }
}

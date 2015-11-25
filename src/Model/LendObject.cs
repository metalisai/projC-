using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class LendObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }
    }
}

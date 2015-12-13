using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LendWeb.ViewModels.MyObjects
{
    public class MyObjectsModel
    {
        public IEnumerable<LendObject> MyObjects{ get; set; }
        public LendObject AddObject { get; set; }
    }

    public class LendModel
    {
        public string LendToUser { get; set; }

        public string LendToEmail { get; set; }
        public string LendToName { get; set; }
    }
}

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
}

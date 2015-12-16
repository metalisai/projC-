using BLL.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LendWeb.ViewModels.MyObjects
{
    public class MyObjectsModel
    {
        public IEnumerable<LendObjectDTO> MyObjects{ get; set; }

        public IEnumerable<LendingDTO> MyLendings { get; set; }

        public IEnumerable<LendingDTO> MyBorrowings { get; set; }

        public LendObjectDTO AddObject { get; set; }
    }

    public class LendModel
    {
        public LendModel()
        {

        }

        public LendModel(LendToUserModel model)
        {
            ObjectId = model.ObjectId;
            LendToUser = model.LendToUser;
        }

        public LendModel(LendToContactModel model)
        {
            ObjectId = model.ObjectId;
            LendToName = model.LendToName;
            LendToEmail = model.LendToEmail;
        }

        public enum BorrowerType
        {
            User,
            Contact
        }

        public string ObjectId { get; set; }
        public string LendToUser { get; set; }
        public string LendToEmail { get; set; }
        public string LendToName { get; set; }
        public BorrowerType Borrower { get; set; }
    }

    public class LendToUserModel
    {
        public string ObjectId { get; set; }
        public string LendToUser { get; set; }
    }

    public class LendToContactModel
    {
        public string ObjectId { get; set; }
        public string LendToEmail { get; set; }
        public string LendToName { get; set; }
    }
}

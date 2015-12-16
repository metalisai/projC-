using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class LendObjectDTO
    {
        public enum LendObjectStatus
        {
            Available,
            Lent
        }

        public DateTime Added { get; set; }
        public string CurrentLending { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public LendObjectStatus Status { get; set; }

        public LendObjectDTO()
        {
                
        }

        public LendObjectDTO(LendObject lobject)
        {
            Added = lobject.Added;
            CurrentLending = lobject.CurrentLending;
            Name = lobject.Name;
            Id = lobject.Id;
        }

        public LendObject ToLendObject()
        {
            return new LendObject
            {
                Added = this.Added,
                CurrentLending = this.CurrentLending,
                Name = this.Name,
                Id = this.Id
            };
        }
    }
}

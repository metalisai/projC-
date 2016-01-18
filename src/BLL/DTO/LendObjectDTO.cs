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
        public string CurrentBorrowing { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public LendObjectStatus Status { get; set; }
        public IList<string> Images { get; set; }
        public IList<LendObject.LoProperty> Properties { get; set; }

        public LendObjectDTO()
        {
                
        }

        public LendObjectDTO(LendObject lobject)
        {
            Added = lobject.Added;
            CurrentLending = lobject.CurrentLending;
            CurrentBorrowing = lobject.CurrentBorrowing;
            Name = lobject.Name;
            Id = lobject.Id;
            Images = lobject.Images??new List<string>();
            Properties = lobject.Properties;
        }

        public LendObject ToLendObject()
        {
            return new LendObject
            {
                Added = this.Added,
                CurrentLending = this.CurrentLending,
                CurrentBorrowing = this.CurrentBorrowing,
                Name = this.Name,
                Id = this.Id,
                Images = this.Images ?? new List<string>(),
                Properties = this.Properties ?? new List<LendObject.LoProperty>()
            };
        }
    }
}

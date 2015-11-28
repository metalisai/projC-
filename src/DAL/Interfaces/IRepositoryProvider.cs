using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepositoryProvider
    {
        IUserRepository UserRepository { get; }
    }
}

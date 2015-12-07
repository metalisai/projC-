using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepositoryProvider
    {
        IUserRepository UserRepository { get; }
        ILendingRepository LendingRepository { get; }
        ILendObjectRepository LendObjectRepository { get; }
    }
}

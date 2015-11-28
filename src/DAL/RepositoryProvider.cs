using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositoryProvider : IRepositoryProvider
    {
        IMongoDBClient _dbClient;

        IUserRepository _userRepo;

        public RepositoryProvider(IMongoDBClient dbClient)
        {
            _dbClient = dbClient;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepo == null)
                {
                    return (_userRepo = new UserRepository(_dbClient));
                }
                else
                {
                    return _userRepo;
                }
            }
        }
    }
}

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
        ILendingRepository _lendingRepo;
        ILendObjectRepository _lendObjRepo;
        ILogRepository _logRepo;

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

        public ILendingRepository LendingRepository
        {
            get
            {
                if (_lendingRepo == null)
                {
                    return (_lendingRepo = new LendingRepository(_dbClient));
                }
                else
                {
                    return _lendingRepo;
                }
            }
        }

        public ILendObjectRepository LendObjectRepository
        {
            get
            {
                if (_lendObjRepo == null)
                {
                    return (_lendObjRepo = new LendObjectRepository(_dbClient));
                }
                else
                {
                    return _lendObjRepo;
                }
            }
        }

        public ILogRepository LogRepository
        {
            get
            {
                if (_logRepo == null)
                {
                    return (_logRepo = new LogRepository(_dbClient));
                }
                else
                {
                    return _logRepo;
                }
            }
        }
    }
}

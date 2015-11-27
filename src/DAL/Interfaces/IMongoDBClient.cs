using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IMongoDBClient : IMongoClient
    {
        IMongoDatabase Database
        {
            get;
        }
    }
}

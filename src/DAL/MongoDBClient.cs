using DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class MongoDBClient : MongoClient, IMongoDBClient
    {
        string _databaseName;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="settings"></param>
        public MongoDBClient(IOptions<MongoDBConnectionSettings> settings) : base(settings.Value.Server)
        {
            _databaseName = settings.Value.Database;
        }

        /// <summary>
        /// Regular constructor for manual use;
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public MongoDBClient(string server, string database) : base(database)
        {
            _databaseName = database;
        }

        public IMongoDatabase Database
        {
            get
            {
                return GetDatabase(_databaseName);
            }
        }
    }
}

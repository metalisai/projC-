using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DAL.Repositories
{
    public class LendObjectRepository : ILendObjectRepository
    {
        IMongoDBClient _dbClient;
        IMongoDatabase _db;

        public LendObjectRepository(IMongoDBClient client)
        {
            _dbClient = client;
            _db = client.Database;
        }

        public void Add(string userId, LendObject lo)
        {
            var update = Builders<BsonDocument>.Update.Push("LendObjects", lo);
            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument("_id", new ObjectId(userId)), update);
        }

        // TODO: use projection
        public IList<LendObject> GetUserObjects(string userId)
        {
            var user = _db.GetCollection<User>("Users").Find(new BsonDocument("_id", new ObjectId(userId))).FirstOrDefault();
            return user.LendObjects ?? new List<LendObject>();
        }

        // TODO: use projection
        public LendObject GetUserObject(string userId, string objectId)
        {
            var user = _db.GetCollection<User>("Users").Find(new BsonDocument("_id", new ObjectId(userId))).FirstOrDefault();
            var list = user.LendObjects ?? new List<LendObject>();
            return list.Where(x => x.Id == objectId).FirstOrDefault();
        }

        public void Remove(string userId, string objectId)
        {
            throw new NotImplementedException();
        }
    }
}

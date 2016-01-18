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
            if(string.IsNullOrEmpty(lo.Id))
            {
                lo.Id = ObjectId.GenerateNewId().ToString();
            }
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

        public void SetLendObjectLending(string userId, string objectId, string lendingId)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var builder = Builders<BsonDocument>.Filter;
            var filter = Builders<BsonDocument>.Filter.And(builder.Eq("_id", new ObjectId(userId)), builder.Eq("LendObjects._id", new ObjectId(objectId)));
            var update = Builders<BsonDocument>.Update.Set("LendObjects.$.CurrentLending", lendingId);
            collection.UpdateOne(filter, update);
        }

        public void AddImageToLendObject(string userId, string objectId, string fileName)
        {
            var update = Builders<BsonDocument>.Update.Push("LendObjects.$.Images", fileName);

            var conditions = new List<BsonElement>();
            conditions.Add(new BsonElement("_id", new ObjectId(userId)));
            conditions.Add(new BsonElement("LendObjects._id", new ObjectId(objectId)));

            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument(conditions), update);
        }

        public void AddPropertyToLendObject(string userId, string objectId, object property)
        {
            var update = Builders<BsonDocument>.Update.Push("LendObjects.$.Properties", property);

            var conditions = new List<BsonElement>();
            conditions.Add(new BsonElement("_id", new ObjectId(userId)));
            conditions.Add(new BsonElement("LendObjects._id", new ObjectId(objectId)));

            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument(conditions), update);
        }
    }
}

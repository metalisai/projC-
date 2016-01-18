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
    public class LendingRepository : ILendingRepository
    {
        IMongoDBClient _dbClient;
        IMongoDatabase _db;

        public LendingRepository(IMongoDBClient client)
        {
            _dbClient = client;
            _db = client.Database;
        }

        public bool AddUserLending(string userId, Lending lending)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var update = Builders<BsonDocument>.Update.Push("Lendings", lending);
            var result = collection.UpdateOne(new BsonDocument("_id", new ObjectId(userId)), update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public bool AddUserBorrowing(string userId, Lending lending)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var update = Builders<BsonDocument>.Update.Push("Borrowings", lending);
            var result = collection.UpdateOne(new BsonDocument("_id", new ObjectId(userId)), update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public IList<Lending> GetUserLendings(string userId)
        {
            var collection = _db.GetCollection<User>("Users");
            var ret = collection.Find(x => x.Id == userId).Project(x => x.Lendings).ToList().FirstOrDefault();
            return ret ?? new List<Lending>();
        }

        public IList<Lending> GetUserBorrowings(string userId)
        {
            var collection = _db.GetCollection<User>("Users");
            var ret = collection.Find(x => x.Id == userId).Project(x => x.Borrowings).ToList().FirstOrDefault();
            return ret ?? new List<Lending>();
        }

        public Lending GetUserLending(string userId, string lendingId)
        {
            var collection = _db.GetCollection<User>("Users");
            var ret = collection.Find(x => x.Id == userId).Project(x => x.Lendings.Where(y => y.Id == lendingId)).FirstOrDefault().ToList().FirstOrDefault();
            return ret;
        }

        public Lending GetUserBorrowing(string userId, string lendingId)
        {
            var collection = _db.GetCollection<User>("Users");
            var ret = collection.Find(x => x.Id == userId).Project(x => x.Borrowings.Where(y => y.Id == lendingId)).FirstOrDefault().ToList().FirstOrDefault();
            return ret;
        }

        /*public IList<Lending> GetUserBorrowing(string userId, string lendingId)
        {
            var collection = _db.GetCollection<User>("Users");
            var ret = collection.Find(x => x.Id == userId).Project(x => x.Borrowings).ToList().FirstOrDefault();
            return ret ?? new List<Lending>();
        }*/

        public void MarkRetured(string userId, string lendObjectId)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var builder = Builders<BsonDocument>.Filter;
            var filter = Builders<BsonDocument>.Filter.And(builder.Eq("_id", new ObjectId(userId)), builder.Eq("LendObjects._id", new ObjectId(lendObjectId)));
            var update = Builders<BsonDocument>.Update.Unset("LendObjects.$.CurrentLending");
            collection.UpdateOne(filter, update);
            update = Builders<BsonDocument>.Update.Unset("LendObjects.$.CurrentBorrowing");
            collection.UpdateOne(filter, update);
        }

        public void Remove(string userId, string lendingId)
        {
            throw new NotImplementedException();
        }
    }
}

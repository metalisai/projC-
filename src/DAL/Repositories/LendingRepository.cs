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

        public void LendUserObject(string userId, Lending lending)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            if (lending.OtherUser != null)
            {
                // basically deep clone with 1 field changed
                var borrowerLending = new Lending();
                borrowerLending.OtherUser = new ObjectId(userId);
                borrowerLending.LentAt = lending.LentAt;
                borrowerLending.ExpectedReturn = lending.ExpectedReturn;
                borrowerLending.Returned = lending.Returned;
                borrowerLending.LendObjectId = lending.LendObjectId;
                
                var borrowerupdate = Builders<BsonDocument>.Update.Push("Borrowings", lending);
                collection.UpdateOne(new BsonDocument("_id", lending.OtherUser), borrowerupdate);
            }
            var update = Builders<BsonDocument>.Update.Push("Lendings", lending);
            collection.UpdateOne(new BsonDocument("_id", new ObjectId(userId)), update);
        }

        public void MarkRetured(string userId, string lendingId)
        {
            throw new NotImplementedException();
        }

        public void Remove(string userId, string lendingId)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using MongoDB.Driver;
using Model;
using MongoDB.Bson;

namespace DAL.Repositories
{
    public class LogRepository : ILogRepository
    {
        IMongoDBClient _client;
        IMongoDatabase _db;

        public LogRepository(IMongoDBClient dbClient)
        {
            _client = dbClient;
            _db = dbClient.Database;

        }

        public IList<LogEntry> GetUserActions(string userId)
        {
            var collection = _db.GetCollection<UserLog>("UserLogs");
            ObjectId oid;
            bool valid = ObjectId.TryParse(userId, out oid);
            if (valid && _db.GetCollection<BsonDocument>("UserLogs").Count(new BsonDocument("_id", oid)) > 0)
            {
                // TODO: do reversing on database level!
                var ret = valid ? collection.Find(new BsonDocument("_id", oid)).FirstOrDefault() : null;
                return ret.UserActions.Reverse().ToList();
            }
            else
            {
                return new List<LogEntry>();
            }
        }

        public void LogUserAction(string userId, string action)
        {
            var col = _db.GetCollection<BsonDocument>("UserLogs");
            var oid = new ObjectId(userId);

            if (col.Count(new BsonDocument("_id", oid)) <= 0)
            {
                var userLog = new UserLog() { userId = userId, UserActions = new List<LogEntry>() };
                var doc = userLog.ToBsonDocument();
                col.InsertOne(doc);
            }
            var uAction = new LogEntry() { Created = DateTime.Now, Description = action };
            var update = Builders<BsonDocument>.Update.Push("UserActions", uAction);
            col.UpdateOne(new BsonDocument("_id", oid), update);
        }
    }
}

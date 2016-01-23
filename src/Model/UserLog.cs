using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class LogEntry
    {
        public DateTime Created { get; set; }
        public string Description { get; set; }
    }
    public class UserLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string userId { get; set; }

        public IList<LogEntry> UserActions { get; set; }

    }
}

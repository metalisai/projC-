using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class LendObject : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Added { get; set; }
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrentLending { get; set; }
    }
}

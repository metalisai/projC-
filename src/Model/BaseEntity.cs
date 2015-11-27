using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class BaseEntity
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public ObjectId Id { get; set; }
    }
}

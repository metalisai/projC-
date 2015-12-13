using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Model
{
    public class Lending : BaseEntity
    {
        [BsonRequired]
        public DateTime LentAt { get; set; }
        public DateTime? ExpectedReturn { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? Returned { get; set; }
        /// <summary>
        /// Other side of the lending, in case of lending its the borrower and vice versa. Null if the borrower is not an user of this site.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OtherUser { get; set; }
        [BsonIgnoreIfNull]
        public string BorrowerName { get; set; }
        [BsonIgnoreIfNull]
        public string BorrowerEmail { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string LendObjectId { get; set; }
    }
}

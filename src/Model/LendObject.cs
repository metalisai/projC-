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
        public class LoProperty
        {
            public enum LoPropertyType
            {
                Unknown,
                Text,
                ShortText,
                Integer,
                Float,
                Date
            }

            public string PropertyName { get; set; }
            public object Property { get; set; }
            public override string ToString()
            {
                switch(GetLoType())
                {
                    case LoPropertyType.Date:
                        return ((DateTime)Property).ToShortDateString();
                    default:
                        return Property.ToString();
                }
            }

            // NOTE: maybe this makes no sense?
            public LoPropertyType GetLoType()
            {
                if (Property is string && ((string)Property).Length < 20)
                    return LoPropertyType.ShortText;
                else if (Property is string)
                    return LoPropertyType.Text;
                else if (Property is float)
                    return LoPropertyType.Float;
                else if (Property is int)
                    return LoPropertyType.Integer;
                else if (Property is DateTime)
                    return LoPropertyType.Date;
                else
                    return LoPropertyType.Unknown;
            }
        }

        public string Name { get; set; }
        public DateTime Added { get; set; }
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrentLending { get; set; }
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrentBorrowing { get; set; }
        public IList<string> Images { get; set; } = new List<string>();
        public IList<LoProperty> Properties = new List<LoProperty>();
    }
}

using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        IMongoDBClient _client;
        IMongoDatabase _db;

        [Flags]
        public enum UserInclude
        {
            Logins = 0,
            Claims = 1,
            Roles = 2
        }

        public UserRepository(IMongoDBClient dbClient)
        {
            _client = dbClient;
            _db = dbClient.Database;

        }

        public IList<User> All
        {
            get
            {
                return _db.GetCollection<User>("Users").Find(_ => true).ToList();
            }
        }

        public void AddUser(User user)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            if(user.Joined == null)
            {
                user.Joined = DateTime.Now;
            }
            var doc = user.ToBsonDocument();
            collection.InsertOne(doc);
            user.Id = doc["_id"].ToString();
        }

        public void AddUserClaim(User user, IdentityUserClaim identityUserClaim)
        {
            // TODO: what if the user doesn't exist?
            var update = Builders<BsonDocument>.Update.Push("Claims", identityUserClaim);
            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public void AddUserLogin(User user, IdentityUserLogin login)
        {
            // TODO: what if the user doesn't exist?
            var update = Builders<BsonDocument>.Update.Push("Logins", login);
            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public void AddUserToRole(User user, string roleName)
        {
            // TODO: what if the user doesn't exist?
            var role = new IdentityUserRole { Name = roleName };
            var update = Builders<BsonDocument>.Update.Push("Roles", role);
            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument("_id", user.Id), update);
            user.Roles.Add(role);
        }

        public User FindById(string userId)
        {
            var collection = _db.GetCollection<User>("Users");
            ObjectId oid;
            bool valid = ObjectId.TryParse(userId, out oid);
            return valid ? collection.Find(new BsonDocument("_id", oid)).FirstOrDefault() : null;
        }

        public User FindByUserName(string username)
        {
            var collection = _db.GetCollection<User>("Users");

            return collection.Find(new BsonDocument("NormalizedUserName", username.ToUpper())).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            var collection = _db.GetCollection<User>("Users");
            return collection.Find(new BsonDocument("NormalizedEmail", email)).FirstOrDefault();
        }

        public User GetUserByLogin(string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }

        public IList<IdentityUserClaim> GetUserClaims(User user)
        {
            // TODO: optimize so you don't query the entire object..
            var founduser = FindById(user.Id);
            return founduser != null ? founduser.Claims.ToList() : new List<IdentityUserClaim>();
        }

        public IList<IdentityUserLogin> GetUserLogins(User user)
        {
            var founduser = FindById(user.Id);
            return founduser != null ? founduser.Logins.ToList() : new List<IdentityUserLogin>();
        }

        public IList<string> GetUserRoles(User user)
        {
            var founduser = FindById(user.Id);
            if(founduser != null)
            {
                // TODO: probably should not keep roles in an object
                return founduser.Roles.Select(x => x.Name).ToList();
            }
            return new List<string>();
        }

        public IList<User> GetUsersForClaim(IdentityUserClaim identityUserClaim)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(User user)
        {
            var collection = _db.GetCollection<User>("Users");
            collection.DeleteOne(new BsonDocument("_id", user.Id));
        }

        public void RemoveUserClaims(User user, IdentityUserClaim oldclaim)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var update = Builders<BsonDocument>.Update.Set<IdentityUserClaim[]>("Claims", new IdentityUserClaim[0]);
            collection.UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public void RemoveUserFromRole(User user, string roleName)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var update = Builders<BsonDocument>.Update.Pull<BsonDocument>("Roles", new BsonDocument("Name", roleName));
            collection.UpdateOne(new BsonDocument("_id", user.Id), update);
            user.Roles.Remove(user.Roles.First(x => x.Name == roleName));
        }

        public void RemoveUserLogin(User user, string loginProvider, string providerKey)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var builder = Builders<BsonDocument>.Filter;
            var filter = Builders<BsonDocument>.Filter.And(builder.Eq("LoginProvider", loginProvider), builder.Eq("ProviderKey", providerKey));
            var update = Builders<BsonDocument>.Update.PullFilter("Logins", filter);
            collection.UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public void SetUserField(User user, string fieldName, object value)
        {
            if (!string.IsNullOrEmpty(user.Id))
            {
                var collection = _db.GetCollection<BsonDocument>("Users");
                var update = Builders<BsonDocument>.Update.Set(fieldName, value);
                collection.UpdateOne(new BsonDocument("_id", user.Id), update);
            }

            var userField = typeof(User).GetProperty(fieldName ,BindingFlags.Public | BindingFlags.Instance);
            if (userField != null)
            {
                userField.SetValue(user, value);
            }
            else
            {
                Console.WriteLine("Trying to set a field that doesn't exist!");
                // Log error?
            }
        }

        public IEnumerable<IdentityUserClaim> UpdateUserClaims(User user, IdentityUserClaim oldclaim, IdentityUserClaim newclaim)
        {
            throw new NotImplementedException();
        }

        public IList<User> SearchByName(string name)
        {
            var collection = _db.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Regex("NormalizedUserName", new BsonRegularExpression(name.ToUpper()));
            return collection.Find(filter).Limit(20).ToList();
        }
    }
}

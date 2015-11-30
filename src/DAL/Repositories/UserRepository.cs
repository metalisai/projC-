using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using DAL;
using Microsoft.Data.Entity;

using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        IMongoDBClient _client;
        IMongoDatabase _db;

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
            var doc = user.ToBsonDocument();
            collection.InsertOne(doc);
            user.Id = new ObjectId(doc["_id"].ToString());
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
            var update = Builders<BsonDocument>.Update.Push("Roles", new IdentityRole { Name = roleName });
            _db.GetCollection<BsonDocument>("Users").UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public User FindById(string userId)
        {
            var collection = _db.GetCollection<User>("Users");
            return collection.Find(new BsonDocument("_id", new ObjectId(userId))).FirstOrDefault();
        }

        public User FindById(ObjectId userId)
        {
            var collection = _db.GetCollection<User>("Users");
            return collection.Find(new BsonDocument("_id", userId)).FirstOrDefault();
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

        public IList<LendObject> GetUserObjects(string userId)
        {
            /*var collection = _db.GetCollection<BsonDocument>("Users");

            LendObject[] l1 = { new LendObject() { Added = DateTime.Now, Name = "manguKala" },
            new LendObject() { Added = DateTime.Now, Name = "manguKala" } };

            var usern = new User2() { LendObjects = l1.ToList() };
            var usern2 = new User2();
            var doc = usern.ToBsonDocument();
            collection.InsertOne(doc);
            //collection.InsertOne(usern2.ToBsonDocument());

            var user = collection.Find(new BsonDocument("_id", userId)).AsQueryable().FirstOrDefault();

            foreach (var lendobject in user.GetElement("LendObjects").Value.AsBsonArray)
            {
                Console.WriteLine(lendobject["Name"]);
            }
            //User usr = GetById(userId);*/
            return new List<LendObject>();
            //throw new NotImplementedException();
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
        }

        public void RemoveUserLogin(User user, string loginProvider, string providerKey)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");
            var builder = Builders<BsonDocument>.Filter;
            var filter = Builders<BsonDocument>.Filter.And(builder.Eq("LoginProvider", loginProvider), builder.Eq("ProviderKey", providerKey));
            var update = Builders<BsonDocument>.Update.PullFilter("Logins", filter);
            collection.UpdateOne(new BsonDocument("_id", user.Id), update);
        }

        public void Update(User user)
        {
            // TODO: i don't think this is the smartest idea
            var collection = _db.GetCollection<User>("Users");
            collection.ReplaceOne(new BsonDocument("_id", user.Id), user);
        }

        public IEnumerable<IdentityUserClaim> UpdateUserClaims(User user, IdentityUserClaim oldclaim, IdentityUserClaim newclaim)
        {
            throw new NotImplementedException();
        }
    }
}

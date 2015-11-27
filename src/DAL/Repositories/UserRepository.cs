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
                throw new NotImplementedException();
            }
        }

        public void AddUserClaim(User user, IdentityUserClaim identityUserClaim)
        {
            throw new NotImplementedException();
        }

        public void AddUserLogin(User user, IdentityUserLogin l)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public User FindByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLogin(string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }

        public IList<IdentityUserClaim> GetUserClaims(User user)
        {
            throw new NotImplementedException();
        }

        public IList<IdentityUserLogin> GetUserLogins(User user)
        {
            throw new NotImplementedException();
        }

        public IList<LendObject> GetUserObjects(string userId)
        {
            var collection = _db.GetCollection<BsonDocument>("Users");

            LendObject[] l1 = { new LendObject() { Added = DateTime.Now, Name = "manguKala" },
            new LendObject() { Added = DateTime.Now, Name = "manguKala" } };

            var usern = new User2() { LendObjects = l1.ToList() };
            var usern2 = new User2();
            var doc = usern.ToBsonDocument();
            collection.InsertOne(doc);
            //collection.InsertOne(usern2.ToBsonDocument());

            var user = collection/*.Find(new BsonDocument("_id", userId))*/.AsQueryable().FirstOrDefault();

            foreach (var lendobject in user.GetElement("LendObjects").Value.AsBsonArray)
            {
                Console.WriteLine(lendobject["Name"]);
            }
            //User usr = GetById(userId);
            return new List<LendObject>();
        }

        public IList<string> GetUserRoles(User user)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUsersForClaim(IdentityUserClaim identityUserClaim)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserClaims(User user, IdentityUserClaim oldclaim)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromRole(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserLogin(User user, string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityUserClaim> UpdateUserClaims(User user, IdentityUserClaim oldclaim, IdentityUserClaim newclaim)
        {
            throw new NotImplementedException();
        }
    }
}

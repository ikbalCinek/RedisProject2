using Redis.Cache.Abstract;
using Redis.Cache.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Redis.Cache.Concrete
{
    public class UserCacheService : ICacheService<User>
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private const string userKey = "userCache";
        public UserCacheService()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _database = _redis.GetDatabase(RedisDatabases.Users);
        }

        public bool DeleteValue(string key)
        {
            if (_database.HashDelete(userKey, key)) return true;
            return false;
        }

        public List<User> GetAll()
        {
            var userList=new List<User>();  
            var cacheUser=_database.HashGetAll(userKey);
            foreach (var item in cacheUser)
            {
                var user = JsonSerializer.Deserialize<User>(item.Value);
                userList.Add(user);
            }
            return userList;
        }

        public User GetValue(string key)
        {
            if(_database.KeyExists(userKey))
            {
                var user = _database.HashGet(userKey, key);
                return user.HasValue ? JsonSerializer.Deserialize<User>(user) : null;
            }
            return null;
        }

        public void SetValue(User value)
        {
            _database.HashSet(userKey, value.Id.ToString(), JsonSerializer.Serialize(value));
        }
    }
}

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
    public class CityCacheService : ICacheService<City>
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public CityCacheService()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _database = _redis.GetDatabase(RedisDatabases.Cities);
        }


        public bool DeleteValue(string key)
        {
            if(_database.KeyDelete(key)) return true;
            return false;
        }

        public List<City> GetAll()
        {
            var cityList=new List<City>();
            var keys = _redis.GetServer("localhost:6379").Keys(RedisDatabases.Cities);
            foreach (var item in keys)
            {
                if(_database.StringGet(item).HasValue)
                {
                    cityList.Add(JsonSerializer.Deserialize<City>(_database.StringGet(item)));
                }
            }
            return cityList;
        }

        public City GetValue(string key)
        {
            if(_database.KeyExists(key))
            {
                var city=_database.StringGet(key);
                return city.HasValue ? JsonSerializer.Deserialize<City>(city) : null;
            }
            return null;
        }

        public void SetValue(City value)
        {
            _database.StringSet(value.Id.ToString(), JsonSerializer.Serialize(value));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Cache.Abstract
{
    public interface ICacheService<T>
    {
        void SetValue(T value);

        T GetValue(string key);

        List<T> GetAll();

        bool DeleteValue(string key);

    }
}

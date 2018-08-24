using System;
using System.Collections.Generic;

namespace Warframebot.Storage.Implementation
{
    public class InMemoryStorage : IDataStorage
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public void StoreObject(object obj, string key)
        {
            if (_dictionary.ContainsKey(key)) return;
            _dictionary.Add(key,obj);
        }

        public T RestoreObject<T>(string key)
        {
            if (!_dictionary.ContainsKey(key))
            { throw new ArgumentNullException($"The provided key was not found!");}

            return (T)_dictionary[key];
        }
    }
}
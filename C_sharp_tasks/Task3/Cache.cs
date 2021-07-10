using System;
using System.Collections.Generic;

namespace Task3
{
    public class Cache<T>
    {
        private readonly TimeSpan _lifetime;
        private readonly int _maxSize;
        private readonly Dictionary<string, Entry> _entries;
        
        private class Entry
        {
            public Entry(T value, DateTime creationTime)
            {
                _value = value;
                _creationTime = creationTime;
            }
            
            public readonly T _value;
            public readonly DateTime _creationTime;
        }
        
        public Cache(TimeSpan lifetime, int maxSize)
        {
            if (lifetime <= TimeSpan.Zero || maxSize <= 0)
            {
                throw new ArgumentException();
            }
            
            _lifetime = lifetime;
            _maxSize = maxSize;
            _entries = new();
        }
        
        public T Get(string key)
        {
            Update();
            
            if (!_entries.ContainsKey(key))
            {
                throw new KeyNotFoundException();
            }
            
            return _entries[key]._value;
        }

        public void Save(string key, T value)
        {
            Update();

            if (_entries.ContainsKey(key))
            {
                throw new ArgumentException();
            }
            
            if (_maxSize == _entries.Count)
            {
                DateTime minTime = DateTime.MaxValue;
                string keyToRemove = null;

                foreach (var (currentKey, currentEntry) in _entries)
                {
                    if (currentEntry._creationTime < minTime)
                    {
                        minTime = currentEntry._creationTime;
                        keyToRemove = currentKey;
                    }
                }

                _entries.Remove(keyToRemove);

            }
            
            _entries.Add(key, new Entry(value, DateTime.Now));
        }

        private void Update()
        {
            foreach (var (key, entry) in _entries)
            {
                if (DateTime.Now - entry._creationTime > _lifetime)
                {
                    _entries.Remove(key);
                } 
            }
        }
    }
}
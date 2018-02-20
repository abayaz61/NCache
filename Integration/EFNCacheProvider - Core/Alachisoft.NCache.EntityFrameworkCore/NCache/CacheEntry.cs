﻿using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;

namespace Alachisoft.NCache.EntityFrameworkCore
{
    [Serializable]
    internal class CacheEntry
    {
        private object _key;
        private object _value;

        private TimeSpan _slidingExpTime;
        private DateTime _absoluteExpTime;
        private ExpirationType _expirationType;
        private string _queryIdentifier;
        private StoreAs _storedAs = StoreAs.Collection;
        private Alachisoft.NCache.Runtime.CacheItemPriority _priority;
        private bool _createDbDependency;
        private CacheDependency _dbDependency;

        [NonSerialized]
        private Alachisoft.NCache.Web.Caching.Cache _cache;

        public object Key => _key;

        public object Value
        {
            get => _value;
            set => _value = value;
        }

        public CacheDependency Dependencies
        {
            get => _dbDependency;
            set => _dbDependency = value;
        }

        public DateTime AbsoluteExpirationTime
        {
            get => _absoluteExpTime;
            set => _absoluteExpTime = value;
        }

        public TimeSpan SlidingExpirationTime
        {
            get => _slidingExpTime;
            set => _slidingExpTime = value;
        }

        public ExpirationType ExpirationType
        {
            get => _expirationType;
            set => _expirationType = value;
        }

        public string QueryIDentifier
        {
            get => _queryIdentifier;
            set => _queryIdentifier = value;
        }

        public StoreAs StoredAs
        {
            get => _storedAs;
            set => _storedAs = value;
        }

        public Alachisoft.NCache.Runtime.CacheItemPriority Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public bool CreateDbDependency
        {
            get => _createDbDependency;
            set => _createDbDependency = value;
        }

        public CacheEntry(object key, Alachisoft.NCache.Web.Caching.Cache cache)
        {
            _key = key;
            _cache = cache;
        }

        public static Tag[] GetTags(string[] stringTags)
        {
            List<Tag> tags = new List<Tag>();
            foreach (string tag in stringTags)
                tags.Add(new Tag(tag));

            return tags.ToArray();
        }
    }
}

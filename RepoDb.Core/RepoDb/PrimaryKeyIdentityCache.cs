﻿using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;

namespace RepoDb
{
    /// <summary>
    /// A static class used to get the cached value of data entity primary property as an identity.
    /// </summary>
    internal static class PrimaryKeyIdentityCache
    {
        private static readonly ConcurrentDictionary<string, bool> m_cache = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// Gets the value that defines whether the data entity has primary key is identity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection">The connection object to be used.</param>
        /// <returns>A boolean value indicating the identification of the column.</returns>
        public static bool Get<TEntity>(IDbConnection connection)
           where TEntity : class
        {
            var key = typeof(TEntity).FullName;
            var value = false;
            if (!m_cache.TryGetValue(key, out value))
            {
                var primary = PrimaryKeyCache.Get<TEntity>();
                if (primary != null)
                {
                    var fields = DbFieldCache.Get<TEntity>(connection);
                    if (fields != null)
                    {
                        var field = fields
                            .FirstOrDefault(fd =>
                                string.Equals(fd.Name, primary.GetMappedName(), StringComparison.CurrentCultureIgnoreCase));
                        value = field?.IsIdentity == true;
                    }
                }
                m_cache.TryAdd(key, value);
            }
            return value;
        }
    }
}

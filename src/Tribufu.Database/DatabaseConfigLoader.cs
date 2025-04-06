// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.Extensions.Configuration;

namespace Tribufu.Database
{
    /// <summary>
    /// Provides logic to load database configuration from an <see cref="IConfiguration"/> source.
    /// </summary>
    public static class DatabaseConfigLoader
    {
        /// <summary>
        /// Loads the <see cref="DatabaseConfig"/> from the "database" section or from root-level keys prefixed with "database_".
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <returns>The populated <see cref="DatabaseConfig"/> instance.</returns>
        public static DatabaseConfig LoadFrom(IConfiguration configuration)
        {
            var section = configuration.GetSection("database");
            var useRootFallback = !section.Exists();

            string GetConfig(string key) => useRootFallback ? configuration[$"database_{key}"] : section[key];

            var driverString = GetConfig("driver") ?? throw new Exception("Missing database driver");
            if (!Enum.TryParse<DatabaseDriver>(driverString, true, out var driver))
            {
                throw new Exception($"Unsupported database driver: {driverString}");
            }

            return new DatabaseConfig
            {
                Driver = driver,
                Version = GetConfig("version"),
                Host = GetConfig("host"),
                Port = GetConfig("port"),
                User = GetConfig("user"),
                Password = GetConfig("password"),
                Schema = GetConfig("schema")
            };
        }
    }
}

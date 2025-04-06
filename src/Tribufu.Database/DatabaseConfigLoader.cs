// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.Extensions.Configuration;
using System;

namespace Tribufu.Database
{
    public static class DatabaseConfigLoader
    {
        public static DatabaseConfig LoadFrom(IConfiguration configuration)
        {
            var section = configuration.GetSection("database");
            if (!section.Exists())
            {
                throw new InvalidOperationException("Missing 'database' section in configuration.");
            }

            var driverString = section["driver"] ?? throw new Exception("Missing database driver");
            if (!Enum.TryParse<DatabaseDriver>(driverString, true, out var driver))
            {
                throw new Exception($"Unsupported database driver: {driverString}");
            }

            return new DatabaseConfig
            {
                Driver = driver,
                Version = section["version"],
                Host = section["host"],
                Port = section["port"],
                User = section["user"],
                Password = section["password"],
                Schema = section["schema"]
            };
        }
    }

}

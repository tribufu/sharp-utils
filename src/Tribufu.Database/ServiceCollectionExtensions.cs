// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tribufu.Framework;

namespace Tribufu.Database
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredDbContext<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            var config = DatabaseConfigLoader.LoadFrom(configuration);

            services.AddDbContext<T>(options =>
            {
                switch (config.Driver)
                {
                    case DatabaseDriver.MySql:
                        var mysqlConnection = $"Server={config.Host};Port={config.Port};Uid={config.User};Pwd={config.Password};Database={config.Schema};ConvertZeroDateTime=True;";
                        options.UseMySql(mysqlConnection, ServerVersion.Parse(config.Version ?? "8.0"), mySqlOptions => { });

                        break;
                    case DatabaseDriver.Postgres:
                        var pgsqlConnection = $"Host={config.Host};Port={config.Port};Database={config.Schema};Username={config.User};Password={config.Password};";
                        options.UseNpgsql(pgsqlConnection, npgsqlOptions => { });

                        break;
                    case DatabaseDriver.Sqlite:
                        var savedDirectory = TribufuAppContext.GetSavedDirectory();
                        if (!Directory.Exists(savedDirectory)) Directory.CreateDirectory(savedDirectory);

                        var sqliteDatabaseFile = string.IsNullOrEmpty(config.Schema) ? "default.db" : $"{config.Schema}.db";
                        var sqliteDatabasePath = Path.Combine(savedDirectory, sqliteDatabaseFile);
                        options.UseSqlite($"Data Source={sqliteDatabasePath}", sqliteOptions => { });

                        break;
                    case DatabaseDriver.MongoDb:
                        var mongoUriBuilder = new MongoUrlBuilder
                        {
                            Server = new MongoServerAddress(config.Host, int.Parse(config.Port ?? "27017")),
                            Username = config.User,
                            Password = config.Password,
                            DatabaseName = config.Schema
                        };

                        var mongoClient = new MongoClient(mongoUriBuilder.ToMongoUrl());
                        var mongoDatabase = mongoClient.GetDatabase(config.Schema ?? "default");
                        options.UseMongoDB(mongoDatabase.Client, mongoDatabase.DatabaseNamespace.DatabaseName);

                        break;
                    default:
                        throw new NotSupportedException($"Unsupported database driver: {config.Driver}");
                }
            });

            return services;
        }
    }
}

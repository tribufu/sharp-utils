// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

namespace Tribufu.Database
{
    public class DatabaseConfig
    {
        public DatabaseDriver Driver { get; set; }

        public string? Version { get; set; }

        public string? Host { get; set; }

        public string? Port { get; set; }

        public string? User { get; set; }

        public string? Password { get; set; }

        public string? Schema { get; set; }
    }
}

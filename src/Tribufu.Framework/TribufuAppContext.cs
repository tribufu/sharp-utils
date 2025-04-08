// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Tribufu.Framework
{
    /// <summary>
    /// Provides standardized access to important application directories, such as config, saved data, logs, and platform-specific binaries.
    /// This is especially useful for abstracting file path logic across environments (development, production, etc).
    /// </summary>
    public static class TribufuAppContext
    {
        /// <summary>
        /// Checks if the application is running in a development environment.
        /// This is determined by checking the presence of debug assertions.
        /// </summary>
        /// <returns> True if running in a development environment; otherwise, false.</returns>
        private static bool HasDebugAssertions()
        {
#if DEBUG
            return true;
#else
            // Extra heuristics if needed (expand here if running outside DEBUG context)
            //return Debugger.IsAttached || Environment.GetEnvironmentVariable("DEBUG") == "true";
            return false;
#endif
        }

        /// <summary>
        /// Gets the root base directory of the application.
        /// </summary>
        /// <remarks>
        /// - In development, this resolves to the root of the repository (five levels above bin/Debug or bin/Release).
        /// - In production, it resolves to two levels above the binary location.
        /// - It uses case-insensitive checks and runtime heuristics to improve accuracy.
        /// </remarks>
        /// <returns>The absolute path to the base directory.</returns>
        public static string GetBaseDirectory()
        {
            try
            {
                string baseDirectory;
                string defaultBaseDirectory = AppContext.BaseDirectory;

                bool isDevelopment = HasDebugAssertions() || defaultBaseDirectory.ToLowerInvariant().Contains("debug");

                if (isDevelopment)
                {
                    // Go 5 levels up to simulate project root
                    baseDirectory = Path.Combine(defaultBaseDirectory, "..", "..", "..", "..", "..");
                }
                else
                {
                    baseDirectory = Path.Combine(defaultBaseDirectory, "..", "..");
                }

                return Path.GetFullPath(baseDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[TribufuAppContext] Failed to resolve base directory: {ex.Message}");
                return AppContext.BaseDirectory;
            }
        }

        /// <summary>
        /// Gets the path to the platform-specific binary directory.
        /// </summary>
        /// <returns>
        /// The absolute path to <c>bin/&lt;runtime-identifier&gt;</c> if available,
        /// otherwise falls back to <c>bin/dotnet</c>.
        /// </returns>
        public static string GetBinDirectory()
        {
            var binDirectory = Path.Combine(GetBaseDirectory(), "bin");

            if (!string.IsNullOrEmpty(RuntimeInformation.RuntimeIdentifier))
            {
                binDirectory = Path.Combine(binDirectory, RuntimeInformation.RuntimeIdentifier);
            }
            else
            {
                binDirectory = Path.Combine(binDirectory, "dotnet");
            }

            return binDirectory;
        }

        /// <summary>
        /// Gets the path to the configuration directory.
        /// </summary>
        /// <returns>The absolute path to the <c>config</c> directory.</returns>
        public static string GetConfigDirectory()
        {
            return Path.Combine(GetBaseDirectory(), "config");
        }

        /// <summary>
        /// Gets the path to the assets directory.
        /// </summary>
        /// <returns>The absolute path to the <c>assets</c> directory.</returns>
        public static string GetAssetsDirectory()
        {
            return Path.Combine(GetBaseDirectory(), "assets");
        }

        /// <summary>
        /// Gets the path to the saved data directory.
        /// </summary>
        /// <returns>The absolute path to the <c>saved</c> directory.</returns>
        public static string GetSavedDirectory()
        {
            return Path.Combine(GetBaseDirectory(), "saved");
        }

        /// <summary>
        /// Gets the path to the cache directory inside <c>saved</c>.
        /// </summary>
        /// <returns>The absolute path to the <c>saved/cache</c> directory.</returns>
        public static string GetCacheDirectory()
        {
            return Path.Combine(GetSavedDirectory(), "cache");
        }

        /// <summary>
        /// Gets the path to the logs directory inside <c>saved</c>.
        /// </summary>
        /// <returns>The absolute path to the <c>saved/logs</c> directory.</returns>
        public static string GetLogsDirectory()
        {
            return Path.Combine(GetSavedDirectory(), "logs");
        }
    }
}

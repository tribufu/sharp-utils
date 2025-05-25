// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using System;
using Newtonsoft.Json;

namespace Tribufu.Serialization
{
    public class DecimalNullableStringConverter : JsonConverter<decimal?>
    {
        public override decimal? ReadJson(JsonReader reader, Type objectType, decimal? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer)
            {
                string value = reader.Value?.ToString();

                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                return decimal.Parse(value);
            }

            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing decimal?.");
        }

        public override void WriteJson(JsonWriter writer, decimal? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(value.Value.ToString());
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}

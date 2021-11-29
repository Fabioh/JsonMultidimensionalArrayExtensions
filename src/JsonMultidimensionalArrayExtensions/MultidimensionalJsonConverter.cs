using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonMultidimensionalArrayExtensions
{
    public class MultidimensionalJsonConverter<T> : JsonConverter<T[,]>
    {
        public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            reader.Read();

            var elements = new List<List<T>>();

            var innerElements = new List<T>();

            int? qttOfElementsInFirstLine = null;

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    innerElements.Add(JsonSerializer.Deserialize<T>(ref reader, options));
                    reader.Read();
                }

                if (!qttOfElementsInFirstLine.HasValue)
                {
                    qttOfElementsInFirstLine = innerElements.Count;
                }

                if (innerElements.Count != qttOfElementsInFirstLine)
                {
                    throw new JsonException("the matrix must have the same quantity of elements in each line");
                }

                elements.Add(innerElements);

                reader.Read();
                if (reader.TokenType != JsonTokenType.StartArray && reader.TokenType != JsonTokenType.EndArray)
                {
                    throw new JsonException();
                }
                innerElements = new List<T>();
                reader.Read();
            }

            var ret = new T[elements.Count, qttOfElementsInFirstLine.Value];

            for (int i = 0; i < elements.Count; i++)
            {
                for (int j = 0; j < elements[i].Count; j++)
                {
                    ret[i, j] = elements[i][j];
                }
            }

            return ret;
        }

        public override void Write(Utf8JsonWriter writer, T[,] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            for (int i = 0; i < value.GetLength(0); ++i)
            {
                writer.WriteStartArray();
                for (int j = 0; j < value.GetLength(1); ++j)
                {
                    if (typeof(T) == typeof(int))
                    {
                        writer.WriteNumberValue(
                            value[i, j]
                        );
                    }
                    
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}

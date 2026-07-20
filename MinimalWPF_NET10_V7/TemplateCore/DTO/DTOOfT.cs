//-----------------------------------------------------------------------
// <copyright file="DTOOfT.cs" company="Lifeprojects.de">
//     Class: DTOOfT
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2026</date>
//
// <summary>
// DTO dient als Container zur Aufnahme von beliebigen Datentypen. der Inhalt
// kann dann z.B. in eine JSON Datei gespeichert und gelesen werden.
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    /* Imports from NET Framework */
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public sealed partial class DTOOfT<TKey> where TKey : Enum
    {
        private Dictionary<Enum,object> _DtoDict = new();

        public int Count { get { return this._DtoDict.Count; } }

        public void Set<T>(TKey key, T value)
        {
            if (value == null)
            {
                this._DtoDict[key] = null;
            }
            else
            {
                this._DtoDict[key] = Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);
            }
        }

        public bool Get<T>(TKey key, out T value)
        {
            if (this._DtoDict.TryGetValue(key, out var obj) == true && obj is T tValue)
            {
                value = tValue;
                return true;
            }

            if(obj == null)
            {
                value = (T)obj;
                return true;
            }

            value = default!;
            return false;
        }

        public T Get<T>(TKey key)
        {
            if (this._DtoDict.TryGetValue(key, out var obj) == true && obj is T tValue)
            {
                return tValue;
            }

            return default!;
        }

        public bool Equals(DTOOfT<TKey> anotherDTO)
        {
            if (ReferenceEquals(this._DtoDict, anotherDTO))
            {
                return true;
            }

            if (this._DtoDict == null || anotherDTO == null)
            {
                return false;
            }

            if (this._DtoDict.Count != anotherDTO.Count)
            {
                return false;
            }

            EqualityComparer<object> valueComparer = EqualityComparer<object>.Default;
            foreach (var kvp in this._DtoDict)
            {
                TKey key = (TKey)kvp.Key;
                if (!anotherDTO.Get<object>(key, out var value))
                {
                    return false;
                }

                if (value.GetType().Name == typeof(List<>).Name)
                {
                    var v0 = kvp.Value as System.Collections.IList;
                    var v1 = value as System.Collections.IList;
                    if (v0.Count != v1.Count)
                    {
                        return false;
                    }

                    var diff = v0.Cast<object>().Except(v1.Cast<object>());
                    if (diff.Any() == true)
                    {
                        return false;
                    }
                }
                else
                {
                    if (valueComparer.Equals(kvp.Value, value) == false)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        public void Clear()
        {
            if (_DtoDict != null && _DtoDict.Count > 0)
            {
                _DtoDict.Clear();
            }
        }

        public DTOOfT<TKey> Clone()
        {
            DTOOfT<TKey> newDto = new();
            foreach (var kvp in this._DtoDict)
            {
                newDto._DtoDict[kvp.Key] = kvp.Value;
            }

            return newDto;
        }

        public void ToJson(string filePath)
        {
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true,
            };
            var options = jsonSerializerOptions;

            options.Converters.Add(new EnumObjectDictionaryConverter());
            string json = JsonSerializer.Serialize(this._DtoDict, options);
            if (string.IsNullOrEmpty(json) == false)
            {
                File.WriteAllText(filePath, json);
            }
        }

        public void FromJson(string filePath)
        {
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true,
            };

            JsonSerializerOptions options = jsonSerializerOptions;
            options.Converters.Add(new EnumObjectDictionaryConverter());

            if (string.IsNullOrEmpty(filePath) == false && File.Exists(filePath) == true)
            {
                string jsonText = File.ReadAllText(filePath);
                this._DtoDict = JsonSerializer.Deserialize<Dictionary<Enum, object>>(jsonText, options);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var kvp in this._DtoDict)
            {
                sb.AppendLine(CultureInfo.CurrentCulture,$"{kvp.Key}: {kvp.Value}");
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            int result = 0;
            HashCode hash = new();

            foreach (var kvp in this._DtoDict)
            {
                hash.Add(kvp.Key);
                hash.Add(kvp.Value);
            }

            result = hash.ToHashCode();

            return result;
        }

        private sealed class EnumObjectDictionaryConverter : JsonConverter<Dictionary<Enum, object>>
        {
            public override Dictionary<Enum, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var result = new Dictionary<Enum, object>();

                using var doc = JsonDocument.ParseValue(ref reader);

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    Enum key = (Enum)Enum.Parse(typeof(TKey), prop.Name);
                    result[key] = prop.Value;
                }

                return result;
            }

            public override void Write(Utf8JsonWriter writer, Dictionary<Enum, object> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                foreach (var kvp in value)
                {
                    writer.WritePropertyName(kvp.Key.ToString());
                    JsonSerializer.Serialize(writer, kvp.Value, options);
                }

                writer.WriteEndObject();
            }
        }
    }
}

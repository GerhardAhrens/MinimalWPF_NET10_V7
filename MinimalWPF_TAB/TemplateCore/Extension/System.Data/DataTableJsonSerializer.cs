namespace System.Data
{
    using System.Data;
    using System.IO;
    using System.Text.Json;

    public static class DataTableJsonSerializer
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Speichern eines Datatable in eine JSON Datei
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <example>
        /// DataTableJsonSerializer.Save(dt, "test.json");
        /// </example>
        public static void Save(DataTable table, string fileName)
        {
            ArgumentNullException.ThrowIfNull(table);

            var data = new DataTableJson
            {
                TableName = table.TableName,
                ExtendedProperties = SerializeProperties(table.ExtendedProperties)
            };

            // ------------------------------------------------------------
            // Spalten
            // ------------------------------------------------------------

            foreach (DataColumn column in table.Columns)
            {
                var columnJson = new DataColumnJson
                {
                    Name = column.ColumnName, Type = column.DataType.AssemblyQualifiedName!, 
                    AllowDBNull = column.AllowDBNull,
                    AutoIncrement = column.AutoIncrement,
                    AutoIncrementSeed = column.AutoIncrementSeed,
                    AutoIncrementStep = column.AutoIncrementStep,
                    MaxLength = column.MaxLength,
                    ColumnMapping = column.ColumnMapping,
                    Expression = string.IsNullOrEmpty(column.Expression) ? null : column.Expression,
                    ExtendedProperties = SerializeProperties(column.ExtendedProperties), DefaultValue = SerializeDefaultValue(column.DefaultValue)
                };

                data.Columns.Add(columnJson);
            }

            // ------------------------------------------------------------
            // Primary Key
            // ------------------------------------------------------------

            foreach (DataColumn column in table.PrimaryKey)
            {
                data.PrimaryKey.Add(column.ColumnName);
            }

            // ------------------------------------------------------------
            // Daten
            // ------------------------------------------------------------

            foreach (DataRow row in table.Rows)
            {
                var values = new Dictionary<string, object>();

                foreach (DataColumn column in table.Columns)
                {
                    // Expression-Spalten werden nicht gespeichert.
                    // Ihr Wert wird beim Laden automatisch berechnet.
                    if (!string.IsNullOrEmpty(column.Expression))
                        continue;

                    object value = row[column];

                    values[column.ColumnName] = value == DBNull.Value ? null : value;
                }

                var jsonElement = JsonSerializer.SerializeToElement(values, JsonOptions);

                var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonElement)
                    ?? throw new InvalidOperationException("Die Daten konnten nicht serialisiert werden.");

                data.Rows.Add(dictionary);
            }

            // ------------------------------------------------------------
            // JSON schreiben
            // ------------------------------------------------------------

            string json = JsonSerializer.Serialize(data, JsonOptions);

            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Lesen einer JSON Datei und Rückgabe als Datatable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <example>
        /// DataTable neuDt = DataTableJsonSerializer.Load("Test.json");
        /// </example>

        public static DataTable Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Die JSON-Datei wurde nicht gefunden.", fileName);
            }

            string json = File.ReadAllText(fileName);

            var data = JsonSerializer.Deserialize<DataTableJson>(json, JsonOptions)
                ?? throw new InvalidOperationException("Die JSON-Datei konnte nicht gelesen werden.");

            var table = new DataTable(data.TableName);

            // ------------------------------------------------------------
            // Table ExtendedProperties
            // ------------------------------------------------------------

            DeserializeProperties(table.ExtendedProperties, data.ExtendedProperties);

            // ------------------------------------------------------------
            // Spalten erstellen
            // ------------------------------------------------------------

            foreach (DataColumnJson columnJson in data.Columns)
            {
                Type type =
                    Type.GetType(columnJson.Type)
                    ?? throw new InvalidOperationException($"Der Datentyp '{columnJson.Type}' der Spalte '{columnJson.Name}' konnte nicht gefunden werden.");

                DataColumn column = table.Columns.Add(columnJson.Name, type);

                column.AllowDBNull = columnJson.AllowDBNull;

                column.MaxLength = columnJson.MaxLength;

                column.ColumnMapping = columnJson.ColumnMapping;

                // DefaultValue
                if (columnJson.DefaultValue.HasValue)
                {
                    column.DefaultValue = ConvertJsonValue(columnJson.DefaultValue.Value, type);
                }

                // AutoIncrement
                if (columnJson.AutoIncrement)
                {
                    column.AutoIncrementSeed = columnJson.AutoIncrementSeed;
                    column.AutoIncrementStep = columnJson.AutoIncrementStep;
                    column.AutoIncrement = true;
                }

                // ExtendedProperties
                DeserializeProperties(column.ExtendedProperties, columnJson.ExtendedProperties);

                // Expression wird bewusst zuletzt gesetzt.
                // Dadurch ist sichergestellt, dass die referenzierten
                // Spalten bereits vorhanden sind.
                if (!string.IsNullOrEmpty(columnJson.Expression))
                {
                    column.Expression = columnJson.Expression;
                }
            }

            // ------------------------------------------------------------
            // Primary Key
            // ------------------------------------------------------------

            if (data.PrimaryKey.Count > 0)
            {
                var primaryKeyColumns =
                    data.PrimaryKey
                        .Select(name =>
                            table.Columns[name]
                            ?? throw new InvalidOperationException($"Die Primary-Key-Spalte '{name}' wurde nicht gefunden."))
                        .ToArray();

                table.PrimaryKey =
                    primaryKeyColumns;
            }

            // ------------------------------------------------------------
            // Daten
            // ------------------------------------------------------------

            foreach (Dictionary<string, JsonElement> jsonRow in data.Rows)
            {
                DataRow row = table.NewRow();

                foreach (DataColumn column in table.Columns)
                {
                    // Expression-Spalten werden nicht gesetzt.
                    // DataTable berechnet diese automatisch.
                    if (!string.IsNullOrEmpty(column.Expression))
                        continue;

                    if (!jsonRow.TryGetValue(column.ColumnName, out JsonElement value))
                    {
                        continue;
                    }

                    row[column] = value.ValueKind == JsonValueKind.Null ? DBNull.Value : ConvertJsonValue(value, column.DataType);
                }

                table.Rows.Add(row);
            }

            return table;
        }


        // ================================================================
        // ExtendedProperties
        // ================================================================

        private static List<PropertyJson> SerializeProperties(PropertyCollection properties)
        {
            var result = new List<PropertyJson>();

            foreach (object keyObject in properties.Keys)
            {
                string key = keyObject.ToString()!;

                object value = properties[key];

                if (value == null)
                {
                    result.Add(new PropertyJson {Name = key, Type = null, Value = null});

                    continue;
                }

                result.Add(new PropertyJson
                {
                    Name = key,
                    Type = value.GetType().AssemblyQualifiedName,
                    Value = JsonSerializer.SerializeToElement(value, value.GetType(), JsonOptions)
                });
            }

            return result;
        }


        private static void DeserializeProperties(PropertyCollection properties, List<PropertyJson> serializedProperties)
        {
            foreach (PropertyJson property in serializedProperties)
            {
                if (property.Value == null)
                {
                    properties[property.Name] = null;
                    continue;
                }

                if (string.IsNullOrEmpty(property.Type))
                {
                    properties[property.Name] = property.Value.Value.ToString();

                    continue;
                }

                Type type =
                    Type.GetType(property.Type)
                    ?? throw new InvalidOperationException($"Der Datentyp '{property.Type}' der ExtendedProperty '{property.Name}' konnte nicht gefunden werden.");

                properties[property.Name] = ConvertJsonValue(property.Value.Value, type);
            }
        }


        // ================================================================
        // DefaultValue
        // ================================================================

        private static JsonElement? SerializeDefaultValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return JsonSerializer.SerializeToElement(value, value.GetType(), JsonOptions);
        }


        // ================================================================
        // JSON -> .NET Typ
        // ================================================================

        private static object ConvertJsonValue(
            JsonElement value,
            Type targetType)
        {
            if (value.ValueKind == JsonValueKind.Null)
                return DBNull.Value;

            if (targetType == typeof(string))
                return value.GetString() ?? string.Empty;

            if (targetType == typeof(char))
            {
                string text = value.GetString() ?? throw new InvalidOperationException("Ein char-Wert fehlt.");

                if (text.Length != 1)
                {
                    throw new InvalidOperationException($"'{text}' ist kein gültiger char-Wert.");
                }

                return text[0];
            }

            if (targetType == typeof(bool))
                return value.GetBoolean();

            if (targetType == typeof(byte))
                return value.GetByte();

            if (targetType == typeof(sbyte))
                return value.GetSByte();

            if (targetType == typeof(short))
                return value.GetInt16();

            if (targetType == typeof(ushort))
                return value.GetUInt16();

            if (targetType == typeof(int))
                return value.GetInt32();

            if (targetType == typeof(uint))
                return value.GetUInt32();

            if (targetType == typeof(long))
                return value.GetInt64();

            if (targetType == typeof(ulong))
                return value.GetUInt64();

            if (targetType == typeof(float))
                return value.GetSingle();

            if (targetType == typeof(double))
                return value.GetDouble();

            if (targetType == typeof(decimal))
                return value.GetDecimal();

            if (targetType == typeof(DateTime))
                return value.GetDateTime();

            if (targetType == typeof(DateTimeOffset))
                return value.GetDateTimeOffset();

            if (targetType == typeof(Guid))
                return value.GetGuid();

            if (targetType == typeof(byte[]))
            {
                string base64 = value.GetString() ?? throw new InvalidOperationException("Der Byte-Array-Wert fehlt.");

                return Convert.FromBase64String(base64);
            }

            if (targetType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(
                    value.GetString()
                    ?? throw new InvalidOperationException(
                        "Der TimeSpan-Wert fehlt."));
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(
                    targetType,
                    value.GetString()
                    ?? throw new InvalidOperationException(
                        "Der Enum-Wert fehlt."));
            }

            // Fallback für weitere Datentypen
            return JsonSerializer.Deserialize(value.GetRawText(), targetType)
                   ?? throw new InvalidOperationException($"Der Wert '{value}' konnte nicht in '{targetType.FullName}' konvertiert werden.");
        }


        // ================================================================
        // JSON DTOs
        // ================================================================

        private sealed class DataTableJson
        {
            public string TableName { get; set; }

            public List<PropertyJson> ExtendedProperties { get; set; } = new();

            public List<DataColumnJson> Columns { get; set; } = new();

            public List<string> PrimaryKey { get; set; } = new();

            public List<Dictionary<string, JsonElement>> Rows { get; set; } = new();
        }


        private sealed class DataColumnJson
        {
            public string Name { get; set; } = string.Empty;

            public string Type { get; set; } = string.Empty;

            public bool AllowDBNull { get; set; }

            public bool AutoIncrement { get; set; }

            public long AutoIncrementSeed { get; set; }

            public long AutoIncrementStep { get; set; }

            public JsonElement? DefaultValue { get; set; }

            public int MaxLength { get; set; }

            public MappingType ColumnMapping { get; set; }

            public string Expression { get; set; }

            public List<PropertyJson> ExtendedProperties { get; set; } = new();
        }


        private sealed class PropertyJson
        {
            public string Name { get; set; } = string.Empty;

            public string Type { get; set; }

            public JsonElement? Value { get; set; }
        }
    }
}

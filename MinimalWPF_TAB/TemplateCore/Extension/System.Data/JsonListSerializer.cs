namespace System.Data
{
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Kapselt das Lesen und Schreiben einer List&lt;T&gt; als JSON.
    /// Zusätzlich wird der konkrete Typ T in der JSON-Datei gespeichert.
    /// </summary>
    /// <typeparam name="T">Typ der Listenelemente</typeparam>
    public class JsonListSerializer<T>
    {
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// Version des JSON-Dateiformats.
        /// </summary>
        public int Version { get; set; } = 1;

        public JsonListSerializer(bool indented = true)
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = indented,
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };
        }

        #region Save

        /// <summary>
        /// Speichert eine List&lt;T&gt; in einer JSON-Datei.
        /// Der Typ T wird zusätzlich in der Datei gespeichert.
        /// </summary>
        public void Save(string fileName, List<T> items)
        {
            ArgumentNullException.ThrowIfNull(fileName);
            ArgumentNullException.ThrowIfNull(items);

            string directory = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            JsonListContainer<T> container = CreateContainer(items);

            string json = JsonSerializer.Serialize(container, _options);

            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Asynchrone Variante zum Speichern.
        /// </summary>
        public async Task SaveAsync(string fileName, List<T> items, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(fileName);
            ArgumentNullException.ThrowIfNull(items);

            string directory = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            JsonListContainer<T> container = CreateContainer(items);

            await using FileStream stream = new(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

            await JsonSerializer.SerializeAsync(stream, container, _options, cancellationToken);
        }

        #endregion

        #region Load

        /// <summary>
        /// Liest eine List&lt;T&gt; aus einer JSON-Datei.
        /// Dabei wird geprüft, ob der gespeicherte Typ zu T passt.
        /// </summary>
        public List<T> Load(string fileName)
        {
            ArgumentNullException.ThrowIfNull(fileName);

            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            string json = File.ReadAllText(fileName);

            JsonListContainer<T> container = JsonSerializer.Deserialize<JsonListContainer<T>>(json, _options);

            if (container == null)
            {
                return new List<T>();
            }

            ValidateType(container);

            return container.Items ?? new List<T>();
        }

        /// <summary>
        /// Asynchrone Variante zum Lesen.
        /// </summary>
        public async Task<List<T>> LoadAsync(string fileName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(fileName);

            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            await using FileStream stream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            JsonListContainer<T> container = await JsonSerializer.DeserializeAsync<JsonListContainer<T>>(stream, _options, cancellationToken);

            if (container == null)
            {
                return new List<T>();
            }

            ValidateType(container);

            return container.Items ?? new List<T>();
        }

        #endregion

        #region Container

        private JsonListContainer<T> CreateContainer(List<T> items)
        {
            return new JsonListContainer<T>
            {
                Type = typeof(T).AssemblyQualifiedName
                    ?? typeof(T).FullName
                    ?? typeof(T).Name,

                Version = Version,

                Items = items
            };
        }

        private static void ValidateType(JsonListContainer<T> container)
        {
            if (string.IsNullOrWhiteSpace(container.Type))
            {
                throw new InvalidDataException("Die JSON-Datei enthält keine Typinformation.");
            }

            Type storedType = Type.GetType(container.Type, throwOnError: false);

            if (storedType == null)
            {
                throw new InvalidDataException($"Der in der JSON-Datei gespeicherte Typ '{container.Type}' konnte nicht gefunden werden.");
            }

            if (storedType != typeof(T))
            {
                throw new InvalidDataException($"Der gespeicherte Typ '{storedType.FullName}' entspricht nicht dem erwarteten Typ '{typeof(T).FullName}'.");
            }
        }

        #endregion
    }

    /// <summary>
    /// Interne Struktur der JSON-Datei.
    /// </summary>
    internal class JsonListContainer<T>
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("$version")]
        public int Version { get; set; }

        [JsonPropertyName("$items")]
        public List<T> Items { get; set; } = new();
    }
}

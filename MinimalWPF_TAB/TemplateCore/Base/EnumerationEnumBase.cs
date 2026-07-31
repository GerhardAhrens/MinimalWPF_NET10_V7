namespace System.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Statische Member nicht in generischen Typen deklarieren", Justification = "<Ausstehend>")]
    public abstract class EnumerationEnumBase<T> : IEquatable<T>, IComparable<T> where T : EnumerationEnumBase<T>
    {
        private static readonly Lazy<Dictionary<int, T>> _byValue = new(CreateByValue, LazyThreadSafetyMode.ExecutionAndPublication);
        private static readonly Lazy<Dictionary<string, T>> _byName = new(CreateByName, LazyThreadSafetyMode.ExecutionAndPublication);

        protected EnumerationEnumBase(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; }

        public string Name { get; }

        public static int Count
        {
            get
            {
                return _byValue.Value.Values.ToList().Count;
            }
        }

        private static Dictionary<int, T> CreateByValue()
        {
            var values = LoadValues();

            return values.ToDictionary(x => x.Value);
        }

        private static Dictionary<string, T> CreateByName()
        {
            var values = LoadValues();

            return values.ToDictionary(x => x.Name);
        }

        private static List<T> LoadValues()
        {
            // Erzwingt die Initialisierung der statischen Felder
            RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);

            var fields = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(f => f.FieldType == typeof(T));

            var list = new List<T>();

            foreach (var field in fields)
            {
                if (field.GetValue(null) is T item)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public static IReadOnlyCollection<T> GetValues() => _byValue.Value.Values.ToList().AsReadOnly();

        public static bool HasVaule(object value)
        {
            bool result = false;

            if (value is int)
            {
                result = _byValue.Value.Any(a => a.Key == (int)value);
            }
            else if (value is string)
            {
                result = _byName.Value.Any(a => a.Value == value.ToString());
            }

            return result;
        }

        public static T FromValue(int value) => _byValue.Value.Any(a => a.Key == value) ? _byValue.Value[value] : default(T);

        public static T FromName(string name) => _byName.Value.Any(a => a.Value == name) ? _byName.Value[name] : default(T);

        public static bool TryFromValue(int value, out T result) => _byValue.Value.TryGetValue(value, out result!);

        public static bool TryFromName(string name, out T result) => _byName.Value.TryGetValue(name, out result!);

        public override string ToString() => Name;

        public override int GetHashCode() => Value;

        public override bool Equals(object obj) => obj is T other && Equals(other);

        public bool Equals(T other) => other is not null && Value == other.Value;

        public int CompareTo(T other) => Value.CompareTo(other?.Value ?? -1);

        public static bool operator ==(EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => Equals(left, right);

        public static bool operator !=(EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => !Equals(left, right);

        public static bool operator > (EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => true;
        public static bool operator >=(EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => true;
        public static bool operator <(EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => true;
        public static bool operator <=(EnumerationEnumBase<T> left, EnumerationEnumBase<T> right) => true;

        public static implicit operator int(EnumerationEnumBase<T> value) => value.Value;

        public static implicit operator string(EnumerationEnumBase<T> value) => value.Name;
    }

    public static class EnumerationEnumExtensions
    {
        public static bool In<T>(this T value, params T[] values) where T : EnumerationEnumBase<T>
        {
            return values.Contains(value);
        }

        public static bool NotIn<T>(this T value, params T[] values) where T : EnumerationEnumBase<T>
        {
            return values.Contains(value) == false;
        }

        public static string Description<T>(this T value) where T : EnumerationEnumBase<T>
        {
            // if this is a Flags enum, value may contain multiple items
            var values = value.ToString().Split(',').Select(s => s.Trim()).ToList();
            var enumType = value.GetType();

            var result = string.Join(" | ", values.Select(enumValue => enumType.GetMember(enumValue)
                                                                           .FirstOrDefault()
                                                                           ?.GetCustomAttribute<DescriptionAttribute>()
                                                                           ?.Description
                                                                       ?? enumValue.ToString()));

            return result;
        }

        public static string Category<T>(this T value) where T : EnumerationEnumBase<T>
        {
            // if this is a Flags enum, value may contain multiple items
            var values = value.ToString().Split(',').Select(s => s.Trim()).ToList();
            var enumType = value.GetType();

            var result = string.Join(" | ", values.Select(enumValue => enumType.GetMember(enumValue)
                                                                           .FirstOrDefault()
                                                                           ?.GetCustomAttribute<CategoryAttribute>()
                                                                           ?.Category
                                                                       ?? enumValue.ToString()));

            return result;
        }

        public static string DisplayName<T>(this T value) where T : EnumerationEnumBase<T>
        {
            // if this is a Flags enum, value may contain multiple items
            var values = value.ToString().Split(',').Select(s => s.Trim()).ToList();
            var enumType = value.GetType();

            var result = string.Join(" | ", values.Select(enumValue => enumType.GetMember(enumValue)
                                                                           .FirstOrDefault()
                                                                           ?.GetCustomAttribute<DisplayNameAttribute>()
                                                                           ?.DisplayName
                                                                       ?? enumValue.ToString()));

            return result;
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="ID.cs" company="Lifeprojects.de">
//     Class: ID
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Domain Klasse zum Typ ID<int>
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Numerics;

    public class ID<T> : DomainObjectBase
    {
        public ID(int id)
        {
            this.Value = (T)Convert.ChangeType(id, typeof(int), CultureInfo.CurrentCulture);
        }

        public ID(long id)
        {
            this.Value = (T)Convert.ChangeType(id, typeof(long), CultureInfo.CurrentCulture);
        }

        public ID(decimal id)
        {
            this.Value = (T)Convert.ChangeType(id, typeof(decimal), CultureInfo.CurrentCulture);
        }

        public ID(BigInteger id)
        {
            this.Value = (T)Convert.ChangeType(ToGuid(id), typeof(Guid), CultureInfo.CurrentCulture);
        }

        public ID(Guid id)
        {
            this.Value = (T)Convert.ChangeType(id, typeof(Guid), CultureInfo.CurrentCulture);
        }

        public ID(string id)
        {
            this.Value = (T)Convert.ChangeType(id, typeof(string), CultureInfo.CurrentCulture);
        }

        public T Value { get; }

        public IDStatus Status { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static implicit operator ID<T>(int value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(CultureInfo.CurrentCulture), nameof(value));
            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            Type t = typeof(T);
            if (t == typeof(decimal))
            {
                ID<T> instance = new ID<T>((decimal)value)
                {
                    Status = state
                };
                return instance;
            }
            else
            {
                ID<T> instance = new ID<T>(value)
                {
                    Status = state
                };
                return instance;
            }
        }

        public static implicit operator ID<T>(long value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(CultureInfo.CurrentCulture), nameof(value));
            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else if (value > int.MaxValue)
            {
                state = IDStatus.Error;
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Wert ist größer als Int Max ({value})");
            }
            else
            {
                state = IDStatus.Edit;
            }

            ID<T> instance = new ID<T>(value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID<T>(decimal value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(CultureInfo.CurrentCulture), nameof(value));
            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            ID<T> instance = new ID<T>(value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID<T>(Guid value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value == Guid.Empty)
            {
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            BigInteger intFromGuid = GuidStringToBigInt(value.ToString());

            ID<T> instance = new ID<T>(intFromGuid)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID<T>(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value == string.Empty)
            {
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            int outInt = -1;
            if (int.TryParse(value, out outInt) == false)
            {
                state = IDStatus.Error;
            }

            Type t = typeof(T);
            if (t == typeof(string))
            {
                ID<T> instance = new ID<T>((string)outInt.ToString(CultureInfo.CurrentCulture))
                {
                    Status = state
                };

                return instance;
            }
            else
            {
                ID<T> instance = new ID<T>((int)outInt)
                {
                    Status = state
                };

                return instance;
            }
        }

        private static BigInteger GuidStringToBigInt(string guidString)
        {
            Guid g = new Guid(guidString);
            BigInteger bigInt = new BigInteger(g.ToByteArray());
            return bigInt;
        }

        private static Guid ToGuid(BigInteger value)
        {
            byte[] bytes = new byte[16];
            value.ToByteArray().CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public enum IDStatus
        {
            None = 0,
            New = 1,
            Edit = 2,
            Error = 3,
        }
    }
}

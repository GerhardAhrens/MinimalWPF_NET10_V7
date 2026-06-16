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
// Domain Klasse zum Typ ID
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class ID : DomainObjectBase
    {
        public ID(int id)
        {
            this.Value = id;
        }

        public int Value { get; }

        public IDStatus Status { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.CurrentCulture);
        }

        public static implicit operator ID(int value)
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

            ID instance = new ID(value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(long value)
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

            ID instance = new ID((int)value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(decimal value)
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

            ID instance = new ID((int)value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(string value)
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
            else
            {
                if (outInt < 1)
                {
                    outInt = -1;
                    state = IDStatus.New;
                }
                else
                {
                    state = IDStatus.Edit;
                }
            }

            ID instance = new ID((int)outInt)
            {
                Status = state
            };

            return instance;
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

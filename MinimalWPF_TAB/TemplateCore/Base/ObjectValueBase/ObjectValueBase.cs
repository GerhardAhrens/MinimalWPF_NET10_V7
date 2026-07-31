//-----------------------------------------------------------------------
// <copyright file="ObjectValueBase.cs" company="Lifeprojects.de">
//     Class: ObjectValueBase
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Basis Klasse zur Erstellung von Value Object
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ObjectValueBase
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var valueObject = (ObjectValueBase)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(ObjectValueBase a, ObjectValueBase b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ObjectValueBase a, ObjectValueBase b)
        {
            return !(a == b);
        }
    }
}

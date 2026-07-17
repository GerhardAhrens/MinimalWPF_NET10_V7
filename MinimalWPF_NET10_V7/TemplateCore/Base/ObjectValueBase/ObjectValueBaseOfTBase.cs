
//-----------------------------------------------------------------------
// <copyright file="ObjectValueBaseOfTBase.cs" company="Lifeprojects.de">
//     Class: ObjectValueBaseOfTBase
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Basis Klasse zur Erstellung von Generic Value Object
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    public abstract class ObjectValueBase<T> where T : ObjectValueBase<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            if (ReferenceEquals(valueObject, null))
            {
                return false;
            }

            return EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected abstract int GetHashCodeCore();

        public static bool operator ==(ObjectValueBase<T> a, ObjectValueBase<T> b)
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

        public static bool operator !=(ObjectValueBase<T> a, ObjectValueBase<T> b)
        {
            return !(a == b);
        }
    }
}

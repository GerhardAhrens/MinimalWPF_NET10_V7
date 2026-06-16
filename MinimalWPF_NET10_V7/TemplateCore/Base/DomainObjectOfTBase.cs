
//-----------------------------------------------------------------------
// <copyright file="DomainObjectOfTBase.cs" company="Lifeprojects.de">
//     Class: DomainObjectOfTBase
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Basis Klasse zur Erstllung von Domain Objekten
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    public abstract class DomainObjectOfTBase<T> where T : DomainObjectOfTBase<T>
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

        public static bool operator ==(DomainObjectOfTBase<T> a, DomainObjectOfTBase<T> b)
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

        public static bool operator !=(DomainObjectOfTBase<T> a, DomainObjectOfTBase<T> b)
        {
            return !(a == b);
        }
    }
}

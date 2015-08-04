using System;
using System.Collections.Generic;

namespace Mais
{
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        public GenericComparer(Func<T, object> uniqueCheckerMethod)
        {
            this._uniqueCheckerMethod = uniqueCheckerMethod;
        }

        private Func<T, object> _uniqueCheckerMethod;

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return this._uniqueCheckerMethod(x).Equals(this._uniqueCheckerMethod(y));
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return this._uniqueCheckerMethod(obj).GetHashCode();
        }
    }
}


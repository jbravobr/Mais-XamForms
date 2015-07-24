using System;

namespace Mais
{
    /// <summary>
    /// Interface to expose Locale settings using Dependency Services.
    /// </summary>
    public interface ILocale
    {
        string GetCurrent();

        void SetLocale();
       
    }
}


using System;
using SQLite.Net.Async;

namespace Mais
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}


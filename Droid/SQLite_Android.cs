using System;
using System.Runtime.CompilerServices;
using System.IO;

using Mais.Droid;
using SQLite.Net;

[assembly: Xamarin.Forms.Dependency(typeof(SQLite_Android))]
namespace Mais.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android()
		{
		}

		public SQLite.Net.Async.SQLiteAsyncConnection GetConnection()
		{
			const string sqliteFilename = "Mais.db3";  
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, sqliteFilename);

			// Cria a conexão
			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var param = new SQLite.Net.SQLiteConnectionString(path, false);
			var conn = new SQLite.Net.Async.SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(plat, param));

			return conn;
		}
	}
}


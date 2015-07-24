using System;
using Xamarin.Forms;
using System.IO;
using SQLite.Net.Async;
using SQLite.Net;

[assembly: Dependency(typeof(Mais.iOS.SQLite_iOS))]

namespace Mais.iOS
{
	public class SQLite_iOS : ISQLite
	{
		public SQLite_iOS()
		{
			
		}

		public SQLiteAsyncConnection GetConnection()
		{
			var sqliteFilename = "Mais.db3";
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine(libraryPath, sqliteFilename);
			// Create the connection
			var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
			var param = new SQLite.Net.SQLiteConnectionString(path, false);
			var conn = new SQLite.Net.Async.SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(plat, param));
			// Return the database connection
			return conn;

		}
	}
}
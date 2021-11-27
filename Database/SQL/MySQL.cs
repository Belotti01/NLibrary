using MySql.Data.MySqlClient;
using NL.Exceptions;
using System;
using System.Data;
using System.Text;

namespace NL.Database.SQL {
	public static class MySQL {
		private static MySqlConnection _client;
		public static MySqlConnection Client {
			get {
				return _client;
			}
			private set {
				_client = value;
			}
		}

		private static string _defaultSchema;

		public static void Connect(string host, ushort port, string user, string password, string database, string defaultSchema) {
			if(port == 0) {
				throw new InvalidValueException<ushort>(port, nameof(port), "The port 0 cannot be used.");
			}

			string request = BuildConnectionString(host, port, user, password, database);
			Client = new(request);
			Client.Open();
			_defaultSchema = defaultSchema;
		}

		/// <inheritdoc cref="Connect(string, ushort, string, string, string, string)"/>
		public static bool TryConnect(string host, ushort port, string user, string password, string database, string defaultSchema = null) {
			try {
				Connect(host, port, user, password, database, defaultSchema);
				return true;
			}catch {
				return false;
			}
		}

		public static bool CanConnect(string host, ushort port, string user, string password, string database) {
			try {
				using MySqlConnection testConnection = new(BuildConnectionString(host, port, user, password, database));
				testConnection.Open();
				testConnection.Close();
				return true;
			} catch {
				return false;
			}
		}

		private static string BuildConnectionString(string host, ushort port, string user, string password, string database) {
			StringBuilder request = new StringBuilder()
				.Append($"server={host};")
				.Append($"user={user};")
				.Append($"database={database};")
				.Append($"port={port};")
				.Append($"password={password}");

			return request.ToString();
		}

		public static void Select(string query) {

		}

	}
}

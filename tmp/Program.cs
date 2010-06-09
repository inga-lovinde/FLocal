using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace tmp {
	class Program {
		static void Main(string[] args) {
			string fieldToSelect = args[0];
			using(MySqlConnection connection = new MySqlConnection("Protocol=pipe;Charset=utf8;Database=flocal_production;Username=flocalp;Password=flocalp")) {
				connection.Open();
				using(MySqlCommand command = connection.CreateCommand()) {
					command.CommandText = "SELECT `" + fieldToSelect + "` FROM `boards`";
					Console.WriteLine("Command: " + command.CommandText);
					using(MySqlDataReader reader = command.ExecuteReader()) {
						Console.WriteLine("Command executed");
						while(reader.Read()) {
							Console.WriteLine("Data read");
						}
					}
				}
			}
		}
	}
}

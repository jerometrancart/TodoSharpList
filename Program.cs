﻿using Microsoft.Data.Sqlite;

if (!File.Exists("todo.db")) File.Create("todo.db").Close();

var connection = new SqliteConnection("Filename=todo.db");
try
{
    connection.Open();

    var createTableCommand = "CREATE TABLE IF NOT " +
        "EXISTS Todos (Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        "Value NVARCHAR(2048) NOT NULL, " +
        "Completed BOOLEAN)";

    new SqliteCommand(createTableCommand, connection).ExecuteNonQuery();

    string? choix = "";
    while(choix != "q")
    {
    System.Console.WriteLine("------------- Menu -------------");
    System.Console.WriteLine("1. Lire les todos");
    System.Console.WriteLine("2. Créer un todo");
    System.Console.WriteLine("3. Marquer un todo comme terminé");
    System.Console.WriteLine("Valider avec Q pour quitter");

    choix = Console.ReadLine();

        if ("q".Equals(choix, StringComparison.OrdinalIgnoreCase)) break;
        else if (choix == "1")
        {
            SqliteCommand command = new SqliteCommand(
                "SELECT * FROM Todos",
                connection
            );
            SqliteDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var value = reader.GetString(reader.GetOrdinal("Value"));
                    var completed = reader.GetBoolean(reader.GetOrdinal("Completed"));

                    System.Console.WriteLine($"{id} - {value} (terminé ? {(completed ? "V": "X")})");
                }
            }
            else
            {
                System.Console.WriteLine("Aucun TODO dans le système !");
            }
            Console.WriteLine("Appuyez sur Entrée pour retourner au menu");
        }
        else if (choix == "2")
        {

        }
        else if (choix == "3")
        {

        }
    }
}
finally
{
    connection.Close();
}

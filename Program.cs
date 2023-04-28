using Microsoft.Data.Sqlite;

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
            ///RETOURNE DES VALEURS = EXECUTEREADER
            SqliteDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    ///ON OUBLIE PAS GETORDINAL POUR RECUPERER LES ID DES COLONNES
                    var id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var value = reader.GetString(reader.GetOrdinal("Value"));
                    var completed = reader.GetBoolean(reader.GetOrdinal("Completed"));
                    
                    ///SI UNE TACHE EST COMPLETEE LA CONSOLE AFFICHERA V SINON X
                    System.Console.WriteLine($"{id} - {value} (terminé ? {(completed ? "V" : "X")})");
                }
            }
            else
            {
                System.Console.WriteLine("Aucun TODO dans le système !");
            }
            Console.WriteLine("Appuyez sur Entrée pour retourner au menu");
            Console.ReadLine();
        }
        else if (choix == "2")
        {
            System.Console.WriteLine("Saisir la tâche à réaliser");
            var value = Console.ReadLine(); 
            
            ///INITIALISE COMPLETED A 0 QUAND ON AJOUTE UNE TACHE
            SqliteCommand insert = new SqliteCommand("INSERT INTO Todos(Value, Completed) VALUES (@Value, 0)", connection);
            ///PARAMETRES POUR SECURISER CONTRE INJECTIONS
            insert.Parameters.AddWithValue("@Value", value);
            ///NE RETOURNE RIEN = EXECUTENONQUERY
            insert.ExecuteNonQuery();

            Console.WriteLine("Appuyez sur Entrée pour retourner au menu");
            Console.ReadLine();
        }
        else if (choix == "3")
        {
            Console.WriteLine("Appuyez sur Entrée pour retourner au menu");
            Console.ReadLine();
        }
    }
}
finally
{
    connection.Close();
}

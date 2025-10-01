/* TODO:
   This is an application where you’ll log occurrences of a habit.
   [ ] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
   [ ] Users need to be able to input the date of the occurrence of the habit
   [*] The application should store and retrieve data from a real database
   [*] When the application starts, it should create a sqlite database, if one isn’t present.
   [*] It should also create a table in the database, where the habit will be logged.
   [ ] The users should be able to insert, delete, update and view their logged habit.
   [ ] You should handle all possible errors so that the application never crashes.
   [*] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
   [ ] Follow the DRY Principle, and avoid code repetition.
   [ ] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
   */

// Potential BUG --> Careful with datetime input
using System;
//using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;

class Program
{
    // return either console error or number
    static int UserInput()
    {
        int output;
        string? inp = Console.ReadLine();

        if (!int.TryParse(inp, out output))
        {
            Console.WriteLine($"Invalid input: {inp}!");
            return -1;
        }
        return output;
    }

    static void Main(string[] args)
    {
        string dbFile = "habits.db";
        string connectionString = $"Data Source={dbFile};";

        // open connection
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // 1. CREATE TABLE
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Amount TEXT NOT NULL,
                        Date DATETIME DEFAULT CURRENT_TIMESTAMP 
                        );";

            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table ensured.");
            }

            // Begin loop
            bool running = true;
            while(running)
            {
                Console.WriteLine("1. New Habit\n2. Update Habit\n3. Delete Habit\n4. List Habit\n5. QUIT");

                // Check for valid input
                int? opt = UserInput();
                if (opt < 1 || opt > 5)
                {
                    Console.WriteLine("Invalid input, please try again.");
                    continue;
                }
                
                // Define variables to track lists
                int idInp = 0;
                int idTrack = 0;
                List<string> currentHabitsList = new List<string>();
                string selectQuery;
                string nameUpdate;

                // Switch statement for options
                switch (opt)
                {
                    case 1:
                        // INSERT 
                        // User input
                        Console.WriteLine("------------------");
                        Console.WriteLine("Habit Name: ");
                        string? habitName = Console.ReadLine();
                        Console.WriteLine("Habit Initial Amount: ");
                        string? habitAmount = Console.ReadLine();
                        Console.WriteLine("Habit Date Started (type 't' for today): ");
                        string? habitDate = Console.ReadLine();

                        // Inserting into table
                        string insertQuery = "INSERT INTO Habits (Name, Amount, Date) VALUES (@name, @amount, @date)";
                        using (var insertCommand = new SqliteCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@name", habitName);
                            insertCommand.Parameters.AddWithValue("@amount", habitAmount);
                            insertCommand.Parameters.AddWithValue("@date", habitDate);
                            insertCommand.ExecuteNonQuery();
                            Console.WriteLine("Inserted habit: Meditation.");
                        }
                        break;

                    case 2:
                        // UPDATE 
                        // First, LIST OUT CURRENT HABITS
                        idTrack = 0;
                        currentHabitsList = new List<string>();

                        selectQuery = "SELECT Id, Name, Amount, Date FROM Habits";
                        using (var selectCommand = new SqliteCommand(selectQuery, connection))
                            using (var reader = selectCommand.ExecuteReader())
                            {
                                Console.WriteLine("\n-- Current Habits --");
                                while (reader.Read())
                                {
                                    // track list
                                    idTrack++;

                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string amount = reader.GetString(2);
                                    string date = reader.GetDateTime(3).ToString();

                                    Console.WriteLine($"{id}: {name} ({amount}), created at {date}");
                                    currentHabitsList.Add(name);
                                }
                            }
                        // Ask which ID
                        Console.WriteLine("");
                        Console.WriteLine("Which ID would you like to update?");
                        idInp = UserInput();
                        if (idTrack == 0 || idInp < 1 || idInp > idTrack)
                        {
                            Console.WriteLine("Invalid option!");
                            continue;
                        }
                        nameUpdate = currentHabitsList[idInp-1];

                        Console.WriteLine("New Amount value:");
                        string? amountUpdate = Console.ReadLine();

                        string updateQuery = "UPDATE Habits SET Amount = @amount WHERE Name = @name";
                        using (var updateCommand = new SqliteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@amount", "amount");
                            updateCommand.Parameters.AddWithValue("@name", nameUpdate);
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"\nUpdated rows: {rowsAffected}");
                        }

                        // Verify update
                        using (var verifyCommand = new SqliteCommand(selectQuery, connection))
                            using (var reader = verifyCommand.ExecuteReader())
                            {
                                Console.WriteLine("\n-- Habits After Update --");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["Id"]}: {reader["Name"]} ({reader["Amount"]})");
                                }
                            }
                        break;

                    case 3:
                        // First, LIST OUT CURRENT HABITS
                        idTrack = 0;
                        currentHabitsList = new List<string>();

                        selectQuery = "SELECT Id, Name, Amount, Date FROM Habits";
                        using (var selectCommand = new SqliteCommand(selectQuery, connection))
                            using (var reader = selectCommand.ExecuteReader())
                            {
                                Console.WriteLine("\n-- Current Habits --");
                                while (reader.Read())
                                {
                                    // track list
                                    idTrack++;

                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string amount = reader.GetString(2);
                                    string date = reader.GetDateTime(3).ToString();

                                    Console.WriteLine($"{id}: {name} ({amount}), created at {date}");
                                    currentHabitsList.Add(name);
                                }
                            }
                        // Ask which ID
                        Console.WriteLine("");
                        Console.WriteLine("Which ID would you like to delete?");
                        idInp = UserInput();
                        if (idTrack == 0 || idInp < 1 || idInp > idTrack)
                        {
                            Console.WriteLine("Invalid option!");
                            continue;
                        }
                        nameUpdate = currentHabitsList[idInp-1];

                        // DELETE 
                        string deleteQuery = "DELETE FROM Habits WHERE Name = @name";
                        using (var deleteCommand = new SqliteCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@name", "nameUpdate");
                            int rowsDeleted = deleteCommand.ExecuteNonQuery();
                            Console.WriteLine($"\nDeleted rows: {rowsDeleted}");
                        }

                        // Verify delete
                        using (var verifyCommand = new SqliteCommand(selectQuery, connection))
                            using (var reader = verifyCommand.ExecuteReader())
                            {
                                Console.WriteLine("\n-- Habits After Delete --");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["Id"]}: {reader["Name"]} ({reader["Amount"]})");
                                }
                            }
                        break;

                    case 4:
                        // READ 
                        selectQuery = "SELECT Id, Name, Amount, Date FROM Habits";
                        using (var selectCommand = new SqliteCommand(selectQuery, connection))
                            using (var reader = selectCommand.ExecuteReader())
                            {
                                Console.WriteLine("\n-- Current Habits --");
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string amount = reader.GetString(2);
                                    string date = reader.GetDateTime(3).ToString();

                                    Console.WriteLine($"{id}: {name} ({amount}), created at {date}");
                                }
                            }
                        break;

                        // QUIT program 
                    case 5:
                        return;
                }
            }
        }
    }
}
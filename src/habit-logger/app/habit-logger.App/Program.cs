/* TODO:
This is an application where you’ll log occurrences of a habit.
[ ] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
[ ] Users need to be able to input the date of the occurrence of the habit
[ ] The application should store and retrieve data from a real database
[ ] When the application starts, it should create a sqlite database, if one isn’t present.
[ ] It should also create a table in the database, where the habit will be logged.
[ ] The users should be able to insert, delete, update and view their logged habit.
[ ] You should handle all possible errors so that the application never crashes.
[ ] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
[ ] Follow the DRY Principle, and avoid code repetition.
[ ] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
*/

using System;
using System.Data.SQLite;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string dbFile = "habits.db";
        string connectionString = $"Data Source={dbFile};Version=3;";

        // Ensure DB file exists
        if (!File.Exists(dbFile))
        {
            SQLiteConnection.CreateFile(dbFile);
            Console.WriteLine("Database created.");
        }

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // 1. CREATE TABLE
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Frequency TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table ensured.");
            }

            // 2. INSERT (C in CRUD)
            string insertQuery = "INSERT INTO Habits (Name, Frequency) VALUES (@name, @frequency)";
            using (var insertCommand = new SQLiteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@name", "Meditation");
                insertCommand.Parameters.AddWithValue("@frequency", "Daily");
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Inserted habit: Meditation.");
            }

            // 3. READ (R in CRUD)
            string selectQuery = "SELECT Id, Name, Frequency, CreatedAt FROM Habits";
            using (var selectCommand = new SQLiteCommand(selectQuery, connection))
            using (var reader = selectCommand.ExecuteReader())
            {
                Console.WriteLine("\n-- Current Habits --");
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string frequency = reader.GetString(2);
                    string createdAt = reader.GetDateTime(3).ToString();

                    Console.WriteLine($"{id}: {name} ({frequency}), created at {createdAt}");
                }
            }

            // 4. UPDATE (U in CRUD)
            string updateQuery = "UPDATE Habits SET Frequency = @frequency WHERE Name = @name";
            using (var updateCommand = new SQLiteCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@frequency", "Weekly");
                updateCommand.Parameters.AddWithValue("@name", "Meditation");
                int rowsAffected = updateCommand.ExecuteNonQuery();
                Console.WriteLine($"\nUpdated rows: {rowsAffected}");
            }

            // Verify update
            using (var verifyCommand = new SQLiteCommand(selectQuery, connection))
            using (var reader = verifyCommand.ExecuteReader())
            {
                Console.WriteLine("\n-- Habits After Update --");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}: {reader["Name"]} ({reader["Frequency"]})");
                }
            }

            // 5. DELETE (D in CRUD)
            string deleteQuery = "DELETE FROM Habits WHERE Name = @name";
            using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
            {
                deleteCommand.Parameters.AddWithValue("@name", "Meditation");
                int rowsDeleted = deleteCommand.ExecuteNonQuery();
                Console.WriteLine($"\nDeleted rows: {rowsDeleted}");
            }

            // Verify delete
            using (var verifyCommand = new SQLiteCommand(selectQuery, connection))
            using (var reader = verifyCommand.ExecuteReader())
            {
                Console.WriteLine("\n-- Habits After Delete --");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}: {reader["Name"]} ({reader["Frequency"]})");
                }
            }
        }
    }
}
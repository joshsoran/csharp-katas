/* TODO:
   This is an application where you’ll log occurrences of a habit.
   [*] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
   [*] Users need to be able to input the date of the occurrence of the habit
   [*] The application should store and retrieve data from a real database
   [*] When the application starts, it should create a sqlite database, if one isn’t present.
   [*] It should also create a table in the database, where the habit will be logged.
   [*] The users should be able to insert, delete, update and view their logged habit.
   [*] You should handle all possible errors so that the application never crashes.
   [*] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
   [*] Follow the DRY Principle, and avoid code repetition.
   [ ] Your project needs to contain a Read Me file where you'll explain how your app works. 
   [*] NO DUPLICATE ENTRIES!
   */

using System;
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
            Console.Clear();
            Console.WriteLine($"[ERROR]: \"{inp}\" is not a number!\n");
            return -1;
        }
        return output;
    }
    
    // display list
    static List<string> ReturnList(SqliteConnection connection, bool displayList)
    {
        // init list for return and row counter
        List<string> currentIdList = new List<string>();
        List<string> currentNameList = new List<string>();
        int idTrack = 0;

        // Establish connection and display table from DB
        string selectQuery = "SELECT Id, Name, Amount, Date FROM Habits";
        using (var selectCommand = new SqliteCommand(selectQuery, connection))
            using (var reader = selectCommand.ExecuteReader())
            {
                if (displayList)
                    Console.WriteLine("----(Current Habits)----------------------------");
                while (reader.Read())
                {
                    // track list
                    idTrack++;

                    // variable names
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string amount = reader.GetString(2);
                    string date = reader.GetString(3);

                    if (displayList)
                    {
                        // Display
                        Console.WriteLine($"({idTrack}) {name} ({amount}), created at {date}");
                        // Add to list for return
                        currentIdList.Add(id.ToString());
                    }
                    else  // Don't display & return names not IDs
                    {   
                        currentNameList.Add(name.ToString()); 
                    }
                }
            }
        if (displayList)
        {
            Console.WriteLine("------------------------------------------------\n");
            return currentIdList;
        }
        else
        {
           return currentNameList;
        } 
    }

    // return ID selection from displayed list
    static string ReturnId(SqliteConnection connection, string menuChoice)
    {
        string idDelete;
        // Init list to display and receive
        List<string> currentIdList = ReturnList(connection, true);

        // Is list empty?
        if (currentIdList.Count == 0)
        {
            Console.WriteLine("[ERROR]: List is Empty!");
            return "ERROR";
        }

        // Begin prompts
        Console.WriteLine($"----({menuChoice.ToUpper()})------------------------------------");
        Console.WriteLine($"-> Which Id would you like to {menuChoice.ToUpper()}?");
        int idInp = UserInput();

        // Filter bad options
        if ((idInp == -1) || (idInp <= 0 || idInp > currentIdList.Count))
        {
            Console.Clear();
            Console.WriteLine("[ERROR]: Invalid option!\n");
            return "ERROR";
        }
        return idDelete = currentIdList[idInp-1];
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
                Console.Clear();
                Console.WriteLine("-- [WELCOME TO THE HABIT LOGGER] --\n");
            }

            // Begin loop
            bool running = true;
            while(running)
            {
                Console.WriteLine("1. New Habit\n2. Update Habit\n3. Delete Habit\n4. List Habit\n5. QUIT");
                Console.Write("\n-> Choose: ");

                // Check for valid input
                int? opt = UserInput();
                if (opt < 1 || opt > 5)
                {
                    Console.Clear();
                    Console.WriteLine("[ERROR]: Invalid input, please try again.\n");
                    continue;
                }
                
                // Define variables to track lists
                List<string> currentHabitsList = new List<string>();

                // Switch statement for options
                switch (opt)
                {
                    case 1:
                        // INSERT 
                        // Clear screen first
                        Console.Clear();
                        // Grab list, don't display
                        currentHabitsList = ReturnList(connection, false);
                        // User input
                        Console.WriteLine("----(Insert New Habit)----------------------------");
                        Console.Write("-> Habit Name: ");
                        string? habitName = Console.ReadLine();
                        bool duplicateHabit = false;
                        // Find duplicate entries -- strict
                        foreach (string habit in currentHabitsList)
                        {
                            if (habit?.ToLower() == habitName?.ToLower())
                            {
                                duplicateHabit = true;
                            }
                        }
                        if (duplicateHabit) 
                        { 
                            Console.WriteLine("[ERROR]: Habit already exists! Try updating instead.\n");
                            continue; 
                        }

                        Console.Write("-> Habit Initial Amount: ");
                        string? habitAmount = UserInput().ToString(); // Check for valid number input
                        if (habitAmount == "-1")
                        {
                            //Console.WriteLine("[ERROR]: Amount MUST be a number!\n");
                            continue;
                        }
                        Console.Write("-> Habit Date Started, type 't' for today (yyyy-mm-dd): ");
                        string? habitDate = Console.ReadLine();
                        Console.Clear();
                        if (habitDate?.ToLower() == "t") // if habitDate is null, it won't equal to "t". This clears warnings.
                        {
                            string today = DateTime.Now.ToString("yyyy-MM-dd");
                            habitDate = today; 
                        }
                        else if (DateTime.TryParseExact(habitDate, "yyyy-MM-dd",
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None,
                                    out var habitDateParsed))
                        {
                            habitDate = habitDateParsed.ToString("yyyy-MM-dd");
                            Console.WriteLine($"-> Parsed date: {habitDate}");
                        }
                        else
                        {
                            Console.WriteLine("[ERROR]: Invalid date format. Use yyyy-MM-dd.\n");
                            continue;
                        }

                        // Inserting into table
                        string insertQuery = "INSERT INTO Habits (Name, Amount, Date) VALUES (@name, @amount, @date)";
                        using (var insertCommand = new SqliteCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@name", habitName);
                            insertCommand.Parameters.AddWithValue("@amount", habitAmount);
                            insertCommand.Parameters.AddWithValue("@date", habitDate);
                            insertCommand.ExecuteNonQuery();
                            Console.WriteLine(
                                    $"[SUCCESS!] Inserted habit: {habitName}; " + 
                                    $"Amount: {habitAmount}; " + 
                                    $"Date: {habitDate};\n"
                                    );
                        }
                        break;

                    case 2:
                        // UPDATE 
                        // Clear screen first
                        Console.Clear();
                        // Return existing list, grab ID
                        string idUpdate = ReturnId(connection, "update");
                        if (idUpdate == "ERROR") { continue; }

                        // Input new amount value
                        Console.WriteLine("-> New Amount value:");
                        //string? amountUpdate = Console.ReadLine();
                        string? amountUpdate = UserInput().ToString();
                        if (amountUpdate == "-1") { continue; }

                        // Update amount in table
                        string updateQuery = "UPDATE Habits SET Amount = @amount WHERE Id = @Id";
                        using (var updateCommand = new SqliteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@amount", amountUpdate);
                            updateCommand.Parameters.AddWithValue("@Id", idUpdate);
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.Clear();
                            Console.WriteLine($"\n-> Updated rows: {rowsAffected}\n");
                        }

                        // Verify update
                        ReturnList(connection, true);
                        break;

                    case 3:
                        // DELETE
                        // Clear screen first
                        Console.Clear();
                        // Return existing list, grab ID
                        string idDelete = ReturnId(connection, "delete");
                        if (idDelete == "ERROR") { continue; }

                        // perform delete 
                        string deleteQuery = "DELETE FROM Habits WHERE Id = @Id";
                        using (var deleteCommand = new SqliteCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@Id", idDelete);
                            int rowsDeleted = deleteCommand.ExecuteNonQuery();
                            Console.Clear();
                            Console.WriteLine($"\nDeleted rows: {rowsDeleted}\n");
                        }

                        // Verify delete
                        ReturnList(connection, true);
                        break;

                    case 4:
                        // READ 
                        // Clear screen first
                        Console.Clear();
                        ReturnList(connection, true);
                        break;

                    case 5:
                        // QUIT program 
                        Console.Clear();
                        return;
                }
            }
        }
    }
}
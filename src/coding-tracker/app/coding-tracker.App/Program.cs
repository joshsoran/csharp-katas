/*
 * Author:          Joshua Soran
 * Date:            08/10/2025
 * Description:     ...
 */

/* 
 [ ] - This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
 [ ] - To show the data on the console, you should use the "Spectre.Console" library.
 [ ] - You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
 [ ] - You should tell the user the specific format you want the date and time to be logged and not allow any other format.
 [ ] - You'll need to create a configuration file that you'll contain your database path and connection strings.
 [ ] - You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
 [ ] - The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
 [ ] - The user should be able to input the start and end times manually.
 [ ] - You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
 [ ] - When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.
 [ ] - Follow the DRY Principle, and avoid code repetition.
 */
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;

// CodingTracker Library
using CodingTracker.Sessions;
using CodingTracker.Input;

namespace CodingTracker
{   
    public static class Program 
    {
        public static void Main()
        {
            // Init Input 
            var userInp = new UserInput();
            int optionInp;
            bool running = true;

            // Reset screen on launch
            Console.Clear();


            // Create new db
            using var connection = new SqliteConnection("Data Source=coding-tracker.db");
            connection.Open();

            // Create table
            connection.Execute(@"CREATE TABLE IF NOT EXISTS CodingSession (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL,
                        Duration TEXT NOT NULL
                        );");

            string sTime;
            string eTime;
            string duration;

            // Begin LOOP
            while(running)
            {
                // Intro PROMPT
                Console.WriteLine("1. INSERT\n2. UPDATE\n3. DELETE\n4. DISPLAY\n5. QUIT\n");
                optionInp = userInp.OptionInput(5); // 5 options
                if (optionInp == -1) 
                {
                    Console.WriteLine("[ERROR]: Invalid option.");
                    continue;
                }

                switch(optionInp)
                {
                    // Insert data
                    case 1:
                        // HACK: The way I'm intaking and calculating time can be done in one function...
                        // Doesn't follow DRY principles.
                         
                        // Intro prompt
                        Console.Clear();
                        var rule = new Spectre.Console.Rule("[yellow]Insert[/]");
                        rule.Justification = Spectre.Console.Justify.Left;
                        AnsiConsole.Write(rule);

                        // Time input prompts
                        Console.WriteLine("Start time (hh:mm):");
                        sTime = userInp.TimeInput(true);
                        if (sTime == "ERROR")
                        {
                            Console.WriteLine("[ERROR]: Could not process inputted time.");
                            continue;
                        }

                        Console.WriteLine("End time (hh:mm), type 'n' for now.");
                        eTime = userInp.TimeInput(false);
                        if (eTime == "ERROR")
                        {
                            Console.WriteLine("[ERROR]: Could not process inputted time.");
                            continue;
                        }

                        duration = userInp.ProcessTime(sTime, eTime);
                        
                        connection.Execute("INSERT INTO CodingSession (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration);", new { StartTime = sTime, EndTime = eTime, Duration = duration });
                        break;
                    // Update
                    case 2:
                        var codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");
                        Console.WriteLine("Choose which ID to update:");
                        int idChoice = userInp.SelectId(codingSessions);
                        int inputId = codingSessions.ElementAt(idChoice).Id;

                        // HACK: The way I'm intaking and calculating time can be done in one function...
                        // Doesn't follow DRY principles.

                        // New sTime
                        Console.WriteLine("NEW Start Time (hh:mm): "); 
                        sTime = userInp.TimeInput(true);
                        if (sTime == "ERROR")
                        {
                            Console.WriteLine("[ERROR]: Could not process inputted time.");
                            continue;
                        }

                        // New eTime
                        Console.WriteLine("NEW End Time (hh:mm), type 'n' for now: "); 
                        eTime = userInp.TimeInput(false);
                        if (eTime == "ERROR")
                        {
                            Console.WriteLine("[ERROR]: Could not process inputted time.");
                            continue;
                        }

                        duration = userInp.ProcessTime(sTime, eTime);

                        connection.Execute(@"
                                UPDATE CodingSession
                                SET StartTime = @StartTime,
                                EndTime = @EndTime,
                                Duration = @Duration
                                WHERE Id = @Id", new 
                                {
                                StartTime = sTime,
                                EndTime = eTime,
                                Duration = duration,
                                Id = inputId
                                });
                        break;
                    // Delete
                    case 3:
                        codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");
                        idChoice = userInp.SelectId(codingSessions);
                        inputId = codingSessions.ElementAt(idChoice).Id;

                        connection.Execute("DELETE FROM CodingSession WHERE Id = @Id", new { Id = inputId});

                        codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");
                        userInp.DisplayList(codingSessions);
                        break;
                    // Display
                    case 4:
                        codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");
                        userInp.DisplayList(codingSessions);
                        break;
                    // Quit
                    case 5:
                        running = false;
                        break;
                }
            }
            connection.Close();
        }
    }
}
/*
 * Author:          Joshua Soran
 * Date:            08/10/2025
 * Description:     ...
 */

/* 
 [ ] - To show the data on the console, you should use the "Spectre.Console" library.
 [ ] - Follow the DRY Principle, and avoid code repetition.
 */
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;
using Microsoft.Extensions.Configuration; // Used for config files

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

            // Build DB config
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve connection string
            var connectionString = config.GetConnectionString("Default");
            Console.WriteLine($"Using DB connection: {connectionString}");

            using var connection = new SqliteConnection(connectionString);

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

                        duration = userInp.CalculateDuration(sTime, eTime);
                        
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

                        duration = userInp.CalculateDuration(sTime, eTime);

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
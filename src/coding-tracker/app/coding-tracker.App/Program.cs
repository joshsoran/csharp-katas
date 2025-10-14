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
        public static bool ErrorCheck(string strErr, int intErr)
        {
            if (strErr == "ERROR" || intErr == -1)
            {
                Console.WriteLine("[ERROR]: Invalid input!");
                return true;
            }
            return false; // Returns no errors;
        }

        public static void Main()
        {

            // Init Input 
            var userInp = new UserInput();

            bool running = true;

            string sTime;
            string eTime;
            string duration;

            int optionInp;
            int idChoice;
            int inputId;


            // clear screen on launch
            Console.Clear();

            // Build DB config
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve connection string
            var connectionString = config.GetConnectionString("Default");
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Create table
            connection.Execute(@"CREATE TABLE IF NOT EXISTS CodingSession (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL,
                        Duration TEXT NOT NULL
                        );");

           
            // grab list before options are displayed
            var codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");

            // Begin LOOP
            while(running)
            {
                // Your list of options
                var options = new List<string>
                {
                    "1. INSERT",
                    "2. UPDATE",
                    "3. DELETE",
                    "4. DISPLAY LIST",
                    "5. QUIT"
                };

                // Create a markup string for the options list
                var listMarkup = string.Join("\n", options);

                // Wrap that in a Spectre.Console 'Markup' object
                var listText = new Markup(listMarkup);

                // Wrap it in a panel
                var panel = new Panel(listText)
                {
                    Header = new PanelHeader("Main Menu"),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(2, 1),
                };

                // Render to console
                AnsiConsole.Write(panel);

                //Console.WriteLine("1. INSERT\n2. UPDATE\n3. DELETE\n4. DISPLAY\n5. QUIT\n");
                optionInp = userInp.OptionInput(5); // 5 options
                if (ErrorCheck("", optionInp)){ continue; }


                switch(optionInp)
                {
                    // Insert data
                    case 1:
                        // Intro prompt
                        Console.Clear();
                        var rule = new Spectre.Console.Rule("[yellow]Insert[/]");
                        rule.Justification = Spectre.Console.Justify.Left;
                        AnsiConsole.Write(rule);

                        // Start Time input prompts
                        Console.WriteLine("Start time (hh:mm):");
                        sTime = userInp.TimeInput(true);
                        if (ErrorCheck(sTime, 0)) { continue; }

                        // End time input prompt
                        Console.WriteLine("End time (hh:mm), type 'n' for now.");
                        eTime = userInp.TimeInput(false);
                        if (ErrorCheck(eTime, 0)) { continue; }

                        // Duration
                        duration = userInp.CalculateDuration(sTime, eTime);
                        
                        // execute insert query
                        connection.Execute("INSERT INTO CodingSession (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration);", new { StartTime = sTime, EndTime = eTime, Duration = duration });

                        Console.WriteLine();
                        break;
                    // Update
                    case 2:
                        // Intro prompt
                        Console.Clear();
                        userInp.DisplayList(codingSessions);                        
                        Console.Write("\nChoose which ID to update: ");
                        idChoice = userInp.SelectId(codingSessions);
                        inputId = codingSessions.ElementAt(idChoice).Id;


                        // New sTime
                        Console.Write("NEW Start Time (hh:mm): "); 
                        sTime = userInp.TimeInput(true);
                        if (ErrorCheck(sTime, 0)) { continue; }

                        // New eTime
                        Console.Write("NEW End Time (hh:mm), type 'n' for now: "); 
                        eTime = userInp.TimeInput(false);
                        if (ErrorCheck(eTime, 0)) { continue; }

                        // final new duration
                        duration = userInp.CalculateDuration(sTime, eTime);

                        // execute update query
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

                        Console.Clear();
                        userInp.DisplayList(codingSessions);

                        Console.WriteLine();
                        break;
                    // Delete
                    case 3:
                        // Intro prompt
                        Console.Clear();
                        userInp.DisplayList(codingSessions);                        
                        Console.Write("\nChoose which ID to delete: ");
                        idChoice = userInp.SelectId(codingSessions);
                        inputId = codingSessions.ElementAt(idChoice).Id;

                        // execute delete query
                        connection.Execute("DELETE FROM CodingSession WHERE Id = @Id", new { Id = inputId});

                        Console.Clear();
                        userInp.DisplayList(codingSessions);

                        Console.WriteLine();
                        break;
                    // Display
                    case 4:
                        // Clear screen
                        Console.Clear();
                        // refresh codingSessions list and re-display results
                        codingSessions = connection.Query<CodingSession>("SELECT * FROM CodingSession;");
                        userInp.DisplayList(codingSessions);
                        Console.WriteLine();
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
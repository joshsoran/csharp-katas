# Console Habit Logger
Console based application written in C#. This application allows the user to input habit names alongside the frequency of usage as well as a specific date the habit began in.

# Given requirements
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. 
- [x] NO DUPLICATE ENTRIES!

# Features
* SQLite database connection via ADO.NET
* Usage of `Microsoft.Data.Sqlite` library—better support for C# on MacOS
* Console based UI
* CRUD DB functions

# How-to-Use
Program begins with initial option list. Input a number based on the options list, then hit 'Enter'. Instructions will proceed on screen depending on the options selected.

namespace mathgame.App
{
//namespace mathgame.Tests;
/* 
-You need to create a Math game containing the 4 basic operations

-The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.

-Users should be presented with a menu to choose an operation

-You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.

-You don't need to record results on a database. Once the program is closed the results will be deleted.
*/

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine(); // add extra space
    List<string> historyList = new List<string>();
    bool running = true;
    while (running)
    {
      // Present options
      Console.WriteLine("Choose operations: (1) Addition; (2) Subtraction; (3) Multiplication; (4) Division; (5) HISTORY; (6) EXIT OUT OF PROGRAM");  

      // Grab input
      string? menuInput = Console.ReadLine();
      int menuParse;

      if (!int.TryParse(menuInput, out menuParse))
      { 
        Console.WriteLine("Error-Bad input! Try again...");
        continue;
      }
      if (menuParse > 6 || menuParse < 1)
      {
        Console.WriteLine("Error-Option not available!");
        continue;
      }

      switch (menuInput)
      {
        case "1":
          historyList.Add(Addition());
          break;

        case "2":
          historyList.Add(Subtraction());
          break;

        case "3":
          historyList.Add(Multiplication());
          break;

        case "4":
          historyList.Add(Division());
          break;

        case "5":
          History(historyList);
          break;

        case "6":
          running = false; 
          break;
      }
    }
  }

  public static string Addition()
  {
    string? input;
    int numOne;
    int numTwo;
    Console.WriteLine(); // Line break
    Console.WriteLine("--Addition--"); 
    Console.Write("Choose your first number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numOne))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }
    Console.WriteLine(); // Line break
    Console.Write("Choose your second number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numTwo))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }
    Console.WriteLine($"Answer: {numOne + numTwo}");
    string toAdd = $"Addition of {numOne} and {numTwo} was {numOne + numTwo}";
    return toAdd;
  }

  public static string Subtraction()
  {
    string? input;
    int numOne;
    int numTwo;
    Console.WriteLine(); // Line break
    Console.WriteLine("--Subtraction--"); 
    Console.Write("Choose your first number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numOne))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }

    Console.WriteLine(); // Line break
    Console.Write("Choose your second number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numTwo))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }

    Console.WriteLine($"Answer: {numOne - numTwo}");
    string toAdd = $"Subtraction of {numOne} and {numTwo} was {numOne - numTwo}";
    return toAdd;
  }

  public static string Multiplication()
  {
    string? input;
    int numOne;
    int numTwo;
    Console.WriteLine(); // Line break
    Console.WriteLine("--Multiplication--"); 
    Console.Write("Choose your first number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numOne))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }

    Console.WriteLine(); // Line break
    Console.Write("Choose your second number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numTwo))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER!";
    }

    Console.WriteLine($"Answer: {numOne * numTwo}");
    string toAdd = $"Multiplication of {numOne} and {numTwo} was {numOne * numTwo}";
    return toAdd;
  }

  public static string Division()
  {
    string? input;
    int numOne;
    int numTwo;
    Console.WriteLine(); // Line break
    Console.WriteLine("--Division--"); 
    Console.Write("Choose your first number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numOne))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER";
    }
    if (numOne < 0 || numOne > 100)
    {
      Console.WriteLine("Dividend number invalid. Must be between 0-100!");
      return "Dividend number invalid. Must be between 0-100!";
    }

    Console.WriteLine(); // Line break
    Console.Write("Choose your second number: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out numTwo))
    {
      Console.WriteLine("INVALID NUMBER!");
      return "INVALID NUMBER";
    }
    if (numTwo == 0)
    {
      Console.WriteLine("CANNOT DIVIDE BY ZERO ERROR!");
      return "CANNOT DIVIDE BY ZERO ERROR!";
    }

    // Check if division is integer ONLY
    if (numOne % numTwo != 0)
    {
      Console.WriteLine("INVALID NUMBERS! Division does not yield integer only values.");
      return "INVALID NUMBERS! Division does not yield integer only values.";
    }
    else
    {
      Console.WriteLine($"Answer: {numOne / numTwo}");
    }
    string toAdd = $"Division of {numOne} and {numTwo} was {numOne / numTwo}";
    return toAdd;
  }

  public static void History(List<string> list)
  {
    Console.WriteLine(); // Line break
    Console.WriteLine("--HISTORY--");
    for (int i = 0; i < list.Count; i++)
    {
      Console.WriteLine($"({i+1}) {list[i]}");
    }
  }

}
}
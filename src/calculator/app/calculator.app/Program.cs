using System;

public class Program
{
    public static void Main()
    {
        string? numInput; // Input MAY be null...

        double numOne = 0;
        double numTwo = 0; 

        int opt;
        int loopTimes = 0;

        bool running = true;
        bool histNum = false;

        List<double> historyListDouble = new List<double>(); 
        List<string> historyListString = new List<string>(); 

        Console.WriteLine("\n\t-[CALCULATOR APP]-");
        while (running)
        {
            // Calculator amount usage
            Console.WriteLine($"App used {loopTimes} time(s)!");

            // Menu Options
            Console.WriteLine("---------------------------");
            Console.WriteLine("Now, choose an operation: ");
            Console.WriteLine("(1) Addition");
            Console.WriteLine("(2) Subtraction");
            Console.WriteLine("(3) Multiplication");
            Console.WriteLine("(4) Division");
            Console.WriteLine("(5) Square rooting");
            Console.WriteLine("(6) Power");
            Console.WriteLine("(7) 10x");
            Console.WriteLine("(8) Trig functions");
            Console.WriteLine("(9) HISTORY");
            Console.WriteLine("(10) EXIT PROGRAM");
            Console.Write("-> YOUR OPTION: ");
            numInput = Console.ReadLine();

            // End early if program quits
            if (!int.TryParse(numInput, out opt))
            {
               Console.WriteLine("Not a number, try again...");
               continue;
            }
            if (opt == 10) 
            {
                return;
            }
            if (opt < 1 && opt > 9)
            {
                Console.WriteLine("Not a valid option. Try again...");
                continue;
            }

            // Grab the user input numbers
            if (histNum == false && opt != 9)
            {
                Console.Write("Type a number then press Enter: ");
                numInput = Console.ReadLine();
                if (!double.TryParse(numInput, out numOne))
                {
                    Console.WriteLine("Not a number, try again...");
                    continue;
                }

                Console.Write("Type another number then press Enter: ");
                numInput = Console.ReadLine();
                if (!double.TryParse(numInput, out numTwo))
                {
                    Console.WriteLine("Not a number, try again...");
                    continue;
                }
            }
            else
            {
                histNum = false;
            }
            
            // Line break;
            Console.WriteLine("");

            // Options
            switch(opt)
            {
                case 1:
                    Console.WriteLine($"ADDITION: {numOne} + {numTwo}");
                    Console.WriteLine($"=> {numOne + numTwo}");
                    historyListString.Add($"{numOne} + {numTwo} = {numOne + numTwo}");
                    historyListDouble.Add((double)numOne + numTwo);
                    break;
                case 2:
                    Console.WriteLine($"SUBTRACTION: {numOne} - {numTwo}");
                    Console.WriteLine($"=> {numOne - numTwo}");
                    historyListString.Add($"{numOne} - {numTwo} = {numOne - numTwo}");
                    historyListDouble.Add((double)numOne - numTwo);
                    break;
                case 3:
                    Console.WriteLine($"MULTIPLICATION: {numOne} * {numTwo}");
                    Console.WriteLine($"=> {numOne * numTwo}");
                    historyListString.Add($"{numOne} * {numTwo} = {numOne * numTwo}");
                    historyListDouble.Add((double)numOne * numTwo);
                    break;
                case 4:
                    Console.WriteLine($"DIVISION: {numOne} / {numTwo}");
                    Console.WriteLine($"=> {(double)numOne / numTwo}");
                    historyListString.Add($"{numOne} / {numTwo} = {(double)numOne / numTwo}");
                    historyListDouble.Add((double)numOne / numTwo);
                    break;
                case 5:
                    Console.WriteLine($"SQUARE ROOT OF: {numOne}");
                    Console.WriteLine($"=> {Math.Sqrt((double)numOne)}");
                    Console.WriteLine($"SQUARE ROOT OF: {numTwo}");
                    Console.WriteLine($"=> {Math.Sqrt((double)numTwo)}");
                    historyListString.Add($"SQRT({numOne}) = {Math.Sqrt((double)numOne)}");
                    historyListDouble.Add(Math.Sqrt((double)numOne));
                    historyListString.Add($"SQRT({numTwo}) = {Math.Sqrt((double)numTwo)}");
                    historyListDouble.Add(Math.Sqrt((double)numTwo));
                    break;
                case 6:
                    Console.WriteLine($"POWER: {numOne}^{numTwo}");
                    Console.WriteLine($"=> {Math.Pow(numOne, numTwo)}");
                    historyListString.Add($"{numOne}^{numTwo} = {(double)Math.Pow(numOne, numTwo)}");
                    historyListDouble.Add((double)Math.Pow(numOne, numTwo));
                    break;
                case 7:
                    Console.WriteLine($"10x OF: {numOne}");
                    Console.WriteLine($"=> {numOne*10}");
                    Console.WriteLine($"10x OF: {numTwo}");
                    Console.WriteLine($"=> {numTwo*10}");
                    historyListString.Add($"10x {numOne} = {(double)numOne * 10}");
                    historyListDouble.Add((double)numOne * 10);
                    historyListString.Add($"10x {numTwo} = {(double)numTwo * 10}");
                    historyListDouble.Add((double)numTwo * 10);
                    break;
                case 8:
                    Console.WriteLine($"--Trig Functions--");
                    Console.WriteLine($"(a) SIN OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Sin(numOne)} & {Math.Sin(numTwo)}\n");
                    Console.WriteLine($"(b) COS OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Cos(numOne)} & {Math.Cos(numTwo)}\n");
                    Console.WriteLine($"(c) TAN OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Tan(numOne)} & {Math.Tan(numTwo)}\n");
                    Console.WriteLine($"(d) ASIN OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Asin(numOne)} & {Math.Asin(numTwo)}\n");
                    Console.WriteLine($"(e) ACOS OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Acos(numOne)} & {Math.Acos(numTwo)}\n");
                    Console.WriteLine($"(f) ATAN OF {numOne} & {numTwo}");
                    Console.WriteLine($"=> {Math.Atan(numOne)} & {Math.Atan(numTwo)}\n");
                    historyListString.Add($"SINE OF {numOne} = {Math.Sin(numOne)}");
                    historyListDouble.Add((double)Math.Sin(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Sin(numTwo)}");
                    historyListDouble.Add((double)Math.Sin(numTwo));
                    historyListString.Add($"SINE OF {numOne} = {Math.Cos(numOne)}");
                    historyListDouble.Add((double)Math.Cos(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Cos(numTwo)}");
                    historyListDouble.Add((double)Math.Cos(numTwo));
                    historyListString.Add($"SINE OF {numOne} = {Math.Tan(numOne)}");
                    historyListDouble.Add((double)Math.Tan(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Tan(numTwo)}");
                    historyListDouble.Add((double)Math.Tan(numTwo));
                    historyListString.Add($"SINE OF {numOne} = {Math.Asin(numOne)}");
                    historyListDouble.Add((double)Math.Asin(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Asin(numTwo)}");
                    historyListDouble.Add((double)Math.Asin(numTwo));
                    historyListString.Add($"SINE OF {numOne} = {Math.Acos(numOne)}");
                    historyListDouble.Add((double)Math.Acos(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Acos(numTwo)}");
                    historyListDouble.Add((double)Math.Acos(numTwo));
                    historyListString.Add($"SINE OF {numOne} = {Math.Atan(numOne)}");
                    historyListDouble.Add((double)Math.Atan(numOne));
                    historyListString.Add($"SINE OF {numTwo} = {Math.Atan(numTwo)}");
                    historyListDouble.Add((double)Math.Atan(numTwo));
                    break;
                case 9:
                    bool historyRunning = true;
                    while(historyRunning)
                    {
                        Console.WriteLine("--HISTORY--");
                        Console.WriteLine("(1) View current list");
                        Console.WriteLine("(2) Delete current list");
                        Console.WriteLine("(3) Use answers from list");
                        Console.WriteLine("(4) BACK");
                        numInput = Console.ReadLine();
                        if (!int.TryParse(numInput, out opt))
                        {
                            Console.WriteLine("Invalid option.");
                            continue;
                        } 
                        
                        if (opt == 1)
                        {
                            if (historyListString.Count == 0)
                            {
                                Console.WriteLine("List is currenttly empty...");
                                continue;
                            }
                            Console.WriteLine("\nCURRENT LIST:");
                            for (int i = 0; i < historyListString.Count; i++)
                            {
                                Console.WriteLine("(" + (i+1) + ") " + historyListString[i]);
                            }
                            Console.WriteLine("");
                        }
                        else if (opt == 2)
                        {
                            historyListString.Clear();
                            historyListDouble.Clear();
                            Console.WriteLine("History list successfully cleared!");
                        }
                        else if (opt == 3)
                        {
                            for (int i = 0; i < historyListString.Count; i++)
                            {
                                Console.WriteLine("(" + (i+1) + ") " + historyListString[i]);
                            }
                            Console.WriteLine("Pick first number: ");
                            numInput = Console.ReadLine();
                            if (!double.TryParse(numInput, out numOne))
                            {
                                Console.WriteLine("Invalid option.");
                                continue;
                            } 
                            Console.WriteLine("Pick second number: ");
                            numInput = Console.ReadLine();
                            if (!double.TryParse(numInput, out numTwo))
                            {
                                Console.WriteLine("Invalid option.");
                                continue;
                            } 
                            // do these options exist?
                            if ((int)numOne > historyListDouble.Count || (int)numOne < 0)
                            {
                                Console.WriteLine("INVALID OPTIONS!!!");
                                continue;
                            }
                            if ((int)numTwo > historyListDouble.Count || (int)numTwo < 0)
                            {
                                Console.WriteLine("INVALID OPTIONS!!!");
                                continue;
                            }
                            numOne = historyListDouble[(int)numOne-1];
                            numTwo = historyListDouble[(int)numTwo-1];
                            histNum = true; // enable use of history list chosen values
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
            }
            loopTimes++;
            Console.WriteLine("---------------------------");
        }
    }
}
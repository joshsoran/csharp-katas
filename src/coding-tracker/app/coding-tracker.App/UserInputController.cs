// Tell user specific format to use for date and time -- prevent other formats
using CodingTracker.Sessions;

namespace CodingTracker.Input 
{
    public class UserInput
    {
        public int OptionInput(int maxOptions)
        {
            string? inp = Console.ReadLine();
            int opt;

            if (!int.TryParse(inp, out opt))
            {
               return -1; 
            }

            if (opt <= 0 || opt > maxOptions)
            {
                return -1;
            }
            return opt;
        }

        public string TimeInput(bool firstTimeInput)
        {
            string? inp = Console.ReadLine();
            if (inp?.ToLower() == "n" && firstTimeInput == false)
            {
                return TimeOnly.FromDateTime(DateTime.Now).ToString(); 
            }
            if (!TimeOnly.TryParse(inp, out TimeOnly time))
            {
                return "ERROR";
            }
            return time.ToString();
        }

        public string CalculateDuration(string t1, string t2)
        {
            if (!TimeOnly.TryParse(t1, out TimeOnly time1)) { return "ERROR"; }
            if (!TimeOnly.TryParse(t2, out TimeOnly time2)) { return "ERROR"; }
            
            TimeSpan timeSpan = time2 - time1;
            return timeSpan.ToString();
        }

        public void DisplayList(IEnumerable<CodingSession> codingSession)
        {
            int count = 0;
            foreach (var session in codingSession)
            {
                count++;
                Console.WriteLine($"{count}: {session.StartTime} {session.EndTime} {session.Duration}");
            }
        }

        public int SelectId(IEnumerable<CodingSession> codingSession)
        {
            int opt = OptionInput(codingSession.Count());

            // converting user input into index to find correct element
            opt = opt - 1;

            return opt;
        }
    }
}
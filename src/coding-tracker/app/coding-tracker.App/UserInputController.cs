// Tell user specific format to use for date and time -- prevent other formats

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

        public string TimeInput()
        {
            string? inp = Console.ReadLine();
            if (inp?.ToLower() == "n")
            {
                return TimeOnly.FromDateTime(DateTime.Now).ToString(); 
            }
            if (!TimeOnly.TryParse(inp, out TimeOnly time))
            {
                return "ERROR";
            }
            return time.ToString();
        }

        public string ProcessTime(string t1, string t2)
        {
            if (!TimeOnly.TryParse(t1, out TimeOnly time1)) { return "ERROR"; }
            if (!TimeOnly.TryParse(t2, out TimeOnly time2)) { return "ERROR"; }
            
            TimeSpan timeSpan = time2 - time1;
            return timeSpan.ToString();
        }
    }
}
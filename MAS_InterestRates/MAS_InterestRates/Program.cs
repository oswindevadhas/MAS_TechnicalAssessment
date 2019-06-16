using System;
using System.Globalization;

namespace MAS_InterestRates
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            
            while (true)
            {
                Console.WriteLine();
                CheckForRerun();
            }
        }

        private static void CheckForRerun()
        {
            Console.WriteLine("Press Y for re-run. Any other key for exit...");
            string rerun = Console.ReadLine();
            if (rerun.ToUpper().Equals("Y"))
            {
                Run();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private static void Run()
        {
            Console.WriteLine("Enter an option (1, 2 or 3");
            Console.WriteLine("--------------------------");
            Console.WriteLine("1. Compare financial company rates against bank rates by Months");
            Console.WriteLine("2. Compare overall average of financial company rates against bank rates");
            Console.WriteLine("3. Check interest rates trend");
            string option = string.Empty;

            while (true)
            {
                option = Console.ReadLine();
                if (option != "1" && option != "2" && option != "3")
                {
                    Console.WriteLine("Enter a valid option (1, 2 or 3");
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("1. Compare financial company rates against bank rates by Months");
                    Console.WriteLine("2. Compare overall average of financial company rates against bank rates");
                    Console.WriteLine("3. Check interest rates trend");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Enter date range in format mmm-yyyy e.g. Jan-2017");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("From Date:");
            string fromDate = string.Empty;
            string fromDateCriteria = string.Empty;
            while (true)
            {
                fromDate = Console.ReadLine();
                DateTime fromDateTime;
                if (DateTime.TryParseExact(fromDate, "MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateTime))
                {
                    fromDateCriteria = fromDateTime.ToString("yyyy-MM");
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid From Date. Sample format mmm-yyyy e.g. Jan-2017");
                }
            }

            Console.WriteLine("To Date:");
            string toDate = string.Empty;
            string toDateCriteria = string.Empty;
            while (true)
            {
                toDate = Console.ReadLine();
                DateTime toDateTime;
                if (DateTime.TryParseExact(toDate, "MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDateTime))
                {
                    toDateCriteria = toDateTime.ToString("yyyy-MM");
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid To Date. Sample format mmm-yyyy e.g. Jan-2017");
                }
            }

            Facade facade = new Facade();
            facade.GetData(option, fromDateCriteria, toDateCriteria);
        }
    }
}

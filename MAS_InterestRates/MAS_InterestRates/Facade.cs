using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS_InterestRates
{
    public class Facade
    {
        public void GetData(string option, string fromDateCriteria, string toDateCriteria)
        {
            if (option.Equals("1"))
            {
                InterestRatesMonthly interestRatesMonthly = new InterestRatesMonthly();
                interestRatesMonthly.CompareFinancialRates(fromDateCriteria, toDateCriteria);
            }
            else if (option.Equals("2"))
            {
                InterestRatesMonthly interestRatesMonthly = new InterestRatesMonthly();
                interestRatesMonthly.CompareOverallAverage(fromDateCriteria, toDateCriteria);
            }
            else if (option.Equals("3"))
            {
                InterestRatesMonthly interestRatesMonthly = new InterestRatesMonthly();
                interestRatesMonthly.CheckInterestTrends(fromDateCriteria, toDateCriteria);
            }
        }
    }
}

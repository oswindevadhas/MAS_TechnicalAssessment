using System.Collections.Generic;

namespace MAS_InterestRates.Data
{
    public class Result
    {
        public List<string> Resource_id { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public List<Records> Records { get; set; }
    }
}

using System.Collections.Generic;

namespace ReadersHub.ProductAnalyzer.Dto
{
    public class ExcelDto
    {
        public ExcelDto()
        {
            Description = new List<string>();
        }
        public string ASIN { get; set; }
        public string ISBN { get; set; }
        public decimal ASINSalesRank { get; set; }
        public decimal ISBNSalesRank { get; set; }
        public decimal UsedISBNPrice { get; set; }
        public decimal NewISBNPrice { get; set; }
        public decimal UsedASINPrice { get; set; }
        public decimal NewASINPrice { get; set; }
        public decimal NewDifference
        {
            get
            {
                return NewISBNPrice - NewASINPrice;
            }
        }
        public decimal UsedDifference
        {
            get
            {
                return UsedISBNPrice - UsedASINPrice;
            }
        }
        public List<string> Description { get; set; }
    }
}

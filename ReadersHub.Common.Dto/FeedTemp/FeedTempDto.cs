using System;

namespace ReadersHub.Common.Dto.FeedTemp
{
    public class FeedTempDto
    {
        public long Id { get; set; }
        public string Sku { get; set; } 
        public string Asin { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; } 
        public DateTime CreateDate { get; set; } 
        public string Status { get; set; }
        public string SellerId { get; set; } 
    }
}

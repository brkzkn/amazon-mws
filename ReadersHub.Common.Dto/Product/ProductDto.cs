using System;

namespace ReadersHub.Common.Dto.Product
{
    public class ProductDto
    {
        public int Id { get; set; } 
        public string Isbn { get; set; } 
        public string IsbnName { get; set; } 
        public string Asin { get; set; } 
        public string AsinName { get; set; } 
        public DateTime? PriceUpdateTimeUK { get; set; }
        public DateTime? PriceUpdateTimeUS { get; set; }

        public decimal? IsbnUsedPriceDollar { get; set; }
        public decimal? IsbnNewPriceDollar { get; set; }
        public decimal? IsbnUsedPricePound { get; set; }
        public decimal? IsbnNewPricePound { get; set; }
        
        public decimal? AsinUsedPriceDollar { get; set; }
        public decimal? AsinNewPriceDollar { get; set; }
        public decimal? AsinUsedPricePound { get; set; }
        public decimal? AsinNewPricePound { get; set; }

        public bool? IsFixedNewDollar { get; set; }
        public bool? IsFixedUsedDollar { get; set; }
        public bool? IsFixedNewPound { get; set; }
        public bool? IsFixedUsedPound { get; set; }
    }
}

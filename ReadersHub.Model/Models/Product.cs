using Repository.Pattern;
using System;

namespace ReadersHub.Model
{

    // Product
    
    public class Product : Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string Isbn { get; set; } // ISBN (length: 20)
        public string IsbnName { get; set; } // ISBN_Name (length: 500)
        public string Asin { get; set; } // ASIN (length: 20)
        public string AsinName { get; set; } // ASIN_Name (length: 500)
        public DateTime? PriceUpdateTimeUK { get; set; }
        public DateTime? PriceUpdateTimeUS { get; set; }
        public decimal? MinNewIsbnPriceDollar { get; set; } // Min_New_ISBN_Price_Dollar
        public decimal? MinUsedIsbnPriceDollar { get; set; } // Min_Used_ISBN_Price_Dollar
        public decimal? MinNewIsbnPricePound { get; set; } // Min_New_ISBN_Price_Pound
        public decimal? MinUsedIsbnPricePound { get; set; } // Min_Used_ISBN_Price_Pound
        public decimal? MinNewAsinPriceDollar { get; set; } // Min_New_ASIN_Price_Dollar
        public decimal? MinUsedAsinPriceDollar { get; set; } // Min_Used_ASIN_Price_Dollar
        public decimal? MinNewAsinPricePound { get; set; } // Min_New_ASIN_Price_Pound
        public decimal? MinUsedAsinPricePound { get; set; } // Min_Used_ASIN_Price_Pound
        public bool? IsFixedNewDollar { get; set; }
        public bool? IsFixedUsedDollar { get; set; }
        public bool? IsFixedNewPound { get; set; }
        public bool? IsFixedUsedPound { get; set; }

        public Product()
        {
        }
    }

}


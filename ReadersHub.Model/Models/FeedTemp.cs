using Repository.Pattern;

namespace ReadersHub.Model
{

    // Feed_Temp
    
    public class FeedTemp : Entity
    {
        public long Id { get; set; } // Id (Primary key)
        public string Sku { get; set; } // SKU (length: 50)
        public string Asin { get; set; } // ASIN (length: 50)
        public string Condition { get; set; } // ASIN (length: 10)
        public decimal Price { get; set; } // Price
        public System.DateTime CreateDate { get; set; } // CreateDate
        public string Status { get; set; } // Status (length: 50)
        public string SellerId { get; set; } // Seller_Id (length: 50)
    }

}


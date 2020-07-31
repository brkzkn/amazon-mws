using Repository.Pattern;

namespace ReadersHub.Model
{
    // Store
    public class Store : Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string SellerId { get; set; } // SellerId (length: 50)
        public string MarketPlaceId { get; set; } // MarketPlaceId (length: 50)
        public string CurrencyCode { get; set; } // CurrencyCode (length: 3)
        public string Name { get; set; } // Name (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Criterion> Criteria { get; set; } // Criterion.FK_Criterion_Store

        public Store()
        {
            Criteria = new System.Collections.Generic.List<Criterion>();
        }
    }

}


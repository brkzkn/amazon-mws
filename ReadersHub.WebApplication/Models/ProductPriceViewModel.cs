namespace ReadersHub.WebApplication.Models
{
    public class ProductPriceViewModel
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public bool IsFixedNewDollar { get; set; }
        public bool IsFixedUsedDollar { get; set; }
        public bool IsFixedNewPound { get; set; }
        public bool IsFixedUsedPound { get; set; }
        public string MinNewIsbnPriceDollar { get; set; }
        public string MinUsedIsbnPriceDollar { get; set; }
        public string MinNewIsbnPricePound { get; set; }
        public string MinUsedIsbnPricePound { get; set; }
    }
}
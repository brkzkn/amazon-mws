using System.Collections.Generic;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Models
{
    public class ProductViewModel
    {
        public string Isbn { get; set; }
        public string Asin { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int StoreId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
    }
}
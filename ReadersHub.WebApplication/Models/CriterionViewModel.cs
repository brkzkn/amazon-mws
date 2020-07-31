using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Models
{
    public class CriterionViewModel
    {
        public CriterionViewModel()
        {
            SellerList = new List<string>();
            StoreList = new List<SelectListItem>();
            Countries = new List<string>();
            CountryList = new List<SelectListItem>();
        }
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        [Required]
        public int? Rate { get; set; }

        [Required]
        public decimal? RateCount { get; set; }

        [DisplayName("New")]
        public bool IsNew { get; set; }
        public bool IsLikeNew { get; set; }
        public bool IsVeryGood { get; set; }
        public bool IsGood { get; set; }
        public bool IsAcceptable { get; set; }
        public bool IsUnacceptable { get; set; }

        public bool IsDollar { get; set; }

        public List<string> Countries { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        
        public string SellerIds { get; set; }
        public List<string> SellerList { get; set; }

        public List<SelectListItem> StoreList { get; set; }

        public string IsbnUsedPercentage { get; set; }
        public string IsbnUsedPrice { get; set; }
        public string IsbnNewPercentage { get; set; }
        public string IsbnNewPrice { get; set; }

    }
}
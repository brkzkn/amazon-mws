using System.Collections.Generic;

namespace ReadersHub.SearchProduct
{
    public class ExcelModel
    {
        public ExcelModel()
        {
            ControlResult = new List<string>();
        }
        public string Isbn { get; set; }
        public string IsbnTitle { get; set; }
        public string IsbnSalesRank { get; set; }
        public string IsbnPublicationDate { get; set; }
        public string IsbnBinding { get; set; }
        public string IsbnMinNewPrice { get; set; }
        public string IsbnMinUsedPrice { get; set; }
        public string Asin { get; set; }
        public string AsinTitle { get; set; }
        public string AsinSalesRank { get; set; }
        public string AsinPublicationDate { get; set; }
        public string AsinBinding { get; set; }
        public string AsinMinNewPrice { get; set; }
        public string AsinMinUsedPrice { get; set; }
        public string AsinProductLink { get; set; }
        public string AsinProductImageLink { get; set; }
        public string IsbnProductImageLink { get; set; }
        public string SimilarityRatio { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSeriesSuccess { get; set; }
        public List<string> ControlResult { get; set; }
    }
}

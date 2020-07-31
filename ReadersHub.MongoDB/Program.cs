using MongoDB.Bson;
using MongoDB.Driver;
using ReadersHub.Common.Dto.Product;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReadersHub.MongoDB
{

    class Program
    {

        static void Main(string[] args)
        {
            var db = new ReadersHubEntities();
            var client = new MongoClient("mongodb://ian:secretPassword@46.45.185.58:27017/admin");
            var database = client.GetDatabase("deneme");
            var collection = database.GetCollection<BsonDocument>("test");

            foreach (var item in db.Product.ToList())
            {
                ProductMongoDto dto = new ProductMongoDto();
                dto.Product = new ProductDto()
                {
                    Asin = item.ASIN,
                    AsinName = item.ASIN_Name,
                    Isbn = item.ISBN,
                    IsbnName = item.ISBN_Name,
                    PriceUpdateTimeUK = item.Price_Update_Time_UK,
                    PriceUpdateTimeUS = item.Price_Update_Time_US
                };
                foreach (var price in item.Product_Price)
                {
                    //if (dto.ProductPrice == null)
                    //{
                    //    dto.ProductPrice = new List<ProductPriceDto>();
                    //}

                    //dto.ProductPrice.Add(new ProductPriceDto()
                    //{
                    //    IsFixedNewDollar = price.Is_Fixed_New_Dollar,
                    //    IsFixedNewPound = price.Is_Fixed_New_Pound,
                    //    IsFixedUsedDollar = price.Is_Fixed_Used_Dollar,
                    //    IsFixedUsedPound = price.Is_Fixed_Used_Pound,
                    //    MinNewAsinPriceDollar = price.Min_New_ASIN_Price_Dollar,
                    //    MinNewAsinPricePound = price.Min_New_ASIN_Price_Pound,
                    //    MinNewIsbnPriceDollar = price.Min_New_ISBN_Price_Dollar,
                    //    MinNewIsbnPricePound = price.Min_New_ISBN_Price_Pound,
                    //    MinUsedAsinPriceDollar = price.Min_Used_ASIN_Price_Dollar,
                    //    MinUsedAsinPricePound = price.Min_Used_ASIN_Price_Pound,
                    //    MinUsedIsbnPriceDollar = price.Min_Used_ISBN_Price_Dollar,
                    //    MinUsedIsbnPricePound = price.Min_Used_ISBN_Price_Pound,
                    //});
                }

                var bson = dto.ToBsonDocument();
                collection.InsertOne(bson);
            }
        }
    }
}

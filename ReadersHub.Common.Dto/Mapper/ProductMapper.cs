using ReadersHub.Common.Dto.Product;

namespace ReadersHub.Common.Dto.Mapper
{
    public static class ProductMapper
    {
        public static ProductDto ConvertToDto(this Model.Product entity)
        {
            return new ProductDto()
            {
                Id = entity.Id,
                Asin = entity.Asin,
                AsinName = entity.AsinName,
                Isbn = entity.Isbn,
                IsbnName = entity.IsbnName,
                PriceUpdateTimeUK = entity.PriceUpdateTimeUK,
                PriceUpdateTimeUS = entity.PriceUpdateTimeUS,
                AsinNewPriceDollar = entity.MinNewAsinPriceDollar,
                AsinNewPricePound = entity.MinNewAsinPricePound,
                AsinUsedPriceDollar = entity.MinUsedAsinPriceDollar,
                AsinUsedPricePound = entity.MinUsedAsinPricePound,
                IsbnNewPriceDollar = entity.MinNewIsbnPriceDollar,
                IsbnNewPricePound = entity.MinNewIsbnPricePound,
                IsbnUsedPriceDollar = entity.MinUsedIsbnPriceDollar,
                IsbnUsedPricePound = entity.MinUsedIsbnPricePound,
                IsFixedNewDollar = entity.IsFixedNewDollar,
                IsFixedNewPound = entity.IsFixedNewPound,
                IsFixedUsedDollar = entity.IsFixedUsedDollar,
                IsFixedUsedPound = entity.IsFixedUsedPound
            };
        }

        public static Model.Product ConvertToEntity(this ProductDto dto)
        {
            return new Model.Product()
            {
                Id = dto.Id,
                Asin = dto.Asin,
                AsinName = dto.AsinName,
                Isbn = dto.Isbn,
                IsbnName = dto.IsbnName,
                PriceUpdateTimeUK = dto.PriceUpdateTimeUK,
                PriceUpdateTimeUS = dto.PriceUpdateTimeUS,
                MinNewAsinPriceDollar = dto.AsinNewPriceDollar,
                MinNewAsinPricePound = dto.AsinNewPricePound,
                MinNewIsbnPriceDollar = dto.IsbnNewPriceDollar,
                MinNewIsbnPricePound = dto.IsbnNewPricePound,
                MinUsedAsinPriceDollar = dto.AsinUsedPriceDollar,
                MinUsedAsinPricePound = dto.AsinUsedPricePound,
                MinUsedIsbnPriceDollar = dto.IsbnUsedPriceDollar,
                MinUsedIsbnPricePound = dto.IsbnUsedPricePound,
                IsFixedNewDollar = dto.IsFixedNewDollar,
                IsFixedNewPound = dto.IsFixedNewPound,
                IsFixedUsedDollar = dto.IsFixedUsedDollar,
                IsFixedUsedPound = dto.IsFixedUsedPound
            };
        }
    }
}

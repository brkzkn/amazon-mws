using ReadersHub.Common.Dto.FeedTemp;

namespace ReadersHub.Common.Dto.Mapper
{
    public static class FeedTempMapper
    {
        public static FeedTempDto ConvertToDto(this Model.FeedTemp entity)
        {
            return new FeedTempDto()
            {
                Asin = entity.Asin,
                CreateDate = entity.CreateDate,
                Price = entity.Price,
                Sku = entity.Sku,
                Status = entity.Status,
                Id = entity.Id,
                SellerId = entity.SellerId,
                Condition = entity.Condition
            };
        }

        public static Model.FeedTemp ConvertToEntity(this FeedTempDto dto)
        {
            return new Model.FeedTemp()
            {
                Asin = dto.Asin,
                CreateDate = dto.CreateDate,
                Price = dto.Price,
                Sku = dto.Sku,
                Status = dto.Status,
                Id = dto.Id,
                SellerId = dto.SellerId,
                Condition = dto.Condition
            };
        }
    }
}

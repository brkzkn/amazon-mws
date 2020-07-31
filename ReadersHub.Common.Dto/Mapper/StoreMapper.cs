using ReadersHub.Common.Dto.Store;

namespace ReadersHub.Common.Dto.Mapper
{
    public static class StoreMapper
    {
        public static StoreDto ConvertToDto(this Model.Store entity)
        {
            return new StoreDto()
            {
                Id = entity.Id,
                CurrencyCode = entity.CurrencyCode,
                MarketPlaceId = entity.MarketPlaceId,
                Name = entity.Name,
                SellerId = entity.SellerId
            };
        }

        public static Model.Store ConvertToEntity(this StoreDto dto)
        {
            return new Model.Store()
            {
                Id = dto.Id,
                CurrencyCode = dto.CurrencyCode,
                MarketPlaceId = dto.MarketPlaceId,
                Name = dto.Name,
                SellerId = dto.SellerId
            };
        }
    }
}

using ReadersHub.Common.Dto.Criterion;

namespace ReadersHub.Common.Dto.Mapper
{
    public static class CriterionMapper
    {
        public static CriterionDto ConvertToDto(this Model.Criterion entity)
        {
            return new CriterionDto()
            {
                Id = entity.Id,
                Key = entity.Key,
                Value = entity.Value,
                StoreId = entity.StoreId,
            };
        }

        public static Model.Criterion ConvertToEntity(this CriterionDto dto)
        {
            return new Model.Criterion()
            {
                Id = dto.Id,
                Key = dto.Key,
                Value = dto.Value,
                StoreId = dto.StoreId
            };
        }
    }
}

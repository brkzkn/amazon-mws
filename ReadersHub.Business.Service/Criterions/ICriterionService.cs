using ReadersHub.Common.Dto.Criterion;
using System.Collections.Generic;

namespace ReadersHub.Business.Service.Criterions
{
    public interface ICriterionService
    {
        List<CriterionDto> GetList(string sellerId);
        List<CriterionDto> GetListByStoreId(int storeId);
        CriterionDto GetByKey(string key);
        int CreateCriterion(CriterionDto dto);
        void UpdateCriterionByKey(int storeId, string key, string value);
        void UpdateOrCreate(string key, CriterionDto dto);
        void InsertCriterionRange(List<CriterionDto> dtoList);
        void UpdateCriteriaByStore(int storeId, Dictionary<string, string> criterionList);
    }
}

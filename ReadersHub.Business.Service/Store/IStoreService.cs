using _21stSolution.Dto;
using ReadersHub.Common.Dto.Store;
using System.Collections.Generic;

namespace ReadersHub.Business.Service.Store
{
    public interface IStoreService
    {
        StoreDto GetStoreBySellerId(string sellerId);
        List<SelectItemDto> GetList();
        StoreDto Get(int storeId);
    }
}

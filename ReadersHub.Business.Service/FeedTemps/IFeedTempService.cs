using ReadersHub.Common.Dto.FeedTemp;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;

namespace ReadersHub.Business.Service.FeedTemps
{
    public interface IFeedTempService
    {
        void InsertList(List<FeedTempDto> dtoList);
        List<FeedTempDto> GetFeeds(FeedTempFilterDto filter);
        void DeleteFeeds(List<FeedTempDto> dtoList);
        void CallSpForSeller(string sellerId);
        void CallSpForBulkDelete(string ids);
        void CallSpForMarkedItem(string sellerId);
    }
}

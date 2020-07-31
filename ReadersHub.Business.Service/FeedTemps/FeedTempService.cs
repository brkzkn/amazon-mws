using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System.Linq;
using ReadersHub.Common.Dto.FeedTemp;
using System.Collections.Generic;
using ReadersHub.Common.Dto.Mapper;
using System;
using System.Data.SqlClient;
using ReadersHub.Model;
using System.Linq.Expressions;
using _21stSolution.Extensions;

namespace ReadersHub.Business.Service.FeedTemps
{
    public class FeedTempService : Service<Model.FeedTemp>, IFeedTempService
    {
        private readonly IRepository<Model.FeedTemp> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<Model.FeedTemp> _table;
        public FeedTempService(IRepository<Model.FeedTemp> repository, IUnitOfWork unitOfWork)
            : base(repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _table = base.Queryable();
        }

        public void InsertList(List<FeedTempDto> dtoList)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var entityList = new List<FeedTemp>();
                foreach (var item in dtoList)
                {
                    entityList.Add(item.ConvertToEntity());
                }
                base.InsertRange(entityList);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                entityList.ForEach(x =>
                {
                    x.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Detached;
                });
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public List<FeedTempDto> GetFeeds(FeedTempFilterDto filter)
        {
            var query = _repository.Queryable();
            var condition = GetFilterExpression(filter);
            if (condition != null)
            {
                query = query.Where(condition);
            }

            var result = query.Take(filter.Count.Value).Select(x => new FeedTempDto()
            {
                Sku = x.Sku,
                Id = x.Id,
                Price = x.Price,
                Status = x.Status,
                SellerId = x.SellerId
            }).ToList();

            return result;
        }

        public void DeleteFeeds(List<FeedTempDto> dtoList)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var dto in dtoList)
                {
                    var entity = _repository.Find(dto.Id);
                    _repository.Delete(entity);
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public void CallSpForSeller(string sellerId)
        {
            var param = new SqlParameter("@SellerId", sellerId);
            _repository.ExecuteStoredProcedure("[dbo].[Prepare_Feed_Temp] @SellerId", param);
        }

        public void CallSpForMarkedItem(string sellerId)
        {
            var param = new SqlParameter("@SellerId", sellerId);
            _repository.ExecuteStoredProcedure("[dbo].[Delete_Marked_Feed_Temp] @SellerId", param);
        }

        public void CallSpForBulkDelete(string ids)
        {
            var param = new SqlParameter("@FeedIdList", ids);
            _repository.ExecuteStoredProcedure("[dbo].[Delete_Feed_Temp] @FeedIdList", param);
        }

        private Expression<Func<FeedTemp, bool>> GetFilterExpression(FeedTempFilterDto filter)
        {
            Expression<Func<FeedTemp, bool>> condition = null;

            if (filter != null)
            {
                if (!filter.SellerId.IsNullOrEmpty()) { condition = condition.And(x => x.SellerId == filter.SellerId); }
                if (!filter.Status.IsNullOrEmpty()) { condition = condition.And(x => x.Status == filter.Status); }
            }

            return condition;
        }

    }
}

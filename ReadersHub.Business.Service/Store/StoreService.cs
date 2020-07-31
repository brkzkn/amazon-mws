using _21stSolution.Dto;
using ReadersHub.Common.Dto.Mapper;
using ReadersHub.Common.Dto.Store;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadersHub.Business.Service.Store
{
    public class StoreService : Service<Model.Store>, IStoreService
    {
        private readonly IRepository<Model.Store> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<Model.Store> _table;
        public StoreService(IRepository<Model.Store> repository, IUnitOfWork unitOfWork)
            : base(repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _table = base.Queryable();
        }

        public StoreDto GetStoreBySellerId(string sellerId)
        {
            var res = _repository.Queryable().ToList();
            var entity = _table.SingleOrDefault(x => x.SellerId == sellerId);
            if (entity == null)
            {
                throw new Exception("Mağaza bulunamadı");
            }

            var dto = entity.ConvertToDto();
            return dto;
        }

        public List<SelectItemDto> GetList()
        {
            var result = _table.Select(x => new SelectItemDto()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return result;
        }

        public StoreDto Get(int storeId)
        {
            var entity = _table.SingleOrDefault(x => x.Id == storeId);

            return entity != null ? entity.ConvertToDto() : null;
        }
    }
}

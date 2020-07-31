using ReadersHub.Common.Dto.Criterion;
using ReadersHub.Common.Dto.Mapper;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadersHub.Business.Service.Criterions
{
    public class CriterionService : Service<Model.Criterion>, ICriterionService
    {
        private readonly IRepository<Model.Criterion> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<Model.Criterion> _table;
        public CriterionService(IRepository<Model.Criterion> repository, IUnitOfWork unitOfWork)
            : base(repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _table = base.Queryable();
        }

        public List<CriterionDto> GetList(string sellerId)
        {
            var result = _table.Where(x => x.Store.SellerId == sellerId).Select(x => new CriterionDto()
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value
            }).ToList();

            return result;
        }

        public CriterionDto GetByKey(string key)
        {
            var result = _table.SingleOrDefault(x => x.Key == key);
            CriterionDto dto = null;
            if (result != null)
            {
                dto = new CriterionDto()
                {
                    Id = result.Id,
                    Key = result.Key,
                    Value = result.Value
                };
            }
            return dto;
        }

        public int CreateCriterion(CriterionDto dto)
        {
            var entity = dto.ConvertToEntity();
            base.Insert(entity);
            _unitOfWork.SaveChanges();
            return entity.Id;
        }

        public void UpdateCriterionByKey(int storeId, string key, string value)
        {
            var entity = _table.SingleOrDefault(x => x.Key == key && x.StoreId == storeId);
            if (entity == null)
            {
                throw new Exception("Criterion not found");
            }

            entity.Value = value;
            base.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateOrCreate(string key, CriterionDto dto)
        {
            var entity = _table.SingleOrDefault(x => x.Key == key && x.StoreId == dto.StoreId);
            if (entity == null)
            {
                CreateCriterion(dto);
            }
            else
            {
                UpdateCriterionByKey(dto.StoreId, key, dto.Value);
            }
        }

        public List<CriterionDto> GetListByStoreId(int storeId)
        {
            var result = _table.Where(x => x.StoreId == storeId).Select(x => new CriterionDto()
            {
                Key = x.Key,
                Value = x.Value,
                StoreId = x.StoreId,
                Id = x.Id
            }).ToList();

            return result;
        }

        public void InsertCriterionRange(List<CriterionDto> dtoList)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                foreach (var dto in dtoList)
                {
                    _repository.Insert(dto.ConvertToEntity());
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public void UpdateCriteriaByStore(int storeId, Dictionary<string, string> criterionList)
        {
            foreach (var item in criterionList)
            {
                var entity = _table.SingleOrDefault(x => x.StoreId == storeId && x.Key == item.Key);
                if (entity == null)
                {
                    Model.Criterion newCriterion = new Model.Criterion()
                    {
                        StoreId = storeId,
                        Key = item.Key,
                        Value = item.Value
                    };
                    base.Insert(newCriterion);
                }
                else
                {
                    entity.Value = item.Value;
                    base.Update(entity);
                }
            }
            _unitOfWork.SaveChanges();
        }
    }
}

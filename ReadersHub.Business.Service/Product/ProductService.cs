using _21stSolution.Application.Services.Dto;
using _21stSolution.Extensions;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.Mapper;
using ReadersHub.Common.Dto.Product;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ReadersHub.Business.Service.Product
{
    public class ProductService : Service<Model.Product>, IProductService
    {
        private readonly IRepository<Model.Product> _repository;
        private readonly IRepository<Model.Criterion> _criterionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<Model.Product> _table;
        public ProductService(IRepository<Model.Product> repository, IRepository<Model.Criterion> criterionRepository, IUnitOfWork unitOfWork)
            : base(repository)
        {
            _repository = repository;
            _criterionRepository = criterionRepository;
            _unitOfWork = unitOfWork;
            _table = base.Queryable();
        }

        public List<ProductDto> GetProducts(ProductFilterDto dto)
        {
            throw new NotImplementedException();
        }

        public List<string> GetISBNForUpdate()
        {
            var query = _table.OrderBy(x => x.PriceUpdateTimeUK).ThenBy(x => x.Id).Select(x => x.Isbn).Take(100);
            var result = query.AsNoTracking().ToList();

            return result;
        }

        public PagedResult<ProductDto> GetList(PagedRequest<ProductFilterDto> request)
        {
            /// Set default orders
            /// 
            if (request.Orders.Count == 0)
            {
                request.Orders.Add(new Order()
                {
                    IsDescending = true,
                    OrderBy = "PriceUpdateTimeUK",
                    Priority = 1
                });

                request.Orders.Add(new Order()
                {
                    IsDescending = true,
                    OrderBy = "PriceUpdateTimeUS",
                    Priority = 2
                });
            }

            var query = _table.Sort(request.Orders);

            var condition = GetFilterExpression(request.Filters);
            if (condition != null)
            {
                query = query.Where(condition);
            }

            var count = query.Count();
            int storeId = request.Filters.StoreId;

            var items = query.Skip(request.SkipCount).Take(request.MaxResultCount).ToList();
            List<ProductDto> result = new List<ProductDto>();
            foreach (var x in items)
            {
                result.Add(new ProductDto()
                {
                    Id = x.Id,
                    Asin = x.Asin,
                    Isbn = x.Isbn,
                    IsbnNewPriceDollar = x.MinNewIsbnPriceDollar ?? null,
                    IsbnUsedPriceDollar = x.MinUsedIsbnPriceDollar ?? null,
                    IsbnNewPricePound = x.MinNewIsbnPricePound ?? null,
                    IsbnUsedPricePound = x.MinUsedIsbnPricePound ?? null,
                    AsinNewPriceDollar = x.MinNewAsinPriceDollar ?? null,
                    AsinUsedPriceDollar = x.MinUsedAsinPriceDollar ?? null,
                    AsinNewPricePound = x.MinNewAsinPricePound ?? null,
                    AsinUsedPricePound = x.MinUsedAsinPricePound ?? null,
                    IsFixedNewDollar = x.IsFixedNewDollar ?? null,
                    IsFixedNewPound = x.IsFixedNewPound ?? null,
                    IsFixedUsedDollar = x.IsFixedUsedDollar ?? null,
                    IsFixedUsedPound = x.IsFixedUsedPound ?? null,
                });
            }

            return new PagedResult<ProductDto> { Items = result, TotalCount = count };
        }

        private Expression<Func<Model.Product, bool>> GetFilterExpression(ProductFilterDto filter)
        {
            Expression<Func<Model.Product, bool>> condition = null;

            if (filter != null)
            {
                if (!filter.Asin.IsNullOrEmpty()) { condition = condition.And(e => e.Asin.Contains(filter.Asin)); }
                if (!filter.Isbn.IsNullOrEmpty()) { condition = condition.And(x => x.Isbn.Contains(filter.Isbn)); }
                if (!filter.IsbnName.IsNullOrEmpty()) { condition = condition.And(x => x.IsbnName.Contains(filter.IsbnName)); }
            }

            return condition;
        }

        public void InsertProductRange(List<ProductDto> dtoList)
        {
            foreach (var item in dtoList)
            {
                base.Insert(item.ConvertToEntity());
            }
            _unitOfWork.SaveChanges();
        }

        public ProductDto GetProduct(int id)
        {
            var entity = _table.SingleOrDefault(x => x.Id == id);
            return entity != null ? entity.ConvertToDto() : null;
        }

        public void UpdateProductISBN(ProductDto dto)
        {
            var entity = _table.SingleOrDefault(x => x.Id == dto.Id);
            if (entity == null)
            {
                throw new Exception("Ürün bulunamadı");
            }
            entity.Asin = dto.Asin;
            entity.Isbn = dto.Isbn;
            base.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateProductPrice(string isbn, string currencyCode, decimal isbnPrice, decimal asinPrice, bool isUsed)
        {
            var result = _table.Where(x => x.Isbn == isbn).ToList();

            foreach (var item in result)
            {
                if (currencyCode == "USD")
                {
                    item.PriceUpdateTimeUS = DateTime.Now;
                }
                else if (currencyCode == "GBP")
                {
                    item.PriceUpdateTimeUK = DateTime.Now;
                }
                else
                {
                    item.PriceUpdateTimeUK = DateTime.Now;
                    item.PriceUpdateTimeUS = DateTime.Now;
                }

                if (isbnPrice < 0)
                {
                    _repository.Update(item);
                    continue;
                }

                if (isUsed)
                {
                    if (currencyCode == "USD")
                    {
                        item.MinUsedIsbnPriceDollar = isbnPrice;
                        item.MinUsedAsinPriceDollar = asinPrice;
                    }
                    else if (currencyCode == "GBP")
                    {
                        item.MinUsedIsbnPricePound = isbnPrice;
                        item.MinUsedAsinPricePound = asinPrice;
                    }
                }
                else
                {
                    if (currencyCode == "USD")
                    {
                        item.MinNewIsbnPriceDollar = isbnPrice;
                        item.MinNewAsinPriceDollar = asinPrice;
                    }
                    else if (currencyCode == "GBP")
                    {
                        item.MinNewIsbnPricePound = isbnPrice;
                        item.MinNewAsinPricePound = asinPrice;
                    }
                }
                _repository.Update(item);
            }
            _unitOfWork.SaveChanges();

            //List<SqlParameter> parameters = new List<SqlParameter>();
            //parameters.Add(new SqlParameter("@ISBN", isbn));
            //parameters.Add(new SqlParameter("@Currency_Code", currencyCode));
            //parameters.Add(new SqlParameter("@Is_Used", isUsed));
            //parameters.Add(new SqlParameter("@Min_ISBN_Price", price));

            //_repository.ExecuteStoredProcedure("[dbo].[Set_Min_ISBN_Price] @ISBN, @Currency_Code, @Is_Used, @Min_ISBN_Price", parameters.ToArray());
        }

        public void Delete(string isbn)
        {
            var entity = _table.SingleOrDefault(x => x.Isbn == isbn);
            if (entity != null)
            {
                base.Delete(entity);
                _unitOfWork.SaveChanges();
            }
        }

        public void UpdateAllProductPrice()
        {
            _repository.ExecuteStoredProcedure("[dbo].[Update_Product_Price]");
        }
        public void UpdateProductPrice(ProductDto dto, int storeId)
        {
            var entity = _table.SingleOrDefault(x => x.Id == dto.Id);
            if (entity == null)
            {
                return;
            }
            entity.IsFixedNewDollar = dto.IsFixedNewDollar;
            entity.IsFixedNewPound = dto.IsFixedNewPound;
            entity.IsFixedUsedDollar = dto.IsFixedUsedDollar;
            entity.IsFixedUsedPound = dto.IsFixedUsedPound;

            entity.MinNewIsbnPriceDollar = dto.IsbnNewPriceDollar;
            entity.MinNewIsbnPricePound = dto.IsbnNewPricePound;
            entity.MinUsedIsbnPriceDollar = dto.IsbnUsedPriceDollar;
            entity.MinUsedIsbnPricePound = dto.IsbnUsedPricePound;

            if (dto.IsFixedNewDollar.HasValue && dto.IsFixedNewDollar.Value)
            {
                entity.PriceUpdateTimeUS = DateTime.Now.AddYears(3);
            }
            else
            {
                entity.PriceUpdateTimeUS = DateTime.Now.AddYears(-1);
            }

            if (dto.IsFixedUsedDollar.HasValue && dto.IsFixedUsedDollar.Value)
            {
                entity.PriceUpdateTimeUS = DateTime.Now.AddYears(3);
            }
            else
            {
                entity.PriceUpdateTimeUS = DateTime.Now.AddYears(-1);
            }

            if (dto.IsFixedNewPound.HasValue && dto.IsFixedNewPound.Value)
            {
                entity.PriceUpdateTimeUK = DateTime.Now.AddYears(3);
            }
            else
            {
                entity.PriceUpdateTimeUK = DateTime.Now.AddYears(-1);
            }

            if (dto.IsFixedUsedPound.HasValue && dto.IsFixedUsedPound.Value)
            {
                entity.PriceUpdateTimeUK = DateTime.Now.AddYears(3);
            }
            else
            {
                entity.PriceUpdateTimeUK = DateTime.Now.AddYears(-1);
            }

            entity.MinNewAsinPriceDollar = CalculatePrice(entity.MinNewIsbnPriceDollar, false, storeId);
            entity.MinNewAsinPricePound = CalculatePrice(entity.MinNewIsbnPricePound, false, storeId);
            entity.MinUsedAsinPriceDollar = CalculatePrice(entity.MinUsedIsbnPriceDollar, true, storeId);
            entity.MinUsedAsinPricePound = CalculatePrice(entity.MinUsedIsbnPricePound, true, storeId);

            _repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        private decimal? CalculatePrice(decimal? isbnPrice, bool isUsed, int storeId)
        {
            if (!isbnPrice.HasValue)
            {
                return null;
            }

            decimal? _isbnNewPercentage = null;
            decimal? _isbnUsedPercentage = null;
            decimal? _isbnNewAdditionalPrice = null;
            decimal? _isbnUsedAdditionalPrice = null;

            var dto = _criterionRepository.Queryable().SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPercentage && x.StoreId == storeId);
            if (dto != null)
            {
                decimal percentage;
                if (decimal.TryParse(dto.Value, out percentage))
                {
                    _isbnNewPercentage = percentage;
                }
            }

            dto = _criterionRepository.Queryable().SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPrice && x.StoreId == storeId);
            if (dto != null)
            {
                decimal price;
                if (decimal.TryParse(dto.Value, out price))
                {
                    _isbnNewAdditionalPrice = price;
                }
            }

            dto = _criterionRepository.Queryable().SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPercentage && x.StoreId == storeId);
            if (dto != null)
            {
                decimal percentage;
                if (decimal.TryParse(dto.Value, out percentage))
                {
                    _isbnUsedPercentage = percentage;
                }
            }

            dto = _criterionRepository.Queryable().SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPrice && x.StoreId == storeId);
            if (dto != null)
            {
                decimal price;
                if (decimal.TryParse(dto.Value, out price))
                {
                    _isbnUsedAdditionalPrice = price;
                }
            }

            if (isUsed)
            {
                if (_isbnUsedAdditionalPrice.HasValue && _isbnUsedPercentage.HasValue)
                {
                    return isbnPrice * (1 + (decimal)(_isbnUsedPercentage / 100)) + (decimal)_isbnUsedAdditionalPrice;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (_isbnNewPercentage.HasValue && _isbnNewAdditionalPrice.HasValue)
                {
                    return isbnPrice * (1 + (decimal)(_isbnNewPercentage / 100)) + (decimal)_isbnNewAdditionalPrice;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

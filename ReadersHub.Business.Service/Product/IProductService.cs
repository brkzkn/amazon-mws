using ReadersHub.Common.Dto.Product;
using System.Collections.Generic;
using _21stSolution.Application.Services.Dto;

namespace ReadersHub.Business.Service.Product
{
    public interface IProductService
    {
        List<ProductDto> GetProducts(ProductFilterDto dto);
        ProductDto GetProduct(int id);
        void Delete(string isbn);
        List<string> GetISBNForUpdate();
        PagedResult<ProductDto> GetList(PagedRequest<ProductFilterDto> request);
        void InsertProductRange(List<ProductDto> dtoList);
        void UpdateProductISBN(ProductDto dto);
        void UpdateProductPrice(string isbn, string currencyCode, decimal isbnPrice, decimal asinPrice, bool isUsed);
        void UpdateAllProductPrice();
        void UpdateProductPrice(ProductDto dto, int storeId);

    }
}

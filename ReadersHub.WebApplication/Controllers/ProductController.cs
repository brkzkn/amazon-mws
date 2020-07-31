using _21stSolution.Application.Services.Dto;
using _21stSolution.Dto.DataTable;
using ReadersHub.Business.Service.Product;
using ReadersHub.Business.Service.Store;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.Product;
using ReadersHub.WebApplication.Core;
using ReadersHub.WebApplication.Core.Attribute;
using ReadersHub.WebApplication.Core.Extensions;
using ReadersHub.WebApplication.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Controllers
{
    public class ProductController : AuthorizedController
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        public ProductController(IProductService productService, IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }
        // GET: Product
        [Permission(Permissions.Product)]
        public ActionResult Index(int storeId = 0, bool isUpload = false)
        {
            var model = new ProductViewModel();
            model.StoreId = storeId;
            model.StoreList = _storeService.GetList().Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            if (isUpload)
            {
                SetNotificationInfo(NotificationHelper.MessageType.Success, "Yeni ürünler eklendi");
            }
            return View(model);
        }

        [Permission(Permissions.Product)]
        public ActionResult Upload(FormCollection formCollection, int storeId = 0)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    if (!fileName.Contains(".xlsx") && !fileName.Contains(".xls"))
                    {
                        SetNotificationInfo(NotificationHelper.MessageType.Error, "Yalnızca Excel formatı geçerlidir");
                        return View("Index");
                    }

                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    int excelColCount = 2;
                    List<string[]> result = new List<string[]>();

                    using (var doc = new SLDocument(file.InputStream))
                    {
                        var settings = doc.GetWorksheetStatistics();

                        var rowCount = GetRowCount(doc, settings, excelColCount);
                        for (var row = 1; row <= rowCount; row++)
                        {
                            var columnArray = new string[excelColCount];
                            for (var column = 1; column <= excelColCount; column++)
                            {
                                columnArray[column - 1] = doc.GetCellValueAsString(row, column);
                            }
                            result.Add(columnArray);
                        }
                    }

                    List<ProductDto> productList = new List<ProductDto>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i][1].Trim().Length != 10)
                        {
                            continue;
                        }
                        var a = result[i][0].PadLeft(10, '0');
                        productList.Add(new ProductDto()
                        {
                            Isbn = result[i][0].PadLeft(10, '0'),
                            Asin = result[i][1].Trim(),
                            AsinName = "",
                            IsbnName = "",
                            PriceUpdateTimeUK = DateTime.Now.AddMonths(-1),
                            PriceUpdateTimeUS = DateTime.Now.AddMonths(-1)
                        });
                    }
                    _productService.InsertProductRange(productList);
                }
                else
                {
                    SetNotificationInfo(NotificationHelper.MessageType.Error, "Lütfen eklemek istediğiniz dosyayı seçiniz");
                    return View("Index");
                }
            }

            return RedirectToAction("Index", new { isUpload = true, storeId = storeId });
        }

        private int GetRowCount(SLDocument doc, SLWorksheetStatistics settings, int columnCount)
        {
            bool search = false;
            int rowCount = settings.EndRowIndex;
            //endrowindex may be greater than exact value. So clear empty rows and calculate true value.
            for (int row = settings.EndRowIndex; row >= settings.StartRowIndex; row--)
            {
                search = false;
                for (int col = settings.StartColumnIndex; col <= columnCount; col++)
                {
                    if (doc.HasCellValue(row, col))
                    {
                        search = true;
                        break;
                    }
                }
                if (search)
                    break;
                else
                    rowCount--;
            }
            return rowCount;
        }

        [Permission(Permissions.Product)]
        public JsonResult GetList(DTParametersDto<ProductFilterDto> param, int storeId)
        {
            var request = param.Convert();
            request.Filters.StoreId = storeId;
            PagedResult<ProductDto> response = _productService.GetList(request);

            var result = new DTResult<ProductDto>
            {
                draw = param.Draw,
                data = response.Items,
                recordsFiltered = response.TotalCount,
                recordsTotal = response.TotalCount
            };

            return Json(result);
        }

        [HttpGet]
        [Permission(Permissions.Product)]
        public PartialViewResult Edit(int id)
        {
            var dto = _productService.GetProduct(id);
            var model = new ProductViewModel()
            {
                Id = dto.Id,
                Asin = dto.Asin,
                Isbn = dto.Isbn,
                ProductName = dto.IsbnName
            };
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [Permission(Permissions.Product)]
        public JsonResult Edit(ProductViewModel model)
        {
            _21stSolutionAjaxResponse response = new _21stSolutionAjaxResponse();
            try
            {

                var dto = new ProductDto()
                {
                    Id = model.Id,
                    Isbn = model.Isbn,
                    Asin = model.Asin
                };

                _productService.UpdateProductISBN(dto);
                response.IsSuccess = true;
                response.Message = "Ürün güncellendi";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return ReadersHubJson(response);
        }

        [HttpPost]
        [Permission(Permissions.Product)]
        public JsonResult UpdatePrice(ProductPriceViewModel model)
        {
            _21stSolutionAjaxResponse response = new _21stSolutionAjaxResponse();
            try
            {
                _productService.UpdateProductPrice(GetDto(model), model.StoreId);
                //_productService.UpdateProductISBN(dto);
                response.IsSuccess = true;
                response.Message = "Ürün güncellendi";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return ReadersHubJson(response);
        }

        public ProductDto GetDto(ProductPriceViewModel model)
        {
            var dto = new ProductDto()
            {
                IsFixedNewDollar = model.IsFixedNewDollar,
                IsFixedUsedDollar = model.IsFixedUsedDollar,
                IsFixedNewPound = model.IsFixedNewPound,
                IsFixedUsedPound = model.IsFixedUsedPound,
                Id = model.ProductId
            };

            decimal isbnNewPriceDollar;
            if (model.MinNewIsbnPriceDollar.SmartTryParseDecimal(out isbnNewPriceDollar))
            {
                dto.IsbnNewPriceDollar = isbnNewPriceDollar;
            }

            decimal isbnUsedPriceDollar;
            if (model.MinUsedIsbnPriceDollar.SmartTryParseDecimal(out isbnUsedPriceDollar))
            {
                dto.IsbnUsedPriceDollar = isbnUsedPriceDollar;
            }

            decimal isbnNewPricePound;
            if (model.MinNewIsbnPricePound.SmartTryParseDecimal(out isbnNewPricePound))
            {
                dto.IsbnNewPricePound = isbnNewPricePound;
            }

            decimal isbnUsedPricePound;
            if (model.MinUsedIsbnPricePound.SmartTryParseDecimal(out isbnUsedPricePound))
            {
                dto.IsbnUsedPricePound = isbnUsedPricePound;
            }

            return dto;
        }

    }
}
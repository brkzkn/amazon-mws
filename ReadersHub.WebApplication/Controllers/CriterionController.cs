using _21stSolution.Extensions;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.Product;
using ReadersHub.Business.Service.Store;
using ReadersHub.Common.Constants;
using ReadersHub.WebApplication.Core;
using ReadersHub.WebApplication.Core.Attribute;
using ReadersHub.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Controllers
{
    public class CriterionController : AuthorizedController
    {
        private readonly ICriterionService _criterionService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;

        public CriterionController(ICriterionService criterionService, IStoreService storeService, IProductService productService)
        {
            _criterionService = criterionService;
            _storeService = storeService;
            _productService = productService;
        }

        // GET: Criterion
        [Permission(Permissions.Criterion)]
        public ActionResult Index()
        {
            var model = new CriterionViewModel();
            model.StoreList = _storeService.GetList().Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            return View(model);
        }

        [Permission(Permissions.Criterion)]
        public PartialViewResult GetCriterion(int storeId)
        {
            var model = new CriterionViewModel();
            model.StoreId = storeId;
            var storeDto = _storeService.Get(storeId);
            model.IsDollar = storeDto.CurrencyCode == "USD";
            model.StoreName = storeDto.Name;

            /// TODO: Set criteria value
            /// 
            var result = _criterionService.GetListByStoreId(storeId);
            var criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackRating);
            if (criteria != null)
            {
                model.Rate = int.Parse(criteria.Value);
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackCount);
            if (criteria != null)
            {
                model.RateCount = int.Parse(criteria.Value);
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.SubCondition);
            if (criteria != null)
            {
                var subCondition = criteria.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                model.IsNew = subCondition.Any(x => x == "new");
                model.IsLikeNew = subCondition.Any(x => x == "like_new");
                model.IsVeryGood = subCondition.Any(x => x == "very_good");
                model.IsGood = subCondition.Any(x => x == "good");
                model.IsAcceptable = subCondition.Any(x => x == "acceptable");
                model.IsUnacceptable = subCondition.Any(x => x == "unacceptable");
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.Countries);
            if (criteria != null)
            {
                model.Countries.AddRange(criteria.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.BannedSellerId);
            if (criteria != null)
            {
                model.SellerIds = criteria.Value;
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPercentage);
            if (criteria != null)
            {
                model.IsbnNewPercentage = criteria.Value;
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPrice);
            if (criteria != null)
            {
                model.IsbnNewPrice = criteria.Value;
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPercentage);
            if (criteria != null)
            {
                model.IsbnUsedPercentage = criteria.Value;
            }

            criteria = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPrice);
            if (criteria != null)
            {
                model.IsbnUsedPrice = criteria.Value;
            }

            return PartialView("_Criterion", model);
        }

        [Permission(Permissions.Criterion)]
        public JsonResult UpdateCriterion(CriterionViewModel model)
        {
            _21stSolutionAjaxResponse response = new _21stSolutionAjaxResponse();
            try
            {
                Dictionary<string, string> criteriaKeyValue = new Dictionary<string, string>();

                if (model.RateCount.HasValue)
                {
                    criteriaKeyValue.Add(CriterionKeys.FeedbackCount, model.RateCount.ToString());
                }

                if (model.Rate.HasValue)
                {
                    criteriaKeyValue.Add(CriterionKeys.FeedbackRating, model.Rate.ToString());
                }

                if (model.SellerIds != null && model.SellerIds.Length > 0)
                {
                    criteriaKeyValue.Add(CriterionKeys.BannedSellerId, string.Join(",", model.SellerIds));
                }

                if (model.Countries != null && model.Countries.Count > 0)
                {
                    criteriaKeyValue.Add(CriterionKeys.Countries, string.Join(",", model.Countries));
                }

                List<string> subCondition = new List<string>();
                if (model.IsAcceptable)
                {
                    subCondition.Add("acceptable");
                }
                if (model.IsGood)
                {
                    subCondition.Add("good");
                }
                if (model.IsLikeNew)
                {
                    subCondition.Add("like_new");
                }
                if (model.IsNew)
                {
                    subCondition.Add("new");
                }
                if (model.IsUnacceptable)
                {
                    subCondition.Add("unacceptable");
                }
                if (model.IsVeryGood)
                {
                    subCondition.Add("very_good");
                }
                if (subCondition.Count > 0)
                {
                    criteriaKeyValue.Add(CriterionKeys.SubCondition, string.Join(",", subCondition));
                }

                _criterionService.UpdateCriteriaByStore(model.StoreId, criteriaKeyValue);

                response = new _21stSolutionAjaxResponse()
                {
                    IsSuccess = true,
                    Message = "Kriterler güncellendi",

                };

            }
            catch (Exception e)
            {
                response = new _21stSolutionAjaxResponse()
                {
                    IsSuccess = false,
                    Message = e.ToString(),
                };

            }
            return ReadersHubJson(response);
        }

        [Permission(Permissions.Criterion)]
        public JsonResult UpdatePriceCriterion(CriterionViewModel model)
        {
            _21stSolutionAjaxResponse response = new _21stSolutionAjaxResponse();

            try
            {
                Dictionary<string, string> criteriaKeyValue = new Dictionary<string, string>();

                if (!model.IsbnNewPercentage.IsNullOrEmpty())
                {
                    criteriaKeyValue.Add(CriterionKeys.IsbnNewPercentage, model.IsbnNewPercentage.ToString());
                }

                if (!model.IsbnNewPrice.IsNullOrEmpty())
                {
                    criteriaKeyValue.Add(CriterionKeys.IsbnNewPrice, model.IsbnNewPrice.ToString());
                }

                if (!model.IsbnUsedPercentage.IsNullOrEmpty())
                {
                    criteriaKeyValue.Add(CriterionKeys.IsbnUsedPercentage, model.IsbnUsedPercentage.ToString());
                }

                if (!model.IsbnUsedPrice.IsNullOrEmpty())
                {
                    criteriaKeyValue.Add(CriterionKeys.IsbnUsedPrice, model.IsbnUsedPrice.ToString());
                }

                _criterionService.UpdateCriteriaByStore(model.StoreId, criteriaKeyValue);
                _productService.UpdateAllProductPrice();
                response = new _21stSolutionAjaxResponse()
                {
                    IsSuccess = true,
                    Message = "Kriterler güncellendi",

                };

            }
            catch (Exception e)
            {
                response = new _21stSolutionAjaxResponse()
                {
                    IsSuccess = false,
                    Message = e.ToString(),
                };

            }
            return ReadersHubJson(response);
        }
    }
}
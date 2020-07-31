using ReadersHub.Business.Service.Criterions;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.Criterion;
using ReadersHub.Common.Dto.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ReadersHub.NotificationService
{
    public class Helper
    {
        #region Properties
        public DateTime UpdateDate
        {
            get
            { return _updateDate; }
        }
        #endregion

        #region Fields
        private int _feedbackRating;
        private List<string> _subConditions;
        private List<string> _countries;
        private int _feedbackCount;
        private List<string> _bannedSellerIds;
        private readonly ICriterionService _criterionService;
        private DateTime _updateDate;
        private string _sellerId;
        private StoreDto _storeDto;
        #endregion

        public Helper(ICriterionService criterionService, StoreDto storeDto, string sellerId)
        {
            _criterionService = criterionService;
            _subConditions = new List<string>();
            _bannedSellerIds = new List<string>();
            _countries = new List<string>();
            _sellerId = sellerId;
            _storeDto = storeDto;
            InitializeCriterion();
        }

        public void InitializeCriterion()
        {
            var result = _criterionService.GetList(_sellerId);
            if (result.Count == 0)
            {
                throw new Exception("Mağaza ait kriter bulunmadı.");
            }

            var dto = result.SingleOrDefault(x => x.Key == CriterionKeys.UpdateTime);
            if (dto == null)
            {
                _updateDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddSeconds(-1);
                var criterioDto = new CriterionDto()
                {
                    Key = CriterionKeys.UpdateTime,
                    Value = _updateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                    StoreId = _storeDto.Id
                };

                _criterionService.CreateCriterion(criterioDto);
            }
            else
            {
                _updateDate = DateTime.ParseExact(dto.Value, "dd.MM.yyyy HH:mm:ss", null);
                var newUpdateDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddSeconds(-1);
                _criterionService.UpdateCriterionByKey(_storeDto.Id, CriterionKeys.UpdateTime, newUpdateDate.ToString("dd.MM.yyyy HH:mm:ss"));
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackRating);
            if (dto != null)
            {
                _feedbackRating = int.Parse(dto.Value);
            }
            else
            {
                /// Uygulama çalışmaya devam mı etsin? yoksa default değer mi belirleyelim
                /// 

            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackCount);
            if (dto != null)
            {
                _feedbackCount = int.Parse(dto.Value);
            }
            else
            {
                /// Uygulama çalışmaya devam mı etsin? yoksa default değer mi belirleyelim
                /// 

            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.SubCondition);
            if (dto != null)
            {
                _subConditions.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                /// Uygulama çalışmaya devam mı etsin? yoksa default değer mi belirleyelim
                /// 

            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.BannedSellerId);
            if (dto != null)
            {
                _bannedSellerIds.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                /// Uygulama çalışmaya devam mı etsin? yoksa default değer mi belirleyelim
                /// 

            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.Countries);
            if (dto != null)
            {
                _countries.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                /// Uygulama çalışmaya devam mı etsin? yoksa default değer mi belirleyelim
                /// 

            }
        }

        public bool CheckCriteria(string feedbackRating, string feedbackCount, string sellerId, string subCondition, string country)
        {
            if (!string.IsNullOrEmpty(feedbackCount) && int.Parse(feedbackCount) < _feedbackCount)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(feedbackRating) && int.Parse(feedbackRating) < _feedbackRating)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(sellerId) && _bannedSellerIds.Any(x => x == sellerId))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(subCondition) && !_subConditions.Any(x => x == subCondition))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(country) && !_countries.Any(x => x == country))
            {
                return false;
            }

            return true;
        }
    }
}

using ReadersHub.Business.Service.Criterions;
using ReadersHub.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadersHub.PriceUpdateServiceUK
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
        private decimal? _isbnNewPercentage;
        private decimal? _isbnUsedPercentage;
        private decimal? _isbnNewAdditionalPrice;
        private decimal? _isbnUsedAdditionalPrice;
        #endregion

        public Helper(ICriterionService criterionService, string sellerId)
        {
            _criterionService = criterionService;
            _subConditions = new List<string>();
            _bannedSellerIds = new List<string>();
            _countries = new List<string>();
            _sellerId = sellerId;
            InitializeCriterion();
        }

        public void InitializeCriterion()
        {
            var result = _criterionService.GetList(_sellerId);
            if (result.Count == 0)
            {
                throw new Exception("Mağaza ait kriter bulunmadı.");
            }
            
            var dto = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackRating);
            if (dto != null)
            {
                _feedbackRating = int.Parse(dto.Value);
            }
            else
            {
                /// Default value
                _feedbackRating = 96;
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.FeedbackCount);
            if (dto != null)
            {
                _feedbackCount = int.Parse(dto.Value);
            }
            else
            {
                /// Default value
                _feedbackCount = 1000;
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.SubCondition);
            if (dto != null)
            {
                _subConditions.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                /// Default value
                string defaultValue = "good, like_new,new,very_good";
                _subConditions.AddRange(defaultValue.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.BannedSellerId);
            if (dto != null)
            {
                _bannedSellerIds.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.Countries);
            if (dto != null)
            {
                _countries.AddRange(dto.Value.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPercentage);
            if (dto != null)
            {
                decimal percentage;
                if (decimal.TryParse(dto.Value, out percentage))
                {
                    _isbnNewPercentage = percentage;
                }
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnNewPrice);
            if (dto != null)
            {
                decimal price;
                if (decimal.TryParse(dto.Value, out price))
                {
                    _isbnNewAdditionalPrice = price;
                }
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPercentage);
            if (dto != null)
            {
                decimal percentage;
                if (decimal.TryParse(dto.Value, out percentage))
                {
                    _isbnUsedPercentage = percentage;
                }
            }

            dto = result.SingleOrDefault(x => x.Key == CriterionKeys.IsbnUsedPrice);
            if (dto != null)
            {
                decimal price;
                if (decimal.TryParse(dto.Value, out price))
                {
                    _isbnUsedAdditionalPrice = price;
                }
            }
        }

        public bool CheckCriteria(string feedbackRating, string feedbackCount, string sellerId, string subCondition, string country)
        {
            if (int.Parse(feedbackCount) < _feedbackCount)
            {
                return false;
            }

            if (int.Parse(feedbackRating) < _feedbackRating)
            {
                return false;
            }

            if (_bannedSellerIds.Any(x => x == sellerId))
            {
                return false;
            }

            if (!_subConditions.Any(x => x == subCondition))
            {
                return false;
            }

            if (!_countries.Any(x => x == country) && !string.IsNullOrEmpty(country))
            {
                return false;
            }

            return true;
        }

        public decimal GetMinimumPrice(decimal price, bool isUsed)
        {
            if (isUsed)
            {
                return price * (1 + (decimal)(_isbnUsedPercentage / 100)) + (decimal)_isbnUsedAdditionalPrice;
            }
            else
            {
                return price * (1 + (decimal)(_isbnNewPercentage / 100)) + (decimal)_isbnNewAdditionalPrice;
            }
        }
    }
}

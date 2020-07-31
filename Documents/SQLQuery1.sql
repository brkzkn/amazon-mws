update top(1000) ft
set ft.Price = 
case when ft.Price < p.MinASINPrice then p.MinASINPrice else ft.Price end
from FeedTempTable ft
inner join Product p on ft.ASIN = p.ASIN

update top(1000) FeedTempTable 
set Status = 'Not Found'
where ASIN not in (select ASIN from Product)

/* Tekrar eden kayýtlarýn en yenisi alýp eskisini silme iþlemi */
with cte AS (select Id, ASIN, Price, CreateDate,
row_number() over(partition by ASIN order by CreateDate desc) AS [rn]
FROM Feed_Temp)
select Id from CTE where CTE.rn > 1 order by Id 
--delete from Feed_Temp where Id in(select Id from CTE where CTE.rn > 1) and Id < 17004

/*
	Fiyat belirleme kodu, select kýsmý update olacak. Birde status'u deðiþtirilecek.
*/
DECLARE @currencyCode varchar(3)
  SET @currencyCode = 'USD'
  select ft.Condition, ft.Price, 
	case 
		when ft.Condition = 'new' and @currencyCode = 'USD' and ft.Price < pp.Min_New_ASIN_Price_Dollar then pp.Min_New_ASIN_Price_Dollar 
		when ft.Condition = 'new' and @currencyCode = 'USD' and ft.Price >= pp.Min_New_ASIN_Price_Dollar then ft.Price
		when ft.Condition = 'used' and @currencyCode = 'USD' and ft.Price < pp.Min_Used_ASIN_Price_Dollar then pp.Min_Used_ASIN_Price_Dollar
		when ft.Condition = 'used' and @currencyCode = 'USD' and ft.Price >= pp.Min_Used_ASIN_Price_Dollar then ft.Price
		when ft.Condition = 'new' and @currencyCode = 'GBP' and ft.Price < pp.Min_New_ASIN_Price_Pound then pp.Min_New_ASIN_Price_Pound
		when ft.Condition = 'new' and @currencyCode = 'GBP' and ft.Price >= pp.Min_New_ASIN_Price_Pound then ft.Price
		when ft.Condition = 'used' and @currencyCode = 'GBP' and ft.Price < pp.Min_Used_ASIN_Price_Pound then pp.Min_Used_ASIN_Price_Pound
		when ft.Condition = 'used' and @currencyCode = 'GBP' and ft.Price >= pp.Min_Used_ASIN_Price_Pound then ft.Price
		else -1
	end
  from Feed_Temp ft
  join Product p on p.ASIN = ft.ASIN
  join Product_Price pp on p.Id = pp.Product_Id
  -- where ft.Id <= 55 and ft.Seller_Id = ''
  
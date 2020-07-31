-- =============================================
-- Author:		<Burak Özkan>
-- Create date: <26.08.2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Prepare_Feed_Temp]
	-- Add the parameters for the stored procedure here
	@SellerId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @MaxId int;
	DECLARE @currencyCode varchar(3);

	select @MaxId = Max(Id) from Feed_Temp;

	/* Tekrar eden kayıtların en yenisi alıp eskisini silme işlemi */
	with cte AS (select Id, ASIN, Price, CreateDate, row_number() over(partition by ASIN order by CreateDate desc) AS [rn] FROM Feed_Temp)
	delete from Feed_Temp where Id in(select Id from CTE where CTE.rn > 1) and Id < (@MaxId - 1)
	/*
	update Feed_Temp
	set [Status] = 'Not Found'
	where ASIN not in (select ASIN from Product)
	*/

  select @currencyCode = CurrencyCode from Store where SellerId = @SellerId;

  update ft 
    set ft.Price =
	case 
		when ft.Condition = 'new' and @currencyCode = 'USD' and ft.Price < pp.Min_New_ASIN_Price_Dollar then pp.Min_New_ASIN_Price_Dollar 
		when ft.Condition = 'new' and @currencyCode = 'USD' and ft.Price >= pp.Min_New_ASIN_Price_Dollar then ft.Price
		when ft.Condition = 'new' and @currencyCode = 'USD' and (pp.Min_New_ASIN_Price_Dollar is null and pp.Min_Used_ASIN_Price_Dollar is not null) then pp.Min_Used_ASIN_Price_Dollar * 100
		when ft.Condition = 'used' and @currencyCode = 'USD' and ft.Price < pp.Min_Used_ASIN_Price_Dollar then pp.Min_Used_ASIN_Price_Dollar
		when ft.Condition = 'used' and @currencyCode = 'USD' and ft.Price >= pp.Min_Used_ASIN_Price_Dollar then ft.Price
		when ft.Condition = 'new' and @currencyCode = 'GBP' and ft.Price < pp.Min_New_ASIN_Price_Pound then pp.Min_New_ASIN_Price_Pound
		when ft.Condition = 'new' and @currencyCode = 'GBP' and ft.Price >= pp.Min_New_ASIN_Price_Pound then ft.Price
		when ft.Condition = 'new' and @currencyCode = 'GBP' and (pp.Min_New_ASIN_Price_Pound is null and pp.Min_Used_ASIN_Price_Pound is not null) then pp.Min_Used_ASIN_Price_Pound * 100
		when ft.Condition = 'used' and @currencyCode = 'GBP' and ft.Price < pp.Min_Used_ASIN_Price_Pound then pp.Min_Used_ASIN_Price_Pound
		when ft.Condition = 'used' and @currencyCode = 'GBP' and ft.Price >= pp.Min_Used_ASIN_Price_Pound then ft.Price
		else -1
	end
  from Feed_Temp ft
  join Product p on p.ASIN = ft.ASIN
  join Product_Price pp on p.Id = pp.Product_Id
  --where ft.Id <= 55 and ft.Seller_Id = ''

delete from Feed_Temp where Price < 0;

END

-- =============================================
-- Author:		<Burak Özkan>
-- Create date: <04.10.2016>
-- Description:	<Calculate min ASIN price>
-- =============================================
CREATE FUNCTION [dbo].[Calculate_Min_ASIN]
(
	-- Add the parameters for the function here
	@ISBN_Price smallmoney,
	@StoreID int,
	@IsUsed bit,
	@Currency_Code varchar(3)
)
RETURNS smallmoney
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar smallmoney;
	
	-- Add the T-SQL statements to compute the return value here
	if(@ISBN_Price is null)
		return null;
	declare @StoreCurrencyCode varchar(3);
	declare @IsbnNewPercentage varchar(10);
	declare @IsbnNewPrice varchar(10);
	declare @IsbnUsedPercentage varchar(10);
	declare @IsbnUsedPrice varchar(10);

	select @StoreCurrencyCode = CurrencyCode from Store where Id = @StoreID;

	if(@StoreCurrencyCode is null or @StoreCurrencyCode = '' or @StoreCurrencyCode != @Currency_Code )
		return null;

	select @IsbnNewPercentage = Value from Criterion where [Key] = 'IsbnNewPercentage' and Store_Id = @StoreID;
	select @IsbnNewPrice = Value from Criterion where [Key] = 'IsbnNewPrice' and Store_Id = @StoreID;
	select @IsbnUsedPercentage = Value from Criterion where [Key] = 'IsbnUsedPercentage' and Store_Id = @StoreID;
	select @IsbnUsedPrice  = Value from Criterion where [Key] = 'IsbnUsedPrice' and Store_Id = @StoreID;

	if (@IsUsed = 1 and (@IsbnUsedPercentage is null or @IsbnUsedPercentage = '' or 
	   @IsbnUsedPrice is null or @IsbnUsedPrice = '' or 
	   try_cast(@IsbnUsedPercentage AS smallmoney) is null or try_cast(@IsbnUsedPrice AS smallmoney) is null))
	begin
		return null;
	end
	
	if (@IsUsed = 0 and (@IsbnNewPercentage is null or @IsbnNewPercentage = '' or 
	   @IsbnNewPrice is null or @IsbnNewPrice = '' or 
	   try_cast(@IsbnNewPercentage AS smallmoney) is null or try_cast(@IsbnNewPrice AS smallmoney) is null))
	begin
		return null;
	end
	
	if (@IsUsed = 1)
	begin
		set @ResultVar = (@ISBN_Price * (1 + (cast(@IsbnUsedPercentage as smallmoney) / 100)) + cast(@IsbnUsedPrice as smallmoney));
		if (@ResultVar < 3.01)	
			set @ResultVar = 3.01;
	end
	else
	begin
		set @ResultVar = (@ISBN_Price * (1 + (cast(@IsbnNewPercentage as smallmoney) / 100)) + cast(@IsbnNewPrice as smallmoney));
		if (@ResultVar < 5)
			set @ResultVar = 5;
	end

	-- Return the result of the function
	RETURN @ResultVar;

END
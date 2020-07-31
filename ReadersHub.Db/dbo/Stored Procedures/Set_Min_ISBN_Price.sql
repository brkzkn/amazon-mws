-- =============================================
-- Author:		Burak Özkan
-- Create date: 03.10.2016
-- Description:	Set ISBN Price
-- =============================================
CREATE PROCEDURE [dbo].[Set_Min_ISBN_Price] 
	-- Add the parameters for the stored procedure here
	@ISBN varchar(20),
	@Currency_Code varchar(3),
	@Is_Used bit,
	@Min_ISBN_Price smallmoney
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @StoreID int;
	Declare @ProductID int;

DECLARE Store_Cur CURSOR FOR
	SELECT Id From Store where CurrencyCode = @Currency_Code;

OPEN Store_Cur
FETCH NEXT FROM Store_Cur INTO @StoreID;
WHILE @@FETCH_STATUS = 0
BEGIN
	--PRINT 'Processing StoreID: ' + Cast(@StoreID as Varchar);
	DECLARE Product_Cur CURSOR FOR
		SELECT Id FROM Product Where ISBN= @ISBN;
	OPEN Product_Cur;
	FETCH NEXT FROM Product_Cur INTO @ProductID;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if(@Currency_Code = 'GBP') 
		begin
			update Product
				set Price_Update_Time_UK = GETDATE()
			where Id = @ProductID
		end

		if(@Currency_Code = 'USD') 
		begin
			update Product
				set Price_Update_Time_US = GETDATE()
			where Id = @ProductID
		end

		if(@Currency_Code = 'GBP' and @Is_Used = 1 and @Min_ISBN_Price > 0) 
		begin
			declare @MinUsedASINPricePound smallmoney;
			set @MinUsedASINPricePound = [dbo].[Calculate_Min_ASIN](@Min_ISBN_Price, @StoreID, 1, 'GBP');

			update Product_Price 
				set Min_Used_ISBN_Price_Pound = @Min_ISBN_Price,
					Min_Used_ASIN_Price_Pound = @MinUsedASINPricePound
			where Product_Id = @ProductID and Store_Id = @StoreID
		
			if @@rowcount = 0
			begin
				insert into Product_Price(Product_Id, Store_Id, Min_Used_ISBN_Price_Pound, Min_Used_ASIN_Price_Pound) values (@ProductID,@StoreID, @Min_ISBN_Price, @MinUsedASINPricePound)
			end
		end

		if(@Currency_Code = 'GBP' and @Is_Used = 0 and @Min_ISBN_Price > 0) 
		begin
			declare @MinNewASINPricePound smallmoney;
			set @MinNewASINPricePound = [dbo].[Calculate_Min_ASIN](@Min_ISBN_Price, @StoreID, 0, 'GBP');

			update Product_Price 
				set Min_New_ISBN_Price_Pound = @Min_ISBN_Price,
				Min_New_ASIN_Price_Pound = @MinNewASINPricePound
			where Product_Id = @ProductID and Store_Id = @StoreID
		
			if @@rowcount = 0
			begin
				insert into Product_Price(Product_Id, Store_Id, Min_New_ISBN_Price_Pound, Min_New_ASIN_Price_Pound) values (@ProductID,@StoreID, @Min_ISBN_Price, @MinNewASINPricePound)
			end
		end

		if(@Currency_Code = 'USD' and @Is_Used = 1 and @Min_ISBN_Price > 0) 
		begin
			declare @MinUsedASINPriceDollar smallmoney;
			set @MinUsedASINPriceDollar = [dbo].[Calculate_Min_ASIN](@Min_ISBN_Price, @StoreID, 1, 'USD');

			update Product_Price 
				set Min_Used_ISBN_Price_Dollar = @Min_ISBN_Price,
					Min_Used_ASIN_Price_Dollar = @MinUsedASINPriceDollar
			where Product_Id = @ProductID and Store_Id = @StoreID
		
			if @@rowcount = 0
			begin
				insert into Product_Price(Product_Id, Store_Id, Min_Used_ISBN_Price_Dollar, Min_Used_ASIN_Price_Dollar) values (@ProductID,@StoreID, @Min_ISBN_Price, @MinUsedASINPriceDollar)
			end
		end

		if(@Currency_Code = 'USD' and @Is_Used = 0 and @Min_ISBN_Price > 0) 
		begin
			declare @MinNewASINPriceDollar smallmoney;
			set @MinNewASINPriceDollar = [dbo].[Calculate_Min_ASIN](@Min_ISBN_Price, @StoreID, 0, 'USD');

			update Product_Price 
				set Min_New_ISBN_Price_Dollar = @Min_ISBN_Price
			where Product_Id = @ProductID and Store_Id = @StoreID
		
			if @@rowcount = 0
			begin
				insert into Product_Price(Product_Id, Store_Id, Min_New_ISBN_Price_Dollar, Min_New_ASIN_Price_Dollar) values (@ProductID,@StoreID, @Min_ISBN_Price, @MinNewASINPriceDollar)
			end
		end
		
		--PRINT 'StoreID: ' + Cast(@StoreID as Varchar) + ' ProductID: ' + Cast(@ProductID as Varchar);
		FETCH NEXT FROM Product_Cur INTO @ProductID;
	END;
	CLOSE Product_Cur;
	DEALLOCATE Product_Cur;
	FETCH NEXT FROM Store_Cur INTO @StoreID;
END;
--PRINT 'DONE';
CLOSE Store_Cur;
DEALLOCATE Store_Cur;

    -- Insert statements for procedure here
	--begin tran
	--   update Product_Price 
	--   set Min_New_ISBN_Price_Pound = 4.5
	--   where Product_Id = 1 and Store_Id = 1

	--   if @@rowcount = 0
	--   begin
	--	  insert into Product_Price(Product_Id, Store_Id, Min_New_ASIN_Price_Pound) values (@key,..)
	--   end
	--commit tran
	
END
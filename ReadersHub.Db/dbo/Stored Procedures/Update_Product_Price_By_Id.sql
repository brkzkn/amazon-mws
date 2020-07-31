-- =============================================
-- Author:		<Burak Özkan>
-- Create date: <28.10.2016>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [Update_Product_Price_By_Id]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update Product_Price
	set 
		Min_New_ASIN_Price_Dollar = [dbo].[Calculate_Min_ASIN](Min_New_Isbn_Price_Dollar, Store_Id, 0, 'USD'),
		Min_Used_ASIN_Price_Dollar = [dbo].[Calculate_Min_ASIN](Min_Used_Isbn_Price_Dollar, Store_Id, 1, 'USD'),
		Min_New_ASIN_Price_Pound = [dbo].[Calculate_Min_ASIN](Min_New_Isbn_Price_Pound, Store_Id, 0, 'GBP'),
		Min_Used_ASIN_Price_Pound = [dbo].[Calculate_Min_ASIN](Min_Used_Isbn_Price_Pound, Store_Id, 1, 'GBP')
	where Id = @Id
END
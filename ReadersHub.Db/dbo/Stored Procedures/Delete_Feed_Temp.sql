CREATE PROC [dbo].[Delete_Feed_Temp]
(
	@FeedIdList varchar(MAX)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @SQL varchar(MAX)

	SET @SQL = 
	'Delete from dbo.Feed_Temp
	WHERE Id IN (' + @FeedIdList + ')'

	EXEC(@SQL)	
END
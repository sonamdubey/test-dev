IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewShowroomPrices_Default]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewShowroomPrices_Default]
GO

	-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- EXEC [ViewShowroomPrices_Default] 500,''
-- Modify By : Sanjay Soni on 09/08/2016, included itemId 77 for TCS as default charge 
-- =============================================
CREATE PROCEDURE [dbo].[ViewShowroomPrices_Default] --163, ''
	-- Add the parameters for the stored procedure here
		@ModelId INT,
		@ItemList Varchar(100)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @DefaultItems VARCHAR(100) = '2,3,5,77'

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SELECT CV.ID AS VersionId, CV.Name VersionName
	--FROM CarVersions CV
	--WHERE CV.IsDeleted = 0 AND CV.CarModelId = @ModelId

	SELECT PQI.Id AS ItemId, PQI.CategoryName, PQI.CategoryId
	FROM PQ_CategoryItems PQI WITH (NOLOCK)
	--INNER JOIN PQ_Category PC WITH(NOLOCK) ON PC.CategoryId = PQI.CategoryId 
	WHERE PQI.Id IN (SELECT Items FROM SplitTextRS(ISNULL(@ItemList,@DefaultItems),','))
	ORDER BY PQI.CategoryId
	--ORDER BY PC.SortOrder
    
END





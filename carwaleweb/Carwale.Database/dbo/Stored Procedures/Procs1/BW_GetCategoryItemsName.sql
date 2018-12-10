IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetCategoryItemsName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetCategoryItemsName]
GO

	
-- Created By : Sanjay on 29/10/2014
-- To get CategoryItems
-- Modified By : Ashwini Todkar on 7 Nov 2014
-- BW_GetCategoryItemsName NULL

CREATE PROCEDURE [dbo].[BW_GetCategoryItemsName]
@CategoryItemList Varchar(100) = NULL
AS
BEGIN
	
	IF @CategoryItemList IS NULL
		SELECT ItemCategoryId,ItemName FROM BW_PQ_CategoryItems WITH(NOLOCK) WHERE IsActive = 1
		ORDER BY ItemCategoryId 
	ELSE
		SELECT CI.ItemCategoryId, CI.ItemName FROM BW_PQ_CategoryItems CI WITH(NOLOCK) WHERE CI.IsActive = 1
		AND CI.ItemCategoryId IN (SELECT Items FROM SplitTextRS(@CategoryItemList,','))
		ORDER BY CI.ItemCategoryId
END

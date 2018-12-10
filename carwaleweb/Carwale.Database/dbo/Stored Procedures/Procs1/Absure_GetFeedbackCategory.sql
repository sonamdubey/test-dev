IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetFeedbackCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetFeedbackCategory]
GO

	--========================================================================
-- Author      : Suresh Prajapati
-- Created On  : 17th April, 2015
-- Summary     : To get inspection feedback category
--========================================================================
CREATE PROCEDURE [dbo].[Absure_GetFeedbackCategory]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT Absure_RatingCategoryId [CategoryId]
		,CategoryText
		,Description
	FROM Absure_RatingCategory WITH (NOLOCK)
	WHERE IsActive = 1
	ORDER BY OrderPriority
END


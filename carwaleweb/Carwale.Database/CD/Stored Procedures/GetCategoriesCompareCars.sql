IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCategoriesCompareCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCategoriesCompareCars]
GO

	-- =============================================
-- Author:		Supriya
-- Create date: 9/7/2014
-- Description:	Get Categories data for specs & Features
-- Approved by Manish on 11-07-2014 04:30 pm
-- =============================================
CREATE PROCEDURE [CD].[GetCategoriesCompareCars]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CategoryMasterID Id,CategoryName Name, NodeCode,SortOrder FROM CD.CategoryMaster WITH(NOLOCK) WHERE lvl = 2 AND NodeCode LIKE '/1/%' ORDER BY SortOrder
	SELECT CategoryMasterID Id,CategoryName Name, NodeCode,SortOrder FROM CD.CategoryMaster WITH(NOLOCK) WHERE lvl = 2 AND NodeCode LIKE '/2/%' ORDER BY SortOrder


END


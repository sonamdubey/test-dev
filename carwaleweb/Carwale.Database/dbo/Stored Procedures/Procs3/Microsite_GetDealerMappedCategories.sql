IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerMappedCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerMappedCategories]
GO

	
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 11-06-2015
-- Description:	Get Dealer mapped Category List of Features 
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetDealerMappedCategories]
@DealerId VARCHAR(50),
@DWModelId VARCHAR(50)

AS
BEGIN
	SELECT   msdf.Id AS Value,mmfc.CategoryName AS Text 
	FROM     Microsite_DWModelFeatureCategories msdf
	JOIN     Microsite_ModelFeatureCategories  mmfc 
	ON       mmfc.Id=msdf.Microsite_ModelFeatureCategoriesId
	WHERE    msdf.DealerId=@DealerId AND msdf.DWModelId=@DWModelId AND msdf.IsActive=1 AND mmfc.IsActive=1
END



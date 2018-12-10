IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_AddDealerModelFeatureCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_AddDealerModelFeatureCategories]
GO

	
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-06-2015
-- Description:	Map Model Feature categories with Dealer
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_AddDealerModelFeatureCategories] 
@DealerId INT,
@DWModelId INT,
@CategoryId INT,
@SortOrder INT,
@Result INT OUTPUT
AS
BEGIN
 IF EXISTS(SELECT * FROM Microsite_DWModelFeatureCategories WHERE DealerId=@DealerId AND DWModelId=@DWModelId AND Microsite_ModelFeatureCategoriesId=@CategoryId )
 BEGIN
    SET     @Result=1
 END
 ELSE
 BEGIN
    INSERT INTO    Microsite_DWModelFeatureCategories
                  (DealerId,DWModelId,Microsite_ModelFeatureCategoriesId,SortOrder)
	VALUES        (@DealerId,@DWModelId,@CategoryId,@SortOrder)

	SET      @Result=2
  END
END



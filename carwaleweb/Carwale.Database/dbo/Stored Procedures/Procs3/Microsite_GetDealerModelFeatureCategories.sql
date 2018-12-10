IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerModelFeatureCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerModelFeatureCategories]
GO

	
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-06-2015
-- Description:	Get Category List of Features for Dealer Models
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetDealerModelFeatureCategories]

AS
BEGIN
  SELECT  Id AS Value,CategoryName AS Text  
  FROM    Microsite_ModelFeatureCategories
  WHERE   IsActive=1
END


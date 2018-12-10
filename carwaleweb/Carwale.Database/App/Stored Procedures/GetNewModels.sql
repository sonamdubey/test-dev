IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetNewModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetNewModels]
GO

	
-- =======================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Models of particular Make 
-- =======================================================

CREATE PROCEDURE [App].[GetNewModels]
	@CarMakeId  Integer
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT DISTINCT CM.ID AS ID, CM.Name AS Model, IsNull(CM.ReviewRate, 0) AS ReviewRate, 
		IsNull(CM.ReviewCount, 0) AS ReviewCount, 
		CM.SmallPic, CM.New, CM.HostUrl,CM.LargePic ,
		(SELECT TOP 1 MIN(AvgPrice) FROM Con_NewCarNationalPrices WHERE Con_NewCarNationalPrices.VersionId IN 
		(SELECT ID FROM CarVersions WHERE CarModelId = CM.ID AND New = 1 AND IsDeleted = 0) AND AvgPrice > 0) AS MinPrice 
		FROM CarModels AS CM, CarVersions AS CV, NewCarSpecifications NS 
		WHERE CM.IsDeleted = 0 AND CM.New = 1 AND CV.CarModelId = CM.ID AND  NS.CarVersionID=CV.ID AND 
		CM.CarMakeId = @CarMakeId 
		ORDER BY CM.New DESC, MinPrice ASC, CM.NAME
END

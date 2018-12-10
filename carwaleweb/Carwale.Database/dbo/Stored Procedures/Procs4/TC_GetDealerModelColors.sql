IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerModelColors]
GO

	
-------------------------------------------------------------------
-- =============================================
-- Author:		< HARSH PATEL >
-- Create date: < 2 JUNE 2015 >
-- Description:	< FETCH DEALER MODEL COLORS DETAILS >
-- Modified by:Komal manjare on 7 AUGUST 2015
--OriginalImgPath and HostUrl fetched 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerModelColors]
	-- Add the parameters for the stored procedure here
	@DealerId INT,
	@DealerModelId INT = NULL,
	@MakeId INT = NULL


AS
BEGIN

	SET NOCOUNT ON;

	SELECT DMC.Id AS ID,DMC.ColorName AS ColorName,('http://'+ DMC.HostUrl + DMC.ImgPath + DMC.ImgName) AS ImgUrl,ColorCode AS ColorCode,DMC.HostUrl,DMC.OriginalImgPath,
	CASE 
		WHEN (DM.DWModelName IS NOT NULL) 
		THEN DM.DWModelName 
		ELSE CMO.Name 
	END 
	ModelName,
	DM.DWModelName AS DWModelName,
	DM.ID AS DWModelId,
	CMA.ID AS CWMakeId,CMO.ID AS CWModelId,DMC.IsActive as IsActive,CMA.Name AS MakeName
	FROM 
	CarMakes CMA JOIN CarModels CMO ON CMO.CarMakeId = CMA.ID AND CMA.IsDeleted = 0 AND CMO.IsDeleted = 0
	JOIN TC_DealerModels DM ON DM.CWModelId = CMO.ID AND DM.IsDeleted = 0
	JOIN Microsite_DealerModelColors DMC ON DMC.DWModelId = DM.ID
	WHERE DMC.DealerId = @DealerId 
	AND (@DealerModelId IS NULL OR DMC.DWModelId = @DealerModelId)
	AND (@MakeId IS NULL OR CMA.ID = @MakeId)
	--AND DMC.IsActive = 1 

END
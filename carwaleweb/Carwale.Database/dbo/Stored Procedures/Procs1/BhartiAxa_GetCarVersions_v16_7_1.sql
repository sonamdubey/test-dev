IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarVersions_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarVersions_v16_7_1]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get Versions for particular model for bharti axa
-- Avishkar 21-10-2015 Added for showing default model IDs
-- Rohan 29-06-2016 Commented out bhartiaxa mapping , and showed all carwale versions
-- Modified By: Supreksha Singh
-- Description: Added IsDeleted check for Car Model Version
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarVersions_v16_7_1] 
	@ModelId int,
	@versionCond int = null
AS
BEGIN
IF(@versionCond=1)
BEGIN
	Select DISTINCT CWMV.Name as Text,CWMV.ID as Value
	FROM CarVersions CWMV with(nolock)INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = CWMV.CarModelId --CM.ID=CWM.ModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CM.CarMakeId = CWMK.ID
	Where  CWMV.CarModelId = @ModelId and CWMV.New=1 and CWMV.IsDeleted=0
	Order by Text
END
	
BEGIN
	Select DISTINCT CWMV.Name as Text,CWMV.ID as Value
	FROM CarVersions CWMV with(nolock) INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = CWMV.CarModelId --CM.ID=CWM.ModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CM.CarMakeId = CWMK.ID
	Where CWMV.CarModelId = @ModelId and CWMV.IsDeleted=0
	Order by Text
END
END



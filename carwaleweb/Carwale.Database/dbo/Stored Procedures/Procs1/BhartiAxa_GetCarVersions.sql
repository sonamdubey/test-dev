IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarVersions]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get Versions for particular model for bharti axa
-- Avishkar 21-10-2015 Added for showing default model IDs
-- Rohan 29-06-2016 Commented out bhartiaxa mapping , and showed all carwale versions
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarVersions] 
	@ModelId int,
	@versionCond int = null
AS
BEGIN
IF(@versionCond=1)
BEGIN
	Select DISTINCT CWMV.Name as Text,CWMV.ID as Value
	FROM CarVersions CWMV with(nolock) 
	--INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	--INNER JOIN BhartiAxa_CarVersions BACV with(nolock) on BACV.Reference_No=BAM.RefrenceId
	--INNER JOIN DefaultModelMapping CWM with(nolock) ON CWM.DefaultModelId = CWMV.CarModelId  -- Avishkar 21-10-2015 Added for showing default model IDs
	INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = CWMV.CarModelId --CM.ID=CWM.ModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CM.CarMakeId = CWMK.ID
	Where  CWMV.CarModelId = @ModelId and CWMV.New=1 --CWM.DefaultModelId  = @ModelId and CWMV.New=1
	Order by Text
END
	
BEGIN
	Select DISTINCT CWMV.Name as Text,CWMV.ID as Value
	FROM CarVersions CWMV with(nolock) 
	--INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	--INNER JOIN BhartiAxa_CarVersions BACV with(nolock) on BACV.Reference_No=BAM.RefrenceId
	--INNER JOIN DefaultModelMapping CWM with(nolock) ON CWM.DefaultModelId = CWMV.CarModelId  -- Avishkar 21-10-2015 Added for showing default model IDs
	INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = CWMV.CarModelId --CM.ID=CWM.ModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CM.CarMakeId = CWMK.ID
	Where CWMV.CarModelId = @ModelId --CWM.DefaultModelId  = @ModelId
	Order by Text
END
END


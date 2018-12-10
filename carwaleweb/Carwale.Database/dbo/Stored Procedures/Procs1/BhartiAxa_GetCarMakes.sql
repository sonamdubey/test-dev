IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarMakes]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get Models available with bharti axa for a particular make
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarMakes]
@makeCond int = null
AS
BEGIN
    IF(@makeCond=1)
BEGIN
	Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	FROM CarVersions CWMV with(nolock) INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	INNER JOIN BhartiAxa_CarVersions BACV  with(nolock) on BACV.Reference_No=BAM.RefrenceId
	INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	WHERE CWMK.New=1
	Order by Text
	--Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	--FROM CarVersions CWMV with(nolock) INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	--INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	--INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	--WHERE CWMK.New=1
	--Order by Text
END
	ELSE
BEGIN
Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	FROM CarVersions CWMV with(nolock) INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	INNER JOIN BhartiAxa_CarVersions BACV with(nolock) on BACV.Reference_No=BAM.RefrenceId
	INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	Order by Text
	--Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	--FROM CarVersions CWMV with(nolock) INNER JOIN BhartiAxa_Carwale_MMV BAM with(nolock) ON BAM.CWVersionId = CWMV.ID
	--INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	--INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	--Order by Text
END
END



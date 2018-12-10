IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarMakes_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarMakes_v16_7_1]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get Models available with bharti axa for a particular make
-- Modified By: Supreksha Singh
-- Description: Get old car make-models
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarMakes_v16_7_1]
@makeCond int = null
AS
BEGIN
    IF(@makeCond=1)
BEGIN
	Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	FROM CarVersions CWMV with(nolock) INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	Where CWMK.IsDeleted = 0 and CWMK.New = 1 Order by Text  
END
	ELSE
BEGIN
    Select DISTINCT CWMK.Name as Text,CWMK.ID as Value
	FROM CarVersions CWMV with(nolock) INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	Where CWMK.IsDeleted = 0 Order by Text  
END

END

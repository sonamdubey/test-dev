IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarModels_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarModels_v16_7_1]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 27.02.2014
-- Description:	Get Models for particular make for bharti axa
-- Avishkar 21-10-2015 Added for showing default model IDs
-- Rohan 30-06-2016 Commented out joins to bhartiaxa tables,to show all carwale models
-- Modified By: Supreksha Singh
-- Description: Added IsDeleted check for Car Model
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarModels_v16_7_1] 
	@MakeId int,
	@modelCond int = null
AS
BEGIN
IF(@modelCond=1)
BEGIN
	Select DISTINCT CWM.Name as Text,CWM.ID as Value
	FROM CarVersions CWMV with(nolock)  
	INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	Where CWM.CarMakeId=@MakeId and CWM.New=1 and CWM.IsDeleted=0
	Order by Text
END
	ELSE
BEGIN
	Select DISTINCT CWM.Name as Text,CWM.ID as Value
	FROM CarVersions CWMV with(nolock)  
	INNER JOIN CarModels CWM with(nolock) ON CWM.ID = CWMV.CarModelId
	INNER JOIN CarMakes CWMK with(nolock) ON CWM.CarMakeId = CWMK.ID 
	Where CWM.CarMakeId=@MakeId and CWM.IsDeleted=0
	Order by Text
END
END




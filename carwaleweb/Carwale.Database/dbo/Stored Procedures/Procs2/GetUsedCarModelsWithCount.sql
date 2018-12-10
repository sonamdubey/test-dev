IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarModelsWithCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarModelsWithCount]
GO

	-- =============================================
-- Author:		Akansha Shrivastava
-- Create date: 2/12/2013 10:17:54 PM
-- Description:	To get model of the used cars

-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column

-- =============================================
CREATE PROCEDURE   [dbo].[GetUsedCarModelsWithCount]
AS
BEGIN
	SELECT  (MakeName +' '+ ModelName) AS FullModelName,MakeName,ModelName,MaskingName
	,COUNT(ProfileId) AS ModelCount
	FROM LiveListings LL
	Inner Join CarModels CMO on LL.ModelId=CMO.ID
	GROUP BY (MakeName +' '+ ModelName),MakeName,ModelName,MaskingName
	HAVING COUNT(profileid) > 19
	ORDER BY ModelCount DESC
END

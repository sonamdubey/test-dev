IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarModels_BrowsePage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarModels_BrowsePage]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 14 June 2013
-- Description:	Proc to get the car models data
/*
	GetCarModels_BrowsePage 7
*/
-- Modified By : Ashish G. Kamble on 10 July 2013
-- Modified By : Akansha on 4.2.2014
-- Description : Added Masking Name Column


-- Modified By: Satish Sharma
-- Description: Removed 2 parameters BodyStyleId and SegmentId
-- =============================================
CREATE PROCEDURE [dbo].[GetCarModels_BrowsePage]  --                         
	@CarMakeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DISTINCT CM.ID AS ID
		,CM.CarMakeId AS MakeId
		,CM.NAME AS Model
		,IsNull(CM.ReviewRate, 0) AS ReviewRate
		,IsNull(CM.ReviewCount, 0) AS ReviewCount
		,CM.SmallPic
		,CM.New
		,CM.HostURL		
		,CM.MinPrice
		,CM.MaxPrice		
		,CM.MaskingName
	FROM CarModels AS CM WITH(NOLOCK)
	INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.CarModelId=CM.ID	
	WHERE CM.IsDeleted = 0
	AND CV.IsSpecsExist=1
	AND CM.Futuristic=0
	AND CM.CarMakeId = @CarMakeId
	ORDER BY CM.New DESC, MinPrice ASC, CM.NAME
END



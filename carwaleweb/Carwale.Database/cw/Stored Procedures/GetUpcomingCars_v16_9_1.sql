IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetUpcomingCars_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetUpcomingCars_v16_9_1]
GO

	
-- =============================================  
-- Author:  Vikas  
-- Create date: 20/12/2012  
-- Description: To get the list of upcoming cars  
-- [cw].[GetUpcomingCars] 0, 8  
-- =============================================  
-- =============================================  
-- Modified by: Amit Verma  
-- Modified on: 14/01/2013  
-- Description: Added '@ModelIDs' parameter to avoid those models to appear in the result set  
--    Also added ModelId column and commented the 'SmallDescription' column in the select statements  
--Modified By:Prashant Vishe On 02 Sept 2013  upcoming cars will be sorted by priority in ascending order and then by launch date in ascending order
-- Modified By : Akansha on 4.2.2014
-- Description : Added Masking Name Column
-- Modified by rohan 18-8-2015 Changed ordering and put priority first before launchdate
-- Modified By Ajay singh on 31 august 2016 added condition for fetch only models  
-- =============================================  
CREATE PROCEDURE [cw].[GetUpcomingCars_v16_9_1]
	-- Add the parameters for the stored procedure here  
	@MakeId INT = 0
	,@Cnt TINYINT = 100
	,@ModelIDs VARCHAR(max) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

	-- Insert statements for procedure here  
	IF @MakeId != 0
	BEGIN
		SELECT TOP (@Cnt) MK.ID MakeId
			,MK.NAME MakeName
			,Mo.NAME AS ModelName
			,Mo.ID AS ModelID
			,ECL.ExpectedLaunch
			,ECL.PhotoName
			,EstimatedPriceMin
			,EstimatedPriceMax
			,Mo.HostUrl
			,Mo.OriginalImgPath
			,Mo.SmallPic
			,Mo.LargePic
			,Mo.MaskingName --, CS.SmallDescription AS [Description]  
		FROM ExpectedCarLaunches ECL WITH (NOLOCK)
		INNER JOIN CarModels Mo WITH (NOLOCK) ON ECL.CarModelId = Mo.ID
		INNER JOIN CarMakes MK WITH (NOLOCK) ON MK.ID = Mo.CarMakeId
		LEFT JOIN CarSynopsis CS WITH (NOLOCK) ON CS.ModelId = Mo.CarMakeId
		WHERE Mo.Futuristic = 1
			AND ECL.isLaunched = 0
			AND ECL.IsDeleted = 0
			AND ECL.CarVersionId IS NULL --added by ajay singh
			AND Mo.CarMakeId = @MakeId			
		ORDER BY CASE 
				WHEN ECL.Priority IS NULL
					THEN 1
				ELSE 0
				END
			,ECL.Priority ASC		--18-08-2015 ORDERING BY PRIORTY FIRST
			,ECL.LaunchDate ASC
			--,ECL.Priority ASC
	END
	ELSE
	BEGIN
		SELECT TOP (@Cnt) MK.ID MakeId
			,MK.NAME MakeName
			,Mo.NAME AS ModelName
			,Mo.ID AS ModelID
			,ECL.ExpectedLaunch
			,ECL.PhotoName
			,EstimatedPriceMin
			,EstimatedPriceMax
			,Mo.HostUrl
			,Mo.OriginalImgPath
			,Mo.SmallPic
			,Mo.LargePic
			,Mo.MaskingName --, CS.SmallDescription AS [Description]  
		FROM ExpectedCarLaunches ECL WITH (NOLOCK)
		INNER JOIN CarModels Mo WITH (NOLOCK)  ON ECL.CarModelId = Mo.ID
		INNER JOIN CarMakes MK  WITH (NOLOCK) ON MK.ID = Mo.CarMakeId
		LEFT JOIN CarSynopsis  CS WITH (NOLOCK) ON CS.ModelId = Mo.CarMakeId
		WHERE Mo.Futuristic = 1
			AND ECL.isLaunched = 0
			AND ECL.IsDeleted = 0
			AND ECL.CarVersionId IS NULL --added by ajay singh
			AND Mo.ID NOT IN (
				SELECT items
				FROM dbo.SplitText(@modelIDs, ',')
				) --to avoid the ModelIDs present in @modelIDs to appear in the result set  
		ORDER BY CASE 
				WHEN ECL.Priority IS NULL
					THEN 1
				ELSE 0
				END
			,ECL.Priority ASC		--18-08-2015 ORDERING BY PRIORTY FIRST
			,ECL.LaunchDate ASC
			--,ECL.Priority ASC
	END
END


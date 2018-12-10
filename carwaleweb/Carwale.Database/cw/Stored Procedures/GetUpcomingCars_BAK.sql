IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetUpcomingCars_BAK]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetUpcomingCars_BAK]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 20/12/2012
-- Description:	To get the list of upcoming cars
-- [cw].[GetUpcomingCars] 0, 8
-- =============================================
-- =============================================
-- Modified by:	Amit Verma
-- Modified on: 14/01/2013
-- Description:	Added '@ModelIDs' parameter to avoid those models to appear in the result set
--				Also added ModelId column and commented the 'SmallDescription' column in the select statements
-- =============================================
CREATE PROCEDURE [cw].[GetUpcomingCars_BAK]
	-- Add the parameters for the stored procedure here
	@MakeId INT = 0,
	@Cnt TINYINT = 3,
	@ModelIDs varchar(max) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   IF @MakeId != 0
   BEGIN 
		SELECT TOP (@Cnt) MK.Name MakeName, Mo.Name AS ModelName, Mo.ID as ModelID, ECL.ExpectedLaunch, ECL.PhotoName, EstimatedPriceMin, EstimatedPriceMax, Mo.HostUrl, Mo.SmallPic, Mo.LargePic--, CS.SmallDescription AS [Description]
		FROM 
			ExpectedCarLaunches ECL 
			INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
			INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId 
			LEFT JOIN CarSynopsis CS ON CS.ModelId = Mo.CarMakeId
		WHERE 
			Mo.Futuristic = 1 
			AND ECL.isLaunched = 0 AND ECL.IsDeleted = 0
			AND Mo.CarMakeId = @MakeId 
		ORDER BY ECL.LaunchDate
	END	
	ELSE 
	BEGIN
		SELECT TOP (@Cnt) MK.Name MakeName, Mo.Name AS ModelName, Mo.ID as ModelID, ECL.ExpectedLaunch, ECL.PhotoName, EstimatedPriceMin, EstimatedPriceMax, Mo.HostUrl, Mo.SmallPic, Mo.LargePic--, CS.SmallDescription AS [Description]
		FROM 
			ExpectedCarLaunches ECL 
			INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
			INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId 
			LEFT JOIN CarSynopsis CS ON CS.ModelId = Mo.CarMakeId
		WHERE 
			Mo.Futuristic = 1 
			AND ECL.isLaunched = 0 AND ECL.IsDeleted = 0
			AND Mo.ID NOT IN (SELECT items FROM dbo.SplitText(@modelIDs,','))--to avoid the ModelIDs present in @modelIDs to appear in the result set
		ORDER BY ECL.LaunchDate ASC
	END
END

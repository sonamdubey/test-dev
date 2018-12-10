IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQVersionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQVersionDetails]
GO

	/*
Author: Ashish Verma on 30-07-2014
Description : Proc to get version details.
*/
CREATE  PROCEDURE [dbo].[GetPQVersionDetails]
	@VersionId INT,
	@MakeId BIGINT OUTPUT,
	@MakeName VARCHAR(30) OUTPUT,
	@ModelId BIGINT OUTPUT,
	@ModelName VARCHAR(30) OUTPUT,
	@MaskingName VARCHAR(30) = null OUTPUT,
	@VersionName VARCHAR(50) OUTPUT,
	@LargePic VARCHAR(50) OUTPUT,
	@SmallPic VARCHAR(50) OUTPUT,
	@HostURL VARCHAR(100) OUTPUT,
	@ReviewRate FLOAT OUTPUT,
	@ReviewCount NUMERIC(18,2) OUTPUT,
	@New BIT OUTPUT,
	@MinPrice FLOAT OUTPUT,
	@IsDeleted BIT = 0 OUTPUT
AS
BEGIN
	SELECT 
		@MakeId = MA.ID, @MakeName = MA.Name, @ModelId = MO.ID, @ModelName = MO.Name, @MaskingName=MO.MaskingName, @VersionName = CV.Name, 
		@HostURL = CV.HostURL, @LargePic = CV.largePic, @SmallPic = CV.SmallPic,
		@ReviewRate = ISNULL(CV.ReviewRate,0), @ReviewCount = ISNULL(CV.ReviewCount,0), 
		@New = CV.New, @MinPrice = SP.Price,@IsDeleted=CV.IsDeleted -- Added new parameter @IsDeleted 
	FROM CarVersions CV WITH (NOLOCK)
		INNER JOIN CarModels MO WITH (NOLOCK)  ON MO.ID=CV.CarModelId
		INNER JOIN CarMakes MA WITH (NOLOCK)  ON MA.ID=MO.CarMakeId
		LEFT JOIN NewCarShowroomPrices SP  WITH (NOLOCK) ON SP.CarVersionId = CV.ID
	WHERE CV.ID=@VersionId --Removed AND CV.IsDeleted=0	to list all the cars
END 


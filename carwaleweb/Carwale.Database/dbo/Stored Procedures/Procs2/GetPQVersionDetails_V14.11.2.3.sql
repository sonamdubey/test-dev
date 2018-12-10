IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQVersionDetails_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQVersionDetails_V14]
GO

	/*
Author: Ashish Verma on 30-07-2014
Description : Proc to get version details.
-- modified by ashish  on 11/07/2014 added specSummery
*/
CREATE PROCEDURE [dbo].[GetPQVersionDetails_V14.11.2.3]
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
	@IsDeleted BIT = 0 OUTPUT,
	@SpecSummery VARCHAR (100) OUTPUT -- modified by ashish 
AS
BEGIN
	SELECT 
		@MakeId = MA.ID, @MakeName = MA.Name, @ModelId = MO.ID, @ModelName = MO.Name, @MaskingName=MO.MaskingName, @VersionName = CV.Name, 
		@HostURL = CV.HostURL, @LargePic = CV.largePic, @SmallPic = CV.SmallPic,
		@ReviewRate = ISNULL(CV.ReviewRate,0), @ReviewCount = ISNULL(CV.ReviewCount,0), 
		@New = CV.New, @MinPrice = SP.Price,@IsDeleted=CV.IsDeleted,@SpecSummery =CV.SpecsSummary -- Added new parameter @IsDeleted ,@specSummery by ashish Verma
	FROM CarVersions CV WITH (NOLOCK)
		INNER JOIN CarModels MO WITH (NOLOCK)  ON MO.ID=CV.CarModelId
		INNER JOIN CarMakes MA WITH (NOLOCK)  ON MA.ID=MO.CarMakeId
		LEFT JOIN NewCarShowroomPrices SP  WITH (NOLOCK) ON SP.CarVersionId = CV.ID
	WHERE CV.ID=@VersionId --Removed AND CV.IsDeleted=0	to list all the cars
END 



IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQVersionDetails_v16_6_9]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQVersionDetails_v16_6_9]
GO

	-- =============================================
-- Author:		Ashish Verma on 30-07-2014
-- Create date: <Create Date,,>
-- Description:	 Proc to get version details.
-- modified by ashish  on 11/07/2014 added specSummery
-- Modified by : Shalini on 17/11/14 added CityId as a parameter 
-- Modified by : Vikas J on 17/04/15 added XLargePic in select
-- Mofified By Satish Sharma on 10/Jun/2015, Increases size of input parameter @LargePic, @SmallPic from Varchar 50 to 150
-- Modified By: Rakesh Yadav on 09 Oct 2015,Fetch car version fuelType and fuelTypeId 
-- Modified By: Rakesh Yadav on 1 July 2016, Fetch average price
-- =============================================
CREATE PROCEDURE [dbo].[GetPQVersionDetails_v16_6_9]
	-- Add the parameters for the stored procedure here
	@VersionId INT,
	@MakeId INT OUTPUT,
	@MakeName VARCHAR(30) OUTPUT,
	@ModelId INT OUTPUT,
	@ModelName VARCHAR(30) OUTPUT,
	@MaskingName VARCHAR(30) = null OUTPUT,
	@VersionName VARCHAR(50) OUTPUT,
	@LargePic VARCHAR(150) OUTPUT,
	@SmallPic VARCHAR(150) OUTPUT,
	@XLargePic VARCHAR(300) OUTPUT,
	@HostURL VARCHAR(100) OUTPUT,
	@ReviewRate DECIMAL(14,4) OUTPUT,
	@ReviewCount INT OUTPUT,
	@New BIT OUTPUT,
	@MinPrice FLOAT OUTPUT,
	@MinAvgPrice INT OUTPUT,
	@IsDeleted BIT = 0 OUTPUT,
	@SpecSummery VARCHAR (100) OUTPUT, -- modified by ashish 
	@OriginalImgPath VARCHAR (150) OUTPUT,
	@FuelTypeId INT OUTPUT,--modified by Rakesh Yadav
	@FuelType Varchar(20) OUTPUT,--modified by Rakesh Yadav
	@CityId int,
	@SubSegmentId int output --modified by anchal

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		@MakeId = MA.ID, @MakeName = MA.Name, @ModelId = MO.ID, @ModelName = MO.Name, @MaskingName=MO.MaskingName, @VersionName = CV.Name, 
		@HostURL = CV.HostURL, @LargePic = CV.largePic, @SmallPic = CV.SmallPic, @XLargePic = ISNULL(MO.XLargePic,''),
		@ReviewRate = ISNULL(CV.ReviewRate,0), @ReviewCount = ISNULL(CV.ReviewCount,0),@OriginalImgPath=CV.OriginalImgPath , 
		@New = CV.New, @MinPrice = SP.Price,@MinAvgPrice=NCP.AvgPrice,@IsDeleted=CV.IsDeleted,@SpecSummery =CV.SpecsSummary -- Added new parameter @IsDeleted ,@specSummery by ashish Verma
		,@FuelType=CF.FuelType,@FuelTypeId=CF.FuelTypeId, @SubSegmentId = MO.SubSegmentID
	FROM CarVersions CV WITH (NOLOCK)
		INNER JOIN CarModels MO WITH (NOLOCK)  ON MO.ID=CV.CarModelId
		INNER JOIN CarMakes MA WITH (NOLOCK)  ON MA.ID=MO.CarMakeId
		INNER JOIN CarFuelType CF WITH (NOLOCK) ON CF.FuelTypeId=CV.CarFuelType --Added By Rakesh Yadav on 09 Oct 2015
		LEFT JOIN Con_NewCarNationalPrices NCP WITH(NOLOCK) ON NCP.VersionId = CV.ID
		LEFT JOIN NewCarShowroomPrices SP  WITH (NOLOCK) ON SP.CarVersionId = CV.ID and SP.CityId = @CityId --modified by shalini 
	WHERE CV.ID=@VersionId --Removed AND CV.IsDeleted=0	to list all the cars
END


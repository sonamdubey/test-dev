IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_RequestWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_RequestWarranty]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 12/12/2014
-- Description:	Set OptWarranty to 1 if customer Requests for warranty 
-- Modified BY :Tejashree Patil on 7 JAn 2014, Fetched V.CarFuelType,V.CarTransmission based on version.
-- Modified By: Ashwini Dhamankar on 16/01/2015, Called AbSure_SaveCarPhotos SP for inserting new record for main image of car.
-- Modified By: Ashwini Dhamankar on March 6,2015, Added @AbSure_CarId as OUTPUT Parameter 
-- exec [dbo].[AbSure_RequestWarranty] 610734,5,NULL,NULL,NULL,NULL,NULL,NULL
-- Modified By: Yuga Hatolkar on 16th March, 2015, Added @AppointmentDate, @TimeSlot, and @UserId as Input Parameter
-- Modified By: Ashwini Dhamankar on May 5,2015 , Fetched car details from Absure_CarDetails for Rerequested car
-- Modified by: Ruchira Patil on 1st Jun 2015 (automatic agency assignment)
-- Modified By : Suresh Prajapati on 30th June, 2015
-- Description : Automatic surveyor assignment 
-- Modified By : Kartik rathod on 29 sept 2015 , added @CarSourceId for the car added through Add Stock
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_RequestWarranty] @StockId BIGINT = NULL
	,@BranchId BIGINT
	,@OwnerName VARCHAR(50) = NULL
	,@OwnerAddress VARCHAR(100) = NULL
	,@OwnerMobileNo VARCHAR(50) = NULL
	,@OwnerEmail VARCHAR(50) = NULL
	,@OwnerCityId BIGINT = NULL
	,@OwnerAreaId BIGINT = NULL
	,@AppointmentDate DATETIME = NULL
	,@UserId INT = NULL
	,@TimeSlot VARCHAR(100) = NULL
	,@AbSure_CarId NUMERIC(18, 0) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ImgLargeUrl VARCHAR(250) = NULL
		,@ImgLargeUrlFull VARCHAR(250) = NULL
		,@ImgThumbUrl VARCHAR(250) = NULL
		,@ImgThumbUrlFull VARCHAR(250) = NULL
		,@CarFittedWith TINYINT = NULL
	DECLARE @Make VARCHAR(100)
		,@Model VARCHAR(100)
		,@Version VARCHAR(100)
		,@VersionId INT
		,@VIN VARCHAR(50)
		,@RegNumber VARCHAR(50)
		,@Source SMALLINT
		,@MakeYear DATETIME
		,@Kilometer INT
		,@Owners VARCHAR(50)
		,@Colour VARCHAR(50)
		,@Insurance VARCHAR(50)
		,@InsuranceExpiry DATETIME
		,@IsOrigionalRC BIT
		,@IsBankHypothecation BIT
		,@RegisteredAt VARCHAR(50)
		,@AvailableAt VARCHAR(50)
		,@FuelType SMALLINT
		,@Transmission SMALLINT
		,@RegistrationDate DATETIME
		,@RegistrationType SMALLINT
		,@AbSure_CarDetailsId NUMERIC(18, 0)
		,@ImageUrlThumbSmall VARCHAR(250) = NULL
		,@DirectoryPath VARCHAR(250)
		,@HostUrl VARCHAR(250)
		,@AbSure_CarPhotosId INT
		,@IsReplicated BIT
		,@StatusId TINYINT
		,@SurveyorId INT

	IF (
			@StockId IS NOT NULL
			AND @AbSure_CarId IS NULL
			)
	BEGIN
		SELECT @Make = V.Make
			,@Model = V.Model
			,@Version = V.Version
			,@VersionId = V.VersionId
			,@VIN = NULL
			,@RegNumber = ST.RegNo
			,@Source = 2
			,@MakeYear = ST.MakeYear
			,@Kilometer = ST.Kms
			,@Owners = CC.Owners
			,@Colour = ST.Colour
			,@Insurance = CC.Insurance
			,@InsuranceExpiry = CC.InsuranceExpiry
			,@IsOrigionalRC = NULL
			,@IsBankHypothecation = NULL
			,@RegisteredAt = CC.RegistrationPlace
			,@AvailableAt = NULL
			,@FuelType = V.CarFuelType
			,@Transmission = V.CarTransmission
			,@RegistrationDate = NULL
			,@RegistrationType = NULL
			,@ImgLargeUrl = CP.ImageUrlFull
			,@ImgThumbUrl = CP.ImageUrlThumb
			,@ImageUrlThumbSmall = CP.ImageUrlThumbSmall
			,@DirectoryPath = CP.DirectoryPath
			,@HostUrl = CP.HostUrl
			,@IsReplicated = CP.IsReplicated
			,@StatusId = CP.StatusId
			,@BranchId = ISNULL(@BranchId, ST.BranchId)
		FROM TC_Stock ST WITH (NOLOCK)
		INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.StockId = ST.Id
		INNER JOIN vwMMV V WITH (NOLOCK) ON V.VersionId = ST.VersionId
		LEFT JOIN TC_CarPhotos CP WITH (NOLOCK) ON CP.StockId = ST.Id
			AND Cp.IsMain = 1
			AND CP.IsActive = 1
		WHERE ST.Id = @StockId

		SET @ImgLargeUrlFull = 'http://' + @HostUrl + @DirectoryPath + @ImgLargeUrl
		SET @ImgThumbUrlFull = 'http://' + @HostUrl + @DirectoryPath + @ImgThumbUrl
	END
	ELSE
	BEGIN
		IF (
				@StockId IS NULL
				AND @AbSure_CarId IS NOT NULL
				)
		BEGIN
			SELECT @Make = ACD.Make
				,@Model = ACD.Model
				,@Version = ACD.Version
				,@VersionId = ACD.VersionId
				,@StockId = ACD.StockId
				,@VIN = ACD.VIN
				,@RegNumber = ACD.RegNumber
				,@Source = 2
				,@MakeYear = ACD.MakeYear
				,@Kilometer = ACD.Kilometer
				,@Owners = ACD.Owners
				,@Colour = ACD.Colour
				,@Insurance = ACD.Insurance
				,@InsuranceExpiry = ACD.InsuranceExpiry
				,@IsOrigionalRC = ACD.IsOrigionalRC
				,@IsBankHypothecation = ACD.IsBankHypothecation
				,@RegisteredAt = ACD.RegisteredAt
				,@AvailableAt = ACD.AvailableAt
				,@FuelType = ACd.FuelType
				,@Transmission = ACD.Transmission
				,@RegistrationDate = ACD.RegistrationDate
				,@RegistrationType = ACD.RegistrationType
				,@ImgLargeUrl = ACD.ImgLargeUrl
				,@ImgThumbUrl = ACD.ImgThumbUrl
				,@OwnerName = ACD.OwnerName
				,@OwnerMobileNo = ACD.OwnerPhoneNo
				,@OwnerAddress = ACD.OwnerAddress
				,@OwnerEmail = ACD.OwnerEmail
				,@OwnerCityId = ACD.OwnerCityId
				,@OwnerAreaId = ACD.OwnerAreaId
				,@CarFittedWith = ACD.CarFittedWith
			FROM AbSure_CarDetails ACD WITH (NOLOCK)
			WHERE ACd.Id = @AbSure_CarId

			SET @ImgLargeUrlFull = @ImgLargeUrl
			SET @ImgThumbUrlFull = @ImgThumbUrl
		END
	END

	SET @AbSure_CarDetailsId = @AbSure_CarId

	EXECUTE [AbSure_SaveCarDetails] @Make = @Make
		,@Model = @Model
		,@Version = @Version
		,@VersionId = @VersionId
		,@VIN = @VIN
		,@RegNumber = @RegNumber
		,@Source = @Source
		,@DealerId = @BranchId
		,@StockId = @StockId
		,@MakeYear = @MakeYear
		,@Kilometer = @Kilometer
		,@Owners = @Owners
		,@Colour = @Colour
		,@Insurance = @Insurance
		,@InsuranceExpiry = @InsuranceExpiry
		,@IsOrigionalRC = @IsOrigionalRC
		,@IsBankHypothecation = @IsBankHypothecation
		,@RegisteredAt = @RegisteredAt
		,@AvailableAt = @AvailableAt
		,@FuelType = @FuelType
		,@Transmission = @Transmission
		,@RegistrationDate = @RegistrationDate
		,@RegistrationType = @RegistrationType
		,@AbSure_CarDetailsId = @AbSure_CarDetailsId OUTPUT
		,@ImgLargeUrl = @ImgLargeUrlFull
		,@ImgThumbUrl = @ImgThumbUrlFull
		,@OwnerName = @OwnerName
		,@OwnerAddress = @OwnerAddress
		,@OwnerMobileNo = @OwnerMobileNo
		,@OwnerEmail = @OwnerEmail
		,@OwnerCityId = @OwnerCityId
		,@OwnerAreaId = @OwnerAreaId
		,@AppointmentDate = @AppointmentDate
		,@UserId = @UserId
		,@TimeSlot = @TimeSlot
		,@CarFittedWith = @CarFittedWith
		,@CarSourceId = 1                            -- car is added through the add stock
	IF (@ImgLargeUrl IS NOT NULL)
	BEGIN
		EXECUTE [AbSure_SaveCarPhotos] @AbSure_CarDetailsId = @AbSure_CarDetailsId
			,@ImageUrlLarge = @ImgLargeUrl
			,@ImageUrlThumb = @ImgThumbUrl
			,@ImageUrlSmall = @ImageUrlThumbSmall
			,@DirectoryPath = @DirectoryPath
			,@IsMain = 1
			,@HostUrl = @HostUrl
			,@AbSure_CarPhotosId = @AbSure_CarPhotosId OUTPUT

		IF (@AbSure_CarPhotosId IS NOT NULL)
			UPDATE AbSure_CarPhotos
			SET IsReplicated = @IsReplicated
				,StatusId = @StatusId
			WHERE AbSure_CarPhotosId = @AbSure_CarPhotosId
	END

	UPDATE TC_Stock
	SET IsWarrantyRequested = 1
	WHERE Id = @StockId
		AND BranchId = @BranchId

	SET @AbSure_CarId = @AbSure_CarDetailsId

	--INSERT INTO Absure_Appointments(AbsureCarId, EntryDate, UserId, ScheduledDate) VALUES(@AbSure_CarDetailsId, GETDATE(), @UserId, @AppointmentDate)
	--Added By Ruchira Patil on 1st Jun 2015 (automatic agency assignment)
	--EXEC AbSure_AutomaticAgengyAssignment @AbSure_CarId
	--End

	--Added By Suresh Prajapti on 29th June, 2015 (automatic surveyor assignment)
	DECLARE @Result INT
	EXEC AbSure_AutomaticSurveyorAssignment @AbSure_CarId , @Result OUTPUT 

	IF @Result IS NOT NULL
		SET @SurveyorId = @Result
END
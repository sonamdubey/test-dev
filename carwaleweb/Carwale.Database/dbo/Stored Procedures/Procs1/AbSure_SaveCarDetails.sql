IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveCarDetails]
GO

	
-- =============================================
-- Author:        Vaibhav K
-- Create date: 15 Dec 2014
-- Description:    Save AbSure car details and return newly created id or existing carid on basis of stockid / reg number
-- Modified By: Tejashree Patil on 2 Jan 2014, Commented IsSurveyDone update statement.
-- Modified By: Vinay Kumar Prajapati on 2nd Jan 2015 added car  owner Details for indivisual.
-- Modified By: Ashwini Dhamankar on Feb 23,2015, Added @AdId
-- Modified By: Ruchira Patil on Mar 2,2015, Added @ReqWarranty
-- Modified By: Ashwini Dhamankar on March 11,2013, Added @EngineNo
-- Modified By: Tejashree Patil on 12 March 2015, Allowed rerequest.
-- Modified By: Tejashree Patil on 18 March 2015, Fetched Dealer EmailId.
-- Modified By: Yuga Hatolkar on 25th March, 2015, Added Parameters @AppointmentDate, @UserId, @TimeSlot
-- Modified By: Tejashree Patil on 7 April 2015, Implemented ISNULL(@AppointmentDate,GETDATE()).
-- Modified By: Chetan Navin on 14 April 2015 , Implemented capturing of registered at field from registration number
-- Modified By: Yuga Hatolkar on 23rd April, 2015, Added parameter IsTestDrive.
-- Modified By: Ruchira Patil on 8th Jun 2015(to save the photos taken by surveyor)
-- Modified By: Tejashree Patil on 16 Jun 2015, Executed AbSure_ChangeCertification sp.
-- Modified By: Ashwini Dhamankar on July 17,2015, Fetched Transmission based on VersionId 
-- Modified By : Ruchira Patil on 5th July 2015, Added parameter @IsRcPending
-- Modified By : Ruchira Patil on 1st Sept 2015, Commented the select query to check regnum for add car(handled in another SP: AbSure_VaildateCar)
-- Modified By : Vinay Kumar Prajapati on 8th Sep 2015 Added new Parameter @SurveySubmitMode (1-Online,2- Offline)
-- =============================================        
CREATE PROCEDURE [dbo].[AbSure_SaveCarDetails]
    -- Add the parameters for the stored procedure here
    @Make                   VARCHAR(100),
    @Model                  VARCHAR(100),
    @Version                VARCHAR(100),
    @VersionId              INT,
    @VIN                    VARCHAR(50) = NULL,
    @RegNumber              VARCHAR(50),
    @Source                 SMALLINT    = NULL,
    @DealerId               BIGINT        = NULL,
    @StockId                BIGINT        = NULL,
    @MakeYear               DATETIME    = NULL,
    @Kilometer              INT            = NULL,
    @Owners                 VARCHAR(50) = NULL,
    @Colour                 VARCHAR(50) = NULL,
    @Insurance              VARCHAR(50) = NULL,
    @InsuranceExpiry        DATETIME    = NULL,
    @IsOrigionalRC          BIT            = NULL,
    @IsBankHypothecation    BIT            = NULL,
    @RegisteredAt           VARCHAR(50) = NULL,
    @AvailableAt            VARCHAR(50) = NULL,
    @FuelType               SMALLINT    = NULL,
    @Transmission           SMALLINT    = NULL,
    @RegistrationDate       DATETIME    = NULL,
    @RegistrationType       SMALLINT    = NULL,
    @AbSure_CarDetailsId    NUMERIC(18, 0)= NULL OUTPUT,
    @ImgLargeUrl            VARCHAR(250) = NULL,
    @ImgThumbUrl            VARCHAR(250) = NULL,
    -- Modified By: Vinay Kumar Prajapati
    @OwnerName              VARCHAR(50)    = NULL,
    @OwnerAddress           VARCHAR(100)   = NULL, --Updated By Vinay Kumar Prajapati
    @OwnerMobileNo          VARCHAR(50)    = NULL,
    @OwnerEmail             VARCHAR(100)   = NULL,
    @OwnerCityId            BIGINT         = NULL,
    @OwnerAreaId            BIGINT         = NULL,
    @CarFittedWith          TINYINT        = NULL,
    --@AdId                   VARCHAR(50)  = NULL
    @ReqWarranty            INT = NULL,
    @EngineNo               VARCHAR(50)    = NULL,
    @AppointmentDate        DATETIME       = NULL,
    @UserId                 INT            = NULL,
    @TimeSlot               VARCHAR(50)    = NULL,
    @IsTestDrive            BIT            = NULL,
    @PhotoCount             INT            = NULL,
	@IsRcPending			BIT            = NULL,
	@SurveySubmitMode       SmallInt       = NULL,
	@CarSourceId			INT			   = NULL
  
AS
BEGIN
    -- inserting record in main table (TC_inquiries) of inquiries
	    BEGIN TRY

        BEGIN TRANSACTION ProcessAbSure_SaveCarDetails
        DECLARE @OldCarId NUMERIC(18, 0)= NULL
        DECLARE @StateRTOCode VARCHAR(5)
        DECLARE @RTONo VARCHAR(5)
        DECLARE @StateRTOCodeId INT

        SELECT  @OldCarId = ACD.Id
        FROM    AbSure_CarDetails ACD WITH(NOLOCK)
        WHERE   (ACD.StockId = @StockId OR ACD.Id = @AbSure_CarDetailsId)
                AND ACD.IsActive = 1
                AND (
                    ISNULL(ACD.Status,0) = 3
                    OR
                    (ISNULL(ACD.IsRejected,0)=0 AND DATEDIFF(DAY,ISNULL(ACD.SurveyDate,GETDATE()),GETDATE()) > 30 AND (IsSoldOut <> 1)
                        AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.AbSure_WarrantyTypesId IS NOT NULL))
                    )
              
        IF(@OldCarId IS NOT NULL)
        BEGIN
            UPDATE   AbSure_CarDetails
            SET      IsActive = 0
            WHERE    Id = @OldCarId

			EXEC AbSure_ChangeCertification @StockId,NULL,@AbSure_CarDetailsId 
        END
       
        IF(@OldCarId IS NULL)
        BEGIN
          SELECT @AppointmentDate = ACD.AppointmentDate,@TimeSlot = ACD.AppointmentTime
          FROM AbSure_CarDetails ACD WITH(NOLOCK)
          WHERE id = @AbSure_CarDetailsId   
        END
       
	   ----COMMENTED BY RUCHIRA PATIL ON 1ST SEPT 2015(this condition was for add car - now will validate the regnum according to the various scenarios)----
				--SELECT @AbSure_CarDetailsId = CD.Id
				--    FROM AbSure_CarDetails CD WITH(NOLOCK)
				--WHERE (@AbSure_CarDetailsId IS NULL
				--    AND
				--    (( @StockId IS NULL AND (REPLACE(REPLACE(CD.RegNumber, ' ', ''), '-','') = REPLACE(REPLACE(@RegNumber, ' ', ''), '-','')) )
				--    OR
				--    ( @StockId IS NOT NULL AND CD.StockId = @StockId )))
				--    OR
				--    ( CD.Id=@AbSure_CarDetailsId)
				--    AND IsActive = 1
		----End----

        -- Added By Vinay Kumar Prajapati (Added Car Owner Details ...)
        IF (@DealerId IS NOT NULL )--AND @OwnerMobileNo IS NULL)-- Modified By: Tejashree Patil on 18 March 2015,
        BEGIN
          SELECT    @OwnerAreaId = ISNULL(@OwnerAreaId, D.AreaId), @OwnerCityId = ISNULL(@OwnerCityId, D.CityId), @OwnerAddress = ISNULL(@OwnerAddress, D.Address1),
                    @OwnerName = ISNULL(@OwnerName, D.FirstName)/* + ' ' + ISNULL(D.LastName, '')*/, @OwnerMobileNo = ISNULL(@OwnerMobileNo, D.MobileNo),
                    @OwnerEmail = ISNULL(@OwnerEmail, D.EmailId)-- Modified By: Tejashree Patil on 18 March 2015
          FROM      Dealers D WITH(NOLOCK)
          WHERE     Id = @DealerId     
        END

        --Modifed By: Ashwini Dhamankar on Feb 24,2015 ,Commented
        --IF(@StockId IS NOT NULL)
        --BEGIN
        --    SELECT        @AdId = S.AdId
        --    FROM        TC_Stock S WITH(NOLOCK)
        --    WHERE       S.Id = @StockId
        --END

        -- Added By : Chetan Navin on 14th April 2015
        IF(@RegisteredAt IS NULL)
        BEGIN
            BEGIN
                SELECT @StateRTOCode = Data FROM dbo.StringSplitNew(LTRIM(@RegNumber),' ') WHERE Id = 1
                SELECT @RTONo = CAST(Data AS VARCHAR(5))  FROM dbo.StringSplitNew(LTRIM(@RegNumber),' ') WHERE Id = 2
                SELECT @StateRTOCodeId = StateRTOCodeId FROM StatesRTOCode WITH(NOLOCK) WHERE StateRTOCode = @StateRTOCode
                SELECT @RegisteredAt = CityName FROM StateRTOCities WITH(NOLOCK) WHERE StateRTOCodeId = @StateRTOCodeId AND RTONo = @RTONo
            END
        END

		--Commented By : Ashwini Dhamankar on July 17,2015
		--IF(@FuelType IS NULL)
  --      BEGIN
		--		SELECT   @FuelType = V.CarFuelType
		--		FROM    AbSure_CarDetails CD WITH(NOLOCK)
		--		        INNER JOIN vwMMV V     WITH(NOLOCK) ON V.VersionId = CD.VersionId
		--		WHERE    CD.Id = @AbSure_CarDetailsId
  --      END

		--Modified By : Ashwini Dhamankar on July 17,2015
		IF(@FuelType IS NULL)
		BEGIN
				SELECT  @FuelType = V.CarFuelType
				FROM	vwMMV V     WITH(NOLOCK) 
				WHERE   V.VersionId = @VersionId 
        END
        

		--Modified By : Ashwini Dhamankar on July 17,2015
		IF(@Transmission IS NULL)
        BEGIN
				SELECT   @Transmission = V.CarTransmission
				FROM     vwMMV V WITH(NOLOCK) 
				WHERE    V.VersionId = @VersionId
        END

        IF @AbSure_CarDetailsId IS NULL OR @AbSure_CarDetailsId = -1 OR @OldCarId IS NOT NULL
        BEGIN
            INSERT INTO AbSure_CarDetails
                (
                    Make, Model, Version, VersionId, VIN, RegNumber, Source,
                    DealerId ,StockId, MakeYear, Kilometer, Owners, Colour, Insurance, InsuranceExpiry, IsOrigionalRC,
                    IsBankHypothecation, RegisteredAt, AvailableAt, FuelType, Transmission, RegistrationDate, RegistrationType,
                    ImgLargeUrl, ImgThumbUrl,OwnerName,OwnerAddress,OwnerPhoneNo,OwnerCityId,OwnerAreaId,OwnerEmail,CarFittedWith,ReqWarranty,EngineNo, AppointmentDate,
                    AppointmentTime, IsTestDrive,CarSourceId --,AdId
                )
            VALUES
                (
                    @Make, @Model, @Version, @VersionId, @VIN, @RegNumber, @Source,
                    @DealerId, @StockId, @MakeYear, @Kilometer, @Owners, @Colour, @Insurance, @InsuranceExpiry, @IsOrigionalRC,
                    @IsBankHypothecation, @RegisteredAt, @AvailableAt, @FuelType, @Transmission, @RegistrationDate, ISNULL(@RegistrationType,1) ,
                    @ImgLargeUrl, @ImgThumbUrl,@OwnerName,@OwnerAddress,@OwnerMobileNo,@OwnerCityId,@OwnerAreaId, @OwnerEmail,@CarFittedWith,@ReqWarranty,@EngineNo,
                    ISNULL(@AppointmentDate,GETDATE()), @TimeSlot, @IsTestDrive,@CarSourceId  --,@AdId
                )

            SET @AbSure_CarDetailsId = SCOPE_IDENTITY()

        END
        ELSE
        BEGIN

            INSERT INTO AbSure_CarDetailsLog(AbSureCarId,Make,Model,Version,VersionId,VIN,RegNumber,EntryDate,Source,DealerId,StockId,MakeYear,Kilometer,Owners,Colour,Insurance,InsuranceExpiry,IsOrigionalRC,IsBankHypothecation,
                RegisteredAt,AvailableAt,FuelType,Transmission,RegistrationDate,RegistrationType,IsRejected,IsSurveyDone,RejectedDateTime,ModifiedDate,ImgLargeUrl,ImgThumbUrl,
                CarScore,AbSure_WarrantyTypesId,FinalWarrantyTypeId,WarrantyGivenBy,FinalWarrantyDate,SurveyDate,IsSoldOut,OwnerName,OwnerAddress,OwnerPhoneNo,OwnerCityId,
                OwnerAreaId,OwnerEmail,Status,CancelReason,CancelledBy,CancelledOn,CarFittedWith,/*AdId,*/ReqWarranty,EngineNo,AppointmentDate,AppointmentTime)

            SELECT ID,Make,Model,Version,VersionId,VIN,RegNumber,EntryDate,Source,DealerId,StockId,MakeYear,Kilometer,Owners,Colour,Insurance,InsuranceExpiry,IsOrigionalRC,IsBankHypothecation,
                RegisteredAt,AvailableAt,FuelType,Transmission,RegistrationDate,RegistrationType,IsRejected,IsSurveyDone,RejectedDateTime,ModifiedDate,ImgLargeUrl,ImgThumbUrl,
                CarScore,AbSure_WarrantyTypesId,FinalWarrantyTypeId,WarrantyGivenBy,FinalWarrantyDate,SurveyDate,IsSoldOut,OwnerName,OwnerAddress,OwnerPhoneNo,OwnerCityId,
                OwnerAreaId,OwnerEmail,Status,CancelReason,CancelledBy,CancelledOn,CarFittedWith,/*AdId,*/ReqWarranty,EngineNo,AppointmentDate,AppointmentTime
            FROM AbSure_CarDetails
            WHERE ID = @AbSure_CarDetailsId

            SET @InsuranceExpiry = CASE    WHEN @Insurance = 'N/A' AND @Source = 2 THEN NULL ELSE @InsuranceExpiry END

            UPDATE        AbSure_CarDetails
            SET            VIN                  = ISNULL(@VIN                  , VIN),
                        RegNumber            = ISNULL(@RegNumber            , RegNumber),
                        Source               = ISNULL(@Source                , Source),
                        MakeYear             = ISNULL(@MakeYear                , MakeYear),
                        Kilometer            = ISNULL(@Kilometer            , Kilometer),
                        Owners               = ISNULL(@Owners                , Owners),
                        Colour               = ISNULL(@Colour                , Colour),
                        Insurance            = ISNULL(@Insurance            , Insurance),
                        InsuranceExpiry      = @InsuranceExpiry,
                        IsOrigionalRC        = ISNULL(@IsOrigionalRC        , IsOrigionalRC),
                        IsBankHypothecation  = ISNULL(@IsBankHypothecation, IsBankHypothecation),
                        RegisteredAt         = ISNULL(@RegisteredAt         , RegisteredAt),
                        AvailableAt          =     ISNULL(@AvailableAt          , AvailableAt),
                        FuelType             = ISNULL(@FuelType             , FuelType),
                        Transmission         = ISNULL(@Transmission         , Transmission),
                        RegistrationDate     = ISNULL(@RegistrationDate        , RegistrationDate),
                        RegistrationType     = ISNULL(@RegistrationType     , RegistrationType),
                        ModifiedDate         = GETDATE(),
                        OwnerName            = ISNULL(@OwnerName            , OwnerName),
                        OwnerAddress         = ISNULL(@OwnerAddress         , OwnerAddress),
                        OwnerPhoneNo         = ISNULL(@OwnerMobileNo        , OwnerPhoneNo),
                        OwnerEmail           = ISNULL(@OwnerEmail           , OwnerEmail),
                        OwnerCityId          = ISNULL(@OwnerCityId          , OwnerCityId),
                        OwnerAreaId          = ISNULL(@OwnerAreaId          , OwnerAreaId),          
                        CarFittedWith        = @CarFittedWith,  ---Added By Vinay Kumar Prajapati purposes car fuel type cng or lpg
                        VersionId            = ISNULL(@VersionId             , VersionId),
                        Version              = ISNULL(@Version               , Version),
                        Model                = ISNULL(@Model                 , Model),
                        ReqWarranty          = ISNULL(@ReqWarranty           ,ReqWarranty),
                        EngineNo             = ISNULL(@EngineNo              ,EngineNo),
                        AppointmentDate      = ISNULL(@AppointmentDate         , AppointmentDate),
                        AppointmentTime      = ISNULL(@TimeSlot                 , AppointmentTime),
                        IsTestDrive          = @IsTestDrive,        -- Added By : Yuga Hatolkar--ISNULL(@IsTestDrive             , IsTestDrive)
                        PhotoCount           = @PhotoCount,  --Added By Ruchira Patil on 8th Jun 2015(to save the photos taken by surveyor)
						RCImagePending		 = @IsRcPending, --Added By Ruchira Patil on 5th July 2015(to save the IsRcPending flag)
						SurveySubmitMode     = ISNULL(@SurveySubmitMode, SurveySubmitMode)
                        --AdId               = ISNULL(@AdId               , AdId)

            WHERE   Id                    = @AbSure_CarDetailsId
            --END
            -- Modified By: Tejashree Patil on 2 Jan 2014, Commented IsSurveyDone update statement.
            /*
            IF(@StockId IS NOT NULL)
            BEGIN
                UPDATE    AbSure_CarDetails
                SET        IsSurveyDone = 1,
                        ModifiedDate = GETDATE()
                WHERE   Id = @AbSure_CarDetailsId
            END*/
        END
        COMMIT TRANSACTION ProcessAbSure_SaveCarDetails
    END TRY
   
    BEGIN CATCH
        ROLLBACK TRANSACTION ProcessAbSure_SaveCarDetails
         INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('AbSure_SaveCarDetails',
         (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
         GETDATE(),
            ',        @Make :'                + ISNULL(CAST( @Make AS VARCHAR(50)),'NULL')+
            ',        @Model :'                + ISNULL(CAST( @Model AS VARCHAR(50)),'NULL')+
            ',        @Version :'                + ISNULL(CAST( @Version AS VARCHAR(50)),'NULL')+
            ',        @VersionId :'            + ISNULL(CAST( @VersionId AS VARCHAR(50)),'NULL')+
            ',        @VIN :'                    + ISNULL(CAST( @VIN AS VARCHAR(50)),'NULL')+
            ',        @RegNumber :'            + ISNULL(CAST( @RegNumber AS VARCHAR(50)),'NULL')+
            ',        @Source :'                + ISNULL(CAST( @Source AS VARCHAR(50)),'NULL')+
            ',        @DealerId:'                + ISNULL(CAST( @DealerId AS VARCHAR(50)),'NULL')+
            ',        @StockId :'                + ISNULL(CAST( @StockId AS VARCHAR(50)),'NULL')+
            ',        @MakeYear:'                + ISNULL(CAST( @MakeYear AS VARCHAR(50)),'NULL')+
            ',        @Kilometer :'            + ISNULL(CAST( @Kilometer AS VARCHAR(50)),'NULL')+
            ',        @Owners :'                + ISNULL(CAST( @Owners AS VARCHAR(50)),'NULL')+
            ',        @Colour :'                + ISNULL(CAST( @Colour AS VARCHAR(50)),'NULL')+
            ',        @Insurance :'            + ISNULL(CAST( @Insurance AS VARCHAR(50)),'NULL')+
            ',        @InsuranceExpiry :'        + ISNULL(CAST( @InsuranceExpiry AS VARCHAR(50)),'NULL')+
            ',        @IsOrigionalRC :'        + ISNULL(CAST( @IsOrigionalRC AS VARCHAR(50)),'NULL')+
            ',        @IsBankHypothecation:'  + ISNULL(CAST( @IsBankHypothecation AS VARCHAR(50)),'NULL')+
            ',        @RegisteredAt :'        + ISNULL(CAST( @RegisteredAt AS VARCHAR(50)),'NULL')+
            ',        @AvailableAt :'            + ISNULL(CAST( @AvailableAt AS VARCHAR(50)),'NULL')+
            ',        @FuelType :'            + ISNULL(CAST( @FuelType AS VARCHAR(50)),'NULL')+
            ',        @Transmission :'        + ISNULL(CAST( @Transmission AS VARCHAR(50)),'NULL')+
            ',        @RegistrationDate :'    + ISNULL(CAST( @RegistrationDate AS VARCHAR(50)),'NULL')+
            ',        @RegistrationType :'    + ISNULL(CAST( @RegistrationType AS VARCHAR(50)),'NULL')+
            ',        @AbSure_CarDetailsId:'    + ISNULL(CAST( @AbSure_CarDetailsId AS VARCHAR(50)),'NULL')+
            ',        @ImgLargeUrl :'            + ISNULL(CAST( @ImgLargeUrl AS VARCHAR(50)),'NULL')+
            ',        @ImgThumbUrl :'            + ISNULL(CAST( @ImgThumbUrl AS VARCHAR(50)),'NULL')+
            ',        @OwnerName   :'            + ISNULL(CAST( @OwnerName AS VARCHAR(50)),'NULL')+
            ',        @OwnerAddress :'        + ISNULL(CAST( @OwnerAddress AS VARCHAR(50)),'NULL')+
            ',        @OwnerMobileNo :'        + ISNULL(CAST( @OwnerMobileNo AS VARCHAR(50)),'NULL')+
            ',        @OwnerEmail :'            + ISNULL(CAST( @OwnerEmail AS VARCHAR(50)),'NULL')+
            ',        @OwnerCityId :'            + ISNULL(CAST( @OwnerCityId AS VARCHAR(50)),'NULL')+
            ',        @OwnerAreaId :'            + ISNULL(CAST( @OwnerAreaId AS VARCHAR(50)),'NULL')+
            ',        @CarFittedWith :'        + ISNULL(CAST( @CarFittedWith AS VARCHAR(50)),'NULL')+
            ',        @ReqWarranty :'            + ISNULL(CAST( @ReqWarranty AS VARCHAR(50)),'NULL')+
            ',        @EngineNo :'            + ISNULL(CAST( @EngineNo AS VARCHAR(50)),'NULL')+
            ',        @AppointmentDate :'        + ISNULL(CAST( @AppointmentDate AS VARCHAR(50)),'NULL')+
            ',        @UserId    :'                + ISNULL(CAST( @UserId AS VARCHAR(50)),'NULL')+
            ',        @TimeSlot :'            + ISNULL(CAST( @TimeSlot AS VARCHAR(50)),'NULL')+
			',		  @CarSourceId:'          + ISNULL(CAST(@CarSourceId AS VARCHAR(50)), 'NULL')
         )
                        --SELECT ERROR_NUMBER() AS ErrorNumber;
    END CATCH;
END
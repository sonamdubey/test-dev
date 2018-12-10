IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertOffer]
GO

	CREATE PROCEDURE [dbo].[DO_InsertOffer]
	@Id					NUMERIC,
	@OfferType			SMALLINT,
	@StartDate			DATETIME,
	@EndDate			DATETIME,
	@SourceCategory		NUMERIC,
	@OfferTitle			VARCHAR(200),
	@OfferDescription	VARCHAR(MAX),
	@MaxOfferValue		NUMERIC,
	@OfferUnits			INT,
	@Conditions			VARCHAR(MAX),
	@SourceDescription	VARCHAR(MAX),
	@EnteredBy			NUMERIC,
	@IsCountryWide		BIT,
	@HostUrl			VARCHAR(250),
	@PreBookingEmailIds VARCHAR(200),
	@PreBookingMobile	VARCHAR(200),
	@CouponEmailIds		VARCHAR(200),
	@CouponMobile		VARCHAR(200),
	@DispOnMob			BIT = NULL,
	@DispOnDesk			BIT = NULL,
	@OfferId			NUMERIC OUTPUT,
	@Status				INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	DECLARE @ImagePath VARCHAR(150) = '' ,@ImageName VARCHAR(150) = ''
	IF @Id = -1
		BEGIN
			INSERT INTO DealerOffers(OfferType,StartDate,EndDate,SourceCategory,OfferTitle,OfferDescription,MaxOfferValue,OfferUnits,Conditions,SourceDescription,
			EnteredBy,EntryDate,IsCountryWide,HostURL,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk)
			VALUES(@OfferType,@StartDate,@EndDate,@SourceCategory,@OfferTitle,@OfferDescription,@MaxOfferValue,@OfferUnits,@Conditions,@SourceDescription,
			@EnteredBy,GETDATE(),@IsCountryWide,@HostUrl,@PreBookingEmailIds,@PreBookingMobile,@CouponEmailIds,@CouponMobile,@DispOnMob,@DispOnDesk)
			SET @OfferId = SCOPE_IDENTITY()

			IF @HostUrl IS NOT NULL
			BEGIN
				SET @ImagePath = '/offers/'+ CONVERT(VARCHAR,@OfferId) +'/'
				SET @ImageName = CONVERT(VARCHAR,@OfferId) + '.jpg'
				UPDATE DealerOffers SET ImagePath = @ImagePath, ImageName = @ImageName
				WHERE id = @OfferId
			END

			INSERT INTO DealerOffersLog(OfferId,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,ImagePath,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk,Remarks)
			SELECT ID,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,ImagePath,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,@DispOnMob,@DispOnDesk,'Record Inserted'
			FROM DealerOffers 
			WHERE ID = @OfferId

			SET @Status = 1
		END
	ELSE
		BEGIN

			UPDATE  DealerOffers SET  OfferType = @OfferType, StartDate = @StartDate, EndDate = @EndDate,SourceCategory = @SourceCategory, OfferTitle = @OfferTitle,
			OfferDescription = @OfferDescription, MaxOfferValue = @MaxOfferValue,OfferUnits=@OfferUnits,Conditions = @Conditions,SourceDescription = @SourceDescription,
			UpdatedBy = @EnteredBy,UpdatedOn=GETDATE(), IsCountryWide = @IsCountryWide,PreBookingEmailIds = @PreBookingEmailIds,PreBookingMobile = @PreBookingMobile,
			CouponEmailIds = @CouponEmailIds,CouponMobile = @CouponMobile,DispOnMobile = @DispOnMob ,DispOnDesk = @DispOnDesk
			WHERE ID = @Id
			
			IF @HostUrl IS NOT NULL
			BEGIN
				SET @ImagePath = '/offers/'+ CONVERT(VARCHAR,@Id) +'/'
				SET @ImageName = CONVERT(VARCHAR,@Id) + '.jpg'
				UPDATE DealerOffers SET HostURL = @HostUrl, ImagePath = @ImagePath, ImageName = @ImageName
				WHERE id = @Id
			END

			INSERT INTO DealerOffersLog(OfferId,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,ImagePath,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk,Remarks)
			SELECT ID,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,ImagePath,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,@DispOnMob,@DispOnDesk,'Record Updated'
			FROM DealerOffers 
			WHERE ID = @Id

			SET @OfferId = @Id
			SET @Status = 1 
				
		END
END


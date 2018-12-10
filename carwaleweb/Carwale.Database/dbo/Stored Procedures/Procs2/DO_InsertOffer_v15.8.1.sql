IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertOffer_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertOffer_v15]
GO

	
CREATE PROCEDURE [dbo].[DO_InsertOffer_v15.8.1]
	@Id					INT,
	@OfferType			SMALLINT,
	@StartDate			DATETIME = NULL,
	@EndDate			DATETIME = NULL,
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
	@DispSnippetOnDesk	BIT = NULL,
	@DispSnippetOnMob	BIT = NULL,
	@DispOnOffersPgDesk	BIT = NULL,
	@DispOnOffersPgMob	BIT = NULL,
	@TimeStamp			VARCHAR(25),
	@ImagePath			VARCHAR(150),
	@PQSnippet			VARCHAR(MAX),
	@OfferId			NUMERIC OUTPUT,
	@Status				INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	DECLARE @ImageName VARCHAR(150) = ''
	IF @Id = -1
		BEGIN
			INSERT INTO DealerOffers(OfferType,StartDate,EndDate,SourceCategory,OfferTitle,OfferDescription,MaxOfferValue,OfferUnits,Conditions,SourceDescription,
			EnteredBy,EntryDate,IsCountryWide,HostURL,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk,ShortDescription,DispSnippetOnDesk,DispSnippetOnMob,DispOnOffersPgDesk,DispOnOffersPgMob)
			VALUES(@OfferType,@StartDate,@EndDate,@SourceCategory,@OfferTitle,@OfferDescription,@MaxOfferValue,@OfferUnits,@Conditions,@SourceDescription,
			@EnteredBy,GETDATE(),@IsCountryWide,@HostUrl,@PreBookingEmailIds,@PreBookingMobile,@CouponEmailIds,@CouponMobile,@DispOnMob,@DispOnDesk,@PQSnippet,@DispSnippetOnDesk,@DispSnippetOnMob,@DispOnOffersPgDesk,@DispOnOffersPgMob)
			SET @OfferId = SCOPE_IDENTITY()

			IF @HostUrl IS NOT NULL
			BEGIN
				SET @ImagePath = @ImagePath + CONVERT(VARCHAR,@OfferId) +'/'
				SET @ImageName = CONVERT(VARCHAR,@OfferId) + '.jpg'
				UPDATE DealerOffers SET OriginalImgPath = @ImagePath+ @ImageName
				WHERE id = @OfferId
			END

			INSERT INTO DealerOffersLog(OfferId,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk,ShortDescription,DispSnippetOnDesk,DispSnippetOnMob,DispOnOffersPgDesk,DispOnOffersPgMob,Remarks)
			SELECT ID,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,@DispOnMob,@DispOnDesk,ShortDescription,DispSnippetOnDesk,DispSnippetOnMob,DispOnOffersPgDesk,DispOnOffersPgMob,'Record Inserted'
			FROM DealerOffers WITH(NOLOCK)
			WHERE ID = @OfferId

			SET @Status = 1
		END
	ELSE
		BEGIN

			UPDATE  DealerOffers SET  OfferType = @OfferType, StartDate = @StartDate, EndDate = @EndDate,SourceCategory = @SourceCategory, OfferTitle = @OfferTitle,
			OfferDescription = @OfferDescription, MaxOfferValue = @MaxOfferValue,OfferUnits=@OfferUnits,Conditions = @Conditions,SourceDescription = @SourceDescription,
			UpdatedBy = @EnteredBy,UpdatedOn=GETDATE(), IsCountryWide = @IsCountryWide,PreBookingEmailIds = @PreBookingEmailIds,PreBookingMobile = @PreBookingMobile,
			CouponEmailIds = @CouponEmailIds,CouponMobile = @CouponMobile,DispOnMobile = @DispOnMob ,DispOnDesk = @DispOnDesk, ShortDescription = @PQSnippet, 
			DispSnippetOnDesk = @DispSnippetOnDesk ,DispSnippetOnMob = @DispSnippetOnMob,DispOnOffersPgDesk= @DispOnOffersPgDesk,DispOnOffersPgMob = @DispOnOffersPgMob
			WHERE ID = @Id
			
			IF @HostUrl IS NOT NULL
			BEGIN
				SET @ImagePath = @ImagePath + CONVERT(VARCHAR,@Id) +'/'
				SET @ImageName = CONVERT(VARCHAR,@Id) + '.jpg'
				UPDATE DealerOffers SET HostURL = @HostUrl, OriginalImgPath = @ImagePath+ @ImageName + '?v=' + @TimeStamp
				WHERE id = @Id
			END

			INSERT INTO DealerOffersLog(OfferId,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,DispOnMobile,DispOnDesk,ShortDescription,DispSnippetOnDesk,DispSnippetOnMob,DispOnOffersPgDesk,DispOnOffersPgMob,Remarks)
			SELECT ID,OfferTitle,OfferDescription,MaxOfferValue,Conditions,StartDate,EndDate,EntryDate,EnteredBy,SourceCategory,SourceDescription,
			IsCountryWide,OfferType,OfferUnits,IsActive,IsApproved,ClaimedUnits,UpdatedOn,UpdatedBy,HostURL,ImageName,PreBookingEmailIds,PreBookingMobile,CouponEmailIds,CouponMobile,@DispOnMob,@DispOnDesk,ShortDescription,DispSnippetOnDesk,DispSnippetOnMob,DispOnOffersPgDesk,DispOnOffersPgMob,'Record Updated'
			FROM DealerOffers WITH(NOLOCK)
			WHERE ID = @Id

			SET @OfferId = @Id
			SET @Status = 1 
				
		END
END



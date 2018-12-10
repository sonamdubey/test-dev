IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQLead_API_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQLead_API_V15]
GO

	

-- =============================================
-- Author:		Ashish Verma
-- Create date: 29/09/2014
-- Description:	inserts PQ Lead INTO NewCarPurchaseInquiries and NewPurchaseCities table
--modified by rohan sapkal 03-04-2015, added LTSRC
--modified by vinayak mishra 30-10-2015, added two columns, UTMACOOKIE and UTMZCOOKIE
-- =============================================
CREATE PROCEDURE [dbo].[SavePQLead_API_V15.11.1]
	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId INT
	--Car Version Id
	,@BuyTime VARCHAR(100) = '1 week'
	,@Name VARCHAR(50)
	,@CityId INT
	,@EmailId VARCHAR(100)
	,@PhoneNo VARCHAR(50)
	,@ForwardedLead BIT
	,@SourceId TINYINT
	,@PQPageId SMALLINT
	,@ClientIP VARCHAR(100)
	,@InterestedInLoan BIT
	-- Added by Raghu to capture user's interest to apply for loan
	,@MobVerified BIT = 0
	-- Added by Raght to cpature whether customer verified his mobile or not
	,@ZoneId INT
	,@LTSRC VARCHAR(50) = NULL
	,@UtmaCookie VARCHAR(500)=NULL
	,@UtmzCookie VARCHAR(500)=NULL
	,@InquiryId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @City VARCHAR(50)

	BEGIN
		SELECT @City = ct.NAME
		FROM Cities ct WITH (NOLOCK)
		WHERE ct.ID = @CityId
	END

	-- Insert statements for procedure here
	INSERT INTO NewCarPurchaseInquiries (
		CustomerId
		,CarVersionId
		,BuyTime
		,RequestDateTime
		,IsApproved
		,LatestOffers
		,ForwardedLead
		,SourceId
		,PQPageId
		,ClientIP
		,LTSRC
		,UtmaCookie
		,UtmzCookie
		)
	VALUES (
		- 1
		-----Taking hardcoded value will be updated later after processing in rabbit MQ 		 		
		,@CarVersionId
		,@BuyTime
		,GETDATE()
		,1
		,1
		,@ForwardedLead
		,@SourceId
		,@PQPageId
		,@ClientIP
		,@LTSRC
		,@UtmaCookie
		,@UtmzCookie
		)

	SET @InquiryId = SCOPE_IDENTITY()

	IF (@InquiryId > 0)
	BEGIN
		INSERT INTO NewPurchaseCities (
			InquiryId
			,CityId
			,City
			,EmailId
			,PhoneNo
			,NAME
			,InterestedInLoan
			,MobileVerified
			,ZoneId
			)
		VALUES (
			@InquiryId
			,@CityId
			,@City
			,@EmailId
			,@PhoneNo
			,@Name
			,@InterestedInLoan
			,@MobVerified
			,@ZoneId
			)
	END

	RETURN @InquiryId
END



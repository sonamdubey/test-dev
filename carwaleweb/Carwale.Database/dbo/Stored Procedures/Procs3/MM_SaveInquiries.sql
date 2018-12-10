IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MM_SaveInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MM_SaveInquiries]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 15-Apr-2013
-- Description:	Save Mobile Masking inquiries data from url parameters
-- Modifier:	1. Vaibhav K (24-Apr-2013)
--				Fetch ConsumerId, ConsumerType & MaskingNumber from the DealerMobile(SellerMobile) and insert record for MM_Inquiry
-- Modifier:	Kartik Rathod 14 oct 2016, modify condition for @SellerMobile in case of multiple comma seperated seller number for masking number
-- Modified By : Komal Manjare 14 Oct 2016 fetch InquirySourceId for mapped numbers
-- Modified By : Vaibhav K 4 Nov 2016 chnaged varchar size from 20 to 100 of @SellerMobile
-- =============================================
CREATE PROCEDURE [dbo].[MM_SaveInquiries]
	-- Add the parameters for the stored procedure here
	@CallId				VARCHAR(60),
	@CallDuration		VARCHAR(80),
	@CallStatus			VARCHAR(20),
	@SellerMobile		VARCHAR(100),
	@BuyerMobile		VARCHAR(20),
	@CallStartDate		DATETIME,
	@CallEndDate		DATETIME,
	@RecordingURL		VARCHAR(200),
	@MaskingNumber		VARCHAR(20),
	@MM_InquiriesId		INT = NULL OUTPUT,
	@CallerCircle		VARCHAR(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ConsumerId		INT = -1,
			@ConsumerType	TINYINT = 0,
			@ReceivedSellerNumber VARCHAR(20),
			@ProductTypeId INT,
			@ProductBrand INT,
			@LeadCampaignId INT,
			@DealerType	TINYINT,
			@InquirySourceId INT
	
	
    -- Insert statements for procedure here      
    SET @SellerMobile = REPLACE(@SellerMobile, ' ', '') 
    SET @ReceivedSellerNumber = REPLACE(@SellerMobile, ' ', '')
    SET @MaskingNumber = REPLACE(@MaskingNumber, ' ', '')
	
    IF ISNUMERIC(@MaskingNumber) = 1
		BEGIN
			SELECT TOP 1 @ConsumerId = ConsumerId, @ConsumerType = ConsumerType, @SellerMobile = Mobile,
				@ProductTypeId = ProductTypeId, @ProductBrand = ISNULL(NCDBrandId,0), @LeadCampaignId = ISNULL(LeadCampaignId,0)
				,@InquirySourceId=TC_InquirySourceId --Komal Manjare 14 Oct 2016
			FROM MM_SellerMobileMasking WITH (NOLOCK) WHERE (MaskingNumber = @MaskingNumber OR ('91' + MaskingNumber) = @MaskingNumber)
								
			--Added By Deepak on 17th July 2015
			IF @@ROWCOUNT = 0 AND ISNUMERIC(@SellerMobile) = 1
				BEGIN
					SELECT @ConsumerId = ConsumerId, @ConsumerType = ConsumerType,
					@ProductTypeId = ProductTypeId, @ProductBrand = ISNULL(NCDBrandId,0), @LeadCampaignId = ISNULL(LeadCampaignId,0)
					FROM MM_SellerMobileMasking WITH (NOLOCK) 
					WHERE  (Mobile like '%'+@SellerMobile+'%' OR @SellerMobile IN (select '91'+ListMember from fnSplitCSVToString(Mobile)))-- added by kartik
							-- (Mobile = @SellerMobile OR ('91' + Mobile) = @SellerMobile)
				END
		END
    ELSE IF ISNUMERIC(@SellerMobile) = 1
		BEGIN
			SELECT @ConsumerId = ConsumerId, @ConsumerType = ConsumerType,
			@ProductTypeId = ProductTypeId, @ProductBrand = ISNULL(NCDBrandId,0), @LeadCampaignId = ISNULL(LeadCampaignId,0)
			FROM MM_SellerMobileMasking WITH (NOLOCK) 
			WHERE (Mobile like '%'+@SellerMobile+'%' OR @SellerMobile IN (select '91'+ListMember from fnSplitCSVToString(Mobile))) -- added by kartik
					--(Mobile = @SellerMobile OR ('91' + Mobile) = @SellerMobile)
		END
		
  --  IF ISNUMERIC(@SellerMobile) = 1
		--BEGIN
		--	SELECT @ConsumerId = ConsumerId, @ConsumerType = ConsumerType
		--	FROM MM_SellerMobileMasking WHERE (Mobile = @SellerMobile OR ('91' + Mobile) = @SellerMobile)
			
		--	--Added By Deepak on 17th July 2015
		--	IF @@ROWCOUNT = 0 AND ISNUMERIC(@MaskingNumber) = 1
		--		BEGIN
		--			SELECT TOP 1 @ConsumerId = ConsumerId, @ConsumerType = ConsumerType, @SellerMobile = Mobile
		--			FROM MM_SellerMobileMasking WHERE (MaskingNumber = @MaskingNumber OR ('91' + MaskingNumber) = @MaskingNumber)
		--		END
		--END
  --  ELSE
		--BEGIN
		--	SELECT TOP 1 @ConsumerId = ConsumerId, @ConsumerType = ConsumerType, @SellerMobile = Mobile
		--	FROM MM_SellerMobileMasking WHERE (MaskingNumber = @MaskingNumber OR ('91' + MaskingNumber) = @MaskingNumber)
		--END
	    
    
    SET @MM_InquiriesId = -1
        
	INSERT INTO MM_Inquiries (CallId, CallDuration, CallStatus, SellerMobile, BuyerMobile, CallStartDate, CallEndDate, RecordingURL, ConsumerId, ConsumerType, MaskingNumber, ReceivedSellerNumber, ProductTypeId, LeadCampaignId, CallerCircle)
	VALUES (@CallId, @CallDuration, @CallStatus, @SellerMobile, @BuyerMobile, @CallStartDate, @CallEndDate, @RecordingURL, @ConsumerId, @ConsumerType, @MaskingNumber, @ReceivedSellerNumber, @ProductTypeId, @LeadCampaignId, @CallerCircle)
	
	SET @MM_InquiriesId = SCOPE_IDENTITY()

	DECLARE @CityId INT
	DECLARE @DealerName VARCHAR(50)

	SELECT @CityId = CityId, @DealerName = Organization, @DealerType = ISNULL(ApplicationId,1)
	FROM Dealers WITH(NOLOCK)
	WHERE Id = @ConsumerId
	
	IF @ProductBrand = 0
		SELECT TOP 1 @ProductBrand = ISNULL(VM.ModelId,0) 
		FROM TC_DealerMakes DM WITH (NOLOCK) 
			INNER JOIN vwMMV VM ON VM.MakeId = DM.MakeId 
		WHERE DM.DealerId = @ConsumerId AND VM.IsModelNew = 1

	IF @ProductBrand = 0
		SET @ProductBrand = 196 -- Hyundai i10 Default
		
	SELECT @ConsumerId AS DealerId ,@ProductTypeId AS ProductTypeId,  @ProductBrand AS ProductBrand, @CityId AS CityId, @DealerName AS DealerName, @MM_InquiriesId AS MM_InquiriesId, @SellerMobile AS SellerMobile, @LeadCampaignId AS LeadCampaignId, @DealerType AS DealerType
			,@InquirySourceId AS InquirySourceId -- Komal Manjare 14 Oct 2016

END
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateNewCarInq]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateNewCarInq]
GO

	
-- =============================================
-- Author:	Vivek Gupta
-- Create date: 25th July,2013
-- Description:	This Procedure updates the New car inquiry Details.
-- Modified By: Vivek Gupta on 26th sep,2013, added parameters @IsCorporate,@CompanyName
--Modifid By Vivek Gupta on 8th oct 2013, Added update colorId = null in newcarbooking table while changing version
--Modifid By: Tejashree Patil on 14 Oct 2013, Added @TC_CampaignSchedulingId parameter for new car inquiry.
-- MOdified By: Vishal Srivastava AE1830 on 06-01-2014 1610 hrs ist, updation of TC_NewCarInquiries.CreatedOn by @InquiryDate
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- Modifid By ViCKY Gupta ON 10-08-2015, update LatestVersionId in TC_InquiriesLead 
-- Modified By : Afrose on 14-09-2015, added parameter @CampaignId
-- Modified By : Ashwini Dhamankar on Oct 19,2016 (Fetched applicationid if null or 0 and updated interestedin of tc_tasklist)
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateNewCarInq] @BranchId BIGINT
	,@VersionId BIGINT
	,@ModelId BIGINT = NULL
	,@ColorList VARCHAR(200)
	,@InqId BIGINT
	,@IsCorporate BIT = 0
	,-- Modified By: Vivek Gupta on 26th sep,2013
	@CompanyName VARCHAR(200) = NULL
	,@TC_CampaignSchedulingId INT = NULL
	,@InquiryDate DATETIME = NULL
	,----- Added by  Vishal Srivastava AE1830 on 06-01-2014 
	@carVersionChange INT = NULL
	,-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	@carMakeYearChange DATE = NULL
	,-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	@carKiloChange INT = NULL
	,-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	@carPriceChange INT = NULL
	,-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	@exchange INT = NULL
	,-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	@ApplicationId TINYINT = 1
	,@CampaignId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TC_NewCarInquiryId BIGINT

	SET @TC_NewCarInquiryId = @InqId

	IF(@ApplicationId IS NULL OR @ApplicationId = 0)  -- Modified By : Ashwini Dhamankar on Oct 19,2016 
	BEGIN 
		SELECT @ApplicationId = ApplicationId FROM Dealers WHERE ID = @BranchId
	END

	IF (@VersionId IS NOT NULL)
	BEGIN
		SELECT @ModelId = ModelId
		FROM vwAllMMV
		WHERE ApplicationId = @ApplicationId
		AND VersionId = @VersionId
	END

	IF (@ColorList IS NOT NULL)
	BEGIN
		DELETE
		FROM TC_PrefNewCarColour
		WHERE TC_NewCarInquiriesId = @InqId

		INSERT INTO TC_PrefNewCarColour (
		TC_NewCarInquiriesId
		,VersionColorsId
		)
		SELECT @TC_NewCarInquiryId AS Id
		,ListMember
		FROM dbo.fnSplitCSVValuesWithIdentity(@ColorList)
	END

	DECLARE @CarDetails VARCHAR(MAX)
	DECLARE @LatestVersionId INT

	IF (@VersionId IS NULL) -- This inquiry is added form dealer wbesite of TD
	BEGIN
		SELECT TOP 1 @VersionId = V.ID
		FROM CarVersions V WITH (NOLOCK)
		WHERE V.CarModelId = @ModelId
		AND V.IsDeleted = 0
		AND V.New = 1
		AND V.Futuristic = 0
	END

	/*	

	SELECT	@CarDetails=V.Make + ' ' + V.Model + ' '  + V.Version + ' '  

	FROM	vwMMV V WITH(NOLOCK)

	WHERE	V.VersionId=@VersionId  */
	-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
	SELECT @CarDetails = V.Make + ' ' + V.Model + ' ' + V.Version + ' '
	,@LatestVersionId = V.VersionId
	FROM vwAllMMV V WITH (NOLOCK)
	WHERE V.VersionId = @VersionId
	AND V.ApplicationId = ISNULL(@ApplicationId, 1)

	DECLARE @TC_InquiriesLeadId BIGINT
	DECLARE @UserId BIGINT
	DECLARE @TC_LeadId BIGINT

	SELECT @TC_InquiriesLeadId = TC_InquiriesLeadId
	FROM TC_NewCarInquiries WITH (NOLOCK)
	WHERE TC_NewCarInquiriesId = @InqId

	SELECT @UserId = TC_UserId
	,@TC_LeadId = TC_LeadId
	FROM TC_InquiriesLead WITH (NOLOCK)
	WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId
	AND IsActive = 1

	-- Modified By: Vivek Gupta on 26th sep,2013 commented old condition and add new condition
	--IF(@VersionId IS NOT NULL)
	IF NOT EXISTS (
	SELECT VersionId
	FROM TC_NewCarInquiries WITH (NOLOCK)
	WHERE VersionId = @VersionId
	AND TC_NewCarInquiriesId = @InqId
	)
	BEGIN
		UPDATE TC_NewCarInquiries
		SET VersionId = @VersionId
		,TC_CampaignSchedulingId = @TC_CampaignSchedulingId
		--Modifid By: Tejashree Patil on 14 Oct 2013
		WHERE TC_NewCarInquiriesId = @InqId

		UPDATE TC_InquiriesLead
		SET CarDetails = @CarDetails
		,ModifiedDate = GETDATE()
		,LatestVersionId = @LatestVersionId
		WHERE BranchId = @BranchId
		AND TC_InquiriesLeadId = @TC_InquiriesLeadId

		UPDATE TC_TaskLists    -- Updated By : Ashwini Dhamankar on Oct 19,2016 
		SET InterestedIn = @CarDetails
		,Car = @CarDetails
		WHERE TC_LeadId = @TC_LeadId
		AND BranchId = @BranchId

	--Modifid By Vivek Gupta on 8th oct 2013, Added update colorId = null in newcarbooking table while changing version
		IF EXISTS (
		SELECT CarColorId
		FROM TC_NewCarBooking WITH (NOLOCK)
		WHERE TC_NewCarInquiriesId = @InqId
		AND CarColorId IS NOT NULL
		)
		BEGIN
			UPDATE TC_NewCarBooking
			SET CarColorId = NULL
			WHERE TC_NewCarInquiriesId = @InqId
		END

	-- Modified By: Vivek Gupta on 26th sep,2013 commented old insert statement
	/*INSERT INTO TC_DispositionLog

	   (TC_LeadDispositionId,InqOrLeadId,TC_DispositionItemId,EventCreatedOn,EventOwnerId,TC_LeadId)

	 VALUES 

	(79,1,5,GETDATE(),@UserId,@TC_LeadId)	  */
		EXEC TC_DispositionLogInsert @UserId
		,79
		,@InqId
		,5
		,@TC_LeadId
	END

	IF (@ColorList IS NOT NULL)
	BEGIN
	-- Modified By: Vivek Gupta on 26th sep,2013 commented old insert statement
	/*INSERT INTO TC_DispositionLog

	   (TC_LeadDispositionId,InqOrLeadId,TC_DispositionItemId,EventCreatedOn,EventOwnerId,TC_LeadId)

		VALUES 

		(80,1,5,GETDATE(),@UserId,@TC_LeadId) */
		EXEC TC_DispositionLogInsert @UserId
		,80
		,@InqId
		,5
		,@TC_LeadId
	END

	-------------------------------------------------------------------------------------------------------------------------------------
	-- Modified By: If block added by  Vivek Gupta on 26th sep,2013
	IF NOT EXISTS (
	SELECT TC_NewCarInquiriesId
	FROM TC_NewCarInquiries WITH (NOLOCK)
	WHERE TC_NewCarInquiriesId = @InqId
	AND IsCorporate = @IsCorporate
	AND CompanyName = @CompanyName
	)
	BEGIN
		DECLARE @OldCompanyName VARCHAR(150)

		SELECT @OldCompanyName = CompanyName
		FROM TC_NewCarInquiries WITH (NOLOCK)
		WHERE TC_NewCarInquiriesId = @InqId

		UPDATE TC_NewCarInquiries
		SET IsCorporate = @IsCorporate
		,CompanyName = @CompanyName
		,TC_CampaignSchedulingId = @TC_CampaignSchedulingId
		--Modifid By: Tejashree Patil on 14 Oct 2013

		WHERE TC_NewCarInquiriesId = @InqId

		IF (
		@IsCorporate = 1
		AND @OldCompanyName IS NULL
		)
		BEGIN
			EXEC TC_DispositionLogInsert @UserId
			,82
			,@InqId
			,5
			,@TC_LeadId
		END
		ELSE
		IF (
		@IsCorporate = 0
		AND @OldCompanyName IS NOT NULL
		)
		BEGIN
			EXEC TC_DispositionLogInsert @UserId
			,83
			,@InqId
			,5
			,@TC_LeadId
		END
	-------------------------------------------------------------------------------------------------------------------------------------
	END

	-- MOdified By: Vishal Srivastava AE1830 on 06-01-2014 1610 hrs ist, updation of TC_NewCarInquiries.CreatedOn by @InquiryDate
	IF EXISTS (
	SELECT T.TC_NewCarInquiriesId
	FROM TC_NewCarInquiries AS T WITH (NOLOCK)
	WHERE T.TC_NewCarInquiriesId = @InqId
	AND @InquiryDate IS NOT NULL
	)
	BEGIN
		UPDATE TC_NewCarInquiries
		SET CreatedOn = @InquiryDate
		WHERE TC_NewCarInquiriesId = @InqId
	END

	-- Modified By : Vishal Srivastava AE1830 on 04 APR 2014 1602 HRS IST update exchange car details  from newcarinquiespage.aspx
	UPDATE TC_NewCarInquiries
	SET TC_NewCarExchangeId = @exchange
	WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId

	IF (
	@exchange = 4
	OR @exchange = 3
	)
	BEGIN
		IF EXISTS (
		SELECT TC_NewCarInquiriesId
		FROM TC_ExchangeNewCar WITH (NOLOCK)
		WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
		)
		BEGIN
			UPDATE TC_ExchangeNewCar
			SET CarVersionId = @carVersionChange
			,Kms = @carKiloChange
			,MakeYear = @carMakeYearChange
			,ExpectedPrice = @carPriceChange
			WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
		END
		ELSE
			BEGIN
				INSERT INTO TC_ExchangeNewCar (
				TC_NewCarInquiriesId
				,CarVersionId
				,Kms
				,MakeYear
				,ExpectedPrice
				)
				VALUES (
				@TC_NewCarInquiryId
				,@carVersionChange
				,@carKiloChange
				,@carMakeYearChange
				,@carPriceChange
				)
			END
	END
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_IBCGetData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_IBCGetData]
GO

	
CREATE PROCEDURE [dbo].[CRM_IBCGetData]
	@MobileNo		VARCHAR(20),
	@InterestedIn	SMALLINT,
	@CreatedBy		NUMERIC,
	@InboundCallDataId	NUMERIC,
	@IsCRM			BIT OUTPUT,
	@IsCWUsed		BIT OUTPUT,
	@CWCustId		NUMERIC OUTPUT,
	@CRMCustId		NUMERIC OUTPUT,
	@CName			VARCHAR(20) OUTPUT,
	@CEMail			VARCHAR(50) OUTPUT,
	@CityId			NUMERIC OUTPUT,
	@IsVerified		BIT OUTPUT,
	@IsMultiLead	BIT OUTPUT
	 
	
 AS
	DECLARE @CRMLeadId NUMERIC
	DECLARE @UCInquiryId NUMERIC
    DECLARE @Count SMALLINT
BEGIN

	-- GET CRM Customer Detaails
	SELECT @CName = FirstName, @CEMail = Email, @CityId = CityId, @CRMCustId = Id, @IsVerified= ISNULL(IsVerified,0) 
	FROM CRM_Customers WITH (NOLOCK) WHERE Mobile = @MobileNo
	
	--Get CarWale Customer Details
	-- If already got details
	IF @CEMail <> ''
		BEGIN
			--Get CRM Active Lead Details
			SELECT @CRMLeadId = ISNULL(Id, -1) FROM CRM_Leads WHERE CNS_CustId = @CRMCustId AND LeadStageId <> 3
			--PRINT '@CRMLeadId' + Convert(VarChar,@CRMLeadId)
			SELECT @Count=@@ROWCOUNT
			
			IF (@Count > 1)
				BEGIN
					print 'inside rowcount > 1'
					SET @IsCRM = 1
					SET @IsMultiLead = 1
				END
			ELSE IF (@Count = 1)
				BEGIN
					print 'inside rowcount = 1'
					SET @IsCRM = 1
					SET @IsMultiLead = 0
				END
			ELSE
				BEGIN
					print 'inside rowcount =0 '
					SET @IsCRM = 0
					SET @IsMultiLead = 0
				END
			
			--Get Used Car Details
			SELECT @CWCustId = C.Id, @UCInquiryId = ISNULL((SELECT TOP 1 EventId FROM CH_ScheduledCalls WHERE TBCID = C.Id AND CallType IN(1,7)),0)
			FROM Customers C WITH (NOLOCK) WHERE Mobile = @MobileNo
			SELECT @Count=@@ROWCOUNT
		
			IF @Count <> 0 AND @UCInquiryId <> 0
				BEGIN
					SET @IsCWUsed = 1
				END
			ELSE
				BEGIN
					SET @IsCWUsed = 0
				END
		END
	ELSE
		BEGIN
			SELECT @CWCustId = C.Id, @CName = Name, @CEMail = Email, @CityId = CityId,
				@UCInquiryId = ISNULL((SELECT TOP 1 EventId FROM CH_ScheduledCalls WHERE TBCID = C.Id AND CallType IN(1,7)),0)
			FROM Customers C WITH (NOLOCK) WHERE Mobile = @MobileNo

			SELECT @Count=@@ROWCOUNT

			IF @Count <> 0  AND @UCInquiryId <> 0
				BEGIN
					SET @IsCWUsed = 1
				END
			ELSE
				BEGIN
					SET @IsCWUsed = 0
				END
		END
		
	UPDATE CRM_InboundCallData SET CRMCustId=@CRMCustId, CRM_LeadId=@CRMLeadId, CWCustId=@CWCustId, CreatedBy=@CreatedBy, CreatedOn=GETDATE(), InterestedIn=@InterestedIn, MobileNumber=@MobileNo, UCInquiryId=@UCInquiryId
			WHERE Id = @InboundCallDataId

	PRINT '@IsCWUsed' + Convert(VarChar,@IsCWUsed)
	PRINT '@IsCRM' + Convert(VarChar,@IsCRM)
	PRINT '@InboundCallDataId' + Convert(VarChar,@InboundCallDataId)
END


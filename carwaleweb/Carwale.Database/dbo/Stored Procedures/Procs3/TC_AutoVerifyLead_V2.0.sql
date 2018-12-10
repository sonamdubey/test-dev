IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AutoVerifyLead_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AutoVerifyLead_V2]
GO

	
-- Created By:	Deepak Tripathi
-- Create date: 11th July 2016
-- Description: Autoverify Lead

-- =============================================
create  PROCEDURE [dbo].[TC_AutoVerifyLead_V2.0]
	@LeadId			INT,
	@InqLeadId		INT,
	@LeadOwnerId	INT
AS
	BEGIN
		SET NOCOUNT ON;
		
		DECLARE @TC_CustomerId INT
		DECLARE @ScheduledOn DATETIME = GETDATE()
		DECLARE @TC_BusinessType TINYINT
		
		SELECT @TC_CustomerId = TC_CustomerId, @TC_BusinessType = L.LeadType 
		FROM TC_Lead L WITH (NOLOCK) WHERE TC_LeadId = @LeadId AND ISNULL(TC_LeadStageId, 0) IN(0,1)
		
		SET @TC_BusinessType = dbo.TC_FNGetBusinessType(@TC_BusinessType)
		
		--Lead is neither in consultation stage nor in closing
		IF @@ROWCOUNT = 1
			BEGIN
				--Send lead in consultation stage
				UPDATE TC_Lead SET TC_LeadStageId = 2, LeadVerifiedBy = @LeadOwnerId, 
								LeadVerificationDate = GETDATE(), TC_LeadDispositionId = 2
				WHERE TC_LeadId = @LeadId
				
				--Verify Customer
				UPDATE TC_CustomerDetails SET IsVerified = 1 WHERE Id = @TC_CustomerId
				
				--Update Inquiries Lead
				UPDATE TC_InquiriesLead SET TC_LeadStageId = 2 WHERE TC_InquiriesLeadId = @InqLeadId
				
				--Log Call
				EXEC [TC_LogCall_V2.0] @LeadId = @LeadId, @CallType = 1, @LeadOwnerId	 = @LeadOwnerId, @ScheduledOn = @ScheduledOn, @FollowupComments = 'Inquiry Added & Verified', @TC_BusinessTypeId = @TC_BusinessType
			END
END


------------------------------------------------------------------------------------------------------------------------------------



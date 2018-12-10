IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadDetailsSave_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadDetailsSave_V2]
GO

	

-- Created By:	Deepak Tripathi
-- Create date: 11th July 2016
-- Description:	Adding New Lead
-- Modify By: Tejashree Patil On 10 Aug 2016, Added parameter @BusinessTypeId to identify BusinessType of lead.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_LeadDetailsSave_V2.0]
	@BranchId			INT,
	@CustomerId			INT,
	@InquirySource		SMALLINT,
	@LeadDate			DATETIME,
	@LeadInqTypeId		SMALLINT,
	@LeadStageId		TINYINT,
	@LeadDispositionId	SMALLINT,
	@CampaignSchedulingId	INT,
	@LeadIdOutput	INT OUTPUT,
	@BusinessTypeId TINYINT = 3
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TC_Lead (BranchId ,TC_CustomerId ,TC_InquirySourceId ,TC_LeadStageId ,LeadCreationDate ,LeadType,
				TC_LeadDispositionId ,TC_CampaignSchedulingId, TC_BusinessTypeId) 
			VALUES (@BranchId ,@CustomerId ,@InquirySource, @LeadStageId,@LeadDate ,@LeadInqTypeId,
				@LeadDispositionId ,@CampaignSchedulingId, @BusinessTypeId )

	SET @LeadIdOutput = SCOPE_IDENTITY();
		
	UPDATE	TC_CustomerDetails 
	SET		ActiveLeadId = @LeadIdOutput, IsleadActive = 1 
	WHERE	Id = @CustomerId
END



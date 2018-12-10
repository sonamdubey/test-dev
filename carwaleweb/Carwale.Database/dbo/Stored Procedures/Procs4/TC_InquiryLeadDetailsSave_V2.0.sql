IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryLeadDetailsSave_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryLeadDetailsSave_V2]
GO

	
-- Created By:	Deepak Tripathi
-- Create date: 11th July 2016
-- Description:	Adding New Inquiry Lead
-- Modified By : Nilima More On 22nd August 2016,added @RegistrationNumber as input parameter.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_InquiryLeadDetailsSave_V2.0]
	@BranchId			INT,
	@CustomerId			INT,
	@LeadId				INT,
	@InquirySource		SMALLINT,
	@LeadDate			DATETIME,
	@LeadInqTypeId		SMALLINT,
	@LeadStageId		TINYINT,
	@Eagerness			TINYINT,
	@CreatedBy			INT,
	@CarDetails			VARCHAR(250),
	@LatestVersionId	INT,
	@CampaignId			INT,
	@CampaignSchedulingId	INT,
	@RegistrationNumber VARCHAR(50) = NULL,
	@LeadDispositionId	INT = NULL,
	@INQLeadIdOutput	INT OUTPUT
AS
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO TC_InquiriesLead 
					(	BranchId,TC_CustomerId,TC_InquiryStatusId,CreatedBy,CreatedDate,TC_LeadId
						,TC_LeadInquiryTypeId,TC_LeadStageId,CarDetails,LatestInquiryDate,InqSourceId
						,TC_CampaignSchedulingId,LatestVersionId,CampaignId,RegistrationNumber,TC_LeadDispositionID
					)
		VALUES 
					(	@BranchId,@CustomerId,@Eagerness,@CreatedBy,@LeadDate,@LeadId
						,@LeadInqTypeId,@LeadStageId,@CarDetails,@LeadDate,@InquirySource
						,@CampaignSchedulingId,@LatestVersionId,@CampaignId,@RegistrationNumber,@LeadDispositionId
					) 

			SET @INQLeadIdOutput = SCOPE_IDENTITY();
END


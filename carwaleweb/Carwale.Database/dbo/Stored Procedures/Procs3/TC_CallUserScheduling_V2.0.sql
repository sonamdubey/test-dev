IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CallUserScheduling_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CallUserScheduling_V2]
GO

	-- =====================================================================================================================================
-- Author:  Deepak  
-- Create date: 16th Sept 2016  
-- Description: Update Lead Staus 
-- ======================================================================================================================================
create PROCEDURE [dbo].[TC_CallUserScheduling_V2.0] 
	@TC_LeadId AS INT
	,@TC_Usersid AS INT
AS
BEGIN

	--------------Update customer verification and fake status after showing interest------------    
	UPDATE TC_CustomerDetails SET IsVerified = 1,IsFake = 0 WHERE ActiveLeadId = @TC_LeadId

	----------------Updating  Lead verification id and lead stage id since lead is moving form verification to consultation stage     
	UPDATE TC_Lead SET LeadVerifiedBy = @TC_Usersid ,LeadVerificationDate = GETDATE(),TC_LeadDispositionId = 2,	TC_LeadStageId = 2
	WHERE TC_LeadId = @TC_LeadId
	
	--Update Inquiries Lead
	UPDATE TC_InquiriesLead	SET TC_UserId = @TC_Usersid, TC_LeadStageId = 2  WHERE TC_LeadId = @TC_LeadId AND ISNULL(TC_LeadStageId,0) <> 3 AND IsActive = 1

END


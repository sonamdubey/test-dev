IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQLeadClose]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQLeadClose]
GO

	-- =============================================  
-- Author      : <Author,Nilesh Utture>  
-- Create date : <Create Date,26/07/2013>  
-- Description : <Description, For closing the Lead when single Inquiry is remaining>  
-- Modified By : Chetan Navin on 21st June 2016 , Added call to sp TC_DisposeCall
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQLeadClose]
	@InquiryId INT,
	@InquiryType TINYINT,
	@LeadOwnerId INT,
	@UserId INT,
	@InquiriesLeadId INT,
	@LeadId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @InquiriesLeadCount INT
	DECLARE @InquiryCount INT
	DECLARE @TC_CallsId BIGINT
	
    -- Insert statements for procedure here
	IF @InquiryType = 3 -- change disposition for New car Inquiry 
		BEGIN
			UPDATE TC_InquiriesLead SET TC_LeadStageId = 3 WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
			
			UPDATE TC_Lead SET TC_LeadStageId = 3, LeadClosedDate = GETDATE() WHERE TC_LeadId = @LeadId
			
			UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0 --,IsActive = 0 
			WHERE ActiveLeadId = @LeadId
			
			SELECT @TC_CallsId = TC_CallsId 
			FROM   TC_ActiveCalls WITH (NOLOCK)
			WHERE  TC_UsersId = @LeadOwnerId -- Active call of lead owner should be deleted 
			AND TC_LeadId = @LeadId           
			
			--Dispose Call
			EXEC TC_DisposeCall @TC_CallsId,NULL,NULL,NULL,@UserId
		END
END

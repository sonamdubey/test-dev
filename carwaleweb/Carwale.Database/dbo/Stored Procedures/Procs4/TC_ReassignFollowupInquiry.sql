IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReassignFollowupInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReassignFollowupInquiry]
GO

	
-- Created By:	Surendra
-- Create date: 17 Jan 2012
-- Description:	Reassignment of Inquiries
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReassignFollowupInquiry]
@BranchId NUMERIC,
@AssignedTo BIGINT,
@LeadIds VARCHAR(MAX),
@UserId BIGINT
AS           

BEGIN
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @TC_InquiriesLeadId VARCHAR(30) -- this holds each array value as it is returned  
	SET @LeadIds = @LeadIds + ','  
  
  -- Loop through the string searching for separtor characters    
	WHILE PATINDEX('%' + ',' + '%', @LeadIds) <> 0   
		BEGIN  			
			-- patindex matches the a pattern against a string  
			SELECT  @Separator_position = PATINDEX('%' + ',' + '%',@LeadIds)  
			SELECT  @TC_InquiriesLeadId = LEFT(@LeadIds, @Separator_position - 1)
			
			UPDATE TC_InquiriesLead SET TC_UserId=@AssignedTo,ModifiedBy=@UserId,ModifiedDate=GETDATE()
			WHERE BranchId=@BranchId AND TC_InquiriesLeadId=@TC_InquiriesLeadId
						        
			-- This replaces what we just processed with and empty string  
			SELECT  @LeadIds = STUFF(@LeadIds, 1, @Separator_position, '')  
		END 
END

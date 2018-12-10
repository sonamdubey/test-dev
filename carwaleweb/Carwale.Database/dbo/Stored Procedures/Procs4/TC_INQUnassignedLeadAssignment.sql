IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQUnassignedLeadAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQUnassignedLeadAssignment]
GO

	-- Created By:	Surendra
-- Create date: 17 Jan 2012
-- Description:	Reassignment of Inquiries
-- Modified By: Tejashree Patil on 12 Feb 2013 at 6pm, Inserted LeadCreationDate in ScheduledOn from TC_Calls instead of GETDATE()
-- Modified By: Surendra on 15 Feb 2013 at 3 pm , Change the condition for lead assignment
--              Chetan Navin on 20th June 2016 , Replaced query of calls and added call to sp TC_ProcessCalls
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQUnassignedLeadAssignment]
@BranchId BIGINT,
@UserID BIGINT, 
@InqLeadIds VARCHAR(MAX),
@ModifiedBy BIGINT
AS           

BEGIN
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @LeadId VARCHAR(30) -- this holds each array value as it is returned  
	DECLARE @TC_CallsId AS BIGINT
	DECLARE @ScheduledOn DATETIME = GETDATE()
	-- DECLARE @LeadCreationDate DATETIME -- Modified By: Tejashree Patil on 12 Feb 2013 at 6pm,
	SET @InqLeadIds = @InqLeadIds + ','  
  
  -- Loop through the string searching for separtor characters    
	WHILE PATINDEX('%' + ',' + '%', @InqLeadIds) <> 0   
		BEGIN  			
			-- patindex matches the a pattern against a string  
			SELECT  @Separator_position = PATINDEX('%' + ',' + '%',@InqLeadIds)  
			SELECT  @LeadId = LEFT(@InqLeadIds, @Separator_position - 1)

					-----------Condition modified on 15-02-2013 for  stopping duplicate assignment of leads-----
			IF EXISTS(SELECT  TOP 1 TC_LeadId FROM TC_Lead where TC_LeadId=@LeadId AND TC_LeadStageId IS NULL 
			              AND TC_LeadDispositionId IS NULL) --(@LeadId IS NOT NULL) -- checking for security purpose only
			BEGIN
				BEGIN TRY
				
				BEGIN TRANSACTION LeadAssignment
				-- Modified By: Tejashree Patil on 12 Feb 2013 at 6pm,
					-- SET @LeadCreationDate = NULL
					-- SELECT @LeadCreationDate=LeadCreationDate FROM TC_Lead L WHERE TC_LeadId=@LeadId
					
					-- assigning new lead owner for verification
					UPDATE TC_Lead SET LeadVerifiedBy=@UserId, TC_LeadStageId = 1 WHERE TC_LeadId=@LeadId
					
					-- Assigning all type inquiries lead to same user for verifications
					UPDATE TC_InquiriesLead SET TC_UserId=@UserId,ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE(), TC_LeadStageId = 1
							 WHERE TC_LeadId=@LeadId 

					-- Updating call also							
					EXEC TC_ScheduleCall @UserId,@LeadId,1,@ScheduledOn,null,null,null,null,@TC_CallsId

			      COMMIT TRANSACTION LeadAssignment	
				
				END TRY
					
				BEGIN CATCH	
				ROLLBACK 	TRANSACTION LeadAssignment	
					INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,Inputparameters)
					VALUES('TC_INQUnassignedLeadAssignment',(ERROR_MESSAGE()+', ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),GETDATE(),NULL)
					--SELECT ERROR_NUMBER() AS ErrorNumber;
				END CATCH;

			-- This replaces what we just processed with and empty string  
			SELECT  @InqLeadIds = STUFF(@InqLeadIds, 1, @Separator_position, '')  
		END
		ELSE
		BEGIN
			SELECT  @InqLeadIds = STUFF(@InqLeadIds, 1, @Separator_position, '')  
		END
	END
END

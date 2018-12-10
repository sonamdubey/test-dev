IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InqStatusChngToArchive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InqStatusChngToArchive]
GO

	
-- Modified By: Tejashree
-- Modified date: 22nd March,2012
-- Description: --Status of Inquiiry changed to archive.
-- Check - TC_InqStatusChngToArchive '5,7',968

CREATE PROCEDURE [dbo].[TC_InqStatusChngToArchive]  
(  
@InqLeadId VARCHAR(1000),
@BranchId BIGINT,
@Status TINYINT OUTPUT
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 SET NOCOUNT ON;
	DECLARE @Separator CHAR(1)=','
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned  
  -- For my loop to work I need an extra separator at the end. I always look to the  
  -- left of the separator character for each array value  
	SET @Status=0
    SET @InqLeadId = @InqLeadId + @Separator  
     WHILE PATINDEX('%' + @Separator + '%', @InqLeadId) <> 0   
        BEGIN  			
            -- patindex matches the a pattern against a string  
            SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@InqLeadId)  
            SELECT  @array_value = LEFT(@InqLeadId, @Separator_position - 1)
			 -- Checking whether Inquiry is archived or not			
				-- update in TC_InquiriesLead with TC_InquiriesFollowupActionId=8 i.e archive.
				UPDATE TC_InquiriesLead SET TC_InquiriesFollowupActionId =8 WHERE TC_InquiriesLeadId=@array_value
				AND BranchId=@BranchId
				SET @Status=1
		
				 -- This replaces what we just processed with and empty string  
            SELECT  @InqLeadId = STUFF(@InqLeadId, 1, @Separator_position, '')   
		END --while End
END


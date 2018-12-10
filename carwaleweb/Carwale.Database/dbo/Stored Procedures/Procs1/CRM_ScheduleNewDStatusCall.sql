IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ScheduleNewDStatusCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ScheduleNewDStatusCall]
GO

	
CREATE PROCEDURE [dbo].[CRM_ScheduleNewDStatusCall]      
      
 @LeadId   Numeric,      
 @CallType  Int,      
 @CallerId  Numeric,      
 @Subject  VARCHAR(100),      
 @DealerId  Numeric    
 AS      
       
BEGIN      
 DECLARE @CallId NUMERIC = NULL    
     
 SET @CallId = (SELECT TOP 1 CallId FROM CRM_CallActiveList WHERE LeadId = @LeadId AND CallType = 13   -- 13 - New Dealer Staus Update
                AND IsTeam = 0 AND CallerId = @CallerId AND DealerId = @DealerId ORDER BY CallId DESC)    
     
      
    IF (@CallId <= 0 OR @CallId IS NULL)
		BEGIN    
			INSERT INTO CRM_Calls      
			(      
				LeadId, CallType, IsTeam, CallerId, ScheduledOn, IsActionTaken,ActionTakenId,ActionTakenOn, CreatedOn, Subject, ActionComments, DealerId      
			)       
			VALUES      
			(    
				@LeadId, 13, 0, @CallerId, GETDATE(), 1,1,GETDATE(), GETDATE(), 'Dealer Staus Update', @Subject, @DealerId   
			)   
		END    
    ELSE    
		BEGIN    
   
			UPDATE CRM_Calls SET ActionComments  = ISNULL(ActionComments,'') + @subject,
				IsActionTaken = 1,ActionTakenId = 1, ActionTakenOn = GETDATE(), ActionTakenBy = @CallerId
			WHERE Id = @CallId  
     
			DELETE FROM CRM_CallActiveList WHERE CallId =  @CallId 
			
		END        
END      

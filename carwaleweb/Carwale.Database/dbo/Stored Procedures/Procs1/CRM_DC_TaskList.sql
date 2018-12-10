IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DC_TaskList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DC_TaskList]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 27th Sep 2011
-- Description:	This proc Update data in CRM_CallActiveList,CRM_Calls And INSERT DATA INTO CRM_DealerCallLog
-- Modifier	  : Amit Kumar on 4th feb 2013(changed the way to save data )
-- =============================================
CREATE PROCEDURE [dbo].[CRM_DC_TaskList]
	
	(
	@CRM_DCFollowUp CRM_DCFollowUp readonly,
	@ScheduleDt		DATETIME	
	)
	AS
	BEGIN
		
		--Log Changed CallDateTime in Bulk. When dealership is not responding call.
		INSERT INTO CRM_DealerCallLog(CallId,DealerId,Subject,Comment,UpdatedOn,UpdatedBy)
		SELECT DFU.CallId,DFU.DealerId,DFU.Subject,DFU.ActionComments,DFU.ActionTakenOn,DFU.ActionTakenBy
        FROM @CRM_DCFollowUp DFU
        
		-- Check Whether CallId exists or Not
		--SELECT @LeadId = CAL.LeadId, @CallType = CAL.CallType FROM CRM_CallActiveList CAL WITH (NOLOCK) 
		--INNER JOIN @CRM_DCFollowUp DFU ON  CAL.CallId = DFU.CallId
		
		--IF @@ROWCOUNT > 0
			--BEGIN
			--Log This call against lead
			INSERT INTO CRM_CALLS(LeadId, CallType, IsTeam, CallerId, Subject, ScheduledOn, 
							IsActionTaken, ActionComments, ActionTakenOn, ActionTakenBy,
							CreatedOn, DealerId)
			SELECT DFU.LeadId,DFU.CallType,DFU.IsTeam,DFU.CallerId,DFU.Subject,DFU.ActionTakenOn,DFU.IsActionTaken,
			'BC-' + DFU.ActionComments,DFU.ActionTakenOn,DFU.ActionTakenBy,DFU.CreatedOn,DFU.DealerId
			FROM @CRM_DCFollowUp DFU
						
			--Extend Lead Next Call Date		
			UPDATE CRM_CALLS SET ScheduledOn = @ScheduleDt WHERE  Id IN (SELECT 
			DFU.CallId FROM @CRM_DCFollowUp DFU )  
			
			--Extend Lead Next Call Date
			UPDATE CRM_CallActiveList SET ScheduledOn =@ScheduleDt WHERE  CallId IN (SELECT 
			DFU.CallId FROM @CRM_DCFollowUp DFU )

			--END
	END
						
			
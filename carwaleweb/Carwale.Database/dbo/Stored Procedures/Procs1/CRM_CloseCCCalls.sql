IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CloseCCCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CloseCCCalls]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 16 May 2014
-- Description:	To close cc calls if lead is getting closed and to log it
-- =============================================
CREATE PROCEDURE [dbo].[CRM_CloseCCCalls]
	@LeadId NUMERIC,
	@ActionTakenBy NUMERIC
AS
BEGIN
	DECLARE @ListCallId VARCHAR(MAX)
	DECLARE @CurrentDate DATETIME = GETDATE()
	DECLARE @temptbl table(CallId INT)

	INSERT INTO @temptbl(CallId)
	SELECT CallId FROM CRM_CallActiveList WITH (NOLOCK) WHERE IsTeam = 1 AND LeadId=@LeadId

	UPDATE CRM_Calls SET
		IsActionTaken = 1,	ActionTakenId = 1, ActionTakenOn = @CurrentDate, 
		ActionComments = 'Lead closed by DealerCoordinator', ActionTakenBy = @ActionTakenBy
	WHERE Id IN(SELECT CallId FROM @temptbl)
			
	DELETE FROM CRM_CallActiveList WHERE CallId IN(SELECT CallId FROM @temptbl)
					
END

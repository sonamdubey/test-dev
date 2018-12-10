IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetTaskListDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetTaskListDealers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(17th July 2014)
-- Description	:	Get all dealers the user has assigned to call
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetTaskListDealers] 
	
	@UserID INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT DISTINCT DC.DealerId AS Value, D.Organization AS Text
	FROM DCRM_Calls  AS DC WITH (NOLOCK) 
	INNER JOIN  Dealers AS D WITH (NOLOCK) ON DC.DealerId = D.ID 
	AND  ActionTakenId = 2 
	AND DC.ScheduleDate <= GETDATE()
	AND DC.UserId = @UserId 
	
	ORDER BY Organization 
END


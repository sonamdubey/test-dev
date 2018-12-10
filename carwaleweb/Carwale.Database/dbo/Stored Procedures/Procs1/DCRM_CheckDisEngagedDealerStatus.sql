IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CheckDisEngagedDealerStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CheckDisEngagedDealerStatus]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th Jan 2014)
-- Description	:	Check callid of Disengaged Dealers Status
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_CheckDisEngagedDealerStatus] 
	
	@CallId Numeric(18,0),
	@Result SMALLINT = -1 OUTPUT 

AS
BEGIN
	
	SET NOCOUNT ON;

	SET @Result = 1
	SELECT DC.Id FROM DCRM_Calls DC(NOLOCK) WHERE DC.ActionTakenId = 1 AND DC.Id = @CallId
	IF @@ROWCOUNT = 0 
		SET @Result = 0
	PRINT @Result
END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TeleCallerLog_InsertUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TeleCallerLog_InsertUpdate]
GO

	
CREATE PROCEDURE [dbo].[TeleCallerLog_InsertUpdate]
	@ID				NUMERIC,
	@UserId			NUMERIC,
	@LoginTime		DATETIME,
	@Type			SMALLINT,
	@Status			INT OUTPUT,
	@TeleID			NUMERIC OUTPUT 
AS

BEGIN
	SET @Status = 0

	IF @ID = -1
			
		BEGIN
			INSERT INTO TeleCallerLog (UserID, LoginTime, Type)
			VALUES (@UserId,@LoginTime,@Type)
			SET @TeleID = SCOPE_IDENTITY() 
			SET @Status = 1 
			
			IF @Type = 2
			BEGIN
				UPDATE CH_TeleCallers SET LastLeadTime = GETDATE(), IsNew = 0, IsActiveLogin = 1
				WHERE TCID = @UserId AND TCID IN(SELECT C.TCID FROM CH_TCAssignedTasks AS C WHERE C.TCID = @UserId AND C.TBCType = 2 AND C.CallType = 1)
			END
			
			IF @Type = 1
			BEGIN
				UPDATE CRM_ADM_PEConsultantRules SET IsActiveLogin = 1 WHERE ConsultantId = @UserId
			END
		END
	
END


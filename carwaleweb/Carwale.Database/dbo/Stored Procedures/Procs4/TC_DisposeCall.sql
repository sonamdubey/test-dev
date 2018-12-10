IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DisposeCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DisposeCall]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 23rd June 2016
-- Description : To dispose calls 
-- exec TC_DisposeCall 43359,NULL,NULL,NULL,88916
-- =============================================
CREATE PROCEDURE [dbo].[TC_DisposeCall] 
	@TC_CallId	INT,
	@ActionComment	VARCHAR(500),
	@TC_CallActionId TINYINT,
	@NextFolloupDate DATETIME,
	@TC_UsersId INT
AS
BEGIN

		UPDATE TC_Calls
		SET IsActionTaken = 1
			,ActionTakenOn = GETDATE()
			,ActionComments = @ActionComment
			,TC_CallActionId = ISNULL(@TC_CallActionId,TC_CallActionId)
			,NextFollowUpDate = @NextFolloupDate
			,TC_UsersId = @TC_UsersId
		WHERE TC_CallsId = @TC_CallId
		
		DELETE FROM TC_ActiveCalls WHERE TC_CallsId = @TC_CallId	
	
		EXEC TC_TaskListUpdate 1,@TC_CallId,1							  
END
-------------------------------------

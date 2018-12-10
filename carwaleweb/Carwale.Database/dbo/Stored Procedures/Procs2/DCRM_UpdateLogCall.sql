IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateLogCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateLogCall]
GO

	
--	Modifier	:	Sachin Bharti(22th April 2014)
--	Purpose		:	Log the call only call is assigned to that user
-- Modified by: Deepak, 19/01/2016
CREATE PROCEDURE [dbo].[DCRM_UpdateLogCall]

	@CallId			INT,
	@CalledDate		DateTime,
	@ActionTakenId	Int,
	@ActionComments	VarChar(500),
	@CallStatus		SmallInt,
	@Status			Bit OutPut,
	@CallCategory	INT	= NULL,
	@CallerId		INT = NULL
AS
	
BEGIN
	SET @Status = 0

	IF @CallId > 0
		BEGIN
			DECLARE @ExistCallerId	INT 

			SELECT @ExistCallerId = DC.UserId  FROM DCRM_Calls DC(NOLOCK) WHERE DC.Id = @CallId

			--IF @ExistCallerId = @CallerId 
			BEGIN
				UPDATE DCRM_Calls SET
					ActionTakenId = @ActionTakenId, CalledDate = @CalledDate, 
					Comments = @ActionComments, CallStatus = @CallStatus,CallCategory = @CallCategory
				WHERE Id = @CallId
	
				SET @Status = 1
			END
		END
		
				
END

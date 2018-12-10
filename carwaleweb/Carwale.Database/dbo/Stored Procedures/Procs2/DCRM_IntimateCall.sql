IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_IntimateCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_IntimateCall]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 5-July-2012
-- Description:	Intimate call for dealer if it doesnot exist else update call
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_IntimateCall]
	-- Add the parameters for the stored procedure here
	@DealerId		INT,
	@CallerId		INT,
	@RoleId			INT,
	@Subject		VARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CallId			INT,
			@ScheduledDate	DATETIME = GETDATE(),
			@NewCallId		NUMERIC,
			@NewCallType	INT

    -- Insert statements for procedure here
    --Check for any call pending for dealer for specific calltype
	SELECT @CallId = Id FROM DCRM_Calls WITH(NOLOCK) WHERE DealerId = @DealerId 
	AND CallType IN (SELECT Id FROM DCRM_CallTypes WHERE RoleId = @RoleId)
	
	--If call exist then update details else Schedule new call
	IF @@ROWCOUNT <> 0
		BEGIN
			UPDATE DCRM_Calls
				SET ScheduleDate = @ScheduledDate,
					Subject = @Subject
			WHERE Id = @CallId
		END
	ELSE
		BEGIN
			SELECT  TOP 1 @NewCallType = Id FROM DCRM_CallTypes WHERE RoleId = @RoleId
			EXEC DCRM_ScheduleNewCall @DealerId,@CallerId,@ScheduledDate,13,@ScheduledDate,@Subject,NULL,@NewCallType,@NewCallId
		END
END


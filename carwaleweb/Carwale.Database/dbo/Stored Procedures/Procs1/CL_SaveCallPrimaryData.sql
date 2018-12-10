IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_SaveCallPrimaryData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_SaveCallPrimaryData]
GO

	
CREATE PROCEDURE [dbo].[CL_SaveCallPrimaryData]

	@CategoryId			NUMERIC,
	@ReferenceId		NUMERIC,
	@StartDateTime		DATETIME,
	@AgentId			VARCHAR(50),
	@CalledNumber		VARCHAR(50),
	@IsOpen				BIT,
	@AspectCallId		VARCHAR(50),
	@AspectLogId		NUMERIC,
	@AspectSequenceId	NUMERIC,
	@NewDataId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @NewDataId = -1
	BEGIN

		INSERT INTO CL_Logs
		(
			CategoryId, ReferenceId, StartDateTime, AgentId, CalledNumber, IsOpen, 
			AspectCallId, AspectLogId, AspectSequenceId
		)
		VALUES
		(
			@CategoryId, @ReferenceId, @StartDateTime, @AgentId, @CalledNumber, @IsOpen, 
			@AspectCallId, @AspectLogId, @AspectSequenceId
		)
			
		SET @NewDataId = SCOPE_IDENTITY()
			
	END
END














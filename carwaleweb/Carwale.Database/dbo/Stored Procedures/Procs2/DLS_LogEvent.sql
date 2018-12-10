IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_LogEvent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_LogEvent]
GO

	

CREATE PROCEDURE [dbo].[DLS_LogEvent]
	@CBDId			NUMERIC,
	@EventSubId		NUMERIC,
	@EventOn		DATETIME,
	@EventBy		NUMERIC,
	@IsDealer		BIT,
	@CurrentId		NUMERIC OUTPUT
 AS
	
BEGIN
	SET @CurrentId = -1

	INSERT INTO DLS_EventLog
	(
		EventSubId, CBDId, EventOn, EventBy, IsDealer
	) 
	VALUES
	( 
		@EventSubId, @CBDId, @EventOn, @EventBy, @IsDealer
	)
		
	SET @CurrentId = SCOPE_IDENTITY() 
END




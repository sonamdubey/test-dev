IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_InsertCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_InsertCalls]
GO

	CREATE   PROCEDURE [dbo].[CH_InsertCalls]
	@CallType		AS SMALLINT, 
	@TBCType		AS SMALLINT, 
	@TBCID			AS NUMERIC,
	@TcId			AS NUMERIC, 
	@EventId		AS NUMERIC, 
	@EntryDateTime	AS DATETIME,
	@IsFreshCall    AS BIT = 0,
	@CallId			AS NUMERIC OUTPUT
	
AS
	
BEGIN
	--For Sell Inquiry Pool
	IF @CallType = 17
		SET @IsFreshCall = 1
		
	SELECT Id FROM CH_Calls WHERE CallType = @CallType AND TBCType = @TBCType AND TbcId = @TBCID AND EventId = @EventId AND EventId <> -1
	
	IF @@ROWCOUNT < 1
	BEGIN
		INSERT INTO CH_Calls
			(
				CallType, 	TBCType, 	TBCID, TcId,	EventId,	EntryDateTime, 		Status, IsFreshCall
			)	
		VALUES
			(
				@CallType, 	@TBCType, 	@TBCID, @TcId, 	@EventId,	@EntryDateTime, 	1, @IsFreshCall
			)
		SET @CallId = SCOPE_IDENTITY()
	END
	ELSE SET @CallId = -1
		
END

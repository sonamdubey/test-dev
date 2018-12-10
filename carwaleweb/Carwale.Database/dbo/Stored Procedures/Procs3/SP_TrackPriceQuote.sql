IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SP_TrackPriceQuote]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SP_TrackPriceQuote]
GO

	CREATE PROCEDURE [dbo].[SP_TrackPriceQuote]
	@ID AS NUMERIC,
	@EntryDateTime DateTime,
	@Steps	AS INT,
	@EnteredId AS NUMERIC OUTPUT
	
 AS
	
BEGIN
	IF @ID = -1
	BEGIN
		INSERT INTO 
			TrackPriceQuote( Steps, EntryDateTime)
		VALUES 
			( @Steps, @EntryDateTime)	
			
		SET @EnteredId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		Update TrackPriceQuote Set Steps = @Steps Where ID = @ID
		SET @EnteredId = @ID
	END
	
END


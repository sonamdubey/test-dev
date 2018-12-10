IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddToBannedList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddToBannedList]
GO

	------Modified by Ravi on 13-06-2013

CREATE PROCEDURE [dbo].[AddToBannedList]
@CustomerId NUMERIC (18,0),
@BannedBy NUMERIC (18,0),
@IsAdded BIT OUTPUT
AS
BEGIN

	DECLARE @RowsPresent NUMERIC
	
	SELECT @RowsPresent = COUNT(CustomerId) FROM Forum_BannedList WHERE CustomerId = @CustomerId

	IF @RowsPresent = 0
	BEGIN
		INSERT INTO Forum_BannedList
		(CustomerId, BannedBy, BannedDate)
		VALUES
		(@CustomerId, @BannedBy, GETDATE())
		
		Update Customers SET IsFake = 1 where Id = @CustomerId
	
		SET @IsAdded = 1
	END	
	ELSE
	BEGIN
		SET @IsAdded = 0
	END
	
END

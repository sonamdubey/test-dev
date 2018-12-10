IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Proc_NewPurchaseCarPoll]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Proc_NewPurchaseCarPoll]
GO

	
CREATE PROCEDURE [dbo].[Proc_NewPurchaseCarPoll] 
	@VersionId 	AS Numeric,
	@Price 		AS Numeric,
	@MakeYear 	AS DatEtime,
	@City 		AS VarChar(100),
	@IpAddress	AS VarChar(200),
	@EntryTime	AS DatEtime,
	@CustomerId	AS Numeric
AS
BEGIN
	INSERT INTO NewCarPurchasePoll
		(
			VersionId,		Price,			MakeYear,	
			City,			IpAddress,		EntryTime, 	CustomerId
		)
	VALUES
		(
			@VersionId,		@Price,			@MakeYear,	
			@City,			@IpAddress,		@EntryTime, 	@CustomerId
		)
END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_TokenRequests_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_TokenRequests_SP]
GO

	
CREATE PROCEDURE [dbo].[AE_TokenRequests_SP]
@BidderId Numeric (18, 0),
@NoOfTokens INT,
@TokenRequestId NUMERIC OUTPUT
AS
BEGIN

	INSERT INTO AE_TokenRequests(BidderId, NoOfTokens, EntryDate, Status)	
	VALUES(@BidderId, @NoOfTokens, GETDATE(), '0')
	
	SET @TokenRequestId = SCOPE_IDENTITY()
END

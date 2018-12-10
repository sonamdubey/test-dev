IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UCS_InsertCallsToBuyers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UCS_InsertCallsToBuyers]
GO

	
CREATE PROCEDURE [dbo].[UCS_InsertCallsToBuyers]
@CustomerId NUMERIC (18,0),
@ProfileId VARCHAR(50),
@IsInterested BIT,
@ExcecutiveId NUMERIC(18,0) = 0
AS
BEGIN
	
	INSERT INTO UCS_CallsToBuyers
	(CustomerId,ProfileId,IsInterested,ExcecutiveId,EnteredOn)
	VALUES
	(@CustomerId,@ProfileId,@IsInterested,@ExcecutiveId,GETDATE())

END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Dealer_InsertUpdateAuthorizedNewCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Dealer_InsertUpdateAuthorizedNewCar]
GO

	CREATE PROCEDURE [dbo].[Dealer_InsertUpdateAuthorizedNewCar]

@ID NUMERIC,
@MakeId NUMERIC

AS
	
BEGIN
	if EXISTS (select TOP 1 MakeId from Dealer_AuthorizedNewCar
		where DealerId=@ID)
			BEGIN
				Update Dealer_AuthorizedNewCar 
				Set MakeId = @MakeId 
				Where DealerId = @ID
			END
	ELSE
			BEGIN
				INSERT INTO Dealer_AuthorizedNewCar (DealerId,MakeId) 
				VALUES (@ID,@MakeId)
			END
END

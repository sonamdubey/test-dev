IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_UpdateCWCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_UpdateCWCustomerDetails]
GO

	


CREATE PROCEDURE [dbo].[DLS_UpdateCWCustomerDetails]
	@Id				NUMERIC,
	@Name			VARCHAR(100),
	@Mobile			VARCHAR(20),
	@Phone1			VARCHAR(20),
	@StateId		NUMERIC,
	@CityId			NUMERIC
 AS
	
BEGIN
	UPDATE Customers 
	SET Name = @Name, Mobile = @Mobile, 
		Phone1 = @Phone1, CityId = @CityId,
		StateId = @StateId
	WHERE Id = @Id
END




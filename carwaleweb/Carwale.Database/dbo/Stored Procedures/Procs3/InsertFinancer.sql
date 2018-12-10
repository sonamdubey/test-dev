IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinancer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinancer]
GO

	-- PROCEDURE TO ADD NEW FINANCER

CREATE PROCEDURE [dbo].[InsertFinancer]
	@Id			INT,
	@Name			VARCHAR(100),
	@FinancerLogo 		VARCHAR(10),
	@Status		INTEGER OUTPUT
 AS
	
BEGIN
	IF @Id = -1
	
		BEGIN
			INSERT INTO Financers ( Name, FinancerLogo , isActive )
			VALUES ( @Name , @FinancerLogo, 1)	
			
			SET @Status = 1
		END
	ELSE
		BEGIN
			UPDATE Financers SET Name = @Name, FinancerLogo = @FinancerLogo WHERE Id = @Id

			SET @Status = 2
		END
END
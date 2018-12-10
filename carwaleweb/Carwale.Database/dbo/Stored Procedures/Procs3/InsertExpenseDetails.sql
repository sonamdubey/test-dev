IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertExpenseDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertExpenseDetails]
GO

	-- PROCEDURE TO ADD NEW FINANCER

CREATE PROCEDURE [dbo].[InsertExpenseDetails]
	@Id			NUMERIC,
	@VoucherId		VARCHAR(50),
	@ExpenseDetails	VARCHAR(100),
	@Head 		VARCHAR(100),
	@ExpenseDate		DATETIME,
	@Amount		DECIMAL(18,2),
	@Status		INTEGER OUTPUT
 AS
	
BEGIN
	IF @Id = -1
	
		BEGIN
			INSERT INTO ExpenseDetails (VoucherId, ExpenseDetails ,Head, ExpenseDate, Amount)
			VALUES (@VoucherId, @ExpenseDetails , @Head, @ExpenseDate, @Amount)	
			
			SET @Status = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
			UPDATE ExpenseDetails SET ExpenseDetails = @ExpenseDetails, Head = @Head, ExpenseDate = @ExpenseDate,
				 Amount = @Amount
			 WHERE Id = @Id

			SET @Status = 1
		END
END
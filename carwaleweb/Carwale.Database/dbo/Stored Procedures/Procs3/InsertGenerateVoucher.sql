IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertGenerateVoucher]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertGenerateVoucher]
GO

	-- PROCEDURE TO ADD NEW FINANCER

CREATE PROCEDURE [dbo].[InsertGenerateVoucher]
	@Id			NUMERIC,
	@ExpenseOwner	VARCHAR(100),
	@Place 		VARCHAR(50),
	@VoucherDate		DATETIME,
	@IsCheque		Bit,
	@Status		VARCHAR(50) OUTPUT
 AS

	DECLARE 	@VoucherId 		NUMERIC, 
			@PreVoucherId		NUMERIC,
			@PreVoucherCode	VARCHAR(50),
			@VoucherCode		VARCHAR(50)			

BEGIN
	IF @Id = -1
	
		BEGIN
			INSERT INTO ExpenseVoucher (ExpenseOwner ,Place, VoucherDate, IsCheque )
			VALUES (@ExpenseOwner , @Place, @VoucherDate, @IsCheque)	
			
			SET @VoucherId = SCOPE_IDENTITY() 
			
			IF @IsCheque = 1
				BEGIN
					SELECT @PreVoucherCode = ( SELECT TOP 1 VoucherId  FROM  ExpenseVoucher WHERE IsCheQue = 1 AND ID <>  @VoucherId ORDER BY Id DESC )

					SET @PreVoucherId = Convert(NUMERIC, SUBSTRING(@PreVoucherCode, 4 , LEN(@PreVoucherCode)  - 3 ) ) 
					
					IF @PreVoucherId > 0
						BEGIN
							SET @PreVoucherId = @PreVoucherId + 1
							SET @VoucherCode = 'CHQ' + Convert(VARCHAR, @PreVoucherId, 50)
						END
					ELSE
						BEGIN
							SET @PreVoucherId = @VoucherId
							SET @VoucherCode = 'CHQ' + Convert(VARCHAR, @PreVoucherId, 50)
						END

					UPDATE ExpenseVoucher SET VoucherId = @VoucherCode  WHERE Id = @VoucherId
					
				END
			ELSE
				BEGIN
					SELECT @PreVoucherCode = ( SELECT TOP 1 VoucherId  FROM  ExpenseVoucher WHERE IsCheQue = 0   AND ID <>  @VoucherId ORDER BY Id DESC )
					
					SET @PreVoucherId = Convert(NUMERIC, SUBSTRING(@PreVoucherCode, 4 , LEN(@PreVoucherCode)  - 3) ) 
					
					IF @PreVoucherId > 0
						BEGIN
							SET @PreVoucherId = @PreVoucherId + 1
							SET @VoucherCode = 'CSH' + Convert(VARCHAR, @PreVoucherId, 50)
						END
					ELSE
						BEGIN
							SET @PreVoucherId = @VoucherId
							SET @VoucherCode = 'CSH' + Convert(VARCHAR, @PreVoucherId, 50)
						END	

					UPDATE ExpenseVoucher SET VoucherId = @VoucherCode  WHERE Id = @VoucherId
				END
			SET @Status = @VoucherCode
		END
	ELSE
		BEGIN
			UPDATE ExpenseVoucher SET ExpenseOwner = @ExpenseOwner, Place = @Place, VoucherDate = @VoucherDate 
			 WHERE Id = @Id

			SET @Status = 2
		END
END
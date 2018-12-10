IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddLDBankcarLoan]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddLDBankcarLoan]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[NCS_AddLDBankcarLoan]
	@Id				NUMERIC,
	@LoanId			NUMERIC,
	@BankId			NUMERIC,
	@ReferenceNo    VARCHAR(100),
	@Status			SMALLINT,
	@Reason			VARCHAR(1000),
	@ForwardedDate	DATETIME,
	@StatusChangeDate DATETIME
AS
	
BEGIN
	IF @Id = -1
		BEGIN

			INSERT INTO LDBankCarLoanForward(LoanId, BankId, ReferenceNo, ForwardedDate)			
			VALUES(@LoanId, @BankId, @ReferenceNo, @ForwardedDate)	

			UPDATE TempBankCarLoan SET IsCompleted = 1 WHERE Id = @LoanId
		END

	ELSE
		BEGIN
			UPDATE LDBankCarLoanForward SET Status = @Status, Reason = @Reason,
					StatusChangeDate = @StatusChangeDate
			WHERE LoanId = @Id
		END
END



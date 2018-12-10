IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_TokenPayments_Manage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_TokenPayments_Manage]
GO

	

CREATE PROCEDURE [dbo].[AE_TokenPayments_Manage]  
@Id numeric(18, 0) = NULL,
@BidderId numeric(18, 0) = NULL,  
@PaymentMode smallint = NULL,  
@ChequeDDNo varchar(20) = NULL,   
@ChequeDDDate datetime = NULL,  
@PayableAt varchar(50) = NULL,   
@BankName varchar(50) = NULL,   
@Amount decimal(18, 2) = NULL,   
@NoOfTokens int = NULL,   
@Status smallint = NULL,   
@EntryDate datetime = NULL,  
@StatusActionDate datetime = NULL,   
@UpdatedBy numeric(18, 0) = NULL,
@RequestId AS NUMERIC(18,0) = -1  
AS  
BEGIN  

 IF @Id = -1 
	 BEGIN
	 
		INSERT INTO AE_TokenPayments  
		(BidderId, PaymentMode, ChequeDDNo, ChequeDDDate, PayableAt,  
		 BankName, Amount, NoOfTokens, [Status], EntryDate, StatusActionDate, UpdatedBy, RequestId  
		)  
		VALUES  
		(@BidderId, @PaymentMode, @ChequeDDNo, @ChequeDDDate, @PayableAt,  
		 @BankName, @Amount, @NoOfTokens, @Status, @EntryDate, @StatusActionDate, @UpdatedBy, @RequestId  
		)
	 
	 END 
 
 ELSE
	 BEGIN
		
		UPDATE AE_TokenPayments
		SET
		PaymentMode = @PaymentMode,
		ChequeDDNo = @ChequeDDNo,
		ChequeDDDate = @ChequeDDDate,
		PayableAt = @PayableAt,
		BankName = @BankName,
		Amount = @Amount,
		NoOfTokens = @NoOfTokens,
		UpdatedOn = GETDATE(),
		UpdatedBy = @UpdatedBy,
		RequestId = @RequestId
		WHERE
		Id = @Id
		
	 END
 
END





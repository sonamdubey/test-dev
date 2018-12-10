IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_PaymentCredits_Manage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_PaymentCredits_Manage]
GO

	

CREATE PROCEDURE [dbo].[AE_PaymentCredits_Manage]  
@Id numeric(18, 0) = NULL,
@BidderId numeric(18, 0) = NULL,  
@PaymentMode smallint = NULL,  
@ChequeDDNo varchar(20) = NULL,   
@ChequeDDDate datetime = NULL,  
@PayableAt varchar(50) = NULL,   
@BankName varchar(50) = NULL,   
@Amount decimal(18, 2) = NULL,   
@Comments varchar(250) = NULL,   
@EntryDate datetime = NULL,    
@UpdatedBy numeric(18, 0) = NULL  
AS  
BEGIN  

 IF @Id = -1
	 BEGIN
	 
		INSERT INTO AE_PaymentCredits  
		(BidderId, ReceiptNo, PaymentMode, ChequeDDNo, ChequeDDDate, PayableAt,  
		 BankName, Amount, Comments, EntryDate, UpdatedBy  
		)  
		VALUES  
		(@BidderId, @BidderId, @PaymentMode, @ChequeDDNo, @ChequeDDDate, @PayableAt,  
		 @BankName, @Amount, @Comments, @EntryDate, @UpdatedBy  
		)
	 
	 END 
ELSE
	 BEGIN
		
		UPDATE AE_PaymentCredits
		SET
		PaymentMode = @PaymentMode,
		ChequeDDNo = @ChequeDDNo,
		ChequeDDDate = @ChequeDDDate,
		PayableAt = @PayableAt,
		BankName = @BankName,
		Amount = @Amount,
		Comments = @Comments,
		UpdatedOn = GETDATE(),
		UpdatedBy = @UpdatedBy
		WHERE
		Id = @Id
		
	 END
 
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_AtomPGTransactions_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_AtomPGTransactions_Save]
GO

	-- =============================================      
-- Author:  Kritika Choudhary
-- Create date: 18th Nov 2015
-- Description: To Save Atom PG Transactions

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_AtomPGTransactions_Save]  
(      
 @CustomerId INT=NULL, 
 @Id  INT=NULL OUTPUT,      
 @TransacTypId int=NULL,
 @DealerId int=NULL,
 @Amt int=NULL,
 @UserAgent varchar(500)=NULL,
 @IPAddress varchar(150)=NULL,
 @XmlResp varchar(MAX)=NULL,
 @IsPaymentSuccess bit=NULL,
 @IsPaymentDone bit=NULL,
 @PGFormResp varchar(MAX)=NULL,
 @ServiceStatusId tinyint= NULL
 )      
AS      
BEGIN 
  
   -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@Id IS NULL)
 BEGIN     
   INSERT INTO Microsite_AtomPGTransactions(CustId,TransactionTypeId,DealerId,Amount,UserAgent,IPAddress)     
   VALUES(@CustomerId,@TransacTypId,@DealerId,@Amt,@UserAgent,@IPAddress) ;
   SELECT @Id=SCOPE_IDENTITY(); 
 END
 ELSE
BEGIN
	UPDATE Microsite_AtomPGTransactions
	SET XmlResponse  = ISNULL(@XmlResp, XmlResponse), IsPaymentSuccess = ISNULL(@IsPaymentSuccess, IsPaymentSuccess),
	IsPaymentDone = ISNULL(@IsPaymentDone, IsPaymentDone), PGFormResponse  = ISNULL(@PGFormResp, PGFormResponse),ModifiedDate= GETDATE()
	WHERE ID=@Id;
	IF(@IsPaymentSuccess = 1)
	BEGIN
	UPDATE TC_ServiceInquiries
	SET TC_ServiceStatusId  = ISNULL(@ServiceStatusId, TC_ServiceStatusId),ModifiedDate= GETDATE()
	WHERE TC_ServiceInquiriesId = @CustomerId;
	END
END 
END
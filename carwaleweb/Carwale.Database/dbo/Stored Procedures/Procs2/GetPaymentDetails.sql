IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPaymentDetails]
GO

	
-- =============================================  
-- Author:  Vikas  
-- Create date: 08-10-2012  
-- Description: To get the details of the car listed 
-- Modification: @PaymentType = 2 added for free listing on 10/31/2013 by amit verma
-- EXEC GetPaymentDetails 2140,2
-- =============================================  
CREATE PROCEDURE [dbo].[GetPaymentDetails]

 -- Add the parameters for the stored procedure here  
 @InquiryId NUMERIC,
 @PaymentType SMALLINT 
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
       
 IF @PaymentType = 1
  BEGIN  
   SELECT TOP 1         
    PG.ID As TransactionId,  
    'Online' As PaymentMode,  
    PG.EntryDateTime As PaymentDateTime,
    PG.ResponseCode As ResponseCode,
    '' As ChequeNumber,  
    '' As BankName,  
    '' As BranchName,  
    '' As BankCity  
   FROM   
    CustomerSellInquiries CSI   
    --INNER JOIN Cities C On C.Id = CSI.CityId  
    --INNER JOIN vwMMV vw On vw.VersionId = CSI.CarVersionId      
    INNER JOIN PGTransactions PG On PG.CarId = CSI.ID
   WHERE   
    CSI.ID = @InquiryId 
   ORDER BY PG.ID Desc  
  END   
 ELSE IF @PaymentType = 0 
  BEGIN  
   SELECT TOP 1         
    '' As TransactionId,  
    'Cheque/DD' As PaymentMode,  
    CD.EntryDateTime As PaymentDateTime,  
    '' As ResponseCode, 
    CD.ChequeNumber As ChequeNumber,  
    CD.BankName As BankName,  
    CD.BranchName As BranchName,  
    CD.BankCity As BankCity  
   FROM   
    CustomerSellInquiries CSI   
    --INNER JOIN Cities C On C.Id = CSI.CityId  
    --INNER JOIN vwMMV vw On vw.VersionId = CSI.CarVersionId      
    INNER JOIN CDTransactions CD ON CD.CarId = CSI.ID  
   WHERE   
    CSI.ID = @InquiryId
   ORDER BY CD.ID Desc   
  END
 ELSE IF @PaymentType = 2  --added for free listing 10/31/2013
  BEGIN  
   SELECT TOP 1         
    '' As TransactionId,  
    'Not Applicable' As PaymentMode,  
    '' As PaymentDateTime,  
    '' As ResponseCode, 
    '' As ChequeNumber,  
    '' As BankName,  
    '' As BranchName,  
    '' As BankCity  
   FROM   
    CustomerSellInquiries CSI   
    --INNER JOIN Cities C On C.Id = CSI.CityId  
    --INNER JOIN vwMMV vw On vw.VersionId = CSI.CarVersionId      
    --INNER JOIN CDTransactions CD ON CD.CarId = CSI.ID  
   WHERE   
    CSI.ID = @InquiryId
	ORDER BY CSI.ID Desc   
  END 
END  
  
  



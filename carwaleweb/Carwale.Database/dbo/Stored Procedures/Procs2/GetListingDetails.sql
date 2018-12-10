IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetListingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetListingDetails]
GO

	  
  
-- =============================================  
-- Author:  Vikas  
-- Create date: 08-10-2012  
-- Description: To get the details of the car listed  
-- EXEC GetListingDetails 974  
-- =============================================  
CREATE PROCEDURE [dbo].[GetListingDetails]   
 -- Add the parameters for the stored procedure here  
 @InquiryId NUMERIC  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
     
    DECLARE @ID NUMERIC  
 SELECT @ID = (SELECT Top 1 ID From PGTransactions Where CarId = @InquiryId And ResponseCode IS NOT NULL Order By ID Desc)  
     
 IF @ID IS NOT NULL   
  BEGIN  
   SELECT      
    DATENAME(month, MakeYear) As ModelMonth,   
    YEAR(MakeYear) As ModelYear,   
    Price,   
    C.Name As CityName,   
    (vw.Make + ' ' + vw.Model + ' ' + vw.Version) As CarName,  
    PG.ID As TransactionId,  
    'Online' As PaymentMode,  
    PG.EntryDateTime As PaymentDateTime,  
    '' As ChequeNumber,  
    '' As BankName,  
    '' As BranchName,  
    '' As BankCity  
   FROM   
    CustomerSellInquiries CSI   
    INNER JOIN Cities C On C.Id = CSI.CityId  
    INNER JOIN vwMMV vw On vw.VersionId = CSI.CarVersionId      
    INNER JOIN PGTransactions PG On PG.CarId = CSI.ID And PG.TransactionCompleted = 1  
   WHERE   
    CSI.ID = @InquiryId  
  END   
 ELSE  
  BEGIN  
   SELECT TOP 1     
    DATENAME(month, MakeYear) As ModelMonth,   
    YEAR(MakeYear) As ModelYear,   
    Price,   
    C.Name As CityName,   
    (vw.Make + ' ' + vw.Model + ' ' + vw.Version) As CarName,  
    '' As TransactionId,  
    'Cheque/DD' As PaymentMode,  
    CD.EntryDateTime As PaymentDateTime,  
    CD.ChequeNumber As ChequeNumber,  
    CD.BankName As BankName,  
    CD.BranchName As BranchName,  
    CD.BankCity As BankCity  
   FROM   
    CustomerSellInquiries CSI   
    INNER JOIN Cities C On C.Id = CSI.CityId  
    INNER JOIN vwMMV vw On vw.VersionId = CSI.CarVersionId      
    INNER JOIN CDTransactions CD ON CD.CarId = CSI.ID  
   WHERE   
    CSI.ID = @InquiryId
   ORDER BY CD.ID Desc   
  END  
END  
  
  
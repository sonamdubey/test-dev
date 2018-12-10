IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_RestricBuyer_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_RestricBuyer_SP]
GO

	-- =============================================  
-- Author:  Satish Sharma  
-- Create date: 27/04/2011  
-- Description: SP to restrict user car buyer for viewing number of inquiries.   
--    Currently the limit is 10 in a day(24 hrs)
-- Corrected ViewCount as 1 for first time entry for date  
-- AM Modified 4 June 2012 to use MobileNo instead of BuyerId to restruct inquires from multile Mobile and email combination  
--Modified by Aditi dhaybar on 23rd december 2014 (Increased the maximum limit to 20)
-- =============================================  
  
CREATE PROCEDURE [dbo].[Classified_RestricBuyer_SP]  
 -- Add the parameters for the stored procedure here  
 @RequestDate DATE,  
 @Mobile  CHAR(10),
 @Status   BIT OUTPUT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
 -- by default status will be false, set it to true in case of update and insert command  
 SET @Status = 0  
   
 DECLARE @InquiryViewedCount SMALLINT   
 SET @InquiryViewedCount = (SELECT InquiryViewedCount FROM Classified_RestrictBuyer WHERE RequestDate = @RequestDate AND MobileNo = @Mobile  )  
   
 IF @InquiryViewedCount < 20  --Modified by Aditi dhaybar on 23rd december 2014 (Increased the maximum limit to 20)
 BEGIN  
  UPDATE Classified_RestrictBuyer      
  SET InquiryViewedCount = InquiryViewedCount + 1   
  WHERE RequestDate = @RequestDate AND  MobileNo = @Mobile  
    
  SET @Status = 1  
 END  
 ELSE IF @InquiryViewedCount IS NULL  
 BEGIN  
  INSERT INTO Classified_RestrictBuyer(RequestDate, MobileNo, InquiryViewedCount)  
  VALUES(@RequestDate, @Mobile, 1)  
    
  SET @Status = 1  
 END  
END 



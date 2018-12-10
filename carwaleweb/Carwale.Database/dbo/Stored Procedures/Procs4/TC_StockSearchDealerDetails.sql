IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockSearchDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockSearchDealerDetails]
GO

	-- =============================================  
-- Author:  Tejashree Patil  
-- Create date: 6 Sept 2012  
-- Description: Get dealer details for stock search  
-- exec TC_StockSearchDealerDetails 678,968  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_StockSearchDealerDetails]  
 -- Add the parameters for the stored procedure here  
 @StockId BIGINT,  
 @BranchId BIGINT  
AS  
BEGIN  
	 -- SET NOCOUNT ON added to prevent extra result sets from  
	 -- interfering with SELECT statements.  
	 SET NOCOUNT ON;  
	 IF NOT EXISTS (SELECT TOP 1 TC_StockSearchId FROM TC_StockSearch SS WITH(NOLOCK)WHERE TC_StockId=@StockId AND DealerId = @BranchId)  
	 BEGIN  
	  INSERT INTO TC_StockSearch (TC_StockId,DealerId,EntryDate) VALUES (@StockId,@BranchId,GETDATE())            
	 END  
	   
	 SELECT D.Organization AS SellerName ,  
		 D.ContactPerson AS ContactPerson ,   
		 D.EmailId AS Email,   
		 (CASE WHEN D.MobileNo='' THEN '' ELSE D.MobileNo+', ' END+ D.PhoneNo )  AS Contact,   
		 D.Address1+ ' '+D.Pincode AS Address  
	 FROM   Dealers D WITH(NOLOCK)   
		 INNER JOIN TC_Stock ST WITH(NOLOCK)   
			  ON ST.BranchId=D.ID  
	 WHERE  ST.Id=@StockId   
		 AND BranchId=ST.BranchId      
END  



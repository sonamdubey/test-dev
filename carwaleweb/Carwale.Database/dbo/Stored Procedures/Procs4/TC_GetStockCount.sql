IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockCount]
GO

	-- Created by: Surendra  
-- Created date: 10-07-2011  
-- Description: Checking user for andriod application  

-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetStockCount]    
(  
 @BranchId  VARCHAR(100) --User UniqueId   
)  
AS  
     
BEGIN    
        SET NOCOUNT ON;   
        SELECT COUNT(Id) as StocksCount FROM TC_Stock 
        WHERE BranchId = @BranchId AND IsActive = 1 AND StatusId = 1 AND IsApproved=1
    
END
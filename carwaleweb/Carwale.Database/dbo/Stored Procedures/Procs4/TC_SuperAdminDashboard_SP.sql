IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SuperAdminDashboard_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SuperAdminDashboard_SP]
GO

	  
  
  
-- Author:  TEJASHREE PATIL  
-- Create date: 18 May 2012  
-- Description: Condition for displaying Approved stocks(Default IsApproved=1).  
-- Create date: 18 April 2012  
-- Description: Condition for displaying Active stocks on Outlet Page.  
-- =============================================  
-- Author:  SURENDRA CHOUKSEY  
-- Create date: 6TH JULY 2011  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_SuperAdminDashboard_SP]  
 -- Add the parameters for the stored procedure here  
 @DealerAdminId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    select DB.Id as BranchId,DB.Organization 'Branch',SUM(SA.CWResponseCount) 'CarwaleInquiries',  
 SUM(SA.CWResponseCount+ SA.TCResponseCount) 'TotalInquires',COUNT(TS.ID) 'TotalStock'  
 from Dealers DB LEFT JOIN Tc_Stock TS ON DB.Id=TS.BranchId AND TS.StatusId=1 AND (IsActive=1 AND IsApproved=1)  
 LEFT JOIN TC_StockAnalysis SA ON TS.Id =SA.StockId  
 WHERE DB.IsTCDealer=1 and DB.IsDealerActive=1 and  DB.ID IN(SELECT DealerId FROM TC_DealerAdminMapping WHERE DealerAdminId=@DealerAdminId)  
 Group By DB.Id,DB.Organization  
END  
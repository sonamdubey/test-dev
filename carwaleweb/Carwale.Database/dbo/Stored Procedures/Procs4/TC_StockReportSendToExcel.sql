IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockReportSendToExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockReportSendToExcel]
GO

	 -- =============================================
-- Author:		Suri
-- Create date: 28th Dec, 2012
-- Description:	Used in Report section of Stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockReportSendToExcel]
(
@BranchId BIGINT,
@StatusId INT
)
AS
BEGIN
 
IF(@StatusId=99)
BEGIN
 
	 SELECT  
	  (MA.Name + ' ' + MO.Name + ' ' + CV.Name) As CarName, TSA.CWResponseCount, TSA.TCResponseCount, SI.[ViewCount],
	   DATEDIFF(DD,TS.EntryDate,GETDATE()) AS StockAge, TS.Id AS StockId, TS.RegNo AS RegNo  
	   FROM  TC_Stock TS  WITH(NOLOCK)
	   Inner Join TC_StockAnalysis TSA WITH(NOLOCK) ON TSA.StockId = TS.Id  
	   Left Join SellInquiries SI WITH(NOLOCK)ON SI.TC_StockId = TS.Id  
	   Inner Join TC_StockStatus TSS WITH(NOLOCK) ON TSS.Id = TS.StatusId  
	   Inner Join CarVersions CV WITH(NOLOCK) ON CV.ID = TS.VersionId  
	   Inner Join CarModels MO WITH(NOLOCK) ON MO.ID = CV.CarModelId  
	   Inner Join CarMakes MA WITH(NOLOCK) ON MA.ID = MO.CarMakeId   
	   WHERE  (TS.IsActive=1 AND TS.IsApproved=1)  
		AND TS.BranchId = @BranchId AND TS.StatusId = 1 AND TS.IsSychronizedCW=1 
END
ELSE
BEGIN
	 SELECT  
	  (MA.Name + ' ' + MO.Name + ' ' + CV.Name) As CarName, TSA.CWResponseCount, TSA.TCResponseCount, SI.[ViewCount],
	   DATEDIFF(DD,TS.EntryDate,GETDATE()) AS StockAge, TS.Id AS StockId, TS.RegNo AS RegNo  
	   FROM  TC_Stock TS  WITH(NOLOCK)
	   Inner Join TC_StockAnalysis TSA WITH(NOLOCK) ON TSA.StockId = TS.Id  
	   Left Join SellInquiries SI WITH(NOLOCK)ON SI.TC_StockId = TS.Id  
	   Inner Join TC_StockStatus TSS WITH(NOLOCK) ON TSS.Id = TS.StatusId  
	   Inner Join CarVersions CV WITH(NOLOCK) ON CV.ID = TS.VersionId  
	   Inner Join CarModels MO WITH(NOLOCK) ON MO.ID = CV.CarModelId  
	   Inner Join CarMakes MA WITH(NOLOCK) ON MA.ID = MO.CarMakeId   
	   WHERE  (TS.IsActive=1 AND TS.IsApproved=1)  
		AND TS.BranchId = @BranchId AND TS.StatusId = @StatusId 
END	
	
END 


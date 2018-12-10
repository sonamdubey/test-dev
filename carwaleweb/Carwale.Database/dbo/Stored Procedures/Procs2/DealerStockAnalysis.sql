IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerStockAnalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerStockAnalysis]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 2-8-2012
-- Description:	Dealer Stock analysis
-- This SP run as a SQL job to capture Dealer wise stock , stock details like no of photos, comments, KM, Price and responses on them
--EXEC DealerStockAnalysis
-- Modified By Reshma 31-10-2012 To get Response count as per dealers only
-- Modified by Deepak Tripthi 15 Nov 2012 - Changed GetDate to GETDATE() - 1 and scheduled the job at 12:05 AM Every Day instead of 10:30 PM
-- =============================================
CREATE PROCEDURE [dbo].[DealerStockAnalysis]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #tempStock
	(
	   DealerId INT,
	   Entrydate DATE DEFAULT(GETDATE()-1),
	   CWCount INT,
	   Stockcount INT,
	   Response INT,
	   PhotoCount INT,
	   ResponsePhotoCount INT,
	   NoPhotoCount INT,
	   ResponseNoPhotoCount INT,
	   DescrCount INT,
	   ResponseDescrCount INT,	
	   NoDescrCount INT,
	   ResponseNoDescrCount INT,
	   OverAgeCount INT,
	   ResponseOverAgeCount INT,  
	   OverAgeNoCount INT,
	   ResponseNoOverAgeCount INT,
	   OverKMCount INT,
	   ResponseOverKMCount INT,	
	   OverNoKMCount INT,
	   ResponseNoOverKMCount INT,
	   OverPriceCount INT,
	   ResponseOverPriceCount INT,	 
	   OverNoPriceCount INT,
	   ResponseNoOverPriceCount INT,
	   StandardCount INT,
	   ResponseStandardCount INT,	 
	   NoStandardCount INT,
	   ResponseNoStandardCount INT 	 
	)
	
	CREATE TABLE #tempPhotoStock
	(
	   DealerId INT,
	   PhotoCount INT,
	   ResponsePhotoCount INT,
	   NoPhotoCount INT,
	   ResponseNoPhotoCount INT,	   	   
	   Entrydate DATE DEFAULT(GETDATE()-1)
	)
	
	CREATE TABLE #tempDescrStock
	(
	   DealerId INT,
	   DescrCount INT,
	   ResponseDescrCount INT,	   
	   Entrydate DATE DEFAULT(GETDATE()-1)
	)
	
	--CREATE TABLE #tempPhotoDescrStock
	--(
	--   DealerId INT,
	--   StockCount INT,
	--   ResponseCount INT,	   
	--   Entrydate DATE DEFAULT(GETDATE()-1)
	--)
	
	--CREATE TABLE #tempOverAgeStock
	--(
	--   DealerId INT,
	--   OverAgeCount INT,
	--   ResponseOverAgeCount INT,	   
	--   Entrydate DATE DEFAULT(GETDATE()-1)
	--)
	
	--CREATE TABLE #tempOverKMStock
	--(
	--   DealerId INT,
	--   OverKMCount INT,
	--   ResponseOverKMCount INT,	   
	--   Entrydate DATE DEFAULT(GETDATE()-1)
	--)
	
	--CREATE TABLE #tempOverPriceStock
	--(
	--   DealerId INT,
	--   OverPriceCount INT,
	--   ResponseOverPriceCount INT,	   
	--   Entrydate DATE DEFAULT(GETDATE()-1)
	--)
	
	--CREATE TABLE #tempStandardStock
	--(
	--   DealerId INT,
	--   StandardCount INT,
	--   ResponseStandardCount INT,	   
	--   Entrydate DATE DEFAULT(GETDATE()-1)
	--)
	
	-- Modified By Reshma 31-10-2012 To get Response count as per dealers only
		INSERT INTO #tempStock(DealerId,CWCount,Stockcount,Response)
    SELECT  S.DealerId ,
           COUNT(DISTINCT S.Id) AS CWCount,  
            COUNT(DISTINCT TS.Id) AS Stockcount,
            CNT AS Response
    FROM   SellInquiries AS S WITH(NOLOCK)
           INNER JOIN LiveListings AS L WITH(NOLOCK) ON L.Inquiryid=S.ID AND L.SellerType=1
           LEFT JOIN  TC_Stock AS TS WITH(NOLOCK) ON S.DealerId=TS.BranchId AND TS.StatusId = 1 AND TS.IsApproved = 1 AND TS.IsActive = 1 
		   LEFT JOIN (  SELECT SI.DealerId,COUNT(UCP.Id) CNT 
						FROM SellInquiries SI
						INNER JOIN UsedCarPurchaseInquiries UCP ON UCP.SellInquiryId=SI.ID
						WHERE CONVERT(DATE,UCP.RequestDateTime)=CONVERT(DATE,GETDATE()-1)
						GROUP BY SI.DealerId) AS U ON U.DealerId=S.DealerId  
    WHERE --(S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1)))AND 
          CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)    
    GROUP  BY S.DealerId,CNT
    
    -- Modified By Reshma 31-10-2012 To get Response count as per dealers only
    INSERT INTO #tempPhotoStock(DealerId,PhotoCount,ResponsePhotoCount)
    SELECT  S.DealerId, 
             COUNT(DISTINCT C.InquiryId) AS PhotoCount,
              CNT AS ResponsePhotoCount           
    FROM   SellInquiries AS S WITH(NOLOCK)
           INNER JOIN LiveListings AS L WITH(NOLOCK) ON L.Inquiryid=S.ID and L.SellerType=1 
           INNER JOIN CarPhotos AS C WITH(NOLOCK) ON C.InquiryId=S.ID and C.IsDealer=1 
		   LEFT JOIN (  SELECT SI.DealerId,COUNT(UCP.Id) CNT 
						FROM SellInquiries SI
						INNER JOIN UsedCarPurchaseInquiries UCP ON UCP.SellInquiryId=SI.ID
						WHERE CONVERT(DATE,UCP.RequestDateTime)=CONVERT(DATE,GETDATE()-1)
						GROUP BY SI.DealerId) AS U ON U.DealerId=S.DealerId         WHERE --(S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1)))AND 
               CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)    
    GROUP  BY S.DealerId,CNT
    
    UPDATE #tempStock
    set PhotoCount=p.PhotoCount,
        ResponsePhotoCount=p.ResponsePhotoCount
    from #tempStock as t
        join #tempPhotoStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
    UPDATE t1
    set NoPhotoCount=t1.CWCount-ISNULL(t1.PhotoCount,0),
        ResponseNoPhotoCount= t1.Response-ISNULL(t1.ResponsePhotoCount,0)
    from #tempStock as t1
         --join #tempStock as t2 on t1.DealerId=t2.DealerId --and t1.Entrydate=t2.Entrydate
         
    
    -- Modified By Reshma 31-10-2012 To get Response count as per dealers only
    INSERT INTO #tempDescrStock(DealerId,DescrCount,ResponseDescrCount)
    SELECT  S.DealerId, 
            COUNT(DISTINCT S.Id) AS DescrCount ,
            CNT AS ResponseDescrCount           
    FROM   SellInquiries as S with(nolock)
           JOIN LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1
		   LEFT JOIN (  SELECT SI.DealerId,COUNT(UCP.Id) CNT 
						FROM SellInquiries SI
						INNER JOIN UsedCarPurchaseInquiries UCP ON UCP.SellInquiryId=SI.ID
						WHERE CONVERT(DATE,UCP.RequestDateTime)=CONVERT(DATE,GETDATE()-1)
						GROUP BY SI.DealerId) AS U ON U.DealerId=S.DealerId     WHERE --(S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1)))    
           LEN(s.Comments)>0  AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)    
    GROUP  BY S.DealerId,CNT 
    
    
    
    UPDATE #tempStock
    set DescrCount=p.DescrCount,
        ResponseDescrCount=p.ResponseDescrCount
    from #tempStock as t
        join #tempDescrStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
   UPDATE t1
    set NoDescrCount=t1.CWCount-isnull(t1.DescrCount,0),
        ResponseNoDescrCount= t1.Response-isnull(t1.ResponseDescrCount,0)
    from #tempStock as t1
         --join #tempStock as t2 on t1.DealerId=t2.DealerId and t1.Entrydate=t2.Entrydate
         
   
         
         
 --   INSERT INTO #tempOverAgeStock(DealerId,OverAgeCount,ResponseOverAgeCount)
 --    SELECT  DealerId, 
 --           COUNT(DISTINCT S.Id) OverAgeCount ,
 --           COUNT(DISTINCT U.Id) AS ResponseOverAgeCountCount       
 --   FROM   carwale_com..SellInquiries as S with(nolock)
 --          JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1
 --          Join carwale_com..UsedCarPurchaseInquiries as U with(nolock) on U.SellInquiryId=S.ID 
 --   WHERE --(S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND 
	--		CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)       
 --   GROUP  BY DealerId
    
 --   UPDATE #tempStock
 --   set OverAgeCount=p.OverAgeCount,
 --       ResponseOverAgeCount=p.ResponseOverAgeCount
 --   from #tempStock as t
 --       join #tempOverAgeStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
 --   UPDATE t1
 --   set OverAgeNoCount=t1.CWCount-isnull(t1.OverAgeCount,0),
 --       ResponseNoOverAgeCount= t1.Response-isnull(t1.ResponseOverAgeCount,0)
 --   from #tempStock as t1
 --        --join #tempStock as t2 on t1.DealerId=t2.DealerId and t1.Entrydate=t2.Entrydate
    
       
 --   INSERT INTO #tempOverKMStock(DealerId,OverKMCount,ResponseOverKMCount)
 --    SELECT DealerId,SUM(OverKM) as OverKM,SUM(ResponseOverKMCountCount) AS ResponseOverKMCountCount
	-- FROM (
	-- SELECT  DealerId, 
	--		   sum(case DCO.ORKm when 1 then 1 else 0 end) as OverKM, 0 as ResponseOverKMCountCount			    
	--	FROM   carwale_com..SellInquiries as S with(nolock)
	--		   JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1			  
	--		   Left Outer JOIN carwale_com..DCRM_CarsORData as DCO with(nolock) on S.ID = DCO.InquiryId and DCO.ORKm=1	
	--    --WHERE (S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)
	--	GROUP  BY DealerId
	-- UNION
	--  SELECT  DealerId, 			  
	--		   0 as OverKM,  COUNT(DISTINCT U.Id) AS ResponseOverKMCountCount       
	--	FROM   carwale_com..SellInquiries as S with(nolock)
	--		   JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1
	--		   Join carwale_com..UsedCarPurchaseInquiries as U with(nolock) on U.SellInquiryId=S.ID
	--    --WHERE (S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)
	--	GROUP  BY DealerId 
	--	)A
	--   GROUP  BY DealerId 
	   
   
    
 --   UPDATE #tempStock
 --   set OverKMCount=p.OverKMCount,
 --       ResponseOverKMCount=p.ResponseOverKMCount
 --   from #tempStock as t
 --       join #tempOverKMStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
 --   UPDATE t1
 --   set OverNoKMCount=t1.CWCount-isnull(t1.OverKMCount,0),
 --       ResponseNoOverKMCount= t1.Response-isnull(t1.ResponseOverKMCount,0)
 --   from #tempStock as t1
 --        --join #tempStock as t2 on t1.DealerId=t2.DealerId and t1.Entrydate=t2.Entrydate
         
 --   INSERT INTO #tempOverPriceStock(DealerId,OverPriceCount,ResponseOverPriceCount)
 --    SELECT DealerId,SUM(ORPrice) as ORPrice,SUM(ResponseOverPriceCountCount) AS ResponseOverPriceCountCount
	-- FROM (
	-- SELECT  DealerId, 
	--		   sum(case DCO.ORPrice when 1 then 1 else 0 end) as ORPrice,  0 as ResponseOverPriceCountCount
	-- FROM   carwale_com..SellInquiries as S with(nolock)
	--		  JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1          
	--		   Left Outer JOIN carwale_com..DCRM_CarsORData as DCO with(nolock) on S.ID = DCO.InquiryId and DCO.ORPrice=1
	----WHERE (S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)
	--GROUP  BY DealerId
	 
	-- UNION
	--SELECT  DealerId,        
	--	   0 as ORPrice,  COUNT(DISTINCT U.Id) AS ResponseOverPriceCountCount       
	--FROM   carwale_com..SellInquiries as S with(nolock)
	--	   JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1
	--	   Join carwale_com..UsedCarPurchaseInquiries as U with(nolock) on U.SellInquiryId=S.ID       
	----WHERE (S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)
	--GROUP  BY DealerId 
	--)A
	--GROUP  BY DealerId 
	--ORDER BY  DealerId     
   
    
 --   UPDATE #tempStock
 --   set OverPriceCount=p.OverPriceCount,
 --       ResponseOverKMCount=p.ResponseOverPriceCount
 --   from #tempStock as t
 --       join #tempOverPriceStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
 --   UPDATE t1
 --   set OverNoPriceCount=t1.CWCount-isnull(t1.OverPriceCount,0),
 --       ResponseNoOverPriceCount= t1.Response-isnull(t1.ResponseOverPriceCount,0)
 --   from #tempStock as t1
 --        --join #tempStock as t2 on t1.DealerId=t2.DealerId and t1.Entrydate=t2.Entrydate    
   
    
 --   INSERT INTO #tempStandardStock(DealerId,StandardCount,ResponseStandardCount)
 --   SELECT  DealerId, 
	--		 COUNT(DISTINCT S.Id) StandardCount ,
	--		 COUNT(DISTINCT U.Id) AS ResponseStandardCount       
	-- FROM   carwale_com..SellInquiries as S with(nolock)
	--		   JOIN carwale_com..LiveListings as L with(nolock) on L.Inquiryid=S.ID and L.SellerType=1 
	--		   Join carwale_com..UsedCarPurchaseInquiries as U with(nolock) on U.SellInquiryId=S.ID   
	--		   Join carwale_com..CarPhotos as C with(nolock) on C.InquiryId=S.ID and C.IsDealer=1      
	--		   Left Outer JOIN carwale_com..DCRM_CarsORData as DCO with(nolock) on S.ID = DCO.InquiryId and (DCO.ORPrice=1 or DCO.ORKm=1) and DCO.Id is null
	--WHERE  
	-- len(s.Comments)>0 --AND ( S.StatusId=1 OR (S.StatusId=2 AND CONVERT(DATE,S.LastUpdated)=CONVERT(DATE,GETDATE()-1))) AND CONVERT(DATE,S.PackageExpiryDate)>=CONVERT(DATE,GETDATE()-1)
	
	--GROUP  BY DealerId
	--order by DealerId
    
 --   UPDATE #tempStock
 --   set StandardCount=p.StandardCount,
 --       ResponseStandardCount=p.ResponseStandardCount
 --   from #tempStock as t
 --       join #tempStandardStock as p on t.DealerId=p.DealerId --and t.Entrydate=p.Entrydate
        
 --   UPDATE t1
 --   set NoStandardCount=t1.CWCount-isnull(t1.StandardCount,0),
 --       ResponseNoStandardCount= t1.Response-isnull(t1.ResponseStandardCount,0)
 --   from #tempStock as t1
 --        --join #tempStock as t2 on t1.DealerId=t2.DealerId and t1.Entrydate=t2.Entrydate
    
    INSERT INTO DealerStockResponseAnalysis
		(
				DealerId	,
				Entrydate	,
				CWStockCount	,
				TCStockcount	,
				Response	,
				PhotoCount	,
				ResponsePhotoCount	,
				NoPhotoCount	,
				ResponseNoPhotoCount	,
				DescrCount	,
				ResponseDescrCount	,
				NoDescrCount	,
				ResponseNoDescrCount	,
				OverAgeCount	,
				ResponseOverAgeCount	,
				OverAgeNoCount	,
				ResponseNoOverAgeCount	,
				OverKMCount	,
				ResponseOverKMCount	,
				OverNoKMCount	,
				ResponseNoOverKMCount	,
				OverPriceCount	,
				ResponseOverPriceCount	,
				OverNoPriceCount	,
				ResponseNoOverPriceCount,
				StandardStock,
                ResponseStandardStock,
                NoStandardStock,
                ResponseNoStandardStock	
		)
    SELECT  DealerId	,
			Entrydate	,
			CWCount	,
			Stockcount	,
			Response	,
			PhotoCount	,
			ResponsePhotoCount	,
			NoPhotoCount	,
			ResponseNoPhotoCount	,
			DescrCount	,
			ResponseDescrCount	,
			NoDescrCount	,
			ResponseNoDescrCount	,
			OverAgeCount	,
			ResponseOverAgeCount	,
			OverAgeNoCount	,
			ResponseNoOverAgeCount	,
			OverKMCount	,
			ResponseOverKMCount	,
			OverNoKMCount	,
			ResponseNoOverKMCount	,
			OverPriceCount	,
			ResponseOverPriceCount	,
			OverNoPriceCount	,
			ResponseNoOverPriceCount,
			StandardCount ,
			ResponseStandardCount,
			NoStandardCount ,
			ResponseNoStandardCount	 
    FROM #tempStock order by DealerId desc
   
    
    
    DROP TABLE #tempStock
    DROP TABLE #tempPhotoStock
    DROP TABLE #tempDescrStock
    --DROP TABLE #tempPhotoDescrStock   
    --DROP TABLE #tempOverKMStock
    --DROP TABLE #tempOverPriceStock
    --DROP TABLE #tempStandardStock
    
END

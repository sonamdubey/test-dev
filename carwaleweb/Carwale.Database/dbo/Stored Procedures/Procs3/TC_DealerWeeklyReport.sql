IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerWeeklyReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerWeeklyReport]
GO

	-- =============================================
-- Author:		Khushaboo Patil
-- Create date: 4/06/2015
-- Description:	Weekly Dealer Data
-- Modified By : Khushaboo Patil on 14 Jul added condition to get new car dealers having running pkg and for ucd get dealers who have taken any stock related actions
-- Modified By : Tejashree Patil on 25 Aug 2016 Fetched SoldToCustomerFrom from CarWale only.
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerWeeklyReport]
AS
BEGIN
	DECLARE @FromDate DATETIME =  DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()-6), 0) ;  
	DECLARE @ToDate   DATETIME =  DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0);

	WITH CTE AS
	(
		--SELECT ID AS DId,TC_DealerTypeId,Organization,EmailId,IsTCDealer,IsDealerActive FROM Dealers WITH(NOLOCK) WHERE IsTCDealer = 1 AND IsDealerActive = 1 AND ApplicationId = 1
		-- Modified By : Khushaboo Patil on 14 Jul added condition to get new car dealers having running pkg
		SELECT DISTINCT ID AS DId,TC_DealerTypeId,Organization,EmailId,IsTCDealer,IsDealerActive 
		FROM Dealers D WITH(NOLOCK) 
		LEFT JOIN RVN_DealerPackageFeatures DP WITH(NOLOCK) ON D.ID = DP.DealerId AND TC_DealerTypeId = 2 AND DP.PackageId IN(SELECT Id FROM Packages  WITH(NOLOCK) WHERE INQptCategoryId IN( 24, 27) )
		AND PackageEndDate >= GETDATE() AND PackageStatus = 2
		WHERE IsTCDealer = 1 AND IsDealerActive = 1 AND ApplicationId = 1  AND D.ID = CASE WHEN  TC_DealerTypeId = 2 THEN DP.DealerId ELSE D.ID END
	),
	CTE1 AS
		(SELECT DealerId , 
		COUNT(DISTINCT Inquiryid) TotalStockCnt , 
		SUM(CASE WHEN PhotoCount =0  THEN 0 ELSE 1 END) AS TotalStockWithPhoto ,
		SUM(CASE WHEN InsertionDate BETWEEN  @FromDate and   @ToDate then 1 else 0 end ) AS WeeklyStockCnt,
		SUM(CASE WHEN  PhotoCount <> 0  AND InsertionDate BETWEEN @FromDate and   @ToDate THEN 1 ELSE 0 END) AS WeeklyStockWithPhoto 
		FROM livelistings LL WITH(NOLOCK)
		WHERE SellerType = 1
		GROUP BY DealerId
	),
	CTE2 AS (
		SELECT BranchId AS BID
		,SUM(CASE WHEN L.LeadCreationDate BETWEEN @FromDate and   @ToDate then 1 else 0 end ) AS WeeklyFreshLeadCnt
		FROM TC_Lead L WITH(NOLOCK)
		GROUP BY BranchId),
	CTE3 AS (
		SELECT L.BranchId 
		,COUNT(distinct C.TC_LeadId) TotalLeadWithFollowupWithInDay
		FROM TC_Lead L WITH(NOLOCK)
		JOIN TC_Calls  AS C WITH(NOLOCK) ON L.TC_LeadId=C.TC_LeadId
		WHERE L.LeadCreationDate BETWEEN @FromDate and  @ToDate
		AND C.ActionTakenOn BETWEEN L.LeadCreationDate And L.LeadCreationDate+1
		AND C.IsActionTaken=1
		GROUP BY L.BranchId),
	CTE4 AS (
		SELECT BranchId AS StockBranchId ,COUNT(S.Id) SoldThroughCW
		FROM TC_Stock S WITH(NOLOCK)
		WHERE S.LastUpdatedDate BETWEEN @FromDate and @ToDate AND StatusId = 3
		AND (SoldToCustomerFrom IS NOT NULL AND SoldToCustomerFrom = 'carwale')-- Modified By : Tejashree Patil on 25 Aug 2016 Fetched SoldToCustomerFrom from CarWale only.
		GROUP BY BranchId
	)


	SELECT DISTINCT *
	FROM CTE  WITH(NOLOCK)
	LEFT JOIN CTE1 WITH(NOLOCK) ON CTE1.DealerId=CTE.DId
	LEFT JOIN CTE2 WITH(NOLOCK)ON CTE.DId=CTE2.BId
	LEFT JOIN CTE3 WITH(NOLOCK) ON CTE.DId=CTE3.BranchId
	LEFT JOIN CTE4 WITH(NOLOCK)ON CTE.DId=CTE4.StockBranchId
	WHERE IsTCDealer = 1 AND IsDealerActive = 1 
	AND (
		ISNULL(CTE1.TotalStockCnt,0) > CASE WHEN CTE.TC_DealerTypeId = 1 OR CTE.TC_DealerTypeId = 3 THEN 0 ELSE -1 END  OR
		ISNULL(CTE1.WeeklyStockCnt,0) > CASE WHEN CTE.TC_DealerTypeId = 1 OR CTE.TC_DealerTypeId = 3 THEN 0 ELSE -1 END OR
		ISNULL(CTE2.WeeklyFreshLeadCnt,0) > CASE WHEN CTE.TC_DealerTypeId = 1 OR CTE.TC_DealerTypeId = 3 THEN 0 ELSE -1 END 
	) -- Modified By : Khushaboo Patil on 14 Jul for ucd get dealers who have taken any stock related actions
	AND CTE.TC_DealerTypeId IN(1,2,3)
	--and cte.DId = 3838
	
	--Source wise
	SELECT * FROM (
	SELECT L.BranchId ,
	S.Source
	,COUNT(L.TC_LeadId) AS WeeklyFreshLeadCnt,ROW_NUMBER() OVER(PARTITION BY L.BranchId ORDER BY COUNT(L.TC_LeadId) DESC) RNO
	FROM TC_Lead L WITH(NOLOCK)
	JOIN TC_InquirySource AS S  WITH(NOLOCK) ON L.TC_InquirySourceId=S.Id
	WHERE L.LeadCreationDate BETWEEN @FromDate and  @ToDate
	GROUP BY L.TC_InquirySourceId,S.Source,L.BranchId
	--ORDER BY BranchId,WeeklyFreshLeadCnt DESC
	)TAB WHERE TAB.RNO <=3 AND TAB.BranchId IS NOT NULL

	--EXECUTIVE

	SELECT U.UserName,IL.BranchId AS BIdofExectble,
	COUNT(DISTINCT(CASE WHEN IL.TC_LeadStageId = 1 AND S.TC_LeadDispositionId IS NULL  THEN S.TC_BuyerInquiriesId END)) +
	COUNT(DISTINCT(CASE WHEN IL.TC_LeadStageId = 2 AND S.TC_LeadDispositionId IS NULL  THEN S.TC_BuyerInquiriesId END)) AS Active,
	COUNT(DISTINCT(CASE WHEN (S.TC_LeadDispositionId=4)  THEN S.TC_BuyerInquiriesId END)) AS Converted,
	COUNT(DISTINCT(CASE WHEN (S.TC_LeadDispositionID<>1
							 AND S.TC_LeadDispositionID<>3
							 AND S.TC_LeadDispositionID<>4
							 AND S.TC_LeadDispositionID IS NOT NULL)  THEN S.TC_BuyerInquiriesId END)) AS  Lost,
	COUNT(DISTINCT(CASE WHEN  IL.TC_LeadStageId = 1  AND IL.TC_UserId IS NOT NULL  AND S.TC_LeadDispositionID IS NULL  THEN S.TC_BuyerInquiriesId END)) AS NotContacted
	FROM TC_InquiriesLead IL   WITH(NOLOCK)
	INNER JOIN TC_BuyerInquiries S  WITH(NOLOCK) ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
	INNER JOIN TC_Users U WITH(NOLOCK) ON IL.TC_UserId = U.Id
	WHERE  S.CreatedOn BETWEEN @FromDate and  @ToDate and IL.BranchId IS NOT NULL
	GROUP BY U.UserName,U.Id,IL.BranchId
END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateLLBucketsHourlyExecution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateLLBucketsHourlyExecution]
GO

	-- =============================================
-- Author:		<Supriya Bhide>
-- Create date: <15 March 2016>
-- Description:	<SP updates sortscore of Optimizer and Maximizer dealers' stock to new buckets>
-- Supriya 23-03-2016 Modified to comment SellInquiries join and used Inner join instead of Left join on ConsumerCreditPoints
-- Avishkar 23-03-2016 Added Index on #tempPaidSellerScore
-- Modified by Prachi Phalak on 05-04-2016 , added new buckets for car score greater than 0.3.
-- Modified By Supriya Bhide 06/04/2016, Added premium package dealers who got responses. 
-- Modified By Supriya Bhide 07/04/2016, Added dealer premium package case in pushing down positive dealers.
-- Modified By Supriya Bhide 26/04/2016, Added 3 new packages in bucketing - Optimizer20, Maxi60 and Premier100
-- Modified By Supriya Bhide 19/05/2016, Removed 2 new packages in bucketing - Optimizer20 and Premier100
-- Modified By Supriya Bhide 20/05/2016, Added Maximizer Plus package in bucketing
-- Modified By Supriya Bhide 17/06/2016, Added new condition of unique customer count limit of 50 for deciding sortscore boosting
-- Modified By Supriya Bhide 28-06-2016, Consume new function for sortscore calculation
--Modified by Kinzal and Navead on (25-08-2016), to Fecth carwale dealers who is not migrated but can have iscartrade as 1 flag.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateLLBucketsHourlyExecution] 
	-- Add the parameters for the stored procedure here
AS

BEGIN
	DECLARE @start DATE = GETDATE() - 30			----- start date of period
	DECLARE @end DATE = GETDATE()					----- end date of period
	DECLARE @responsedate DATETIME = GETDATE()      ----- keep end date same as above
	DECLARE @DealerConversionRate NUMERIC(10,2) = 0.05
	DECLARE @CarWaleCommision NUMERIC(10,2) = 0.01
	DECLARE @minCustCountForDealer INT = 40	-- Added By Supriya Bhide 17/06/2016

--Modified by Navead and Prachi on 02/08/2016 Fetched cartrade dealers for sort Score calculations
SELECT CustomerPackageId,ConsumerId,ConsumerType
INTO #cwctDealers
FROM
(
	SELECT CCP.CustomerPackageId AS CustomerPackageId,CCP.ConsumerId as ConsumerId, CCP.ConsumerType
	FROM ConsumerCreditPoints CCP WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON CCP.ConsumerId = D.ID --AND (d.IsCarTrade is null OR d.IsCarTrade = 0)
	LEFT JOIN CWCTDealerMapping CCMP WITH(NOLOCK) on d.ID = CCMP.CWDealerID and isnull(CCMP.IsMigrated,0) = 0 -- Modified by Kinzal and Navead on (25-08-2016).

	UNION

	SELECT CCM.PackageId AS CustomerPackageId, CCM.CWDealerID as ConsumerId, 1 as ConsumerType
	FROM CWCTDealerMapping CCM WITH(NOLOCK) WHERE CCM.IsMigrated = 1
) ccd

-- create temp table for distinct optimizer and maximizer dealers who received responses within last 2 hours
SELECT DISTINCT PSS.DealerId, 0 AS CustCount
				--, 0 AS IsEligibleForBoost	-- Modified By Supriya Bhide 28-06-2016, Removed as logic shifted to function
				,PSS.SellerType,PSS.SVScore,PSS.packagetype
				,CCD.CustomerPackageId AS PackageId	-- Modified By Supriya Bhide 17/06/2016, Added IsEligibleForBoost
INTO #tempPaidSellerScore
FROM PaidSellerScore AS PSS WITH(NOLOCK)
--INNER JOIN SellInquiries AS SI  WITH(NOLOCK) ON PSS.DealerId=SI.DealerId
INNER JOIN UsedCarPurchaseInquiries AS UCPI WITH(NOLOCK)  ON UCPI.SellInquiryId=PSS.Inquiryid 
                                                         AND UCPI.RequestDateTime > DATEADD(HH,-2,GETDATE())
--INNER JOIN ConsumerCreditPoints CCP WITH(NOLOCK) ON CCP.ConsumerId = PSS.DealerId AND CCP.ConsumerType = PSS.SellerType
INNER JOIN #cwctDealers CCD WITH(NOLOCK) ON CCD.ConsumerId = PSS.DealerId -- Modified by Navead on 02-08-2016
WHERE  CCD.CustomerPackageId IN(30,32,34,47,81,98)	-- Modified By Supriya Bhide 06/04/2016, Added IDs 47 and 81 for premium package
													-- Modified By Supriya Bhide 26/04/2016, Added IDs 90,91,92 for new packages
													-- Modified By Supriya Bhide 19/05/2016, Removed Ids 90, 92 for new packages
													-- Modified By Supriya Bhide 20/05/2016, Modified ID for Maximizer Plus from 91 to 98
-- Avishkar Added Index on temp table
CREATE CLUSTERED INDEX IDX_tempPaidSellerScore_DealerId ON #tempPaidSellerScore(DealerId)

-- create temp table for dealer related data to calculate SVScore
SELECT		
			a.DealerId
			,a.DaysLive
			,AvgPrice
			,k.CustCount
		INTO #tempRecordDealers
		FROM
		(
			(SELECT 
				s.DealerId
				,COUNT(1) DaysLive
			FROM 
				DealerStockResponseAnalysis s WITH(NOLOCK)
				INNER JOIN #tempPaidSellerScore AS TPSS WITH(NOLOCK)
					ON TPSS.DealerId = s.DealerId
				INNER JOIN Dealers d WITH(NOLOCK)
					ON d.ID=TPSS.DealerId
				INNER JOIN vwcity c WITH(NOLOCK)
					ON d.CityId = c.CityId
				WHERE 
					Entrydate BETWEEN @start AND @end --Includes the data for begin and end date
				AND 
					TC_DealerTypeId IN ('1','3')
				AND 
					d.Status = 0
				GROUP BY  
					s.DealerId,c.CityId
			) a -- to get the dealer wise date wise response 		
			LEFT OUTER JOIN -- Responses
			(
				SELECT Id,COUNT(DISTINCT BuyerMobile) AS CustCount
				FROM
				(
					SELECT 
						c.Mobile BuyerMobile,
						d.ID
					FROM 
						UsedCarPurchaseInquiries r WITH(NOLOCK)
						INNER JOIN SellInquiries s  WITH(NOLOCK)
							ON r.SellInquiryId=s.ID
						INNER JOIN #tempPaidSellerScore AS TPSS WITH(NOLOCK)
							ON TPSS.DealerId = s.DealerId
						INNER JOIN dealers d  WITH(NOLOCK)
							ON d.ID=TPSS.DealerId
						INNER JOIN Customers c  WITH(NOLOCK)
							ON c.Id=r.CustomerID
						WHERE 
							RequestDateTime BETWEEN @start AND @responsedate 
					UNION

					SELECT 
						BuyerMobile,d1.id
					FROM 
						MM_Inquiries MM WITH(NOLOCK)
						INNER JOIN #tempPaidSellerScore AS TPSS WITH(NOLOCK)
							ON TPSS.DealerId = MM.ConsumerId
							AND MM.ConsumerType=1
						INNER JOIN Dealers D1  WITH(NOLOCK)
							ON D1.ID = MM.ConsumerId 
					WHERE 
						D1.TC_DealerTypeId = 1
					AND 
						MM.CallStartDate BETWEEN @start AND @responsedate
					AND 
						MM.CallStatus NOT IN ('NotConnected'))temp 
					GROUP BY id
				)k 
				ON k.Id = a.DealerId
				LEFT JOIN
				(
					SELECT 
						s.DealerId,
						AVG(isnull(dl.price,0)) AvgPrice
					FROM 
						LiveListingsDailyLog dl WITH(NOLOCK)
						INNER JOIN #tempPaidSellerScore AS TPSS WITH(NOLOCK) 
							ON dl.DealerId=tpss.DealerId
						LEFT JOIN SellInquiries s  WITH(NOLOCK)
							ON s.ID=dl.Inquiryid
					WHERE 
						AsOnDate BETWEEN @start AND @end -- Includes data for begin and end date
					GROUP BY s.DealerId
				)l
				ON 
					l.DealerId=a.DealerId
		)
		

-- create temp table for storing SVScore_Numerator for selected dealers
SELECT * 
INTO #tempForDELSVScore
FROM
(
	-- Seller Value Score for NEGATIVE seller value FOR DEALERS
	SELECT
		(TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (PK.Amount * 1.00 /PK.Validity) * DaysLive) AS SVScoreNum
		,TRD.DealerId, TRD.CustCount	-- Modified By Supriya Bhide 17/06/2016, Added TRD.CustCount column
	FROM #tempRecordDealers TRD WITH(NOLOCK)
		INNER JOIN #tempPaidSellerScore TPSS WITH(NOLOCK)
			ON TPSS.DealerId = TRD.DealerId
		INNER JOIN Packages PK WITH(NOLOCK)
			ON TPSS.PackageId = PK.Id and PK.Validity<>0
	WHERE
		TRD.DaysLive > 0	-- Modified by Prachi Phalak on 05-04-2016
	AND
		TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (PK.Amount * 1.00 /PK.Validity) * DaysLive <= 0

	UNION

	-- Seller Value Score for POSITIVE seller value FOR DEALERS
	SELECT
		(TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (PK.Amount * 1.00 /PK.Validity) * DaysLive) AS SVScoreNum
		,TRD.DealerId, TRD.CustCount	-- Modified By Supriya Bhide 17/06/2016, Added TRD.CustCount column
	FROM #tempRecordDealers TRD WITH(NOLOCK)
		INNER JOIN #tempPaidSellerScore TPSS WITH(NOLOCK)
			ON TPSS.DealerId = TRD.DealerId
		INNER JOIN Packages PK WITH(NOLOCK)
			ON TPSS.PackageId = PK.Id and PK.Validity<>0
	WHERE
		TRD.DaysLive > 0	-- Modified by Prachi Phalak on 05-04-2016
	AND
		TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (PK.Amount * 1.00 /PK.Validity) * DaysLive >= 0
) AS DealersTable


-- update newly calculated SVScore to tempPaidSellerScore table for bucketing
UPDATE #tempPaidSellerScore
SET SVScore = TDEL.SVScoreNum,
CustCount = TDEL.CustCount
--,IsEligibleForBoost =							-- Modified By Supriya Bhide 17/06/2016, Added IsEligibleForBoost
--		(CASE WHEN TDEL.SVScoreNum < 0 OR (TDEL.SVScoreNum >= 0 AND TDEL.CustCount <= @minCustCountForDealer) 
--			THEN 1
--			ELSE 0 
--		END)	-- Modified By Supriya Bhide 28-06-2016, removed as logic shifted to function
FROM #tempPaidSellerScore TPSS WITH(NOLOCK)
INNER JOIN #tempForDELSVScore TDEL WITH(NOLOCK)
	ON TPSS.DealerId = TDEL.DealerId


-- create temp table for livelisting stock of selected dealers
SELECT LL.ProfileId,LL.Inquiryid,LL.SellerType,LL.Score,LL.PackageType,LL.DealerId,LL.PhotoCount,
		TPSS.PackageId AS PackageId,TPSS.SVScore AS SVScoreNum,LL.SortScore
		,dbo.CalculateSortScoreForDealer(TPSS.PackageId,TPSS.SVScore,LL.Score
										,ABS(LL.SortScore) - FLOOR(ABS(LL.SortScore))
										,LL.PhotoCount,TPSS.CustCount
										) AS SortScoreNew	-- Modified By Supriya Bhide 28-06-2016
INTO #tempLiveListings
FROM LiveListings AS LL WITH(NOLOCK)
INNER JOIN #tempPaidSellerScore AS TPSS WITH(NOLOCK) 
	ON TPSS.DealerId = LL.DealerId
	AND TPSS.SellerType = LL.SellerType


UPDATE livelistings
SET SortScore = TLL.SortScoreNew
FROM livelistings LL WITH(NOLOCK)
INNER JOIN #tempLiveListings TLL WITH(NOLOCK)
	ON TLL.Inquiryid = LL.Inquiryid
	AND TLL.SellerType = LL.SellerType


DROP TABLE #tempLiveListings	
DROP TABLE #tempPaidSellerScore
DROP TABLE #tempForDELSVScore
DROP TABLE #tempRecordDealers

--SELECT TABLE #tempLiveListings
--SELECT TABLE #tempPaidSellerScore
--SELECT TABLE #tempForDELSVScore
--SELECT TABLE #tempRecordDealers

END
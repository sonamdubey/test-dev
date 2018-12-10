IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ComputeLLSortScoreDailyExecution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ComputeLLSortScoreDailyExecution]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by Manish on 23-04-2014 added with(nolock) keyword
-- Modified by Manish on 13-08-2014 Changed the lastupdateddate=getdate() to lastupdateddate=getdate()-1 since this will execute at once in whole day.
-- Modified on dec 15, 2014 by Shikhar the code to remove cities for last updated date parameter in selected cities
-- Modified on May 22, 2015 by Shikhar to include the paid seller score algorithm
-- Avishkar 25-05-2015 Disable and Enable Livelisting trigger to avoid locking	
-- Modified by Manish on 23-06-2015 added try block for exception handling.
-- Modified by Shikhar on 21-08-2015 added the additional bucket of 4 + score for negative valued paid sellers
-- Modified by Purohith Guguloth on 6th october, 2015 added a condition of photocount near SortScore section
-- Modified by Supriya Bhide(5-2-2016) changed SortScore calculation logic
-- Modified by Prachi Phalak on 17th Feb,2016, added bucket 9 for Dealer Premium Pan-India Package
-- Modified by Supriya Bhide(16-3-2016), changed bucketing logic for Optimizer and Maximizer package
-- Modified by Supriya Bhide(18-3-2016), modified sortscore updation from new temp table storing newly calculted sort score
-- Modified by Supriya Bhide(28-3-2016), Handled free seller case for sortscore calculation, Changed limit for carscore in bucketing from 0.3 to 0.2
-- Modified by Prachi Phalak on 29-03-2016, Update SVScore in livelistings table. 
-- Modified by Prachi Phalak on 05-04-2016 , added new buckets for car score greater than 0.3.
-- Modified by Supriya Bhide on 06-04-2016, Modified first select query with join on LL.DealerId instead of PSS.DealerId
-- Modified by Supriya Bhide on 26-04-2016, Added 3 new packages in bucketing - Optimizer20, Maxi60 and Premier100
-- Modified by Supriya Bhdie on 19-05-2016, Removed 2 new packages from bucketing - Optimizer20 and Premier100
-- Modified by Supriya Bhide on 31-05-2016, Added new buckets for Maximizer Plus package
-- Modified by Supriya Bhide on 20-06-2016, Added new column IsEligibleForBoost to decide buckets
-- Modified by Supriya Bhide on 28-06-2016, Consumed new functions for sort score calcualtion
-- Modified by Navead and Prachi on 02/08/2016, Fetched cartrade dealers for sort Score calculations
--Modified by Kinzal and Navead on (25-08-2016), to Fecth carwale dealers who is not migrated but can have iscartrade as 1 flag.
-- =============================================
CREATE PROCEDURE [dbo].[ComputeLLSortScoreDailyExecution] 
	-- Add the parameters for the stored procedure here
	@ProfileId VARCHAR(50) = NULL
AS
BEGIN
	DECLARE @SortScore AS INT	-- Modified by Supriya Bhide(5-2-2016)
	DECLARE @minCustCountForDealer INT = 40	-- Modified by Supriya Bhide on 20-06-2016
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

-- AFTER

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

SELECT LL.Score,LL.Inquiryid,LL.SellerType,LL.IsPremium,LL.responses,LL.ProfileId,LL.PackageType,LL.SortScore-- Modified by Supriya Bhide(16-3-2016)
		,PSS.SVScore,CCD.CustomerPackageId AS CustomerPackageId,PhotoCount,PSS.NewScore,PSS.CustCount
INTO #tempLiveListings 
FROM livelistings AS LL WITH(NOLOCK)
LEFT JOIN PaidSellerScore PSS WITH(NOLOCK) ON LL.Inquiryid = PSS.Inquiryid AND LL.SellerType = PSS.SellerType
LEFT JOIN #cwctDealers CCD WITH(NOLOCK) ON CCD.ConsumerId = LL.DealerId -- Modified by Navead on 02-08-2016
																		-- Modified by Supriya Bhide on 06-04-2016
	AND CCD.ConsumerType = LL.SellerType -- Modified by Supriya Bhide(09-02-2016)  
--LEFT JOIN Packages P WITH(NOLOCK) ON LL.PackageType = P.InqPtCategoryId

--=====================================================
      -- Modified by Supriya Bhide(18-3-2016) - Commented out as new column of new score is being used
--=======================================================

--UPDATE #tempLiveListings 
--SET 
--	Score = PSS.NewScore
--FROM
--	#tempLiveListings TLL WITH(NOLOCK)
--	INNER JOIN PaidSellerScore PSS WITH(NOLOCK)
--		ON TLL.Inquiryid = PSS.Inquiryid AND TLL.SellerType = PSS.SellerType

UPDATE #tempLiveListings
SET CustomerPackageId = 46
FROM #tempLiveListings TLL WITH(NOLOCK)
WHERE TLL.PackageType = 28
AND TLL.SellerType = 1 

-- Avishkar 25-05-2015 Disable Livelisting trigger to avoid locking		
ALTER TABLE dbo.LiveListings DISABLE TRIGGER TrigUpdateLiveListingCities
    BEGIN TRY 							
				SELECT a.*
				INTO #tempSortScore		-- Modified by Supriya Bhide(18-3-2016)
				FROM
				(SELECT TLL.ProfileId,TLL.Inquiryid,TLL.SellerType,TLL.PackageType,TLL.SVScore --Modified by Prachi Phalak on 29-03-2016
						,dbo.CalculateSortScoreForDealer(TLL.CustomerPackageId,TLL.SVScore,TLL.Score
														,ISNULL(TLL.NewScore,ABS(TLL.Score) - FLOOR(ABS(TLL.Score)))
														,TLL.PhotoCount,TLL.CustCount
														)AS SortScore	-- Modified by Supriya Bhide on 28-06-2016
					FROM #tempLiveListings as TLL
					WHERE TLL.SellerType=1
					UNION
					SELECT 	TLL.ProfileId,TLL.Inquiryid,TLL.SellerType,TLL.PackageType,TLL.SVScore --Modified by Prachi Phalak on 29-03-2016
							,dbo.CalculateSortScoreForIndividual(TLL.IsPremium, TLL.Score
																,TLL.NewScore, TLL.PhotoCount
																)AS SortScore	-- Modified by Supriya Bhide on 28-06-2016
					FROM #tempLiveListings as TLL
					WHERE TLL.SellerType=2
				)a
				--SELECT @ProfileId,TSS.SortScore,TSS.SVScore,TSS.Inquiryid,TSS.SellerType,TSS.PackageType
				
				UPDATE ll
					SET ll.SortScore = TSS.SortScore		-- Modified by Supriya Bhide(18-3-2016)
					,ll.SVScore = ISNULL(TSS.SVScore,0)     --Modified by Prachi Phalak on 29-03-2016
					FROM livelistings LL WITH (NOLOCK)
					INNER JOIN #tempSortScore TSS WITH(NOLOCK)	-- Modified by Supriya Bhide(18-3-2016)
						ON LL.Inquiryid = TSS.Inquiryid AND LL.SellerType = TSS.SellerType
					LEFT JOIN PackageTypePriority PT WITH (NOLOCK) ON TSS.PackageType = PT.PackageType
					WHERE (@ProfileId IS NULL OR @ProfileId = TSS.ProfileId)

				-- Avishkar 25-05-2015 Disable Livelisting trigger to avoid locking		
				ALTER TABLE dbo.LiveListings ENABLE TRIGGER TrigUpdateLiveListingCities

				DROP TABLE #tempLiveListings
				--SELECT * from #tempLiveListings

	 END TRY
      BEGIN CATCH

	           ALTER TABLE dbo.LiveListings ENABLE TRIGGER TrigUpdateLiveListingCities

	  END CATCH;
   


END


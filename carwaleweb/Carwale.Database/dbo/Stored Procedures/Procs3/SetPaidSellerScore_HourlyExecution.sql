IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SetPaidSellerScore_HourlyExecution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SetPaidSellerScore_HourlyExecution]
GO

	
-- Author: Shikhar Maheshwari
-- Created on: March 25, 2015
-- Last Modifications: Removed the variable of @ExperimentalCities to make the Algorithm Pan India on June 15, 2015
-- Modified By Vicky Gupta on 27-07-2015, calculated value offered to each dealer and updated it into PaidSellerScore table
-- Shikhar modified 11-12-2015 Sathish Chander wanted to change weightage to increase seller value score
-- Modified by Navead 15-12-2015 Included CPS changes for getting amount from ConsumerCreditPoint
--Removed by Purohith Guguloth -the logic of Truncating PaidSellerScore Table and Inserting into it from Livelisting table on 05 jan, 2016
--Removed by Purohith Guguloth -Removed Individual customer Queries on 05 jan, 2016

CREATE PROCEDURE [dbo].[SetPaidSellerScore_HourlyExecution]
AS
BEGIN
		DECLARE @CSPrefix NUMERIC(10,2) = 0.6 -- Car Score multiplier -- Shikhar modified 11-12-2015 Sathish Chander wanted to change weightage to increase seller value score
		DECLARE @SVPrefix NUMERIC(10,2) = 0.4 -- Seller Value Score multiplier -- Shikhar modified 11-12-2015 Sathish Chander wanted to change weightage to increase seller value score
		--DECLARE @ExperimentalCities VARCHAR(100) = '176,12,40,2,105,198,10, 224, 225, 246, 273'
		/***** @CSPrefix + @SVPrefix = 1 *****/

		DECLARE @DealerConversionRate NUMERIC(10,2) = 0.05
		DECLARE @IndividualConversionRate NUMERIC(10,2) = 0.2

		DECLARE @CarWaleCommision NUMERIC(10,2) = 0.01

		DECLARE @start DATE = GETDATE() - 30			----- start date of period
		DECLARE @end DATE = GETDATE()					----- end date of period
		DECLARE @responsedate DATETIME = GETDATE()      ----- keep end date same as above 

		--TRUNCATE TABLE PaidSellerScore

		--/*********** Fetching the basic data for Individuals ***********/
		--/********* For Individuals **********/
		--SELECT 
		--	p.InqPtCategoryId
		--	,p.Id AS PackageId
		--	,Inquiryid
		--	,dl.CityName
		--	,dl.CityId
		--	,new.EntryDate
		--	,dl.Price
		--	,dayslive
		--	,r.cust CustCount
		--INTO #tempRecordIndividuals
		--FROM
		--(
		--SELECT 
		--	Inquiryid 
		--	,CityName
		--	,CityId
		--	,AVG(dl.Price) Price
		--	,COUNT(1) dayslive
		--FROM 
		--	LiveListingsDailyLog dl WITH(NOLOCK)
		--WHERE 
		--	AsOnDate BETWEEN @start AND @end 
		--AND 
		--	SellerType=2
		--AND 
		--	dl.PackageType=31
		--GROUP BY 
		--	Inquiryid
		--	,CityName
		--	,CityId
		--)dl
		--LEFT JOIN (SELECT id,CustomerId,CustomerEmail,CustomerName
		--		FROM CustomerSellInquiries WITH(NOLOCK) ) CSI 
		--ON CSI.ID=dl.Inquiryid
		--LEFT JOIN 
		--(
		--	SELECT 
		--		ss.id, 
		--		COUNT(DISTINCT c.Mobile) cust 
		--	FROM 
		--		CustomerSellInquiries ss WITH(NOLOCK)
		--		LEFT JOIN ClassifiedRequests re WITH(NOLOCK)
		--		ON re.SellInquiryId=ss.ID
		--	LEFT JOIN Customers c WITH(NOLOCK)
		--		ON c.Id=re.CustomerId 
		--	WHERE 
		--		RequestDateTime BETWEEN @start AND @responsedate  
		--	GROUP BY 
		--		ss.Id
		--)r 
		--ON r.ID=CSI.Id 
		--LEFT JOIN
		--( 
		--	SELECT 
		--	ConsumerId
		--	,PackageId
		--	,EntryDate
		--	FROM 
		--	( 
		--		SELECT 
		--			ConsumerId
		--			,PackageId
		--			,Id
		--			,EntryDate
		--			,ActualValidity
		--			,row_number() 
		--		OVER (PARTITION BY ConsumerId ORDER BY Id DESC) AS RowNum  
		--		FROM consumerpackagerequests WITH(NOLOCK)
		--		WHERE 
		--			isApproved = 1
		--		AND 
		--			isActive = 1
		--		AND 
		--			ConsumerType = 2
		--	) ltst
		-- WHERE ltst.RowNum = 1
		--)new  
		--ON new.ConsumerId = CSI.CustomerId
		--LEFT JOIN Packages p  WITH(NOLOCK)
		--	ON p.Id=new.PackageId
		--WHERE dayslive > 15
		/******************* Fetch the data for Individuals ********************/


		/******************* Fetching the data for the Dealers *******************/
		SELECT  
			a.*
			,l.ActualCarSlotsSold
			,pkg.Name pkg
			,pkg.InqPtCategoryId
			,pkg.Id AS PackageId
			,AvgPrice
			,EntryDate
			,ActualAmount
			,ActualValidity
			,k.CustCount
		INTO #tempRecordDealers
		FROM
		(
			SELECT 
				DealerId
				,Organization
				,City
				,c.CityId
				,COUNT(1) DaysLive
				,SUM(ISNULL(response,0)) TotalResponse
				,AVG(CWStockCount)as AverageStock
			FROM 
				DealerStockResponseAnalysis s WITH(NOLOCK)
				LEFT JOIN Dealers d WITH(NOLOCK)
					ON d.ID=s.DealerId
				LEFT JOIN vwcity c WITH(NOLOCK)
					ON d.CityId = c.CityId
				WHERE 
					Entrydate BETWEEN @start AND @end --Includes the data for begin and end date
				AND 
					TC_DealerTypeId IN ('1','3')
				AND 
					IsDealerActive = '1'
				GROUP BY  
					DealerId,Organization,City,c.CityId
		) a -- to get the dealer wise date wise response 
 
		LEFT OUTER JOIN -- getting the latest package 
		( 
			SELECT 
				ConsumerId
				,PackageId
				,EntryDate
				,ActualAmount
				,ActualValidity
			FROM
			(SELECT 
				ConsumerId
				,PackageId	
				,Id
				,ActualAmount
				,ActualValidity
				,EntryDate
				,row_number() 
					OVER (PARTITION BY ConsumerId
					ORDER BY Id DESC) AS RowNum  
			FROM 
				consumerpackagerequests WITH(NOLOCK)
			WHERE 
				isApproved = 1
			AND
				isActive = 1
			AND 
				ConsumerType = 1
			) ltst
			WHERE ltst.RowNum = 1
			)new  
				ON new.ConsumerId = a.DealerId
				LEFT JOIN packages pkg  WITH(NOLOCK)
					ON new.packageid = pkg.id
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
					LEFT JOIN SellInquiries s  WITH(NOLOCK)
						ON r.SellInquiryId=s.ID
					LEFT JOIN dealers d  WITH(NOLOCK)
						ON d.ID=s.DealerId
					LEFT JOIN Customers c  WITH(NOLOCK)
						ON c.Id=r.CustomerID
					WHERE 
						RequestDateTime BETWEEN @start AND @responsedate 
				UNION

				SELECT 
					BuyerMobile,d1.id
				FROM 
					MM_Inquiries MM WITH(NOLOCK)
					LEFT JOIN Dealers D1  WITH(NOLOCK)
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
					,COUNT(1) ActualCarSlotsSold
				FROM 
					LiveListingsDailyLog dl WITH(NOLOCK)
					LEFT JOIN SellInquiries s  WITH(NOLOCK)
						ON s.ID=dl.Inquiryid
				WHERE 
					AsOnDate BETWEEN @start AND @end -- Includes data for begin and end date
				GROUP BY s.DealerId
			)l
			ON 
				l.DealerId=a.DealerId

		/***** End of Fetching the data for Dealers *****/

		DECLARE @a int 
		select @a = dayslive from #tempRecordDealers 
		--print (@a)
		/*********************************************************/
		-- Create a temporary table for packages
		SELECT Id, Name, Validity, InqPtCategoryId, Amount, isActive 
		INTO #tempPackagesForRating
		FROM Packages WITH(NOLOCK)
		WHERE 
			InqPtCategoryId IN (18,19, 29, 31, 38)
		AND
			Id NOT IN (31,33)

		-- Updating the Validity for Premium individuals to 90 days
		UPDATE #tempPackagesForRating SET Validity = 90 WHERE Id=49
		/*********************************************************/


		--INSERT INTO PaidSellerScore(ProfileId, Inquiryid, SellerType, CityId, DealerId, Score,[packagetype])
		--SELECT ProfileId, Inquiryid, SellerType, CityId, DealerId, Score,packagetype
		--FROM LiveListings WITH(NOLOCK) 
		--WHERE PackageType IN (SELECT InqPtCategoryId FROM #tempPackagesForRating) 
		--AND CityId IN (SELECT * FROM dbo.fnSplitCSV(@ExperimentalCities))

		/*********************************************************/

		/*******************************************************/
		--Create temperory amount table for getting amount from ConsumerCreditPoints for CPS and Packages table for Non CPS
		SELECT * INTO #tempDealerAmount FROM(
		SELECT ConsumerId,CustomerPackageId,Amount FROM ConsumerCreditPoints  WITH(NOLOCK)
		WHERE CustomerPackageId IN (77,76)
		UNION
		SELECT CCP.ConsumerId,CCP.CustomerPackageId, P.Amount 
		FROM #tempPackagesForRating P WITH(NOLOCK) 
		INNER JOIN ConsumerCreditPoints CCP WITH(NOLOCK)
		ON P.Id=CCP.CustomerPackageId 
		WHERE CCP.CustomerPackageId NOT IN(77,76)) AS DealerAmountTable

		/****************End************************************/

		--Calculate the Total (Dealers + Individuals) FOR Positive Value SELLERS
		SELECT 
			PSV.SellerValue AS TotalValue
			,PSV.SellerId 
			,PSV.CityId 
		INTO #tempPSV
		FROM
		(
		-- Positive Seller Valued Dealers
		SELECT
			TRD.AvgPrice * ISNUll(TRD.CustCount,0) * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive AS SellerValue
			,'D'+CONVERT(VARCHAR(10), TRD.DealerId) AS SellerId
			,TRD.CityId
		FROM #tempRecordDealers TRD WITH(NOLOCK)
			INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
				ON TRD.PackageId = PK.Id
			INNER JOIN #tempDealerAmount DA WITH(NOLOCK)
				ON TRD.DealerId=DA.ConsumerId
		WHERE
			TRD.DaysLive > 15
		--AND 
			--TRD.CityId IN (SELECT * FROM dbo.fnSplitCSV(@ExperimentalCities))
		AND
			TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive >= 0
		GROUP BY 
			TRD.CityId, TRD.AvgPrice,TRD.CustCount, DA.Amount, PK.Validity, TRD.DaysLive, TRD.DealerId
		--UNION
		---- Positive Seller Valued Individuals
		--SELECT (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) AS SellerValue
		--	,'I' + CONVERT(VARCHAR(10), TRI.Inquiryid) AS SellerId
		--	,TRI.CityId 
		--	FROM #tempRecordIndividuals TRI WITH(NOLOCK)
		--INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
		--	ON TRI.PackageId = PK.Id
		--	WHERE (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) >= 0	
		--	--AND TRI.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
				

		) AS PSV -- Positive Seller Value

		SELECT 
			SUM(TotalValue) AS PositiveTotalValue, 
			CityId 
		INTO #tempTotalPSV
		FROM #tempPSV 
		GROUP BY CityId

		/****************************************************************************************************/

		/****************************************************************************************************/
		/*********Calculate the Total (Dealers + Individuals) FOR Negative Value SELLERS*********/
		SELECT 
			NSV.SellerValue AS TotalValue
			,NSV.SellerId
			,NSV.CityId 
		INTO #tempNSV
		FROM
		(
		-- Negative Seller Valued Dealers
		SELECT
			TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive AS SellerValue
			,'D' + CONVERT(VARCHAR(10), TRD.DealerId)  AS SellerId
			,TRD.CityId
		FROM #tempRecordDealers TRD WITH(NOLOCK)
			INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
				ON TRD.PackageId = PK.Id
			INNER JOIN #tempDealerAmount DA WITH(NOLOCK)
				ON TRD.DealerId=DA.ConsumerId
		WHERE
			TRD.DaysLive > 15
		--AND 
			--TRD.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
		AND
			TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive < 0
		GROUP BY 
			TRD.CityId, TRD.AvgPrice,TRD.CustCount, DA.Amount, PK.Validity, TRD.DaysLive, TRD.DealerId

		--UNION

		-- Negative Seller Valued Individuals
		--SELECT (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) AS SellerValue
		--	,'I'+ CONVERT(VARCHAR(10), TRI.Inquiryid)  AS SellerId
		--	,TRI.CityId 
		--	FROM #tempRecordIndividuals TRI WITH(NOLOCK)
		--INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
		--	ON TRI.PackageId = PK.Id
		--	WHERE (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) < 0	
		--	--AND TRI.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
		) AS NSV -- Negative Seller Value

		SELECT 
			SUM(TotalValue) AS NegativeTotalValue,
			CityId 
		INTO #tempTotalNSV
		FROM #tempNSV 
		GROUP BY CityId
		/****************************************************************************************************/

		/****************************************************************************************************/
		/***** Create the temporary table for dealers to store there seller value score (both +ve / -ve)*****/
		SELECT * 
		INTO #tempForDELSVScore
		FROM
		(
		-- Seller Value Score for NEGATIVE seller value FOR DEALERS
		SELECT
			-1.0 * (TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive) / TNSV.NegativeTotalValue AS SVScore
			,TRD.DealerId
			,TRD.CityId
		FROM #tempRecordDealers TRD WITH(NOLOCK)
			INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
				ON TRD.PackageId = PK.Id
			LEFT JOIN #tempTotalNSV TNSV
				ON TNSV.CityId = TRD.CityId
			INNER JOIN #tempDealerAmount DA WITH(NOLOCK)
				ON TRD.DealerId=DA.ConsumerId
		WHERE
			TRD.DaysLive > 15
		--AND 
			--TRD.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
		AND
			TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive <= 0
		GROUP BY 
			TRD.CityId, TRD.AvgPrice,TRD.CustCount, DA.Amount, PK.Validity, TRD.DaysLive, TRD.DealerId, TNSV.NegativeTotalValue

		UNION

		-- Seller Value Score for POSITIVE seller value FOR DEALERS
		SELECT
			(TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive) / TPSV.PositiveTotalValue AS SVScore
			,TRD.DealerId
			,TRD.CityId
		FROM #tempRecordDealers TRD WITH(NOLOCK)
			INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
				ON TRD.PackageId = PK.Id
			LEFT JOIN #tempTotalPSV TPSV
				ON TPSV.CityId = TRD.CityId
			INNER JOIN #tempDealerAmount DA WITH(NOLOCK)
				ON TRD.DealerId=DA.ConsumerId
		WHERE
			TRD.DaysLive > 15
		--AND 
			--TRD.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
		AND
			TRD.AvgPrice * TRD.CustCount * @DealerConversionRate * @CarWaleCommision - (DA.Amount * 1.00 /PK.Validity) * DaysLive >= 0
		GROUP BY 
			TRD.CityId, TRD.AvgPrice,TRD.CustCount, DA.Amount, PK.Validity, TRD.DaysLive, TRD.DealerId, TPSV.PositiveTotalValue
		) AS DealersTable
		/****************************************************************************************************/

		/****************************************************************************************************/
		/*** Create the temporary table for individuals to store there seller value score (both +ve / -ve)***/
		--SELECT * 
		--INTO #tempForINDSVScore
		--FROM
		--(
		----Seller Value Score for NEGATIVE seller value FOR INDIVIDUALS
		--SELECT -1.0 *(ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) / TNSV.NegativeTotalValue  AS SVScore
		--	,TRI.InquiryId
		--	,TRI.CityId
		--FROM #tempRecordIndividuals TRI WITH(NOLOCK)
		--INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
		--	ON TRI.PackageId = PK.Id
		--INNER JOIN #tempTotalNSV TNSV
		--	ON TRI.CityId = TNSV.CityId
		--	WHERE (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) <= 0	
		--	--AND TRI.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))			

		--UNION

		----Seller Value Score for POSIIVE seller value FOR INDIVIDUALS
		--SELECT (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) / TPSV.PositiveTotalValue  AS SVScore
		--	,TRI.InquiryId
		--	,TRI.CityId
		--FROM #tempRecordIndividuals TRI WITH(NOLOCK)
		--INNER JOIN #tempPackagesForRating PK WITH(NOLOCK)
		--	ON TRI.PackageId = PK.Id
		--INNER JOIN #tempTotalPSV TPSV
		--	ON TRI.CityId = TPSV.CityId
		--	WHERE (ISNULL(TRI.CustCount, 0) * @IndividualConversionRate * TRI.Price * @CarWaleCommision - (PK.Amount  * 1.00 /PK.Validity) * TRI.dayslive) >= 0	
		--	--AND TRI.CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))
				
		--) AS IndividualsTable
		/****************************************************************************************************/

		/****************************************************************************************************/
		/***** Update final table with the Individuals Score *****/
		--UPDATE PaidSellerScore
		--	SET 
		--		SVScore = TIND.SVScore,
		--		NewScore = @CSPrefix * TFT.Score + @SVPrefix * (1 - TIND.SVScore)
		--FROM
		--	PaidSellerScore TFT WITH(NOLOCK)
		--	INNER JOIN #tempForINDSVScore TIND WITH(NOLOCK)
		--		ON TFT.Inquiryid = TIND.Inquiryid AND TFT.SellerType = 2

		-- Update final table with the Dealers Score
		UPDATE PaidSellerScore
			SET 
				SVScore = TDEL.SVScore,
				NewScore = CONVERT(NUMERIC(6,4), (@CSPrefix * ISNULL(TFT.Score, 0) + @SVPrefix * (1 - TDEL.SVScore)))
		FROM
			PaidSellerScore TFT WITH(NOLOCK)
			INNER JOIN #tempForDELSVScore TDEL WITH(NOLOCK)
				ON TFT.DealerId = TDEL.DealerId AND TFT.SellerType = 1

		-- Update the entries which have not been updated with the default formula in the experimental cities
		UPDATE PaidSellerScore
		SET 
			SVScore = 0.0,
			NewScore = @CSPrefix * Score + @SVPrefix
		WHERE NewScore IS NULL			
		--AND CityId IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))

		-- Update the tables for those listings for which experiment is not done
		UPDATE PaidSellerScore
		SET 
			NewScore = Score
		WHERE NewScore IS NULL	
		--AND CityId NOT IN (SELECT ListMember FROM dbo.fnSplitCSV(@ExperimentalCities))


		
/****************************************************************************************************/

			-- Update Value Offered to each dealer
		UPDATE PaidSellerScore
			SET 				
				ValueOffered = CONVERT(FLOAT, (TRD.CustCount * 0.1 * TRD.AvgPrice * 0.01)*100/TRD.ActualAmount)
		FROM
			PaidSellerScore TFT WITH(NOLOCK)
			INNER JOIN #tempRecordDealers TRD WITH(NOLOCK)
				ON TFT.DealerId = TRD.DealerId AND TFT.SellerType = 1 AND TRD.ActualAmount> 0
/****************************************************************************************************/
		--SELECT * from #tempRecordIndividuals
		--SELECT * from #tempRecordDealers
		--SELECT * from #tempPackagesForRating
		--SELECT * from #tempPSV
		--SELECT * from #tempTotalPSV
		--SELECT * from #tempNSV
		--SELECT * from #tempTotalNSV
		--SELECT * from #tempForDELSVScore
		--SELECT * from #tempForINDSVScore
		--SELECT * from #tempDealerAmount

			/*** Dropping all the temporary tables ***/
		--DROP TABLE #tempRecordIndividuals
		DROP TABLE #tempRecordDealers
		DROP TABLE #tempPackagesForRating
		DROP TABLE #tempPSV
		DROP TABLE #tempTotalPSV
		DROP TABLE #tempNSV
		DROP TABLE #tempTotalNSV
		DROP TABLE #tempForDELSVScore
	--	DROP TABLE #tempForINDSVScore
		DROP TABLE #tempDealerAmount
END
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsFetch]
GO

	

-- =================================================================================================================================
-- Author		:	Manish Chourasiya
-- Create date	:	08/05/2013
-- Description	:	Fetching Stock detail , all filter and order is also handled in SP 
-- Modified By  :	Nilesh Utture on 30th Dec, 2013 Added Left Join with TC_MMDealersMatchCount to retrieve Mix and Match Count
-- Modified By  :   Vivek Gupta on 23-01-2014, Added left join with tc_mappingoffersonttock to get active offers
-- Modified By  :   Vivek Gupta on 04-02-2014, Added parameter @OfferIds to filter stocks against offers
-- Modified By  :   Vishal Srivastava on 10-02-2014, add stock rating and time of update parameter
-- Modified By  :   Vivek Gupta on 10-02-2014,Added a condition to retrieve all stocks with no offers applied.
-- Modified By  :   Manish Chourasiya on 30-06-2015 added with recompile option
-- Modified By  :   Deepak DealerId condition in Sellinquiries table
-- Modified By  :   Manish Chourasiya on 07-10-2015 commented with recompile option
-- Modified By  :   Vivek Rajak on 21-07-2015 to fetch photo count.
-- Modified By  :   Vivek Rajak on 04-08-2015 to filter certified and non-certified cars.
-- Modified By  :   Vicky gupta on 02/10/2015 to disply stock for query generated from dashboard stock details
-- Modified By  :   Khushaboo Patil on 18/02/2016 to changed conditions of certified and non certified stocks
-- Modified By  :   Khushaboo Patil on 17/03/2016 added suspended stock filter on topup expired
--  TC_StockDetailsFetch 'EntryDate~2',NULL,NULL,NULL,NULL,NULL,NULL,1,2000,1,NULL,NULL,5 ,NULL,null,0,NULL,NULL
-- Modified By : Tejashree Patil on 29 March 2016, Changes related manage carwale listings and package.
-- Modified By : Suresh Prajapati on 15th April, 2016
-- Description : Added Left Join on ConsumerCreditPoints
-- Modified By : 3/05/2016 Added IsEligible flag to upload stock 
-- Modified By : Khushaboo Patil on 4/05/2016 Modified @IsCWListing conditions to fetch not eligible to upload stock 
-- Modified By : Ruchira Patil on 25th May, 2016 (inserted input parameters in TC_Exceptions table)
-- =================================================================================================================================   
CREATE PROCEDURE [dbo].[TC_StockDetailsFetch] @OrderBy VARCHAR(100) = NULL
	,@MakeId INT = NULL
	,@ModelId INT = NULL
	,@MinPrice INT = NULL
	,@MaxPrice INT = NULL
	,@MaxKms INT = NULL
	,@MinKms INT = NULL
	,@FromIndex INT
	,@ToIndex INT
	,@Status TINYINT
	,@SyncStatus TINYINT = NULL
	,@IsParkNSale TINYINT = NULL
	,@BranchId INT
	,@OfferIds VARCHAR(MAX) = NULL
	,-- Modified By  :   Vivek Gupta on 04-02-2014
	@chkCertifiedCarsStatus BIT = NULL, -- MOdified By  :   Vivek Rajak on 04-08-2015
	@DashboardActivity INT = 0 -- to know whether request has come from dashboard or not
	,@SuspendedDate DATETIME = NULL
	,@IsCWListing TINYINT = NULL
AS
BEGIN
	--IF(@BranchId IN (21229,7717))
	--BEGIN
	---- Modified By : Ruchira Patil on 25th May, 2016 (inserted input parameters in TC_Exceptions table)
	--INSERT INTO TC_Exceptions
 --                     (Programme_Name,
 --                      TC_Exception,
 --                      TC_Exception_Date,
 --                      InputParameters)
	--		 VALUES('TC_StockDetailsFetch:StockListError',
	--		 '',
	--		 GETDATE(),
	--		 ' @OrderBy:' + ISNULL(CAST(@OrderBy AS VARCHAR(100)),'NULL') + 
	--		 ' @MakeId:' + ISNULL(@MakeId,'NULL') + 
	--		 ' @ModelId :'+ ISNULL(@ModelId,'NULL') + 
	--		 ' @MinPrice : '+	ISNULL(@MinPrice,'NULL')  +
	--		 ' @MaxPrice: '+ISNULL(@MaxPrice,'NULL')+
	--		 ' @MaxKms: ' + ISNULL(@MaxKms,'NULL')+
	--		 ' @MinKms: '+  ISNULL(@MinKms,'NULL') +
	--		 ' @FromIndex : ' +ISNULL( @FromIndex,'NULL')+
	--		 ' @ToIndex: ' + ISNULL(@ToIndex,'NULL')+
	--		 ' @Status: '+ISNULL(@Status,'NULL') +
	--		 ' @SyncStatus : ' +ISNULL(@SyncStatus,'NULL')+
	--		 ' @IsParkNSale : '+ISNULL(@IsParkNSale,'NULL')+
	--		 ' @BranchId: ' + ISNULL(@BranchId,'NULL')+
	--		 ' @OfferIds: '+ ISNULL(CAST( @OfferIds AS VARCHAR(MAX)),'NULL')+
	--		 ' @chkCertifiedCarsStatus: ' + ISNULL(@chkCertifiedCarsStatus,'NULL')+	
	--		 ' @DashboardActivity: '+ISNULL(@DashboardActivity,'NULL')+
	--		 ' @SuspendedDate: ' +ISNULL(CAST(@SuspendedDate AS datetime),'NULL')+
	--		 ' @IsCWListing : '+ISNULL(@IsCWListing,'NULL')
	--		 )   
	--END	 
	DECLARE @CarGroupType VARCHAR(100) = NULL
	SELECT	@CarGroupType = CASE WHEN CC.CarGroupType IS NULL AND CC.TopUpCarGroupType IS NULL THEN NULL ELSE ISNULL(CC.CarGroupType, '') + ',' + ISNULL(CC.TopUpCarGroupType, '') END
	FROM	ConsumerCreditPoints CC WITH (NOLOCK)
	WHERE	ConsumerId = @BranchId AND ConsumerType = 1;

	IF(@DashboardActivity = 0) -- Modified by Vicky Gupta on 02/10/2015
	BEGIN
		-- Modified By  :   Vivek Gupta on 04-02-2014
		CREATE TABLE #tempOffers (OfferIds INT)

		INSERT INTO #tempOffers
		SELECT @OfferIds
		FROM [dbo].[fnSplitCSV](@OfferIds);

		WITH CTE1
		AS (
			SELECT DISTINCT St.id
				,St.isfeatured
				,St.isbooked
				,St.makeyear
				,St.kms
				,dbo.Titlecase(St.colour) Colour
				,(
					SELECT Count(DISTINCT tc_buyerinquiriesid)
					FROM tc_buyerinquiries B WITH(NOLOCK)
						,tc_inquirieslead L WITH(NOLOCK)
					WHERE B.stockid = St.id
						AND B.tc_leaddispositionid IS NULL
						AND B.tc_inquiriesleadid = L.tc_inquiriesleadid
						AND Isnull(L.tc_leadstageid, 0) <> 3
					) AS InquiryCount
				,Upper(St.regno) RegNo
				,St.price
				,St.entrydate
				,St.statusid
				,Tcs.Status
				,Ma.id MakeId
				,Mo.id ModelId
				,(Ma.NAME + ' ' + Mo.NAME + ' ' + Ve.NAME) AS MakeModelVersion
				,issychronizedcw
				,(
					SELECT TOP 1 id
					FROM tc_carphotos Cp WITH(NOLOCK)
					WHERE Cp.stockid = St.id
						AND Cp.isactive = 1
					) Photos
				,(
					SELECT count(CP.id) --Added by vivek rajak to count photos on 21-07-2015
					FROM tc_carphotos Cp WITH (NOLOCK)
					WHERE Cp.stockid = St.id
						AND Cp.isactive = 1
					) PhotoCount
				,Si.id AS sellInquiryId
				,Mc.MatchViewCount
				,OS.StockId AS HasActiveOffer
				,-- Added By : Vivek Gupta on 23-01-2014
				st.StockRating AS StockRating
				,-- Added By : Vishal Srivastava on 10-02-2014 
				st.LastStockRatingUpdate AS LastUpdate -- Added By : Vishal Srivastava on 10-02-2014	
				,(SELECT TOP 1 InvCertifiedDate FROM TC_CarTradeCertificationData  WITH (NOLOCK)
				 WHERE ListingId = St.Id ORDER BY InvCertifiedDate DESC 
				 )InvCertifiedDate,SuspendedDate,CG.CarGroupTypeId,CG.IsActive cgIsActive,CC.ExpiryDate ccExpiryDate,

				CASE	
					WHEN  @CarGroupType IS NULL AND CONVERT(DATE,CC.ExpiryDate) >= CONVERT(DATE,GETDATE()) --ST.IsSychronizedCW = 0 AND
					THEN 1 
				ELSE 
					CASE	
						WHEN  (CG.CarGroupTypeId IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT) AND CG.IsActive = 1 --ST.IsSychronizedCW = 0 AND
							AND CONVERT(DATE,CC.ExpiryDate) >= CONVERT(DATE,GETDATE())) 
						THEN 1 
						ELSE 0 
					END 
				END IsEligible
				 --,CG.ModelId CGModelId
			FROM tc_stock St WITH (NOLOCK)
			INNER JOIN tc_stockstatus Tcs WITH (NOLOCK) ON Tcs.id = St.statusid
			INNER JOIN carversions Ve WITH (NOLOCK) ON Ve.id = St.versionid
			INNER JOIN carmodels Mo WITH (NOLOCK) ON Mo.id = Ve.carmodelid
				AND (
					@ModelId IS NULL
					OR Mo.ID = @ModelId
					)
			INNER JOIN carmakes Ma WITH (NOLOCK) ON Ma.id = Mo.carmakeid
				AND (
					@MakeId IS NULL
					OR Ma.ID = @MakeId
					)
			LEFT JOIN ConsumerCreditPoints CC WITH(NOLOCK) ON CC.ConsumerId = ST.BranchId AND CC.ConsumerType = 1 -- Added by Suresh.P  on 15/04/2016 left join
			--LEFT JOIN CarGroupTypes AS CG WITH (NOLOCK) ON CG.ModelId = Mo.Id
			LEFT JOIN CarGroupTypes AS CG WITH (NOLOCK) ON CG.VersionId = ST.VersionId
			LEFT JOIN sellinquiries Si WITH (NOLOCK) ON Si.tc_stockid = St.id
				AND Si.dealerid = @BranchId
			LEFT JOIN TC_MMDealersMatchCount Mc WITH (NOLOCK) ON Mc.StockId = St.Id
				AND Mc.DealerId = @BranchId
				AND CONVERT(DATE, Mc.LastUpdatedOn) = CONVERT(DATE, GETDATE())
			LEFT JOIN TC_MappingOfferWithStock OS WITH(NOLOCK) ON St.Id = OS.StockId
				AND OS.IsActive = 1 --Added By Vivek Gupta on 23-01-2014
			WHERE St.branchid = @BranchId 
				--AND CC.ConsumerType = 1
				--AND CG.ModelId IS NULL
				AND St.statusid = @Status
				AND (
					@MinPrice IS NULL
					OR ST.Price >= @MinPrice
					)
				AND (
					@IsParkNSale IS NULL
					OR ST.IsParkNSale = @IsParkNSale
					)
				AND (
					@SyncStatus IS NULL
					OR St.IsSychronizedCW = @syncStatus
					)
				AND (
					@MaxPrice IS NULL
					OR ST.Price <= @MaxPrice
					)
				AND (
					@MinKms IS NULL
					OR ST.Kms >= @MinKms
					)
				AND (
					@MaxKms IS NULL
					OR ST.Kms <= @MaxKms
					)
				AND (
					St.isactive = 1
					AND St.isapproved = 1
					)
				AND (
					@OfferIds IS NULL
					OR (
						@OfferIds <> '-2'
						AND OS.TC_UsedCarOfferId IN (
							SELECT OfferIds
							FROM #tempOffers WITH (NOLOCK)
							)
						)
					OR (
						@OfferIds = '-2'
						AND st.Id NOT IN (
							SELECT DISTINCT StockId
							FROM TC_MappingOfferWithStock WITH (NOLOCK)
							WHERE IsActive = 1
							)
						)
					)
				-- Modified By  :   Vivek Gupta on 04-02-2014, -- Modified By  :   Vivek Gupta on 10-02-2014\
				AND (
					@chkCertifiedCarsStatus IS NULL
					OR (
						@chkCertifiedCarsStatus = 1
						AND ST.Id IN (
							--SELECT StockId
							--FROM AbSure_CarDetails ACD WITH (NOLOCK)
							--WHERE DealerId = @BranchId
							--	AND ACD.IsActive = 1
							--	AND ACD.StockId IS NOT NULL
							--	AND (
							--		(
							--			ACD.Status = 4
							--			AND DATEDIFF(DAY, ACD.SurveyDate, GETDATE()) <= 30
							--			)
							--		OR (
							--			ACD.Status = 8
							--			--AND DATEDIFF(DAY, ACD.SurveyDate, GETDATE()) <= 30
							--			)
							--		)
								SELECT TR.ListingId 
								FROM TC_CarTradeCertificationRequests TR WITH (NOLOCK)
								INNER JOIN TC_CarTradeCertificationLiveListing TL WITH (NOLOCK) ON TR.ListingId = TL.ListingId
								WHERE TR.CertificationStatus = 1 AND TR.DealerId = @BranchId
							)							
						)
					OR (
						--@chkCertifiedCarsStatus IS NULL 
						--OR
						@chkCertifiedCarsStatus = 0
						AND ST.Id NOT IN (
								SELECT TR.ListingId 
								FROM TC_CarTradeCertificationRequests TR WITH (NOLOCK)
								WHERE TR.CertificationStatus <> 1 AND TR.DealerId = @BranchId
							)
						)
					)
				AND
				(
					(@SuspendedDate IS NULL
					OR CONVERT(DATE,ST.SuspendedDate) = CONVERT(DATE,@SuspendedDate)
					)
				)
				--------Modified By : Tejashree Patil on 28 March 2016, Changes to show stocklist based on new package and topup of dealer and cargrouptype.------
				--AND 
				--(
				--	(
				--		--if "Manage CW Listing" is not selected
				--		(ISNULL(@IsCWListing,0) = 0)
				--		OR 
				--		( 
				--			/*
				--				if "Manage CW Listing" is selected then check conditions
				--				1)Package is active : yes
				--					1.1)then check for package type, if Old package then @CarGroupType IS NULL
				--					1.2)Else if new package with or without topup show all records with cargroup condition
				--				2)Package expired   : no stock will be visible
				--			*/
				--			(@IsCWListing = 1
				--				AND ST.IsSychronizedCW = 0 AND ( 
				--					--old package 
				--					@CarGroupType IS NULL
				--					OR
				--					(CG.CarGroupTypeId IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT) AND CG.IsActive = 1
				--					AND CONVERT(DATE,CC.ExpiryDate) >= CONVERT(DATE,GETDATE()))
				--				)
				--			)
				--		)
				--	)
				--	----------------------------------------------------------------------------------------------------------------------------------------------
				--)
				-- modified by khushaboo on 4/5/2016
				AND
				(
					@IsCWListing IS NULL
					OR
					(
						@IsCWListing = 1
						AND ST.IsSychronizedCW = 0 
						AND CONVERT(DATE,CC.ExpiryDate) >= CONVERT(DATE,GETDATE()) AND( 
							--old package 
							(@CarGroupType IS NULL )
							OR
							(CG.CarGroupTypeId IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT) AND CG.IsActive = 1)
						)
					)
					-- Added by khushaboo on 4/5/2016
					OR
					(
						@IsCWListing = 0
						AND ST.IsSychronizedCW = 0 
						AND 
						( 
							CONVERT(DATE,CC.ExpiryDate) < CONVERT(DATE,GETDATE())
							OR
							(
								@CarGroupType IS NOT NULL 
								AND(
								CG.CarGroupTypeId IS NULL OR CG.IsActive = 0 OR CG.CarGroupTypeId NOT IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT)
								)
							)					
						)
					)
				)
				------------------------
			)
			,CTE2
		AS (
			SELECT *
				,ROW_NUMBER() OVER (
					ORDER BY ---1 is for asc and 2 is for desc
						CASE 
							WHEN @OrderBy = 'EntryDate~2'
								THEN EntryDate
							END DESC
						,CASE 
							WHEN @OrderBy = 'Car~1'
								THEN MakeModelVersion
							END
						,CASE 
							WHEN @OrderBy = 'Car~2'
								THEN MakeModelVersion
							END DESC
						,CASE 
							WHEN @OrderBy = 'MakeYear~1'
								THEN MakeYear
							END
						,CASE 
							WHEN @OrderBy = 'MakeYear~2'
								THEN MakeYear
							END DESC
						,CASE 
							WHEN @OrderBy = 'Kms~1'
								THEN Kms
							END
						,CASE 
							WHEN @OrderBy = 'Kms~2'
								THEN Kms
							END DESC
						,CASE 
							WHEN @OrderBy = 'Colour~1'
								THEN Colour
							END
						,CASE 
							WHEN @OrderBy = 'Colour~2'
								THEN Colour
							END DESC
						,CASE 
							WHEN @OrderBy = 'RegNo-1'
								THEN RegNo
							END
						,CASE 
							WHEN @OrderBy = 'RegNo~2'
								THEN RegNo
							END DESC
						,CASE 
							WHEN @OrderBy = 'Price~1'
								THEN Price
							END
						,CASE 
							WHEN @OrderBy = 'Price~2'
								THEN Price
							END DESC
						,CASE 
							WHEN @OrderBy = 'InquiryCount~1'
								THEN InquiryCount
							END
						,CASE 
							WHEN @OrderBy = 'InquiryCount~2'
								THEN InquiryCount
							END DESC
						,CASE 
							WHEN @OrderBy = 'EntryDate~1'
								THEN EntryDate
							END
						,CASE 
							WHEN @OrderBy = 'InvCertifiedDate~1'
								THEN InvCertifiedDate
							END
						,CASE 
							WHEN @OrderBy = 'InvCertifiedDate~2'
								THEN InvCertifiedDate
							END DESC
						,CASE 
							WHEN @OrderBy = 'SuspendedDate~1'
								THEN SuspendedDate
							END
						,CASE 
							WHEN @OrderBy = 'SuspendedDate~2'
								THEN SuspendedDate
							END DESC
					) AS RowN
			FROM CTE1
			)
		SELECT *
		INTO #TempTable
		FROM CTE2

		--SELECT *
		--FROM #TempTable

		-- OPTION (RECOMPILE);
		SELECT TOP (@ToIndex) RowN
			,Id
			,IsFeatured
			,IsBooked
			,MakeYear
			,Kms
			,Colour
			,InquiryCount
			,RegNo
			,Price
			,EntryDate
			,StatusId
			,Status
			,MakeId
			,ModelId
			,MakeModelVersion
			,IsSychronizedCW
			,Photos
			,sellInquiryId
			,MatchViewCount
			,HasActiveOffer
			,StockRating
			,-- Added By : Vishal Srivastava on 10-02-2014 
			LastUpdate
			,-- Added By : Vishal Srivastava on 10-02-2014 
			PhotoCount --Added by : Vivek Rajak on 21-07-2015
			,SuspendedDate
			,IsEligible
			,CarGroupTypeId, cgIsActive,ccExpiryDate
		FROM #TempTable
		WHERE RowN >= @FromIndex
			AND RowN <= @ToIndex
		ORDER BY RowN

		SELECT COUNT(*) AS RecordCount
		FROM #TempTable

		SELECT COUNT(*) AS RecordCount1
		FROM #TempTable WHERE IsEligible = 1

		SELECT COUNT(*) AS RecordCount2
		FROM #TempTable WHERE IsEligible = 0

		DROP TABLE #TempTable

		DROP TABLE #tempOffers -- Modified By  :   Vivek Gupta on 04-02-2014
	END    -- end for dashactivity 0 
	ELSE ------------------------------------------------------------------------------------------------FOR REQUST FROM DASHBOARD
		BEGIN

			DECLARE @IsDealerInspection BIT   =  0

			CREATE TABLE #tempOffers1 (OfferIds INT)

			INSERT INTO #tempOffers1
			SELECT *
			FROM [dbo].[fnSplitCSV](@OfferIds);

			SELECT * INTO #tempTC_Stock FROM TC_Stock WITH (NOLOCK) WHERE BranchId = @BranchId AND StatusId=1  AND IsActive = 1 AND isapproved = 1
			-- Inserted all stock of the dealer into temp table #tempTC_Stock to reduce join cost


			SELECT DISTINCT TS.Id AS StockId,TS.IsFeatured,TS.IsBooked,TS.MakeYear,TS.Kms, 
			dbo.Titlecase(TS.colour) Colour,(
					SELECT Count(DISTINCT tc_buyerinquiriesid)
					FROM tc_buyerinquiries B WITH(NOLOCK)
						,tc_inquirieslead L WITH(NOLOCK)
					WHERE B.stockid = TS.Id
						AND B.tc_leaddispositionid IS NULL
						AND B.tc_inquiriesleadid = L.tc_inquiriesleadid
						AND Isnull(L.tc_leadstageid, 0) <> 3
					) AS InquiryCount, Upper(TS.regno) RegNo,TS.Price,TS.EntryDate,TS.statusid,SS.Status,CMa.ID MakeId,CMo.ID ModelId,
					(CMa.Name + ' ' + CMo.Name + ' ' + CV.Name) AS MakeModelVersion,TS.issychronizedcw, 
					(
					SELECT TOP 1 id
					FROM tc_carphotos Cp WITH(NOLOCK)
					WHERE Cp.stockid = TS.Id
						AND Cp.isactive = 1
					) Photos,
					(
					SELECT count(CP.id) 
					FROM tc_carphotos Cp WITH (NOLOCK)
					WHERE Cp.stockid = TS.id
						AND Cp.isactive = 1
					) PhotoCount, 
					Si.ID AS sellInquiryId,Mc.MatchViewCount,OS.StockId AS HasActiveOffer,
					TS.StockRating AS StockRating,
					TS.LastStockRatingUpdate AS LastUpdate, 
			Year(TS.MakeYear) AS MkYear,
			MONTH(TS.MakeYear)AS MKMonth,
			D.CityId ,CV.ID AS VersionId, 
			CV.CarFuelType,
			 DATEDIFF(d, TS.EntryDate, GETDATE()) AS Age,
			
			DATEDIFF(d,TS.LastUpdatedDate,GETDATE()) AS LastUpdatedInDay,

			(SELECT COUNT(DISTINCT TC_InquiriesLeadId) FROM TC_BuyerInquiries WITH (NOLOCK)
			WHERE StockId =TS.Id) Response,	
			CC.Comments,-- DFC.DFC_Id,
			DATEDIFF(m,TS.MakeYear,GETDATE())*1250 AS ValidKm ,
			CASE WHEN TS.IsSychronizedCW = 0 AND @CarGroupType IS NULL AND CONVERT(DATE,CCP.ExpiryDate) >= CONVERT(DATE,GETDATE())  THEN 1 ELSE CASE WHEN TS.IsSychronizedCW = 0 AND (CG.CarGroupTypeId IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT) AND CG.IsActive = 1
				AND CONVERT(DATE,CCP.ExpiryDate) >= CONVERT(DATE,GETDATE())) THEN 1 ELSE 0 END END IsEligible
			INTO #DashboardTempTable               ------------------------------------ we will apply desied query on #DashboardTempTable 
 

			FROM
			#tempTC_Stock TS  
			INNER JOIN	Dealers D WITH(NOLOCK) ON D.ID = TS.BranchId
			INNER JOIN  CarVersions CV WITH (NOLOCK) ON TS.VersionId = CV.ID
			INNER JOIN	CarModels CMo WITH (NOLOCK) ON CV.CarModelId = CMo.ID 
			INNER JOIN	CarMakes CMa WITH (NOLOCK) ON CMo.CarMakeId = CMa.ID 
			LEFT JOIN TC_CarCondition CC WITH (NOLOCK) ON TS.Id = CC.StockId
			INNER JOIN TC_StockStatus SS WITH (NOLOCK) ON TS.StatusId =SS.Id
			LEFT JOIN sellinquiries Si WITH (NOLOCK) ON Si.tc_stockid = TS.id
			AND Si.dealerid = @BranchId
			LEFT JOIN TC_MMDealersMatchCount Mc WITH (NOLOCK) ON Mc.StockId = TS.Id
			AND Mc.DealerId = @BranchId
			AND CONVERT(DATE, Mc.LastUpdatedOn) = CONVERT(DATE, GETDATE())
			LEFT JOIN TC_MappingOfferWithStock OS WITH(NOLOCK) ON TS.Id = OS.StockId AND OS.IsActive = 1
			LEFT JOIN ConsumerCreditPoints CCP WITH(NOLOCK) ON CCP.ConsumerId = TS.BranchId AND CCP.ConsumerType = 1 -- Added by Suresh.P  on 15/04/2016 left join
			--LEFT JOIN CarGroupTypes AS CG WITH (NOLOCK) ON CG.ModelId = Mo.Id
			LEFT JOIN CarGroupTypes AS CG WITH (NOLOCK) ON CG.VersionId = TS.VersionId
			WHERE
					(
					@MakeId IS NULL
					OR CMa.ID = @MakeId
					)
				AND(
					@ModelId IS NULL
					OR CMo.ID = @ModelId
					)
				AND(
					@MinPrice IS NULL
					OR TS.Price >= @MinPrice
					)
				AND (
					@IsParkNSale IS NULL
					OR TS.IsParkNSale = @IsParkNSale
					)
				AND (
					@SyncStatus IS NULL
					OR TS.IsSychronizedCW = @syncStatus
					)
				AND (
					@MaxPrice IS NULL
					OR TS.Price <= @MaxPrice
					)
				AND (
					@MinKms IS NULL
					OR TS.Kms >= @MinKms
					)
				AND (
					@MaxKms IS NULL
					OR TS.Kms <= @MaxKms
					)
				AND(-----------------------------------------------------------ASTART
					@OfferIds IS NULL
					OR (
						@OfferIds <> '-2'
						AND OS.TC_UsedCarOfferId IN (
							SELECT OfferIds
							FROM #tempOffers1 WITH (NOLOCK)
							)
						)
					OR (
						@OfferIds = '-2'
						AND TS.Id NOT IN (
							SELECT DISTINCT StockId
							FROM TC_MappingOfferWithStock WITH (NOLOCK)
							WHERE IsActive = 1
							)
						)
					)--------------------------------------------------------------AEND
					AND
					 (
						@chkCertifiedCarsStatus IS NULL
						OR
						 (----------------------------------------------------ORA-START
							@chkCertifiedCarsStatus = 1
							AND TS.Id IN
							 (
								SELECT StockId
								FROM AbSure_CarDetails ACD WITH (NOLOCK)
								WHERE DealerId = @BranchId
									AND ACD.IsActive = 1
									AND ACD.StockId IS NOT NULL
									AND	
									(
										(
										ACD.Status = 4
										AND DATEDIFF(DAY, ACD.SurveyDate, GETDATE()) <= 30
										)	
									OR ACD.Status = 8
									)
							)
						)-------------------------------------------------------ORA-END
					OR (
							@chkCertifiedCarsStatus = 0
							AND TS.Id NOT IN
							(
								SELECT DISTINCT StockId
								FROM AbSure_CarDetails ACD WITH (NOLOCK)
								WHERE DealerId = @BranchId
								AND IsActive = 1
								AND StockId IS NOT NULL
							)
						)
						)
						------------------------- Added by khushaboo on 4/5/2016
						AND
						(
							@IsCWListing IS NULL
							OR
							(
								@IsCWListing = 1
								AND TS.IsSychronizedCW = 0 
								AND CONVERT(DATE,CCP.ExpiryDate) >= CONVERT(DATE,GETDATE()) AND( 
									--old package 
									(@CarGroupType IS NULL )
									OR
									(CG.CarGroupTypeId IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT) AND CG.IsActive = 1)
							)
						)
						OR
						(
							@IsCWListing = 0
							AND TS.IsSychronizedCW = 0 
							AND 
							( 
								CONVERT(DATE,CCP.ExpiryDate) < CONVERT(DATE,GETDATE())
								OR
								(
									@CarGroupType IS NOT NULL 
									AND(
									CG.CarGroupTypeId IS NULL OR CG.IsActive = 0 OR CG.CarGroupTypeId NOT IN (SELECT DISTINCT CT.ListMember FROM fnSplitCSVValues(@CarGroupType) AS CT)
									)
								)					
							)
						)
					)
					------------------------
					
			DECLARE  @DisplayForDashboardRequest TABLE  ----- this is common table, only one of all "if"(written below) part insert data in this  
			(
				StockId NUMERIC NOT NULL,
				IsFeatured BIT ,
				IsBooked BIT,
				MakeYear DATETIME NOT NULL,
				Kms NUMERIC NOT NULL,
				Colour VARCHAR(MAX) NOT NULL,
				InquiryCount NUMERIC NOT NULL,
				RegNo VARCHAR(MAX) NOT NULL,
				Price NUMERIC NOT NULL,
				EntryDate DATETIME NOT NULL,
				statusid INT NOT NULL,
				Status VARCHAR(MAX) NOT NULL,
				MakeId NUMERIC NOT NULL,
				ModelId NUMERIC NOT NULL,
				MakeModelVersion VARCHAR(MAX),
				issychronizedcw BIT,
				Photos NUMERIC,
				PhotoCount INT,
				sellInquiryId NUMERIC,
				MatchViewCount INT,
				HasActiveOffer INT,
				StockRating FLOAT, 
				LastUpdate DATETIME,
				MkYear INT,
				MKMonth INT,
				CityId NUMERIC,
				VersionId NUMERIC,
				CarFuelType INT,
				Age  INT,
				LastUpdatedInDay INT,
				Response INT,
				Comments VARCHAR(MAX),
				ValidKm NUMERIC,
				IsEligible BIT,
				RowN NUMERIC
			)

			  
			IF(@DashboardActivity = 1) ----------------- FOR KMS OUT OF RANGE
			BEGIN 
				SELECT * INTO #KmsOutOfRange FROM #DashboardTempTable
				WHERE Kms>ValidKm

				INSERT INTO @DisplayForDashboardRequest
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN 
				FROM #KmsOutOfRange

				DROP TABLE #KmsOutOfRange
			END

			IF(@DashboardActivity = 2) -------------------- FOR WITHOUT PHOTO
			BEGIN 
			
				SELECT * INTO #WithoutPhotoTable FROM #DashboardTempTable
				WHERE PhotoCount=0
				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #WithoutPhotoTable
				

				DROP TABLE #WithoutPhotoTable
			END
			IF(@DashboardActivity = 4) ---------------------------------FOR LESS PHOTO
			BEGIN
				SELECT * INTO #LessPhotoTable FROM #DashboardTempTable
				WHERE PhotoCount>0 AND PhotoCount <5

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #LessPhotoTable

				DROP TABLE #LessPhotoTable
			END

			IF(@DashboardActivity = 5) ---------------------------------FOR FEATURED CAR
			BEGIN
				SELECT * INTO #FeaturedTable FROM #DashboardTempTable
				WHERE IsFeatured = 1;

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #FeaturedTable

				DROP TABLE #FeaturedTable
			END
			IF(@DashboardActivity = 6) ---------------------------------FOR POOR RESPONSE
			BEGIN
				SELECT * INTO #PoorResponse FROM #DashboardTempTable
				WHERE Response < 5 AND IsSychronizedCW = 1

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #PoorResponse

				DROP TABLE #PoorResponse
			END

			IF(@DashboardActivity = 7) ---------------------------------FOR CARS NOT MOVING
			BEGIN
				SELECT * INTO #CarNotMoving FROM #DashboardTempTable
				WHERE Age >30

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #CarNotMoving

				DROP TABLE #CarNotMoving
			END

			IF(@DashboardActivity = 8) ---------------------------------FOR CARS Older Than One Year
			BEGIN
				SELECT * INTO #OlderThanOneYear FROM #DashboardTempTable
				WHERE Age >365

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #OlderThanOneYear

				DROP TABLE #OlderThanOneYear
			END

			IF(@DashboardActivity = 9) ---------------------------------FOR Updated Before One Month
			BEGIN
				SELECT * INTO #UpdatedBefore FROM #DashboardTempTable
				WHERE LastUpdatedInDay > 30

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #UpdatedBefore

				DROP TABLE #UpdatedBefore
			END

			IF(@DashboardActivity = 10) ---------------------------------FOR CARS OLDER THAN 6 Months
			BEGIN
				SELECT * INTO #OlderThanSixMonths FROM #DashboardTempTable
				WHERE Age >180

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #OlderThanSixMonths

				DROP TABLE #OlderThanSixMonths
			END
			IF(@DashboardActivity = 12) ---------------------------------FOR CARS NOT INSPECTED 
			BEGIN

					SELECT DT.* INTO #AllRowForInspection
					FROM
					#DashboardTempTable DT INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON DT.ModelId = AE.ModelId AND AE.IsActive = 1
					 AND AE.IsEligibleCertification =1
					LEFT JOIN AbSure_CarDetails  ABCD WITH(NOLOCK) ON DT.StockId = ABCD.StockId AND ABCD.IsActive = 1
					WHERE ABCD.StockId IS NULL OR ABCD.Status  = 3 OR(ABCD.IsSurveyDone = 1  AND DATEDIFF(DAY,ABCD.SurveyDate,GETDATE()) > 30 AND ABCD.Status = 4)-- 3 FOR CANCEAL 

					INSERT INTO @DisplayForDashboardRequest
					SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN FROM #AllRowForInspection

					DROP TABLE #AllRowForInspection
			END

			IF(@DashboardActivity = 13) ---------------------------------FOR WARRANTY NOT REQUESTED
			BEGIN

					SELECT DT.* INTO #AllRowForWarrnty
					FROM
					#DashboardTempTable DT INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON DT.ModelId = AE.ModelId AND AE.IsActive =1
					AND AE.IsEligibleWarranty=1  AND DT.Kms <=85000
					AND (DATEDIFF(MONTH,DT.MakeYear,GETDATE()) > 24 AND DATEDIFF(MONTH,DT.MakeYear,GETDATE()) <= 72)
					AND DT.CarFuelType IN (1,2,3)                                            -------- Condition taken from SP [AbSure_IsWarranty]
					LEFT JOIN AbSure_CarDetails ABCD WITH(NOLOCK) ON DT.StockId = ABCD.StockId AND ABCD.IsActive =1
					WHERE  (((ABCD.IsSurveyDone = 1  OR ABCD.Status = 4) AND DATEDIFF(DAY,ABCD.SurveyDate,GETDATE()) > 30 )--- 4 FOR accepted but date expired
					OR ABCD.StockId IS NULL OR ABCD.Status = 3)

					INSERT INTO @DisplayForDashboardRequest
					SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN FROM #AllRowForWarrnty

					DROP TABLE #AllRowForWarrnty
			END

			IF(@DashboardActivity = 14) ---------------------------------FOR CARS WITHOUT DESCRIPTION
			BEGIN
				SELECT * INTO #WithoutDescription FROM #DashboardTempTable
				WHERE Comments = '' OR Comments IS NULL

				INSERT INTO @DisplayForDashboardRequest 
				SELECT *,ROW_NUMBER() OVER (ORDER BY StockId)AS RowN  
				FROM #WithoutDescription

				DROP TABLE #WithoutDescription
			END

			SELECT TOP (@ToIndex) RowN
			,StockId AS Id
			,IsFeatured
			,IsBooked
			,MakeYear
			,Kms
			,Colour
			,InquiryCount
			,RegNo
			,Price
			,EntryDate
			,StatusId
			,Status
			,MakeId
			,ModelId
			,MakeModelVersion
			,IsSychronizedCW
			,Photos
			,sellInquiryId
			,MatchViewCount
			,HasActiveOffer
			,StockRating
			,LastUpdate
			,PhotoCount 
			,IsEligible
		FROM @DisplayForDashboardRequest
		WHERE RowN >= @FromIndex
			AND RowN <= @ToIndex
		ORDER BY RowN

		SELECT COUNT(*) AS RecordCount
		FROM @DisplayForDashboardRequest

		
		DROP TABLE #tempTC_Stock
		DROP TABLE #DashboardTempTable
		DROP TABLE #tempOffers1
		

		END ----------END for else 
END

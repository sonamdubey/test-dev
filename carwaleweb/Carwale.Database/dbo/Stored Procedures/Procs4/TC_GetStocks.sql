IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStocks]
GO

	
-- Modified By : Surendra on 16 Oct 2012, resolved the problem of displaying make twice
-- Author: Surendra Chouksey  
-- Create date: 10th July 2012  
-- Description: Getting All STock for dealer  
-- execute TC_GetStocks 5,NULL '2013-02-11 11:30AM'
-- Modified By : Nilesh Utture on 19 November, 2012, Added Order by st.Id so that newly added stock is displayed at top
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By: Nilesh Utture on 13th March, 2013 Added IsActive, StatusId in SELECT Statement
-- Modified By: Nilesh Utture on 29th March, 2013 Added IsFeatured, IsParkNSale, CertificationId in SELECT Statement
-- Modified By: Vivek Gupta on 30-10-2013, Added FuelType, TransmissionType, Status, BodyStyleId, DirectoryPath,MakeId
-- Modified By Vivek Gupta on 26-11-2013, Added EntryDate And VersionId
-- Modified By Tejashree Patil on 8/8/2014, Fetched ProfileId.
-- Modified By Vivek Gupta on 11-11-2014, Added ModelId
-- Modified By Vivek Gupta on 24-03-2014, added StockViews and MyInq for API Call
-- Modified By Vivek Rajak on 21-07-2015, added photocount for API Call
-- Modified By Vivek Rajak on 21-07-2015, implemented pagination logic
-- Modified By Vicky Gupta on 14-08-2015, added filters for api
-- Modified By : Suresh Prajapati on 19th Aug, 2015
-- Description : Added validation to get only Available Stocks
-- Modified By : Suresh Prajapati on 21st Aug, 2015
-- Description : Modified Make Year filter to get stocks only by specified year
-- EXEC TC_GetStocks 5,null,1,1000,null,null,null,null,null,null
-- Modified By : Khushaboo Patil on 4/4/2016 added filter for suspended cars
--Modified By : Ashwini Dhamankar on April 22,2016 (Added constraint of isMain and isActive on TC_CarPhotos)
-- ============================================================================================================================  
CREATE PROCEDURE [dbo].[TC_GetStocks] (
	@BranchId BIGINT
	,@LastUpdatedDate DATETIME = NULL
	,@FromIndex INT = NULL
	,@ToIndex INT = NULL
	,@MakeId INT = NULL
	,--Added by vicky Gupta on 14-08-2015
	@ModelId INT = NULL
	,@VersionId INT = NULL
	,@MinPrice INT = NULL
	,@MaxPrice INT = NULL
	,@MakeYear VARCHAR(5) = NULL
	,@MinKms INT = NULL
	,@MaxKms INT = NULL
	,@Status TINYINT = 1
	,@UploadedToCW BIT = NULL
	,@SuspendedDate DATETIME = NULL
	)
AS
BEGIN
	-- interfering with SELECT STatements.  
	SET NOCOUNT ON;

	DECLARE @IsCertified BIT = 0;

	-- Modified By: Nilesh Utture on 13th March, 2013  
	-- Modified By: Vivek Gupta on 30-10-2013
	WITH Cte1
	AS (
		SELECT ST.IsActive
			,ST.StatusId
			,ST.Id StockId
			,V.Make + ' ' + V.Model + ' ' + V.Version AS CarName
			,ST.MakeYear
			,ST.LastUpdatedDate
			,ST.Kms
			,ST.Price
			,ST.Colour Color
			,ST.IsSychronizedCW
			,ST.IsBooked
			,ISNULL(P.HostUrl, '') + ISNULL(P.DirectoryPath, '') + ISNULL(P.ImageUrlMedium, '') AS CarImage
			,CASE 
				WHEN ST.CertificationId IS NULL
					THEN CONVERT(BIT, 0)
				ELSE CONVERT(BIT, 1)
				END AS IsCertified
			,ST.IsFeatured
			,ST.IsParkNSale
			,V.BodyStyleId
			,V.CarFuelType AS FuelType
			,P.DirectoryPath
			,CASE V.CarTransmission
				WHEN 1
					THEN 'Automatic'
				WHEN 2
					THEN 'Manual'
				END AS TransmissionType
			,CASE ST.StatusId
				WHEN 1
					THEN 'Available'
				WHEN 2
					THEN 'Not Available'
				WHEN 3
					THEN 'Sold'
				WHEN 4
					THEN 'Suspended'
				END AS STATUS
			,V.MakeId
			,ST.EntryDate
			,ST.VersionId
			,V.ModelId
			-- Modified By Vivek Gupta on 26-11-2013, Added EntryDate And VersionId
			-- 'D' + CONVERT(VARCHAR(15), SI.ID) AS ProfileId
			-- ,ISNULL(SI.ViewCount, 0) AS StockViews
			,(
				SELECT count(cp.id) --Added by vivek rajak to count photos on 21-07-2015
				FROM tc_carphotos Cp WITH (NOLOCK)
				WHERE Cp.stockid = ST.id
					AND Cp.isactive = 1
				) PhotoCount
			,(
				SELECT Count(DISTINCT tc_buyerinquiriesid)
				FROM tc_buyerinquiries B
					,tc_inquirieslead L
				WHERE B.stockid = ST.id
					AND B.tc_leaddispositionid IS NULL
					AND B.tc_inquiriesleadid = L.tc_inquiriesleadid
					AND Isnull(L.tc_leadstageid, 0) <> 3
				) AS MyInq
			,ISNULL(Mc.MatchViewCount, 0) AS MatchViewCount
		FROM TC_Stock ST WITH (NOLOCK)
		INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.STockId = ST.Id
		INNER JOIN vwMMV V WITH (NOLOCK) ON ST.VersionId = V.VersionId
		LEFT OUTER JOIN TC_CarPhotos P WITH (NOLOCK) ON ST.Id = P.StockId
			AND P.IsActive = 1
			AND P.IsMain = 1
		LEFT JOIN TC_MMDealersMatchCount Mc WITH (NOLOCK) ON Mc.StockId = ST.Id
		--LEFT JOIN SellInquiries SI WITH (NOLOCK) ON St.Id = SI.TC_StockId
		--	AND ST.IsSychronizedCW = 1
		--	AND SI.StatusId = 1
		--	AND SI.DealerId = @BranchId
		WHERE ST.BranchId = @BranchId
			AND IsApproved = 1
			AND (
				ST.LastUpdatedDate > @LastUpdatedDate
				OR (
					@LastUpdatedDate IS NULL
					AND ST.IsActive = 1
					)
				)
			AND (
				@MakeId IS NULL
				OR V.MakeId = @MakeId
				)
			AND (
				@ModelId IS NULL
				OR V.ModelId = @ModelId
				)
			AND (
				@VersionId IS NULL
				OR V.VersionId = @VersionId
				)
			AND (
				@MinPrice IS NULL
				OR ST.Price >= @MinPrice
				)
			AND (
				@MaxPrice IS NULL
				OR ST.Price <= @MaxPrice
				)
			AND (
				@MakeYear IS NULL
				OR YEAR(ST.MakeYear) = YEAR(@MakeYear)
				)
			AND (
				@MinKms IS NULL
				OR ST.Kms >= @MinKms
				)
			AND (
				@MaxKms IS NULL
				OR ST.Kms <= @MaxKms
				)
			AND (ST.StatusId = ISNULL(@Status, 1)) --Added By Suresh Prajapati on 19th Aug, 2015
			AND (
				@UploadedToCW IS NULL
				OR ST.IsSychronizedCW = @UploadedToCW
				)
			AND (
				(
					@SuspendedDate IS NULL
					OR CONVERT(DATE, ST.SuspendedDate) = CONVERT(DATE, @SuspendedDate)
					)
				)
			--ORDER BY ST.Id DESC
		)
		,Cte2
	AS (
		SELECT *
			,ROW_NUMBER() OVER (
				PARTITION BY StockId ORDER BY StockId DESC
				) RowNumber
		FROM Cte1
		)
	SELECT *
		,ROW_NUMBER() OVER (
			ORDER BY StockId DESC
			) NumberForPaging
	INTO #TblTemp
	FROM Cte2
	WHERE RowNumber = 1

	SELECT *
	FROM #TblTemp
	WHERE (
			@FromIndex IS NULL
			AND @ToIndex IS NULL
			)
		OR (
			NumberForPaging BETWEEN @FromIndex
				AND @ToIndex
			)

	SELECT COUNT(*) AS RecordCount
	FROM #TblTemp

	DROP TABLE #TblTemp
END

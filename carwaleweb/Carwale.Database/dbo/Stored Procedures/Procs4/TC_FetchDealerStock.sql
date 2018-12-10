IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchDealerStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchDealerStock]
GO

	-- =============================================
-- Author : Vicky Gupta
-- Purpose  : to get stock information of given dealer. To show on dashboard
-- Date : 29/09/2015
-- Modified by: Nilima More On 5th Oct 2016,modified insert into temtable
-- [TC_FetchDealerStock] 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchDealerStock] 
(
	@DealerId NUMERIC
)
AS
	DECLARE @NotUploadedCarwale INT =0		 -- to count no. of stock not uploaded to carwale
	DECLARE @CarNotInspected INT =0			 -- to count  no. of Stock eligible for Inspection but not inspected
	DECLARE @NotRequestedWarranty INT = 0    -- to count no. of stock for which  Warranty not requested

BEGIN

-- added by: Nilima More On 5th Oct 2016,modified insert into temtable
	CREATE TABLE  #tempTC_Stock (Id numeric,BranchId int,VersionId numeric,StatusId int,Price numeric,Kms numeric,MakeYear datetime,Colour varchar(50) ,RegNo varchar(50)
	,EntryDate datetime,LastUpdatedDate datetime,IsActive bit ,IsSychronizedCW bit
	,CertificationId smallint,IsFeatured bit,IsBooked bit ,ModifiedBy int ,IsApproved bit,IsParkNSale bit,TC_SellerInquiriesId bigint,CertifiedLogoUrl varchar(200)
	,AlternatePrice bigint,StockRating float
	,LastStockRatingUpdate datetime,SoldToCustomerFrom varchar(200),ShowEMIOnCarwale bit,InterestRate float,LoanToValue float,LoanAmount int,Tenure smallint,OtherCharges int 
	,EMI int,BranchLocationId int
	,MFCSourceId int, IsWarrantyAccepted bit,WarrantyAcceptDate datetime,IsWarrantyRequested bit,TC_ActionApplicationId int,ISPointsGiven bit,
	PurchaseCost int,RefurbishmentCost int,SoldToCustomerId bigint,VIN varchar(19),SuspendedDate datetime)



	INSERT INTO #tempTC_Stock
	(Id ,BranchId ,VersionId ,StatusId ,Price ,Kms ,MakeYear ,Colour ,RegNo 
	,EntryDate ,LastUpdatedDate ,IsActive  ,IsSychronizedCW 
	,CertificationId ,IsFeatured ,IsBooked  ,ModifiedBy  ,IsApproved ,IsParkNSale ,TC_SellerInquiriesId ,CertifiedLogoUrl 
	,AlternatePrice ,StockRating 
	,LastStockRatingUpdate ,SoldToCustomerFrom,ShowEMIOnCarwale ,InterestRate ,LoanToValue ,LoanAmount ,Tenure ,OtherCharges  
	,EMI ,BranchLocationId 
	,MFCSourceId , IsWarrantyAccepted ,WarrantyAcceptDate ,IsWarrantyRequested ,TC_ActionApplicationId ,ISPointsGiven ,
	PurchaseCost ,RefurbishmentCost ,SoldToCustomerId ,VIN ,SuspendedDate )

	SELECT * FROM TC_Stock WITH (NOLOCK) WHERE BranchId = @DealerId AND StatusId=1  AND IsActive = 1 AND IsApproved = 1
	-- Inserted all stock of the dealer into temp table #tempTC_Stock to reduce join cost


	SELECT DISTINCT TS.Id,TS.IsFeatured, TS.Kms,TS.IsSychronizedCW,
	TS.Price, DATEDIFF(d, TS.EntryDate, GETDATE()) AS Age,
	
	DATEDIFF(d,TS.LastUpdatedDate,GETDATE()) AS LastUpdated, --- To get result of stocks updated before 1 month
	
	(SELECT COUNT(StockId)FROM TC_CarPhotos WITH (NOLOCK) WHERE isActive=1 AND StockId = TS.Id)  -- to get result for stock without photo and less photo
	AS CarPhoto,

	(SELECT COUNT(DISTINCT TC_InquiriesLeadId) FROM TC_BuyerInquiries WITH (NOLOCK)
	WHERE StockId =TS.Id) Response,
	
	CC.Comments, DATEDIFF(m,TS.MakeYear,GETDATE())*1250 AS ValidKm    -- validkm is here for to get result for out of range kms stocks
 

	FROM
	#tempTC_Stock TS WITH (NOLOCK) 
	INNER JOIN  CarVersions CV WITH (NOLOCK) ON TS.VersionId = CV.ID   -- to match exact count from stock list page
	INNER JOIN	CarModels CMo WITH (NOLOCK) ON CV.CarModelId = CMo.ID 
	INNER JOIN	CarMakes CMa WITH (NOLOCK) ON CMo.CarMakeId = CMa.ID 
	LEFT JOIN TC_CarCondition CC WITH (NOLOCK) ON TS.Id = CC.StockId
	ORDER BY TS.Id


	SELECT @NotUploadedCarwale=COUNT(Id) from TC_Stock WITH (NOLOCK) WHERE IsActive = 1 AND StatusId = 1 AND IsSychronizedCW =0 AND IsApproved = 1 AND BranchId = @DealerId 
	-- Count not uploaded to carwale


		SELECT @CarNotInspected = COUNT(TST.Id)   --- counts for stocks not inspected
		FROM 
		#tempTC_Stock TST INNER JOIN CarVersions CVS WITH(NOLOCK) ON  TST.VersionId = CVS.ID 
		INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON CVS.CarModelId = AE.ModelId AND AE.IsActive = 1
		AND AE.IsEligibleCertification =1
		LEFT JOIN AbSure_CarDetails  ABCD WITH(NOLOCK) ON TST.Id = ABCD.StockId AND ABCD.IsActive = 1
		WHERE ABCD.StockId IS NULL OR ABCD.Status  = 3 OR(ABCD.IsSurveyDone = 1  AND DATEDIFF(DAY,ABCD.SurveyDate,GETDATE()) > 30 AND ABCD.Status = 4)-- 3 FOR CANCEAL 


		CREATE TABLE #EligibleForWarranty (Id numeric);
	    
		INSERT INTO  #EligibleForWarranty (Id)
		SELECT ST.Id --INTO #EligibleForWarranty
		FROM 
		#tempTC_Stock ST INNER JOIN CarVersions CVS WITH(NOLOCK) ON  ST.VersionId = CVS.ID 
		INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON CVS.CarModelId = AE.ModelId AND AE.IsActive = 1
		WHERE AE.IsEligibleWarranty=1 
		AND ST.Kms <= 85000 
		AND (DATEDIFF(MONTH,ST.MakeYear,GETDATE()) > 24 AND DATEDIFF(MONTH,ST.MakeYear,GETDATE()) <= 72)
		AND CVS.CarFuelType IN (1,2,3)                                     -------- Condition taken from SP [AbSure_IsWarranty]
		

		SELECT @NotRequestedWarranty = COUNT(EFW.Id) --- stocks for which warranty not requested
		FROM 
		#EligibleForWarranty EFW LEFT JOIN AbSure_CarDetails ABCD WITH(NOLOCK) ON EFW.Id = ABCD.StockId AND ABCD.IsActive = 1
		WHERE  (((ABCD.IsSurveyDone = 1  OR  ABCD.Status = 4 ) AND DATEDIFF(DAY,ABCD.SurveyDate,GETDATE()) > 30) --- FOR accepted but date expired
		OR ABCD.StockId IS NULL OR ABCD.Status = 3)
		

	

	CREATE TABLE #SecondTable
	(
    NotUploadedCarwale INT,
	CarNotInspected INT,
	NotRequestedWarranty INT
	)
	INSERT INTO #SecondTable VALUES(@NotUploadedCarwale,@CarNotInspected,@NotRequestedWarranty)
	SELECT * FROM #SecondTable

	DROP TABLE #tempTC_Stock
	DROP TABLE #EligibleForWarranty
	DROP TABLE #SecondTable


END


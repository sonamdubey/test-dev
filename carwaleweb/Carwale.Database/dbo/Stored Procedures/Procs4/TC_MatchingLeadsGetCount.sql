IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MatchingLeadsGetCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MatchingLeadsGetCount]
GO

	
-- Author:		Tejashree Patil
-- Create date: 27 Jan,2013
-- Description:	GET COUNT OF ALL MATCHING LEADS(BUYER,SELLER,STOCK,OLDSTOCK)
-- execute TC_MatchingLeadsGetCount 5,null,null,80
---Modified: --Added|  Ranjeet || removed self count of the stock.
--=============================================

CREATE PROCEDURE [dbo].[TC_MatchingLeadsGetCount]
@BranchId BIGINT,
@StockId BIGINT,-- this will be null when when refer from inquiries page
@TC_SellerInquiriesId BIGINT, -- this will be null when when refer from stock pages,
@TC_BuyInqWithoutStockId BIGINT -- this will be null when when refer from stock pages,
AS
BEGIN
	
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
	
	DECLARE @FromPrice BIGINT,@ToPrice BIGINT
	DECLARE @FromMakeYear DATETIME,@ToMakeYear DATETIME
	DECLARE @ModelIds VARCHAR(100),@BodyType VARCHAR(100),@FuelType VARCHAR(100)
	DECLARE @ModelId INT, @MakeId INT, @CarName VARCHAR(100) -- will used to display car name in matching laed popuup
	DECLARE @ModelNames VARCHAR(500),@BodyTypeNames VARCHAR(500),@FuelTypeNames VARCHAR(500)
	
	/*
	-- Getting match criteria and Carname based on input
	IF(@StockId IS NOT NULL) -- Match is clicked from either Stock list page or Buyer Inquiry
	BEGIN	
		SELECT @ModelId=V.ModelId,@CarName=V.Car
		FROM TC_Stock S WITH (NOLOCK)
		INNER JOIN vwMMV V ON S.VersionId=V.VersionId WHERE S.Id=@StockId
		
	END
	ELSE IF(@TC_SellerInquiriesId IS NOT NULL) -- Match is clicked from seller inquiry
	BEGIN	
		SELECT @ModelId=V.ModelId ,@MakeId=V.MakeId,@CarName=V.Car
		FROM TC_SellerInquiries S WITH (NOLOCK)
		INNER JOIN TC_InquiriesLead T WITH (NOLOCK) ON S.TC_InquiriesLeadId=T.TC_InquiriesLeadId
		INNER JOIN vwMMV V ON S.CarVersionId=V.VersionId 				
		WHERE S.TC_SellerInquiriesId=@TC_SellerInquiriesId 
	END
	ELSE -- Match is clicked from Loose inquiry
	BEGIN 				 
		SELECT @FromPrice=B.PriceMin,@ToPrice=B.PriceMax ,
		@FromMakeYear= CONVERT(VARCHAR,B.MakeYearFrom),
		@ToMakeYear=CONVERT(VARCHAR,B.MakeYearTo)
		--,
		--@BodyType=BS.BodyType,
		--@FuelType=FT.FuelType--,@ModelIds=COALESCE(@ModelIds+',','') + convert(VARCHAR,MM.ModelId)
		
		FROM TC_BuyerInquiries B WITH (NOLOCK)
		LEFT JOIN TC_PrefBodyStyle BS WITH (NOLOCK) ON BS.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId	
		LEFT JOIN TC_PrefFuelType FT WITH (NOLOCK) ON FT.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId	
		LEFT JOIN TC_PrefModelMake MM WITH (NOLOCK) ON MM.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId	
		LEFT JOIN TC_CarFuelType TCCF WITH (NOLOCK) ON TCCF.FuelTypeId=FT.FuelType
		LEFT JOIN CarBodyStyles CBS WITH (NOLOCK) ON CBS.ID=BS.BodyType
		WHERE B.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId 
	END
	
	-- Here we are retrieving count based on match criteria
	DECLARE @TotalBuyerInq SMALLINT,@TotalSellerInq SMALLINT ,@TotalMatchStock SMALLINT,@TotalMatchStockOld SMALLINT 	

	IF (@TC_BuyInqWithoutStockId IS NULL)
		BEGIN	
			-- Total Buyer Inquiry
			SELECT @TotalBuyerInq=COUNT(*) 
				FROM TC_BuyerInquiries B  WITH (NOLOCK)
					INNER JOIN TC_Stock S WITH (NOLOCK)ON  B.StockId=S.Id
					INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
					INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId=B.TC_InquiriesLeadId
					WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId 
					AND StatusId =1 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0) AND B.TC_LeadDispositionId IS NULL
					AND IL.TC_LeadStageId <> 3	
			-- Total Seller Inquiry
			SELECT @TotalSellerInq=COUNT(S.TC_SellerInquiriesId)
				FROM TC_SellerInquiries S WITH (NOLOCK)
				INNER JOIN TC_InquiriesLead IL WITH (NOLOCK)ON S.TC_InquiriesLeadId=IL.TC_InquiriesLeadId
				INNER JOIN vwMMV V ON S.CarVersionId=V.VersionId 				
				WHERE V.ModelId=@ModelId AND IL.BranchId=@BranchId AND S.TC_SellerInquiriesId <> ISNULL(@TC_SellerInquiriesId,0)	
					  AND S.PurchasedStatus IS NULL AND S.TC_LeadDispositionId IS NULL	AND IL.TC_LeadStageId <> 3	
			-- Total Match Stock
			SELECT @TotalMatchStock=COUNT(S.Id)
				FROM TC_Stock S WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId
				AND StatusId =1 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0)	
				AND IsApproved=1 
			-- Total Match Old Stock
			SELECT @TotalMatchStockOld=COUNT(S.Id)
				FROM TC_Stock S WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId
				AND StatusId =3 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0)	
				AND IsApproved=1
		END
	ELSE --in case of loose inq 
		BEGIN
			DECLARE @tblModel TABLE(ModelId INT)
			DECLARE @ModelMakeCount AS TINYINT
			SELECT @ModelMakeCount=COUNT(*) FROM TC_PrefModelMake WHERE TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND IsActive=1
			
			IF(@ModelMakeCount=0)
			BEGIN
				INSERT INTO @tblModel(ModelId) SELECT Id FROM CarModels M WHERE M.IsDeleted=0				
			END
			ELSE
			BEGIN
				INSERT INTO @tblModel(ModelId)
				SELECT	M.ModelId 
				FROM	TC_PrefModelMake M WITH(NOLOCK)
				WHERE	M.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND M.IsActive=1
				
				SELECT	@ModelNames = COALESCE(@ModelNames+',','') + CM.Name 
				FROM	CarModels CM
				WHERE	CM.ID IN (SELECT ModelId FROM @tblModel)
			END
					
			
			DECLARE @tbBody TABLE(BodyStyleId INT)
			DECLARE @BodyStyleCount AS TINYINT
			SELECT @BodyStyleCount=COUNT(*) FROM TC_PrefBodyStyle WHERE TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND IsActive=1
			
			
			IF(@BodyStyleCount=0)
			BEGIN
				INSERT INTO @tbBody(BodyStyleId) SELECT ID FROM CarBodyStyles 
			END
			ELSE
			BEGIN
				INSERT INTO @tbBody(BodyStyleId)
				SELECT	BS.BodyType
				FROM	TC_PrefBodyStyle BS WITH(NOLOCK)
				WHERE	BS.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND BS.IsActive=1				
				
				SELECT	@BodyTypeNames = COALESCE(@BodyTypeNames+',','') + CB.Name
				FROM	CarBodyStyles CB
				WHERE	CB.ID IN (SELECT BodyStyleId FROM @tbBody)
			END
			
			DECLARE @tblFuel TABLE(FuelTypeId INT)
			DECLARE @FuelTypeCount AS TINYINT
			SELECT @FuelTypeCount=COUNT(*) FROM TC_PrefFuelType WHERE TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND IsActive=1
			
			IF(@FuelTypeCount=0)
			BEGIN
				INSERT INTO @tblFuel(FuelTypeId) SELECT FuelTypeId FROM CarFuelType 
				--SELECT DISTINCT(CarFuelType) FROM CarVersions V WHERE V.IsDeleted=0
			END
			ELSE
			BEGIN
				INSERT INTO @tblFuel(FuelTypeId) 
				SELECT	FT.FuelType
				FROM	TC_PrefFuelType FT WITH(NOLOCK)
				WHERE	FT.TC_BuyerInquiriesId=@TC_BuyInqWithoutStockId AND FT.IsActive=1
				
				SELECT	@FuelTypeNames = COALESCE(@FuelTypeNames+',','') + CF.FuelType
				FROM	CarFuelType CF
				WHERE	CF.FuelTypeId IN (SELECT FuelTypeId FROM @tblFuel)
			END
						
			SELECT @CarName =([dbo].TC_CarDetailBuyInq(@FromPrice,@ToPrice,YEAR(@FromMakeYear),
			YEAR(@ToMakeYear),@ModelNames,@BodyTypeNames,@FuelTypeNames))
			
			SET @ToMakeYear=DATEADD(yy,1,@ToMakeYear)
			
			-- Total Buyer Inquiry
			SELECT  @TotalBuyerInq=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN TC_BuyerInquiries B WITH (NOLOCK)ON S.Id=B.StockId	
				INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId=B.TC_InquiriesLeadId	AND IL.BranchId = S.BranchId	--added || Ranjeet	
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON IL.TC_CustomerId = C.Id AND C.ActiveLeadId = IL.TC_LeadId  --added || Ranjeet
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelTypeId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyStyleId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =1 AND S.IsActive=1 AND S.IsApproved=1
				AND B.TC_LeadDispositionID IS NULL AND IL.TC_LeadStageId <> 3 
			
			-- Total Seller Inquiry
			SELECT @TotalSellerInq=COUNT(S.TC_SellerInquiriesId)
			FROM TC_SellerInquiries S WITH (NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH (NOLOCK)ON S.TC_InquiriesLeadId=IL.TC_InquiriesLeadId 
			INNER JOIN vwMMV V ON S.CarVersionId=V.VersionId 
			INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON IL.TC_CustomerId = C.Id AND C.ActiveLeadId = IL.TC_LeadId  --added || Ranjeet				
			WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelTypeId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyStyleId FROM @tbBody )
				AND IL.BranchId=@BranchId AND S.PurchasedStatus IS NULL 
				AND S.TC_LeadDispositionId IS NULL	AND IL.TC_LeadStageId <> 3
				
			-- Total Match Stock		
			SELECT @TotalMatchStock=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelTypeId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyStyleId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =1 AND S.IsActive=1 AND S.IsApproved=1
				AND  S.Id<> ISNULL(@StockId,0)  --Added Ranjeet || removed self count of the stock.
				
				-- Total Match Old Stock
			SELECT @TotalMatchStockOld=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelTypeId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyStyleId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =3 AND S.IsActive=1 AND S.IsApproved=1
				AND  S.Id<> ISNULL(@StockId,0) --Added Ranjeet || removed self count of the stock.
			
		END*/
			--SELECT @TotalBuyerInq 'TotalBuyerInquiry',@TotalSellerInq 'TotalSellerInq', @TotalMatchStock 'TotalMatchStock',@TotalMatchStockOld 'TotalMatchStockOld',@CarName 'CarName'
			SELECT 0 'TotalBuyerInquiry',0 'TotalSellerInq', 0 'TotalMatchStock',0 'TotalMatchStockOld',@CarName 'CarName'
		
END





IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MatchingLeadsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MatchingLeadsCount]
GO

	-- Author:		Surendra
-- Create date: 25th July,2012
-- Description:	GET COUNT OF ALL MATCHING LEADS(BUYER,SELLER,STOCK,OLDSTOCK)
-- execute TC_MatchingLeadsCount 5,,null,null
-- Modified By : Tejashree Patil on 30 Jan 2013, implemented else in each temperary table variable
--=============================================

CREATE  Procedure [dbo].[TC_MatchingLeadsCount]
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
	DECLARE @ModelId INT, @MakeId INT, @CarName VARCHAR(80) -- will used to display car name in matching laed popuup	
	
	-- Getting Match Criteria based on input
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
		INNER JOIN TC_Inquiries T WITH (NOLOCK) ON S.TC_InquiriesId=T.TC_InquiriesId
		INNER JOIN vwMMV V ON T.VersionId=V.VersionId 				
		WHERE S.TC_SellerInquiriesId=@TC_SellerInquiriesId
	END
	ELSE -- Match is clicked from Loose inquiry
	BEGIN 
		SELECT @FromPrice=B.MinPrice,@ToPrice=B.MaxPrice,@FromMakeYear=(case ISNULL(B.FromMakeYear,0) WHEN 0 THEN null ELSE cast(B.FromMakeYear AS VARCHAR) END),
		@ToMakeYear=(CASE ISNULL(B.ToMakeYear,0) WHEN 0 THEN null ELSE cast(B.ToMakeYear AS VARCHAR)END),
		@BodyType=B.BodyType,@FuelType=B.FuelType,@ModelIds=B.ModelIds,
		@CarName =((CASE WHEN (@ModelIds IS NOT NULL) THEN B.ModelNames ELSE 
	   (CASE WHEN (B.MinPrice IS NOT NULL AND B.MaxPrice  IS NOT NULL) 
			 THEN CASE WHEN B.MinPrice=0 THEN ' Below Rs. '+CONVERT(VARCHAR(10),B.MaxPrice)		
			 ELSE ' Rs. '+CONVERT(VARCHAR(10),B.MinPrice)+' - '+CONVERT(VARCHAR(10),B.MaxPrice)END
		ELSE (CASE WHEN (COALESCE(' Rs. '+CONVERT(VARCHAR(10),B.MinPrice),'Rs. '+CONVERT(VARCHAR(10),B.MaxPrice))IS NOT NULL) 
			 THEN COALESCE(' Rs. '+CONVERT(VARCHAR(10),B.MinPrice)+',',' Rs. '+CONVERT(VARCHAR(10),B.MaxPrice)) 
			 ELSE '' END)END)+(CASE WHEN (B.FromMakeYear IS NOT NULL AND B.ToMakeYear  IS NOT NULL) 
			 THEN	', Year  '+CONVERT(VARCHAR(10),B.FromMakeYear)+' - '+CONVERT(VARCHAR(10),B.ToMakeYear)				
			 ELSE (CASE WHEN (COALESCE(', Year '+CONVERT(VARCHAR(10),B.FromMakeYear),', Year '+CONVERT(VARCHAR(10),B.ToMakeYear))IS NOT NULL) 
			 THEN COALESCE(', Year '+CONVERT(VARCHAR(10),B.FromMakeYear)+',',', Year '+CONVERT(VARCHAR(10),B.ToMakeYear)+',') ELSE '' END)
		END)+(CASE ISNULL(B.BodyType,'') WHEN '' THEN '' ELSE ISNULL(','+B.BodyTypeName,'') END)+
		(CASE ISNULL(B.FuelType,'') WHEN '' THEN '' ELSE ISNULL(','+B.FuelTypeName,'') END)END))
		FROM TC_BuyerInqWithoutStock B WITH (NOLOCK)
		WHERE B.TC_BuyerInqWithoutStockId=@TC_BuyInqWithoutStockId
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
					WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId		
					AND StatusId =1 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0)	
			-- Total Seller Inquiry
			SELECT @TotalSellerInq=COUNT(S.TC_SellerInquiriesId)
				FROM TC_SellerInquiries S WITH (NOLOCK)
				INNER JOIN TC_Inquiries T WITH (NOLOCK)ON S.TC_InquiriesId=T.TC_InquiriesId
				INNER JOIN vwMMV V ON T.VersionId=V.VersionId 				
				WHERE V.ModelId=@ModelId AND T.BranchId=@BranchId AND S.TC_SellerInquiriesId <> ISNULL(@TC_SellerInquiriesId,0)	 
			-- Total Match Stock
			SELECT @TotalMatchStock=COUNT(S.Id)
				FROM TC_Stock S WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId
				AND StatusId =1 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0)	
			-- Total Match Old Stock
			SELECT @TotalMatchStockOld=COUNT(S.Id)
				FROM TC_Stock S WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId=@ModelId AND S.BranchId=@BranchId
				AND StatusId =3 AND S.IsActive=1 AND S.Id <> ISNULL(@StockId,0)	
		END
	ELSE --in case of loose inq 
		BEGIN
			DECLARE @tblModel TABLE(ModelId INT)
			IF(@ModelIds IS NULL)
			BEGIN
				INSERT INTO @tblModel(ModelId) SELECT Id FROM CarModels M WHERE M.IsDeleted=0
			END
			ELSE
			BEGIN
				INSERT INTO @tblModel(ModelId) SELECT listmember FROM [dbo].[fnSplitCSV](@ModelIds)
			END
			
			DECLARE @tbBody TABLE(BodyId INT)
			IF(@BodyType IS NULL)
			BEGIN
				INSERT INTO @tbBody(BodyId) SELECT ID FROM CarBodyStyles 
			END
			ELSE
			BEGIN
				INSERT INTO @tbBody(BodyId) SELECT listmember FROM [dbo].[fnSplitCSV](@BodyType)
			END
			
			DECLARE @tblFuel TABLE(FuelId INT)
			IF(@FuelType IS NULL)
			BEGIN
				INSERT INTO @tblFuel(FuelId) SELECT DISTINCT(CarFuelType) FROM CarVersions V WHERE V.IsDeleted=0
			END
			ELSE
			BEGIN
				INSERT INTO @tblFuel(FuelId) SELECT listmember FROM [dbo].[fnSplitCSV](@FuelType)
			END
			
			SET @ToMakeYear=DATEADD(yy,1,@ToMakeYear)
			
			-- Total Buyer Inquiry
			SELECT  @TotalBuyerInq=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN TC_BuyerInquiries B WITH (NOLOCK)ON S.Id=B.StockId				
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =1 AND S.IsActive=1 AND S.IsApproved=1 
				--AND B.TC_BuyerInquiriesId <> ISNULL(@TC_BuyInqWithoutStockId,0)
			
			-- Total Seller Inquiry
			SELECT @TotalSellerInq=COUNT(S.TC_SellerInquiriesId)
			FROM TC_SellerInquiries S WITH (NOLOCK)
			INNER JOIN TC_Inquiries T WITH (NOLOCK)ON S.TC_InquiriesId=T.TC_InquiriesId
			INNER JOIN vwMMV V ON T.VersionId=V.VersionId 				
			WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
				AND T.BranchId=@BranchId
				
			-- Total Match Stock		
			SELECT @TotalMatchStock=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =1 AND S.IsActive=1 AND S.IsApproved=1
				
				-- Total Match Old Stock
			SELECT @TotalMatchStockOld=COUNT(S.Id)
				FROM TC_Stock S  WITH (NOLOCK)
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
				AND S.BranchId=@BranchId AND S.StatusId =3 AND S.IsActive=1 AND S.IsApproved=1
			
		END
		
	SELECT @TotalBuyerInq 'TotalBuyerInquiry',@TotalSellerInq 'TotalSellerInq' ,@TotalMatchStock 'TotalMatchStock',@TotalMatchStockOld 'TotalMatchStockOld',@CarName 'CarName'
END

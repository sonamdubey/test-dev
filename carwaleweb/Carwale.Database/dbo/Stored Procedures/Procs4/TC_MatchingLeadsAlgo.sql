IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MatchingLeadsAlgo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MatchingLeadsAlgo]
GO

	-- Author:		Surendra
-- Create date: 31st May,2012
-- Description:	getting matching comma seperated stockid and TC_SellerInquiriesId based on some calculation
-- Modified By : Teajshree Patil : Checked conditions for loose inq , added @TC_BuyInqWithoutStockId,@SrcPage parameters
--execute TC_MatchingLeadsAlgo 5,null,null,68,2
---- =============================================

CREATE PROCEDURE [dbo].[TC_MatchingLeadsAlgo]
@BranchId BIGINT,
@StockId BIGINT,-- THIS WILL BE NULL WHEN WHEN REFER FROM INQUIRIES PAGE
@TC_SellerInquiriesId BIGINT, -- THIS WILL BE NULL WHEN WHEN REFER FROM STOCK PAGES,
@TC_BuyInqWithoutStockId BIGINT, -- THIS WILL BE NULL WHEN WHEN REFER FROM STOCK PAGES,
@SrcPage TINYINT --TO IDENTIFY THE SOURCE OF VIEW MATCH REQUEST
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- INTERFERING WITH SELECT STATEMENTS.
	SET NOCOUNT ON;	
	
	DECLARE @FromPrice BIGINT,@ToPrice BIGINT
	DECLARE @FromMakeYear varchar(20),@ToMakeYear varchar(20)
	DECLARE @ModelIds VARCHAR(100),@BodyType VARCHAR(100),@FuelType VARCHAR(100)
	DECLARE @ModelId INT, @MakeId INT	
	
	IF(@StockId IS NOT NULL)
	BEGIN
		SELECT @ModelId=V.ModelId,@MakeId=V.MakeId
		FROM TC_Stock S  WITH (NOLOCK)
		INNER JOIN vwMMV V ON S.VersionId=V.VersionId WHERE S.Id=@StockId
	END
	ELSE IF(@TC_SellerInquiriesId IS NOT NULL)
	BEGIN
		SELECT @ModelId=V.ModelId ,@MakeId=V.MakeId
		FROM TC_SellerInquiries S WITH (NOLOCK) 
		INNER JOIN TC_Inquiries T WITH (NOLOCK) ON S.TC_InquiriesId=T.TC_InquiriesId
		INNER JOIN vwMMV V ON T.VersionId=V.VersionId 				
		WHERE S.TC_SellerInquiriesId=@TC_SellerInquiriesId
	END
	ELSE
	BEGIN -- IF @TC_BuyInqWithoutStockId IS NOT NULL
	
		SELECT @FromPrice=B.MinPrice,@ToPrice=B.MaxPrice,
		@FromMakeYear=(CASE ISNULL(B.FromMakeYear,0) WHEN 0 THEN NULL ELSE CAST(B.FromMakeYear AS VARCHAR) END),
		@ToMakeYear=(CASE ISNULL(B.ToMakeYear,0) WHEN 0 THEN NULL ELSE CAST(B.ToMakeYear AS VARCHAR)END),
		@BodyType=B.BodyType,@FuelType=B.FuelType,@ModelIds=B.ModelIds
		FROM TC_BuyerInqWithoutStock B	WITH (NOLOCK) 			
		WHERE B.TC_BuyerInqWithoutStockId=@TC_BuyInqWithoutStockId
	END
	
	IF(@SrcPage=2)--IF SOURCE PAGE OF VIEW MATCH IS SELLER INQUIRIES(SRCPAGE=2)
	BEGIN	
		
		DECLARE @CityId SMALLINT 
		SET @CityId=(SELECT D.CityId from Dealers D WITH (NOLOCK) Where D.Id=@BranchId)		
		DECLARE @SearchCriteria VARCHAR(500)	
				
		IF(@TC_BuyInqWithoutStockId IS NOT NULL)--CHECK FOR LOOSE INQ OR NOT
		BEGIN	
			execute TC_MatchingLeadsInner @BranchId =@BranchId,@FromPrice=@FromPrice,@ToPrice=@ToPrice,
					@FromMakeYear=@FromMakeYear,@ToMakeYear =@ToMakeYear,@ModelIds =@ModelIds,@BodyType=@BodyType,
					@FuelType=@FuelType,@SrcPage=@SrcPage,@StatusId =1
			-- TO GET UsedCarSearchCriteria IN CASE OF SELLER INQ.
			SELECT @SearchCriteria = dbo.GetUsedCarSearchCriteria(@CityId,@FromPrice,@ToPrice,@FromMakeYear,@ToMakeYear,null,null,@ModelIds,@FuelType,@BodyType,1)
			SELECT @SearchCriteria 'UCDSearchCriteria'
		END
		ELSE --SOURCE IS STOCK OR SPECIFIC INQ  
		BEGIN
			SELECT  S.TC_SellerInquiriesId
			FROM TC_SellerInquiries S WITH (NOLOCK) 
			INNER JOIN TC_Inquiries T WITH (NOLOCK) ON S.TC_InquiriesId=T.TC_InquiriesId
			INNER JOIN vwMMV V ON T.VersionId=V.VersionId 				
			WHERE V.ModelId=@ModelId 		
				AND T.BranchId=@BranchId AND S.TC_SellerInquiriesId<>ISNULL(@TC_SellerInquiriesId,0)
			-- TO GET UsedCarSearchCriteria IN CASE OF SELLER INQ.				
			SELECT @SearchCriteria = dbo.GetUsedCarSearchCriteria(@CityId,null,null,null,null,@MakeId,@ModelId,null,null,null,0)
			SELECT @SearchCriteria 'UCDSearchCriteria'	
			
		END	
		
	END
	ELSE
	BEGIN
		IF(@SrcPage<>4)--IF SOURCE PAGE IS BUYER INQ(SRCPAGE=1) OR STOCK PAGE(SRCPAGE=3)
		BEGIN			
			IF(@TC_BuyInqWithoutStockId IS NOT NULL)--CHECK FOR LOOSE INQ OR NOT
				BEGIN
					execute TC_MatchingLeadsInner @BranchId =@BranchId,@FromPrice=@FromPrice,@ToPrice=@ToPrice,
						@FromMakeYear=@FromMakeYear,@ToMakeYear =@ToMakeYear,@ModelIds =@ModelIds,@BodyType=@BodyType,
						@FuelType=@FuelType,@SrcPage=@SrcPage,@StatusId =1 -- ACTIVE STOCKS
				END
			ELSE
				BEGIN --SOURCE IS STOCK OR SPECIFIC INQ  
					SELECT  S.Id FROM TC_Stock S WITH (NOLOCK) 
					INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
					WHERE V.ModelId=@ModelId 		
					AND S.BranchId=@BranchId AND StatusId =1  AND S.IsActive=1 AND S.IsApproved=1 AND S.Id<> ISNULL(@StockId,0)
				END
		END
		ELSE --IF SOURCE PAGE IS OLDSTOCK PAGE
		BEGIN
			IF(@TC_BuyInqWithoutStockId IS NOT NULL)--CHECK FOR LOOSE INQ OR NOT
			BEGIN				
				execute TC_MatchingLeadsInner @BranchId =@BranchId,@FromPrice=@FromPrice,@ToPrice=@ToPrice,
					@FromMakeYear=@FromMakeYear,@ToMakeYear =@ToMakeYear,@ModelIds =@ModelIds,@BodyType=@BodyType,
					@FuelType=@FuelType,@SrcPage=@SrcPage,@StatusId =3
			END
			ELSE
			BEGIN
				SELECT  S.Id FROM TC_Stock S WITH (NOLOCK) --SOURCE IS STOCK OR SPECIFIC INQ  
				INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
				WHERE V.ModelId=@ModelId 		
				AND S.BranchId=@BranchId AND StatusId =3 AND S.IsActive=1 AND S.IsApproved=1
			END
		END		
	END
	
END




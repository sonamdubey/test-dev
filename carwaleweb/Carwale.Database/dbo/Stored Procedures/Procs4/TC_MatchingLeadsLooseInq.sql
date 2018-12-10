IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MatchingLeadsLooseInq]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MatchingLeadsLooseInq]
GO

	-- Author:		Tejashree Patil 
-- Create date: 30 Jan 2013
-- Description:	GET COUNT OF ALL MATCHING LEADS(BUYER,SELLER,STOCK,OLDSTOCK) IN CASE OF VIEW MATCH REQUEST FROM LOOSE INQUIRY
-- TC_MatchingLeadsLooseInq 5 ,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,1
---- =============================================

CREATE PROCEDURE [dbo].[TC_MatchingLeadsLooseInq]
@BranchId BIGINT,
@FromPrice BIGINT,
@ToPrice BIGINT,
@FromMakeYear DATE,
@ToMakeYear DATE,
@ModelIds VARCHAR(100),
@BodyType VARCHAR(100),
@FuelType VARCHAR(100),
@SrcPage TINYINT,-- TO IDENTIFY REQUEST SOURCE PAGE
@StatusId TINYINT-- STOCK STATUS
AS
BEGIN
	-- FOR MODELIDS
	DECLARE @tblModel table(ModelId INT)
	IF(@ModelIds IS NULL)
	BEGIN
		INSERT INTO @tblModel(ModelId) SELECT Id FROM CarModels M WHERE M.IsDeleted=0
	END
	BEGIN
		INSERT INTO @tblModel(ModelId) SELECT listmember FROM [dbo].[fnSplitCSV](@ModelIds)
	END
	
	--FOR BODYTYPE
	DECLARE @tbBody table(BodyId INT)
	IF(@BodyType IS NULL)
	BEGIN
		INSERT INTO @tbBody(BodyId) SELECT ID FROM CarBodyStyles 
	END
	BEGIN
		INSERT INTO @tbBody(BodyId) SELECT listmember FROM [dbo].[fnSplitCSV](@BodyType)
	END
	--FOR FUEL TYPE
	DECLARE @tblFuel table(FuelId INT)
	IF(@FuelType IS NULL)
	BEGIN
		INSERT INTO @tblFuel(FuelId) SELECT Distinct(CarFuelType) FROM CarVersions V WHERE V.IsDeleted=0
	END
	BEGIN
		INSERT INTO @tblFuel(FuelId) SELECT listmember FROM [dbo].[fnSplitCSV](@FuelType)
	END
		
	SET @ToMakeYear=DATEADD(yy,1,@ToMakeYear)
	print @ToMakeYear
	
	IF(@SrcPage=2)-- REQUEST OF VIEW MATCH FROM SELLER INQUIRY
	BEGIN		
		BEGIN
			SELECT  S.TC_SellerInquiriesId
			FROM TC_SellerInquiries S WITH (NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH (NOLOCK)ON S.TC_InquiriesLeadId=IL.TC_InquiriesLeadId
			INNER JOIN vwMMV V ON S.CarVersionId=V.VersionId 				
			WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
				AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
				AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
				AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
				AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
				AND IL.BranchId=@BranchId AND S.PurchasedStatus IS NULL 
				AND S.TC_LeadDispositionId IS NULL	AND IL.TC_LeadStageId <> 3
		END 
	END
	ELSE -- REQUEST OF VIEW MATCH FROM STOCK,OLDSTOCK OR FROM BUYER INQUIRY
	BEGIN	
		SELECT  S.Id
		FROM TC_Stock S WITH (NOLOCK)
		INNER JOIN vwMMV V ON S.VersionId=V.VersionId 
		WHERE V.ModelId IN( SELECT ModelId FROM @tblModel )
		AND ((S.Price BETWEEN @FromPrice AND @ToPrice) OR (@FromPrice IS NULL AND @ToPrice IS NULL))
		AND ((S.MakeYear BETWEEN @FromMakeYear AND @ToMakeYear) OR (@FromMakeYear IS NULL AND @ToMakeYear IS NULL))
		AND V.CarFuelType IN(SELECT FuelId FROM @tblFuel )
		AND V.BodyStyleId IN(SELECT BodyId FROM @tbBody )
		AND S.BranchId=@BranchId AND S.StatusId =@StatusId AND S.IsActive=1 AND S.IsApproved=1
	END
END

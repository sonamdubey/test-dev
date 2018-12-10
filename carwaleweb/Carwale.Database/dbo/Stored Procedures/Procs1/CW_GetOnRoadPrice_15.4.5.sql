IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetOnRoadPrice_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetOnRoadPrice_15]
GO

	
-- =============================================
-- Author:		Ashish Verma					EXEC [dbo].[GetNewOnRoadPrice] 6865
-- Create date: 14/07/2014
-- Description:	get new on-road price
-- Modified By : Raghu on <27/12/2013> Added WITH(NOLOCK) Conditions on table
-- modified by ashish verma on 22/08/2014 for android
-- modified by Vikas J on 16/04/2015 added XLargePic in select
-- modified by Sanjay Soni on 09/07/2015 added PageId
-- modified by sanjay Son on 16/07/2015 changed Image Source to carVersion table and for XlargePic carModel
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetOnRoadPrice_15.4.5]
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC(18, 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @PQCarversionId INT
	DECLARE @PQCityId INT
		,@ZoneId INT
	DECLARE @City VARCHAR(50)
		,@Zone VARCHAR(50)
	DECLARE @PageId INT

	SELECT @PQCarversionId = NCP.CarVersionId
		,@PQCityId = NPC.CityId
		,@ZoneId = NPC.ZoneId
		,@PageId = NCP.PQPageId
	FROM NewCarPurchaseInquiries NCP WITH (NOLOCK)
	INNER JOIN NewPurchaseCities NPC WITH (NOLOCK) ON NCP.Id = NPC.InquiryId
	WHERE Id = @PQId

	IF (@ZoneId != '')
	BEGIN
		SELECT @City = ct.NAME
			,@Zone = cz.ZoneName
		FROM Cities ct WITH (NOLOCK)
		INNER JOIN CityZones cz WITH (NOLOCK) ON ct.ID = cz.CityId
		WHERE ct.ID = @PQCityId
			AND cz.Id = @ZoneId
	END
	ELSE
	BEGIN
		SELECT @City = ct.NAME
		FROM Cities ct WITH (NOLOCK)
		WHERE ct.ID = @PQCityId
	END

	IF (
			@PQCarversionId > 0
			AND @PQCityId > 0
			)
	BEGIN
		SELECT PQC.CategoryId
			,Ci.Id AS CategoryItemId
			,CI.CategoryName AS categoryName
			,PQN.PQ_CategoryItemValue AS Value
			,PQLT.IsTaxOnTax
		FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
		INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
		INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
		LEFT JOIN PriceQuote_LocalTax PQLT WITH (NOLOCK) ON CI.Id = PQLT.CategoryItemid
			AND PQLT.CityId = @PQCityId
		--LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON PQN.CityId = PQLT.CityId AND PQLT.CityId = @PQCityId
		WHERE CarVersionId = @PQCarversionId
			AND PQN.CityId = @PQCityId
		ORDER BY PQC.SortOrder ASC
	END

	SELECT CM.ID AS MakeId
		,CM.NAME AS MakeName
		,CMO.ID AS ModelId
		,CMO.NAME AS ModelName
		,CV.ID AS VersionId
		,CV.NAME AS VersionName
		,@PQCityId AS CityId
		,CMO.MaskingName AS MaskingName
		,CV.HostURL + CV.SmallPic AS SmallPic
		,CV.HostURL + CV.LargePic AS LargePic
		,CV.OriginalImgPath
		,CV.HostURL
		,CMO.HostURL + CMO.XLargePic AS XLargePic
		,@City AS CityName
		,@Zone AS ZoneName
		,@ZoneId AS ZoneId
		,@PageId AS PageId
		,CMO.ReviewRate AS ReviewRate
		,CV.SpecsSummary AS SpecsSummery --modified by ashish verma on 22/08/2014 for android
	FROM CarMakes CM WITH (NOLOCK)
	INNER JOIN CarModels CMO WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.CarModelId = CMO.ID
	WHERE CV.ID = @PQCarversionId
END

/****** Object:  StoredProcedure [dbo].[GetOnRoadPriceandPQIdV1.1]    Script Date: 8/27/2014 8:49:25 AM ******/
SET ANSI_NULLS ON


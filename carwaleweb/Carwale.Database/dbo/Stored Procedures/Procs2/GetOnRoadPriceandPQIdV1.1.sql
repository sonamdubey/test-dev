IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOnRoadPriceandPQIdV1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOnRoadPriceandPQIdV1]
GO

	
-- =============================================
-- Author:		<Ashish VErma>
-- Create date: <16/7/2014>
-- Description:	<Description:We fetch InquiryId and On-RoadPrice using this Sp>
-- modified by ashish verma on 22/08/2014 for android
-- =============================================
CREATE PROCEDURE [dbo].[GetOnRoadPriceandPQIdV1.1]
	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId NUMERIC(18, 0)
	,-- Car Version Id
	@BuyTime VARCHAR(100) = '1 week'
	,@Name VARCHAR(50)
	,@CityId NUMERIC(18, 0)
	,@EmailId VARCHAR(100)
	,@PhoneNo VARCHAR(50)
	,@ForwardedLead BIT
	,@SourceId TINYINT
	,@PQPageId SMALLINT
	,@ClientIP VARCHAR(100)
	,@InterestedInLoan BIT
	,-- Added by Raghu to capture user's interest to apply for loan
	@MobVerified BIT = 0
	,-- Added by Raght to cpature whether customer verified his mobile or not
	@ZoneId INT
	
AS
BEGIN
	DECLARE @City VARCHAR(50),@ZoneName VARCHAR(50)
	IF (@ZoneId != '')
	BEGIN
		SELECT @City=ct.NAME,
			@ZoneName =cz.ZoneName 
		FROM Cities ct WITH (NOLOCK)
		INNER JOIN CityZones cz WITH (NOLOCK) ON ct.ID = cz.CityId
		WHERE ct.ID = @CityId
			AND cz.Id = @ZoneId
	END
	ELSE
	BEGIN
		SELECT @City=ct.NAME
		FROM Cities ct WITH (NOLOCK)
		WHERE ct.ID = @CityId
	END
	--print @City
	INSERT INTO NewCarPurchaseInquiries (
		CustomerId
		,CarVersionId
		,BuyTime
		,RequestDateTime
		,IsApproved
		,LatestOffers
		,ForwardedLead
		,-- Added by Raghu
		SourceId
		,PQPageId
		,ClientIP
		)
	VALUES (
		- 1
		,-----Taking hardcoded value will be updated later after processing in rabbit MQ 		 		
		@CarVersionId
		,@BuyTime
		,GETDATE()
		,1
		,1
		,@ForwardedLead
		,@SourceId
		,@PQPageId
		,@ClientIP
		)

	Declare @InquiryId NUMERIC = SCOPE_IDENTITY()
--	print @City
	IF (@InquiryId > 0)
	BEGIN
		---Data Reader first
		SELECT PQC.CategoryId
			,Ci.Id AS CategoryItemId
			,CI.CategoryName AS categoryItem
			,PQN.PQ_CategoryItemValue AS Value
			,PQLT.IsTaxOnTax
		FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
		INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
		INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
		LEFT JOIN PriceQuote_LocalTax PQLT WITH (NOLOCK) ON CI.Id = PQLT.CategoryItemid
			AND PQLT.CityId = @CityId
		WHERE CarVersionId = @CarVersionId
			AND PQN.CityId = @CityId
		ORDER BY PQC.SortOrder ASC
		---Data Reader second
		SELECT CM.ID AS MakeId
				,CM.NAME AS MakeName
			,CMO.ID AS ModelId
			,CMO.NAME AS ModelName
			,CV.ID AS VersionId
			,CV.NAME AS VersionName
			,CMO.MaskingName AS Maskingname
			,CMO.LargePic AS LargePic,@City as CityName,@ZoneName as ZoneName,@InquiryId as InquiryId,CV.SpecsSummary as SpecsSummery,CMO.ReviewRate AS ReviewRate,CMO.SmallPic AS SmallPic --modified by ashish verma on 22/08/2014 for android
		FROM CarMakes CM WITH (NOLOCK)
		INNER JOIN CarModels CMO WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
		INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.CarModelId = CMO.ID
		WHERE CV.ID = @CarVersionId

		INSERT INTO NewPurchaseCities (
			InquiryId
			,CityId
			,City
			,EmailId
			,PhoneNo
			,NAME
			,InterestedInLoan
			,MobileVerified
			,ZoneId
			)
		VALUES (
			@InquiryId
			,@CityId
			,@City
			,@EmailId
			,@PhoneNo
			,@Name
			,@InterestedInLoan
			,@MobVerified
			,@ZoneId
			)

		--print @City
	END
END



/****** Object:  StoredProcedure [dbo].[GetSimilarCarModels]    Script Date: 8/27/2014 8:49:52 AM ******/
SET ANSI_NULLS ON

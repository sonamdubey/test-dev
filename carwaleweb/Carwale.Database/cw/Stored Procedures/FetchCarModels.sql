IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[FetchCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[FetchCarModels]
GO

	-- =============================================
-- Author:		Shikhar
-- Create date: 27-07-2012
-- Description:	Returns Car Models for a make id 
-- edited on :  07-02-2013
-- edited by :  Shikhar - Added the Specification, User Review, Price Quote availability and other fields in SP
-- Last Edited on: 21-02-2013 by Shikhar - The Specification column is currently commented as currently it is deployed
-- on production. Once, it is deployed, remove the comments.
-- Last Edited on: 20-03-2013 by Shikhar - Added LiveListingPrice and LiveListingCount.
-- Last Edited on: 07-08-2013 by Amit V - Added columns CarVersionID_Top and SubsegmentId in select statement
-- Last Edited on: 24-09-2013 by Shikhar - Fetching the Min and Max Car Price directly from the CarModels Table 'Mo.MinPrice','Mo.MaxPrice'
-- Modified By Satish Sharma on 27-12-2013 3:30 PM -- 
--	Resolved Price quote bug :  Commented refrence to Con_NewCarNationalPrices table, Added MO.new = 1 AND Mo.MinPrice IS NOT NULL
-- Modified By : Akansha on 10.2.2014
-- Description : Added masking name column
--Modified by Manish on 17-12-2015 added with (nolock) keyword wherever not found.
--Modified by Manish on 30-12-2015 added create index script on temp table.
--Modified by Manish on 30-12-2015 commented create index script on temp table.
--Modified by jitendra singh create temp table then insert into temp table
-- =============================================
CREATE PROCEDURE   [cw].[FetchCarModels]
	@MakeId SMALLINT = 0
AS
BEGIN

	CREATE  TABLE #tempModel(
		[Text] varchar(30),
		Value int,
		CarMake varchar(30),
		CarMakeId int
		,New bit
		,Used bit
		,Futuristic bit
		,SmallPic varchar(200)
		,HostURL varchar(100)
		,ReviewRate Decimal(18,2)
		,ReviewCount int
		,MaskingName varchar(50)
		,RoadTest int
		,Specification int
		,PriceQuote int
		,UserReview int
		,MinLiveListingPrice int
		,LiveListingCount int
		,TotalReviews int
		,MinPrice int		
		,MaxPrice int	
		,CarVersionID_Top int	
		,SubsegmentId int
	)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	INSERT INTO #tempModel
	(
		[Text] ,
		Value,
		CarMake,
		CarMakeId
		,New 
		,Used
		,Futuristic
		,SmallPic
		,HostURL
		,ReviewRate
		,ReviewCount
		,MaskingName
		,RoadTest
		,Specification
		,PriceQuote
		,UserReview
		,MinLiveListingPrice
		,LiveListingCount
		,TotalReviews
		,MinPrice		
		,MaxPrice	
		,CarVersionID_Top	
		,SubsegmentId
	)
	SELECT Mo.NAME AS [Text]
		,Mo.ID AS Value
		,Ma.NAME AS CarMake
		,Mo.CarMakeId
		,Mo.New
		,Mo.Used
		,Mo.Futuristic
		,Mo.SmallPic
		,Mo.HostURL
		,Mo.ReviewRate
		,Mo.ReviewCount
		,Mo.MaskingName
		,NULL AS RoadTest
		,NULL AS Specification
		,NULL AS PriceQuote
		,NULL AS UserReview
		,NULL AS MinLiveListingPrice
		,NULL AS LiveListingCount
		,(
			SELECT COUNT(ID)
			FROM CustomerReviews WITH (NOLOCK)
			WHERE ModelId = Mo.ID
				AND IsActive = 1
				AND IsVerified = 1
			) AS TotalReviews
		,Mo.MinPrice		--Edited on: 24-09-2013 by Shikhar - Fetching the Min Car Price directly from the CarModels Table
		,Mo.MaxPrice		--Edited on: 24-09-2013 by Shikhar - Fetching the Max Car Price directly from the CarModels Table
		,CarVersionID_Top	--Edited on: 07-08-2013 by Amit - Added columns CarVersionID_Top and SubsegmentId in select statement
		,SubsegmentId		--Edited on: 07-08-2013 by Amit - Added columns CarVersionID_Top and SubsegmentId in select statement
	FROM CarModels Mo WITH (NOLOCK)
	INNER JOIN CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId
	WHERE (
			Mo.CarMakeId = @MakeId
			OR @MakeId = 0
			)
		AND Mo.IsDeleted = 0


   -- CREATE INDEX IX_tempModel_Value   ON   #tempModel (Value);  -- Line added by Manish on 30-12-2015

	UPDATE #tempModel
	SET RoadTest = 1
	WHERE Value IN (
			SELECT Mo.ID
			FROM CarModels Mo WITH (NOLOCK)
			INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON Mo.ID = CC.ModelId
			INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON (
					CC.BasicId = CB.Id
					AND CC.IsActive = 1
					)
			WHERE Mo.IsDeleted = 0
				AND CB.CategoryId = 8
				AND CB.IsActive = 1
				AND CB.IsPublished = 1
				AND Mo.CarMakeId = @MakeId
			)

	--UPDATE #tempModel
	--SET Specification = 1
	--WHERE Value IN
	--( SELECT CarModelId FROM CarVersions WITH (NOLOCK) WHERE IsDeleted = 0 AND IsSpecsAvailable = 1 )

	UPDATE #tempModel
	SET MinLiveListingPrice = LLData.Price
		,LiveListingCount = LLData.CNT
	FROM (
		SELECT ModelId
			,MIN(Price) AS Price
			,COUNT(ProfileId) CNT
		FROM LiveListings WITH (NOLOCK)
		GROUP BY ModelId
		) AS LLData
	WHERE LLData.ModelId = #tempModel.Value

	UPDATE #tempModel
	SET Specification = 1
	WHERE Value IN (
			SELECT CM.ID
			FROM CarModels AS CM WITH (NOLOCK)
			JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId = CM.ID
			JOIN NewCarSpecifications AS NS WITH (NOLOCK) ON NS.CarVersionId = CV.ID
			WHERE CM.New = 1
			)

	UPDATE #tempModel
	SET PriceQuote = 1
	WHERE Value IN (
			SELECT Mo.ID
			FROM CarModels Mo WITH (NOLOCK)
			INNER JOIN CarVersions CV WITH (NOLOCK) ON (
					Mo.ID = CV.CarModelId
					AND CV.IsDeleted = 0
					)
			-- Modified By Satish Sharma on 27-12-2013 3:30 PM -- 
            --	Commented refrence to Con_NewCarNationalPrices table
			--INNER JOIN Con_NewCarNationalPrices NCP WITH (NOLOCK) ON (
			--		CV.ID = NCP.VersionId
			--		AND NCP.IsActive = 1
			--		)
			WHERE Mo.CarMakeId = @MakeId
				AND CV.New = 1 
				AND Mo.New = 1 -- Modified By Satish Sharma on 27-12-2013 3:30 PM -- Added MO.new = 1 AND Mo.MinPrice IS NOT NULL
				AND Mo.IsDeleted = 0
				AND Mo.MinPrice IS NOT NULL
			)

	UPDATE #tempModel
	SET UserReview = 1
	WHERE Value IN (
			SELECT CR.ModelId
			FROM CarModels CMO WITH (NOLOCK)
			INNER JOIN CustomerReviews CR WITH (NOLOCK) ON CMO.ID = CR.ModelId
			WHERE CMO.CarMakeId = @MakeId
				AND IsActive = 1
				AND IsVerified = 1
			)

	SELECT 		
		[Text] ,
		Value,
		CarMake,
		CarMakeId
		,New 
		,Used
		,Futuristic
		,SmallPic
		,HostURL
		,ReviewRate
		,ReviewCount
		,MaskingName
		,RoadTest
		,Specification
		,PriceQuote
		,UserReview
		,MinLiveListingPrice
		,LiveListingCount
		,TotalReviews
		,MinPrice		
		,MaxPrice	
		,CarVersionID_Top	
		,SubsegmentId	
	FROM #tempModel

	DROP TABLE #tempModel
END
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarValuationSuggestions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarValuationSuggestions]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 17-01-2012
-- Description:	Populate ProfileIds matching with Used car Valuation customer search criteria
-- Avishkar 13-02-2012 Added and modified code to return unique cars 
-- exec [dbo].[UsedCarValuationSuggestions] 270,'4/1/2009 12:00:00 AM',1,25000,221500
-- AM Modified 14-02-2013 Added MakeName and ModelName
-- Modified by Manish on 17-04-2013 changing datatype of lattitude and longidude and commenting the order by clause in insert statement
--Modified by Reshma Shetty on 18/4/2013 changed varchar sizes in temp table
-- Modified By : Akansha Srivastava on 12.2.2014
-- Description : Added MaskingName Column
-- Modified by : Manish Chourasiya added WITH(NOLOCK) keyword wherever not found.
-- Modified by: Kirtan Shetty on 10.7.2014 Added ImageUrlMedium in all the queries
-- Modified by: Akansha on 11.7.2014 Added IsPremium in all the queries
-- =============================================
CREATE PROCEDURE [dbo].[UsedCarValuationSuggestions] @Version INT
	,@MakeYear DATETIME
	,@Cityid INT
	,@CarKm BIGINT
	,@CarPrice [numeric] (
	18
	,0
	)
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Lattitude [decimal] (
		18
		,4
		)
		,@Longitude [decimal] (
		18
		,4
		)
		,@LattDiff [decimal] (
		18
		,4
		)
		,@LongDiff [decimal] (
		18
		,4
		)
	DECLARE @Varmakeyear SMALLINT

	SET @Varmakeyear = YEAR(@makeyear)

	DECLARE @CarKmDiff SMALLINT

	SET @CarKmDiff = 5000

	DECLARE @sql VARCHAR(max)
		,@Versions VARCHAR(1000)
	DECLARE @similarversion VARCHAR(1000)
	DECLARE @cnt INT

	-- AM Modified 14-02-2013 Added MakeName and ModelName
	CREATE TABLE #tempCars (
		CarId TINYINT identity(1, 1)
		,ProfileId VARCHAR(50)
		,Seller VARCHAR(50)
		,CarName VARCHAR(200)
		,MakeName VARCHAR(100)
		,ModelName VARCHAR(100)
		,MaskingName varchar(100)
		,AreaName VARCHAR(50)
		,CityName VARCHAR(50)
		,MakeYear DATETIME
		,Price BIGINT
		,Kilometers BIGINT
		,FrontImagePath VARCHAR(200)
		,HostURL VARCHAR(50)
		,ImageUrlMedium VARCHAR (250)
		,IsPremium bit
		)

	SELECT @Lattitude = Lattitude
		,@Longitude = Longitude
	FROM Cities WITH (NOLOCK)
	WHERE Id = @Cityid

	SET @LattDiff = 100
	SET @LongDiff = 100

	-- AM Modified 14-02-2013 Added MakeName and ModelName
	INSERT INTO #tempCars (
		ProfileId
		,Seller
		,CarName
		,MakeName
		,ModelName
		,MaskingName
		,CityName
		,AreaName
		,MakeYear
		,Price
		,Kilometers
		,FrontImagePath
		,HostURL
		,ImageUrlMedium
		,IsPremium
		)
	--select top 4 Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,MakeYear,Price,Kilometers
	SELECT TOP 4 ProfileId
		,Seller
		,MakeName + ' ' + ModelName + ' ' + VersionName AS CarName
		,MakeName
		,ModelName
		,MaskingName
		,CityName
		,AreaName
		,MakeYear
		,Price
		,Kilometers
		,FrontImagePath
		,LL.HostURL
		,ll.ImageUrlMedium
		,IsPremium
	FROM LiveListings LL WITH (NOLOCK)
	Inner Join CarModels CMO WITH (NOLOCK) on LL.ModelId=CMO.ID
	WHERE VersionId = @version
		AND year(MakeYear) BETWEEN (@Varmakeyear - 1)
			AND (@Varmakeyear + 1)
		AND CityId = @cityid
		AND Kilometers BETWEEN @CarKm - 5000
			AND @CarKm + 5000
		AND Lattitude BETWEEN @Lattitude - @LattDiff
			AND @Lattitude + @LattDiff
		AND Longitude BETWEEN @Longitude - @LongDiff
			AND @Longitude + @LongDiff
		AND Price BETWEEN (@CarPrice * 0.98)
			AND (@CarPrice * 1.2)
	ORDER BY ABS(Price - @CarPrice)

	SET @cnt = @@ROWCOUNT
	SET @cnt = 4 - @cnt

	IF @cnt > 0
	BEGIN
		SELECT @similarversion = SimilarVersions
		FROM SimilarCars WITH (NOLOCK)
		WHERE VersionId = @version

		IF (LEN(@similarversion) < 1)
			SET @similarversion = '0'
		--set @CarPrice=1000
		SET @sql = ' select top ' + cast(@cnt AS CHAR(2)) + '  L.ProfileId,Seller,L.MakeName+'' ''+L.ModelName+'' ''+L.VersionName as CarName,L.MakeName,L.ModelName,CMO.MaskingName,L.CityName,L.AreaName,L.MakeYear,L.Price,L.Kilometers,L.FrontImagePath,L.HostURL,L.ImageUrlMedium,L.IsPremium ' +
			--set @sql=' select top '+cast(@cnt as CHAR(2))+'  Seller,MakeName+''''+ModelName+''''+VersionName as CarName,CityName,MakeYear,Price,Kilometers'+
			' from LiveListings	as L WITH (NOLOCK) Inner Join CarModels CMO WITH (NOLOCK) on L.ModelId=CMO.ID where  VersionId in (' + cast(@similarversion AS VARCHAR(max)) + ')' + ' AND year(MakeYear) between ' + Cast(@Varmakeyear AS VARCHAR(20)) + '-1 AND ' + cast(@Varmakeyear AS VARCHAR(20)) + '+1 AND CityId=' + cast(@cityid AS VARCHAR(10)) + ' AND Kilometers between ' + cast(@CarKm AS VARCHAR(20)) + '-5000 AND ' + cast(@CarKm AS VARCHAR(20)) + '+ 5000 ' + ' AND Price between (' + cast(@CarPrice AS VARCHAR(20)) + '*0.98) and (' + cast(@CarPrice AS VARCHAR(20)) + '*1.2)' + ' AND Lattitude BETWEEN ' + cast(@Lattitude AS VARCHAR(20)) + ' - ' + cast(@LattDiff AS VARCHAR(20)) + 'AND ' + cast(@Lattitude AS VARCHAR(20)) + '+' + cast(@LattDiff AS VARCHAR(20)) + ' AND Longitude BETWEEN ' + cast(@Longitude AS VARCHAR(20)) + ' - ' + cast(@LongDiff AS VARCHAR(20)) + 'AND ' + cast(@Longitude AS VARCHAR(20)) + '+' + cast(@LongDiff AS VARCHAR(20))

		--print @sql
		INSERT INTO #tempCars (
			ProfileId
			,Seller
			,CarName
			,MakeName
			,ModelName
			,MaskingName
			,CityName
			,AreaName
			,MakeYear
			,Price
			,Kilometers
			,FrontImagePath
			,HostURL
			,ImageUrlMedium
			,IsPremium
			)
		EXECUTE (@sql)
	END

	SELECT @cnt = COUNT(*)
	FROM #tempCars

	SET @cnt = 4 - @cnt

	IF @cnt > 0
	BEGIN
		--insert into #tempCars(ProfileId,Seller,CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL)
		--select top 4 ProfileId,Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL
		----select top 4 Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,MakeYear,Price,Kilometers
		--from LiveListings	
		--where CityId=@cityid
		--AND Lattitude BETWEEN @Lattitude - @LattDiff AND @Lattitude + @LattDiff
		--   AND Longitude BETWEEN @Longitude - @LongDiff AND @Longitude + @LongDiff  
		--Order by ABS(Price - @CarPrice)
		-- Avishkar 13-02-2012 Added " left outer join #tempCars" to return unique cars 
		INSERT INTO #tempCars (
			ProfileId
			,Seller
			,CarName
			,MakeName
			,ModelName
			,MaskingName
			,CityName
			,AreaName
			,MakeYear
			,Price
			,Kilometers
			,FrontImagePath
			,HostURL
			,ImageUrlMedium
			,IsPremium
			)
		SELECT TOP 4 L.ProfileId AS ProfileID
			,L.Seller AS [Seller]
			,L.MakeName + ' ' + L.ModelName + ' ' + L.VersionName AS CarName
			,L.MakeName
			,L.ModelName
			,CMO.MaskingName
			,L.CityName
			,L.AreaName
			,L.MakeYear
			,L.Price
			,L.Kilometers
			,L.FrontImagePath
			,L.HostURL
			,L.ImageUrlMedium
			,IsPremium
		FROM LiveListings AS L WITH (NOLOCK)
		Inner Join CarModels CMO WITH (NOLOCK) on L.ModelId=CMO.ID
		--left outer join #tempCars as t 	on t.ProfileId=L.ProfileId and t.ProfileId is null
		WHERE L.CityId = @cityid
			AND L.ProfileId NOT IN (
				SELECT ProfileId
				FROM #tempCars
				)
			AND L.Lattitude BETWEEN @Lattitude - @LattDiff
				AND @Lattitude + @LattDiff
			AND L.Longitude BETWEEN @Longitude - @LongDiff
				AND @Longitude + @LongDiff
		ORDER BY ABS(@CarPrice - L.Price) ASC
	END

	-- Avishkar 13-02-2012 Added to return unique cars 
	SELECT TOP 4 ProfileId
		,Seller
		,CarName
		,MakeName
		,ModelName
		,MaskingName
		,AreaName
		,CityName
		,MakeYear
		,Price
		,Kilometers
		,FrontImagePath
		,HostURL
		,ImageUrlMedium
		,IsPremium
	FROM (
		SELECT ROW_NUMBER() OVER (
				PARTITION BY ProfileId ORDER BY ProfileId
				) AS rowid
			,ProfileId
			,Seller
			,CarName
			,MakeName
			,ModelName
			,MaskingName
			,AreaName
			,CityName
			,MakeYear
			,Price
			,Kilometers
			,FrontImagePath
			,HostURL
			,ImageUrlMedium
			,IsPremium
		FROM #tempCars
		) a
	WHERE rowid = 1

	-- Avishkar 13-02-2012 commented to return unique cars 
	--select Top 4 
	--        ProfileId,
	--		Seller,
	--		CarName,
	--		AreaName,
	--		CityName,
	--		MakeYear
	--		Price,
	--		Kilometers,
	--		FrontImagePath,
	--		HostURL
	--from #tempCars 
	DROP TABLE #tempCars
END
/****** Object:  StoredProcedure [dbo].[WA_UpComingCarDetails]    Script Date: 7/30/2014 8:04:42 PM ******/
SET ANSI_NULLS ON

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarComparisonData_Android]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarComparisonData_Android]
GO

	-- =============================================

-- Author:		<Author,,Amit Verma>

-- Create date: <Create Date,,04 May 2014>

-- Description:	<Description,,Car Comapre data>

-- Author:		<Author,,Amit Verma>

-- Create Modified: <Create Date,,07 May 2014>

-- Description:	<Description,,Fetch colour details>
--	Modified by Rakesh Yadav on 09 May 2014
--Desc: Fething modelid's for both versions
--Approved by: Manish Chourasiya on 01-07-2014 4:50pm , With (NoLock) is used
--Modified by ajay singh on 05-oct-2016 for change creation of temp table.first create then insert data
-- =============================================

/*

		EXEC GetCarComparisonData_Android 2522,2199

*/

CREATE PROCEDURE [dbo].[GetCarComparisonData_Android]

	-- Add the parameters for the stored procedure here

	@VersionId1 INT,
	@VersionId2 INT

AS

BEGIN
	SET NOCOUNT ON;
	
	DECLARE @tempTable TABLE (Id INT IDENTITY, VersionId INT)
	INSERT INTO @tempTable (VersionId) VALUES(@VersionId1)
	INSERT INTO @tempTable (VersionId) VALUES(@VersionId2)

	--modified by ajay singh on 05 oct 2016
	CREATE TABLE #CarData(Name VARCHAR(300),
	                      ItemMasterId INT,
						  NodeCode VARCHAR(2500),--changed nvarchar to varchar
						  SortOrder INT,
						  OverviewSortOrder INT,
						  IsOverviewable BIT,
						  Value1 VARCHAR(320),
						  Value2 VARCHAR(320)
						  )



	SELECT CategoryName,NodeCode,SortOrder FROM CD.CategoryMaster WITH(NOLOCK) WHERE lvl = 1
	SELECT CategoryName,NodeCode,SortOrder FROM CD.CategoryMaster WITH(NOLOCK) WHERE lvl = 2

	INSERT INTO #CarData (Name,
	                      ItemMasterId,
						  NodeCode,
						  SortOrder,
						  OverviewSortOrder,
						  IsOverviewable,
						  Value1,
						  Value2
						  )
	SELECT IM.Name + (CASE UT.UnitTypeId WHEN 1 THEN '' WHEN 2 THEN '' ELSE ISNULL(' (' + UT.Name + ')','') END) Name
	,IM.ItemMasterId,CI.NodeCode,IM.SortOrder,OverviewSortOrder,IM.IsOverviewable
	,CASE IV1.DatatypeId
		WHEN 2 THEN
			CASE IV1.ItemValue WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END
			ELSE ISNULL(IV1.CustomText,'')+ISNULL(CAST(IV1.ItemValue AS VARCHAR(20)),'')+ISNULL(UD1.Name,'') END Value1
	,CASE IV2.DatatypeId
		WHEN 2 THEN
			CASE IV2.ItemValue WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END
			ELSE ISNULL(IV2.CustomText,'')+ISNULL(CAST(IV2.ItemValue AS VARCHAR(20)),'')+ISNULL(UD2.Name,'') END Value2
	--INTO #CarData
	FROM CD.ItemMaster IM WITH(NOLOCK)
	LEFT JOIN CD.ItemValues IV1 WITH(NOLOCK) ON IM.ItemMasterId = IV1.ItemMasterId AND IV1.CarVersionId = @VersionId1
	LEFT JOIN CD.ItemValues IV2 WITH(NOLOCK) ON IM.ItemMasterId = IV2.ItemMasterId AND IV2.CarVersionId = @VersionId2
	LEFT JOIN CD.UserDefinedMaster UD1 WITH(NOLOCK) ON IV1.UserDefinedId = UD1.UserDefinedId
	LEFT JOIN CD.UserDefinedMaster UD2 WITH(NOLOCK) ON IV2.UserDefinedId = UD2.UserDefinedId
	LEFT JOIN CD.CategoryItemMapping CI WITH(NOLOCK) ON IM.ItemMasterId = CI.ItemMasterId
	LEFT JOIN CD.UnitTypes UT WITH(NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId
	WHERE IM.IsPublished = 1 AND IM.ItemTypeId IN (1,3) AND IM.IsActive = 1
	SELECT Name,NodeCode,Value1,Value2,SortOrder FROM #CarData WHERE Value1 != '' OR Value2 != ''
	SELECT Name,NodeCode,Value1,Value2,OverviewSortOrder SortOrder FROM #CarData WHERE (Value1 != '' OR Value2 != '') AND IsOverviewable = 1
	
	DROP TABLE #CarData

	SELECT Color c, HexCode h FROM VersionColors WITH(NOLOCK) WHERE IsActive=1 and CarVersionID = @VersionId1

	SELECT Color c, HexCode h FROM VersionColors WITH(NOLOCK) WHERE IsActive=1 and CarVersionID = @VersionId2
	
	SELECT CMA.ID MakeId
		,CMA.Name MakeName
		,CM.ID ModelId
		,CM.Name ModelName
		,CM.MaskingName
		,CV.ID VersionId
		,CV.Name VersionName
	FROM CarVersions CV WITH(NOLOCK)
	INNER JOIN @tempTable T ON CV.ID = T.VersionId
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	ORDER BY T.Id ASC

END

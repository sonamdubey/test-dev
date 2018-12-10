IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarComparisonDataByVersionIDs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarComparisonDataByVersionIDs]
GO

	
-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects specification and features of all the versions for a model
-- declare @p1 nvarchar(max) exec [CD].[GetCarComparisonDataByVersionIDs] '2424,2425',NULL ,@p1 out select @p1 
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma				  14/03/2013					version image url is returned instead of model image url
       Amit Verma                 28/03/2013                    added logic to consider group items      
       Amit Verma                 28/05/2013                    select CarModelID to get usedcar details in comparison page
       Ashish Kamble			  17 July 2013					Prices are referred from new delhi
       Amit Verma                 26/09/2013                    added model small image column in select statement
       Amit Verma                 19/05/2014                    added nolock at various places
       Amit Verma                 15/06/2014                    made enhancements in item select query
	   Supriya Khartode			  07/08/2014					fetching featuredversionid from [CD].[GetFeaturedVersionIDByVersionID] sp instead of CD.GetFeaturedVersionID function 
																to track sponsored version 
       -- Amit Added 30-8-2013 To fetch tracking codes
*/
CREATE PROCEDURE   [CD].[GetCarComparisonDataByVersionIDs]
	@Versions VARCHAR(100),
	@FeaturedVersionID int=NULL OUTPUT,
	@FCTrackingCode NVARCHAR(MAX) = '' OUTPUT
	--@ValVersionIDs varchar(100) OUTPUT
AS
  BEGIN
      DECLARE @TempCarVersionID TABLE
        (
           id TINYINT IDENTITY,
           VersionId INT
        )
      DECLARE @Counter1 TINYINT
      DECLARE @Counter2 TINYINT

      INSERT INTO @TempCarVersionID (VersionId)
      SELECT VID.items FROM [dbo].[SplitText](@Versions,',') as VID join CD.vwMMV on VID.items = CD.vwMMV.VersionId

	
	   EXEC [CD].[GetFeaturedVersionIDByVersionID]  @Versions =@Versions,
                                                    @FeaturedVersionId=@FeaturedVersionID OUTPUT

													
	 -- SET @FeaturedVersionID = (SELECT CD.GetFeaturedVersionID(@Versions))
	  IF(@FeaturedVersionID is not NULL)
	  BEGIN
		INSERT INTO @TempCarVersionID (VersionId) values(@FeaturedVersionID)
		SET @FCTrackingCode = (SELECT [dbo].[GetTrackingCodeForFC] (@FeaturedVersionID)) -- Amit Added 30-8-2013 To fetch tracking codes
	  END
	  ELSE
	  BEGIN
		SET @FeaturedVersionID = -1
		SET @FCTrackingCode = ''
	  END
	
		
	  select T.* from @TempCarVersionID T1 left join(
		SELECT     
		Distinct
		Vs.ID as VersionId,
		'http://'+ Vs.HostURL+'/cars/'+ Vs.LargePic AS ModelImage,
		SP.Price,
		Ma.Name Make,
		Mo.Name Model,
		Vs.Name Version,
		Vs.CarModelId,-- amit verma 28/05/2013
		'http://'+ Vs.HostURL+'/cars/'+ Vs.smallPic AS ModelImageSmall,---- amit verma 26/09/2013
		Mo.MaskingName
		FROM CarVersions Vs WITH(NOLOCK) 
		INNER JOIN  CarModels Mo WITH (NOLOCK) ON MO.ID = Vs.CarModelId       
		INNER JOIN CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId    
--		LEFT JOIN Con_NewCarNationalPrices NP WITH (NOLOCK) ON NP.VersionId = VS.ID AND NP.IsActive = 1
		LEFT JOIN NewCarShowroomPrices SP WITH (NOLOCK) ON SP.CarVersionId = Vs.ID AND SP.CityId = 10
		) T on T1.VersionId = T.VersionId

	
SELECT ItemMasterId,
	Category,
	SubCategory,
	NodeCode,
	ItemName,
	Layout,
	Icon,
	lvl,
	IsOverviewable,
	OverviewSortOrder
		FROM (SELECT DISTINCT
		IM.ItemMasterId,
		CM1.CategoryName Category,
		CM.CategoryName SubCategory,
		CM.NodeCode NodeCode,
		IM.Name + ISNULL (' ('+ CASE WHEN UT.Name = 'CUSTOM' OR UT.Name = 'BIT' THEN NULL ELSE UT.Name END +')', '') AS 'ItemName',
		CM.Layout,
		Icon,
		CM.lvl,
		IsOverviewable,
		OverviewSortOrder,
		CM.SortOrder SortOrderCat,
		IM.SortOrder SortOrderItem
	FROM CD.ItemMaster IM WITH(NOLOCK)
	INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
	AND IM.IsPublished = 1 AND IM.IsActive = 1 AND (IM.ItemTypeId = 1 OR IM.ItemTypeId = 3)
	INNER JOIN CD.CategoryMaster CM WITH(NOLOCK) ON CIM.NodeCode = CM.NodeCode
	INNER JOIN CD.CategoryMaster CM1 WITH(NOLOCK) ON SUBSTRING(CM.NodeCode,0,4) = CM1.NodeCode
	INNER JOIN CD.ItemValues IV WITH(NOLOCK) ON IM.ItemMasterId = IV.ItemMasterId
	INNER JOIN @TempCarVersionID CV ON IV.CarVersionId = CV.VersionId
	LEFT JOIN CD.UnitTypes UT WITH(NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId ) T
ORDER BY T.SortOrderCat,T.SortOrderItem

SELECT @Counter1=MIN(id),@Counter2=MAX(id) FROM @TempCarVersionID
      WHILE ( @Counter1 <= @Counter2)
        BEGIN
            DECLARE @SQL               NVARCHAR(max),
                    @versionid         NVARCHAR(max),
                    @versionName       NVARCHAR(max),
                    @SingleQuotesTwice VARCHAR(2) = '''''';

            SELECT @versionid = TC.VersionId 
            FROM   @TempCarVersionID TC
            WHERE   TC.id=@Counter1;
                            
            
			SELECT IV.ItemMasterId I,
			ISNULL(IV.CustomText,'')
			+ ISNULL(CASE WHEN IV.DataTypeId = 2 THEN (CASE WHEN IV.ItemValue = 1 THEN 'Yes' ELSE 'No' END) ELSE CAST(IV.ItemValue AS VARCHAR(20)) END,'')
			+ ISNULL(UD.Name,'') as V
            FROM CD.ItemValues IV WITH(NOLOCK) LEFT JOIN CD.UserDefinedMaster UD WITH(NOLOCK) on IV.UserDefinedId = UD.UserDefinedId
			WHERE CarVersionId = @versionid
			
            SET @Counter1=@Counter1+1
        END
  END




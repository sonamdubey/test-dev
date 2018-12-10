IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarComparisonDataByVersionIDs_032013]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarComparisonDataByVersionIDs_032013]
GO

	
/*
	THIS STORED PROCEDURE COLLECTS SPECIFICATION OF ALL THE VERSIONS FOR A MODEL

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------[CD].[GetCarComparisonDataByVersionIDs] '1537,2279',0
*/
CREATE  PROCEDURE   [CD].[GetCarComparisonDataByVersionIDs_032013]
	@Versions VARCHAR(100),
	@FeaturedVersionID int OUTPUT
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

      --SET @ValVersionIDs =''
      --SELECT @ValVersionIDs = @ValVersionIDs+VID.items+',' FROM [dbo].[SplitText](@Versions,',') as VID join CD.vwMMV on VID.items = CD.vwMMV.VersionId
      --set @ValVersionIDs =LEFT(@ValVersionIDs, LEN(@ValVersionIDs) - 1)
      --select @ValVersionIDs
      INSERT INTO @TempCarVersionID (VersionId)
      SELECT VID.items FROM [dbo].[SplitText](@Versions,',') as VID join CD.vwMMV on VID.items = CD.vwMMV.VersionId

	  -- 
	  SET @FeaturedVersionID = (SELECT CD.GetFeaturedVersionID(@Versions))
	  IF(@FeaturedVersionID is not NULL)
		INSERT INTO @TempCarVersionID (VersionId) values(@FeaturedVersionID)
	  ELSE
		SET @FeaturedVersionID = -1
	
		
	  select T.* from @TempCarVersionID T1 left join(
		SELECT     
		Distinct
		Vs.ID as VersionId,
		'http://'+MO.HostURL+'/cars/'+Mo.LargePic AS ModelImage,
		--(Ma.Name + ' ' + Mo.Name +' '+Vs.Name) AS CarName, 
		--Vs.Name,   
		--Mo.Id as ModelId,         
		--Mo.ReviewRate AS ReviewRate,    
		--Mo.ReviewCount,   
		--MO.HostURL,    
		NP.Price,
		Ma.Name Make,
		Mo.Name Model,
		Vs.Name Version
		   
		FROM CarVersions Vs 
		INNER JOIN  CarModels Mo WITH (NOLOCK) ON MO.ID = Vs.CarModelId       
		INNER JOIN CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId    
		LEFT JOIN NewCarShowroomPrices NP WITH (NOLOCK) ON NP.CarVersionId = VS.ID AND NP.CityId = 10 AND NP.IsActive = 1
		) T on T1.VersionId = T.VersionId
		--WHERE Vs.ID IN(SELECT * FROM @TempCarVersionID)
   
      SELECT ItemMasterId,
             Category,
             SubCategory,
             NodeCode,
             Field_Name
             + ISNULL (' ('+UT.Name+')', '') AS 'ItemName',
             Layout,
             Icon,
             lvl,
             IsOverviewable,
             OverviewSortOrder
             --SortOrder
      FROM   (SELECT T2.ItemMasterId AS 'ItemMasterId',
                     T3.CMName       AS 'Category',
                     SubCategory,
                     T3.NodeCode,
                     T3.Name         AS 'Field_Name',
                     T3.UnitTypeId   AS 'UnitTypeId',
                     Layout,
                     Icon,
                     lvl,
                     CategorySort,
                     ItemSort,
                     IsOverviewable,
                     OverviewSortOrder
              FROM   (SELECT DISTINCT ItemMasterid
                      FROM   CD.ItemValues
                      WHERE  CarVersionId IN (SELECT VersionId
                                              FROM   @TempCarVersionID)) AS T2
                     INNER JOIN (SELECT IM.ItemMasterId AS
                                       Itemmasterid,
                                       CMName,
                                       SubCategory,
                                       NodeCode,
                                       Layout,
                                       lvl,
                                       IM.Name         AS Name,
                                       IM.UnitTypeId   AS UnitTypeId,
                                       IM.Icon		   AS Icon,
                                       IM.SortOrder AS ItemSort
                                       ,CategorySort,
                                       IM.IsOverviewable,
                                       IM.OverviewSortOrder
                                FROM   CD.ItemMaster AS IM
                                       INNER JOIN (SELECT
               CIM.ItemMasterId,
                                       CIM.NodeCode,
                                       CM.CategoryName as SubCategory,
                                       CM.lvl,
                                       CM.Layout,
                                       CM.SortOrder AS CategorySort,
                                       (select CategoryName from CD.CategoryMaster where HierId = CM.HierID.GetAncestor(1)) AS Cmname
                                                  FROM   CD.CategoryItemMapping AS CIM
                                       INNER JOIN cd.Categorymaster CM
			 ON CIM.NodeCode = CM.NodeCode) AS T1
             ON IM.ItemMasterId = T1.ItemMasterId where IM.IsPublished = 1) AS T3
             ON T2.ItemMasterId = T3.ItemMasterId) AS t4
             LEFT JOIN CD.UnitTypes AS UT
                    ON t4.UnitTypeId = UT.UnitTypeId order by CategorySort,ItemSort
                    
SELECT @Counter1=MIN(id),@Counter2=MAX(id) FROM   @TempCarVersionID
      WHILE ( @Counter1 <= @Counter2)
        BEGIN
            DECLARE @SQL               NVARCHAR(max),
                    @versionid         NVARCHAR(max),
                    @versionName       NVARCHAR(max),
                    @SingleQuotesTwice VARCHAR(2) = '''''';

            SELECT @versionName =Make+' '+Model+' '+Version ,@versionid = TC.VersionId 
            FROM   CD.vwMMV CV
				INNER JOIN @TempCarVersionID TC ON TC.VersionId=CV.VersionId
            WHERE   TC.id=@Counter1;
                            
            

            SET @SQL=N'select CD.ItemValues.ItemMasterId, ISNULL(CustomText,'
                     + @SingleQuotesTwice
                     + ')+ISNULL(CAST(ItemValue AS VARCHAR(20)),'
                     + @SingleQuotesTwice
                     + ')+ISNULL(CD.UserDefinedMaster.Name,'
                     + @SingleQuotesTwice
                     + ') as'
                     + '['
                     + @versionid
                     + ']'
                     + 'from cd.ItemValues left join CD.UserDefinedMaster on CD.ItemValues.UserDefinedId = CD.UserDefinedMaster.UserDefinedId where CarVersionId = '
                     + @versionid + ' ORDER  BY ItemMasterId'

            EXEC Sp_executesql
              @SQL

            SET @Counter1=@Counter1+1
        END
  END


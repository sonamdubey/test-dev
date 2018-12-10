IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarDataByVersionIDNew]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarDataByVersionIDNew]
GO

	/*
	THIS STORED PROCEDURE COLLECTS SPECIFICATION OF ALL THE VERSIONS FOR A MODEL

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/
CREATE PROCEDURE [CD].[GetCarDataByVersionIDNew]
	@Versions VARCHAR(100),
	@FeaturedVersionID int OUTPUT
AS
  BEGIN
      DECLARE @TempCarVersionID TABLE
        (
           id INT
        )

      INSERT INTO @TempCarVersionID (id)
      SELECT * FROM [dbo].[SplitText](@Versions,',')
	  --INSERT INTO @TempCarVersionID (id)
	  SET @FeaturedVersionID = (SELECT CD.GetFeaturedVersionID(@Versions))
	  IF(@FeaturedVersionID is not NULL)
		INSERT INTO @TempCarVersionID (id) values(@FeaturedVersionID)
	  ELSE
		SET @FeaturedVersionID = -1
		--select @FeaturedVersionID
	  select T.* from @TempCarVersionID T1 left join(
		SELECT     
		Distinct
		Vs.ID,
		--null as emptyCell,
		Mo.LargePic AS ModelImage,
		(Ma.Name + ' ' + Mo.Name +' '+Vs.Name) AS CarName, 
		--Vs.Name,   
		--Mo.Id as ModelId,         
		--Mo.ReviewRate AS ReviewRate,    
		--Mo.ReviewCount,   
		--MO.HostURL,    
		NP.Price 
		   
		FROM Carwale_com..CarVersions Vs 
		INNER JOIN  Carwale_com..CarModels Mo WITH (NOLOCK) ON MO.ID = Vs.CarModelId       
		INNER JOIN Carwale_com..CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId    
		LEFT JOIN Carwale_com..NewCarShowroomPrices NP WITH (NOLOCK) ON NP.CarVersionId = VS.ID AND NP.CityId = 10 AND NP.IsActive = 1
		) T on T1.ID = T.ID
		--WHERE Vs.ID IN(SELECT * FROM @TempCarVersionID)
   
      SELECT ItemMasterId,
             Category,
             SubCategory,
             NodeCode,
             Field_Name
             + ISNULL (' ('+UT.Name+')', '') AS 'ItemName',
             Layout,
             Icon,
             lvl
      FROM   (SELECT T2.ItemMasterId AS 'ItemMasterId',
                     T3.CMName       AS 'Category',
                     SubCategory,
                     T3.NodeCode,
                     T3.Name         AS 'Field_Name',
                     T3.UnitTypeId   AS 'UnitTypeId',
                     Layout,
                     Icon,
                     lvl
              FROM   (SELECT DISTINCT ItemMasterid
                      FROM   CD.ItemValues
                      WHERE  CarVersionId IN (SELECT *
                                              FROM   @TempCarVersionID)) AS T2
                     LEFT JOIN (SELECT IM.ItemMasterId AS
                                       Itemmasterid,
                                       CMName,
                                       SubCategory,
                                       NodeCode,
                                       Layout,
                                       lvl,
                                       IM.Name         AS Name,
                                       IM.UnitTypeId   AS UnitTypeId,
                                       IM.Icon		   AS Icon
                                FROM   CD.ItemMaster AS IM
                                       LEFT JOIN (SELECT
                                       CIM.ItemMasterId,
                                       CIM.NodeCode,
                                       CM.CategoryName as SubCategory,
                                       CM.lvl,
                                       CM.Layout,
                                       (select CategoryName from CD.CategoryMaster where HierId = CM.HierID.GetAncestor(1)) AS Cmname
                                                  FROM   CD.CategoryItemMapping AS CIM
                                       LEFT JOIN cd.Categorymaster CM
			 ON CIM.NodeCode = CM.NodeCode) AS T1
             ON IM.ItemMasterId = T1.ItemMasterId) AS T3
             ON T2.ItemMasterId = T3.ItemMasterId) AS t4
             LEFT JOIN CD.UnitTypes AS UT
                    ON t4.UnitTypeId = UT.UnitTypeId order by ItemMasterId

      WHILE ( (SELECT Count(*)
               FROM   @TempCarVersionID) != 0 )
        BEGIN
            DECLARE @SQL               NVARCHAR(max),
                    @versionid         NVARCHAR(max),
                    @versionName       NVARCHAR(max),
                    @SingleQuotesTwice VARCHAR(2) = '''''';

            SET @versionName = (SELECT Make+' '+Model+' '+Version FROM   CD.vwMMV WHERE  VersionId =(SELECT TOP 1 *
                                             FROM   @TempCarVersionID));
            SET @versionid = (SELECT TOP 1 *
                              FROM   @TempCarVersionID);                             
            

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

            DELETE TOP (1) FROM @TempCarVersionID
        END
  END

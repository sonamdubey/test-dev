IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarDataDiffByModelID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarDataDiffByModelID]
GO

	/*
	THIS STORED PROCEDURE COLLECTS SPECIFICATION OF ALL THE VERSIONS FOR A MODEL

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/
CREATE PROCEDURE [CD].[GetCarDataDiffByModelID] @ModelID INT
AS
    BEGIN
      DECLARE @TempCarVersionID TABLE
        (
           id INT
        )

      INSERT INTO @TempCarVersionID
                  (id)
      SELECT VW.VersionID
      FROM  CD.vwMMV as VW left join Carwale_com..Con_NewCarNationalPrices as CP
      on VW.VersionId = CP.VersionId where VW.ModelId = @ModelID order by CP.MinPrice asc


      SELECT ItemMasterId,
             Category,
             ItemName,
             CD.UnitTypes.Name as UnitTypes,
             ItemImportance
      FROM   (SELECT T2.ItemMasterId AS 'ItemMasterId',
                     T3.CategoryName       AS 'Category',
                     T3.ItemName         AS 'ItemName',
                     T3.UnitTypeId   AS 'UnitTypeId',
                     T3.ItemImportance as ItemImportance
              FROM   (SELECT DISTINCT ItemMasterid
                      FROM   cd.Itemvalues
                      WHERE  CarVersionId IN (SELECT *
                                              FROM   @TempCarVersionID)) AS T2
                     LEFT JOIN (SELECT cd.ItemMaster.ItemMasterId AS Itemmasterid,
                                       T1.CategoryName,
                                       cd.ItemMaster.Name         AS ItemName,
                                       cd.ItemMaster.UnitTypeId   AS UnitTypeId,
                                       CD.ItemMaster.ItemImportance
                                FROM   cd.ItemMaster
                                       LEFT JOIN (select CIM.ItemMasterId,CM.CategoryName from CD.CategoryItemMapping as CIM
									   left join CD.CategoryMaster as CM on  CIM.NodeCode = CM.NodeCode where CIM.NodeCode like '/2/%') AS T1
             ON cd.Itemmaster.ItemMasterId = T1.ItemMasterId) AS T3
             ON T2.ItemMasterId = T3.ItemMasterId) AS t4
             LEFT JOIN CD.UnitTypes
                    ON t4.UnitTypeId = CD.UnitTypes.UnitTypeId
                    where ItemImportance is not null


      WHILE ( (SELECT Count(*)
               FROM   @TempCarVersionID) != 0 )
        BEGIN
            DECLARE @SQL               NVARCHAR(max),
                    @versionid         NVARCHAR(max),
                    @versionName       NVARCHAR(max),
                    @SingleQuotesTwice VARCHAR(2) = '''''',
                    @imp			   NVARCHAR(max) ='''$''';

            SET @versionName = (SELECT Version
                                FROM   CD.vwMMV
                                WHERE  VersionId = (SELECT TOP 1 *
                                             FROM   @TempCarVersionID));
            SET @versionid = (SELECT TOP 1 *
                              FROM   @TempCarVersionID);                             
            

            SET @SQL=N'select T2.* from CD.ItemMaster as T1 right outer join (select CD.ItemValues.ItemMasterId, ISNULL(CustomText,'
                     + @SingleQuotesTwice
                     + ')+ISNULL(CAST(ItemValue AS VARCHAR(20)),'
                     + @SingleQuotesTwice
                     + ')+ISNULL(CD.UserDefinedMaster.Name,'
                     + @SingleQuotesTwice
                     + ')+ISNULL('+'+'+@imp+'+'+'convert(varchar,CD.UserDefinedMaster.ValueImportance),'
                     + @SingleQuotesTwice
                     + ') as'
                     + '['
                     + @versionName
                     + ']'
                     + 'from cd.ItemValues left join CD.UserDefinedMaster on CD.ItemValues.UserDefinedId = CD.UserDefinedMaster.UserDefinedId where CarVersionId = '
                     + @versionid + ') as T2 on T2.ItemMasterId = T1.ItemMasterId where T1.ItemImportance is not null'
                     
                     
            EXEC Sp_executesql
              @SQL
--select * from CD.UserDefinedMaster
            DELETE TOP (1) FROM @TempCarVersionID
        END
  END
  
  
--select * from carwale..vwMMV

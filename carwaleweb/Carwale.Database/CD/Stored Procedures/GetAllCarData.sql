IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetAllCarData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetAllCarData]
GO

	/*
	THIS STORED PROCEDURE COLLECTS SPECIFICATION OF ALL THE VERSIONS

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/
CREATE PROCEDURE [CD].[GetAllCarData] @ModelID INT
AS
  BEGIN
      DECLARE @TempCarVersionID TABLE
        (
           id INT
        )

      INSERT INTO @TempCarVersionID
                  (id)
      SELECT VersionID
      FROM   CD.vwMMV--carwale..vwMMV--
      where ModelId =@ModelID
      if (@ModelID = -1)
      begin
      select Itemmasterid,CategoryName,ItemName,UnitType,DT.Name as DataType from (select T2.Itemmasterid,T2.CategoryName,T2.Name as ItemName,T2.DatatypeId,UT.Name as UnitType from (
									   SELECT cd.ItemMaster.ItemMasterId AS ItemMasterId,
                                       CMName as CategoryName,
                                       cd.ItemMaster.Name         AS Name,
                                       cd.ItemMaster.UnitTypeId   AS UnitTypeId,
                                       cd.ItemMaster.DatatypeId
                                FROM   cd.ItemMaster
                                       LEFT JOIN (SELECT
                                       cd.Categoryitemmapping.ItemMasterId,
                                       (select CategoryName from CD.CategoryMaster where HierId = CM.HierID.GetAncestor(1))+' - ' + CM.CategoryName AS Cmname
                                                  FROM   cd.Categoryitemmapping
                                       LEFT JOIN cd.Categorymaster CM
			 ON cd.Categoryitemmapping.NodeCode = CM.NodeCode) AS T1
             ON cd.Itemmaster.ItemMasterId = T1.ItemMasterId) AS T2
             left join CD.UnitTypes AS UT ON T2.UnitTypeId = UT.UnitTypeId)as T3
             left join CD.DataTypes AS DT ON T3.DatatypeId = DT.DataTypeId
      end
      
      else
      begin

      WHILE ( (SELECT Count(*)
               FROM   @TempCarVersionID) != 0 )
        BEGIN
            DECLARE @SQL               NVARCHAR(max),
                    @versionid         NVARCHAR(max),
                    @versionName       NVARCHAR(max),
                    @SingleQuotesTwice VARCHAR(2) = '''''';

            --SET @versionName = (SELECT name
            --                    FROM   carwale_com..Carversions
            --                    WHERE  id = (SELECT TOP 1 *
            --                                 FROM   @TempCarVersionID));
            
            
            SET @versionName = (SELECT Make+' '+Model+' '+Version as name
                                FROM   CD.vwMMV--carwale..vwMMV--
                                WHERE  VersionId = (SELECT TOP 1 *
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
                     + @versionName +'('+@versionid+')'
                     + ']'
                     + 'from cd.ItemValues left join CD.UserDefinedMaster on CD.ItemValues.UserDefinedId = CD.UserDefinedMaster.UserDefinedId where CarVersionId = '
                     + @versionid + ' ORDER  BY ItemMasterId'

            EXEC Sp_executesql
              @SQL

            DELETE TOP (1) FROM @TempCarVersionID
        END
        end
  END

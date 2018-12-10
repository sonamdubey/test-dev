IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarDataByModelID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarDataByModelID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: <Create Date,,>
-- Description:	this stored procedure collects specification and features of all the versions for a model
-- [CD].[GetCarDataByModelID] 353,0
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma                 26/3/2013                     Replaced Inner join with Left Join
       Amit Verma                 28/3/2013                     added logic to consider group items      
       Amit Verma                 16/9/2013                     added logic to select only active items      
*/
CREATE  PROCEDURE   [CD].[GetCarDataByModelID]
	@ModelID int,@OnlyNewVersion int,@isPublished int = null
AS
  BEGIN
      DECLARE @TempCarVersionID TABLE
        (
           id TINYINT IDENTITY,
           VersionId INT
        )
      DECLARE @Counter1 TINYINT
      DECLARE @Counter2 TINYINT
      
      IF (@OnlyNewVersion = 1)
		  BEGIN
			INSERT INTO @TempCarVersionID
				  (VersionId)
		  SELECT VersionID
		  FROM   CD.vwMMV
		  WHERE  ModelId = @ModelID and IsVerionNew = 1
		  ORDER BY VersionId --Amit Verma   26/3/2013 Added
		  END
      ELSE
		BEGIN
			INSERT INTO @TempCarVersionID
				  (VersionId)
		  SELECT VersionID
		  FROM   CD.vwMMV
		  WHERE  ModelId = @ModelID
		  ORDER BY VersionId --Amit Verma   26/3/2013 Added
		END
      
      SELECT VersionId,Version,MakeId FROM CD.vwMMV WHERE VersionId IN (SELECT VersionId FROM @TempCarVersionID)
      
	SELECT IM.ItemMasterId, 
		   (SELECT CategoryName 
			FROM   cd.CategoryMaster 
			WHERE  HierId = CM.HierID.GetAncestor(1)) Category, 
		   CM.CategoryName                            SubCategory, 
		   CIM.NodeCode, 
		   IM.Name + Isnull (' ('+UT.Name+')', '')    ItemName, 
		   IM.DatatypeId
		   --im.IsPublished
	FROM   cd.ItemMaster IM 
		   INNER JOIN cd.CategoryItemMapping CIM 
				   ON IM.ItemMasterId = CIM.ItemMasterId 
		   INNER JOIN cd.CategoryMaster CM 
				   ON CIM.NodeCode = CM.NodeCode 
		   LEFT JOIN cd.UnitTypes UT 
				   ON IM.UnitTypeId = UT.UnitTypeId 
		   --Amit Verma   26/3/2013 Replaced Inner join with Left Join
		   --INNER JOIN cd.UnitTypes UT 
				 --  ON IM.UnitTypeId = UT.UnitTypeId
	WHERE (IM.ItemTypeId = 1 OR IM.ItemTypeId = 2) --added by amit v
	AND IM.IsActive = 1 AND (@isPublished IS NULL OR IsPublished = @isPublished) --Amit Verma	16/9/2013	added logic to select only active items 
	ORDER  BY CM.CategoryMasterID 
                    
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

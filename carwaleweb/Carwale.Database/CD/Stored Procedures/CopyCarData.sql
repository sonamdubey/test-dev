IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[CopyCarData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[CopyCarData]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE COPY's THE ITEM VALUES FOR AN EXISTING CAR VERSION

	WRITTEN BY : SHIKHAR MAHESHWARI ON 16 APR 2012

	Changes History:
       
		Edited By               Edited ON               	Description
		Shikhar Maheshwari      May 24, 2012               	Added a Copy Data functionality Category Wise
		--------                -------		       	        added the @categoryMasterId Parameter to SP
       
		Edited By               Edited ON               	Description
		Shikhar Maheshwari      May 29, 2012               	Added a Bulk Copy Data functionality to SP by 
		--------				-------						adding the 'ItemMasterId NOT IN' filter

		Edited By               Edited ON               	Description
		Amit Verma		        Mar 28, 2013               	Added logic to fill group item values 
		--------				-------						
		
		Edited By               Edited ON               	Description
		Amit Verma		        Aug 16, 2013               	Added logic to fill table dbo.NewCarSpecifications
		--------				-------			
		
		Edited By               Edited ON               	Description
		Amit Verma		        Aug 21, 2013               	Added logic to generate spec summary on bulk copy
		--------				-------	
		
		Edited By               Edited ON               	Description
		Amit Verma		        Oct 07, 2013               	Added logic to update recommendcars table on bulk copy

		Edited By               Edited ON               	Description
		amit verma				10/14/2013					Update model summary on value change
		--------				-------			
*/

CREATE PROCEDURE [CD].[CopyCarData]
	@categoryMasterId INT = 0,
	@newCarVersionId INT,
	@oldCarVersionId INT,
	@userName VARCHAR(50) = NULL
AS

IF NOT EXISTS (
SELECT
	1
FROM 
	CD.ItemValues IV WITH(NOLOCK)
	INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
		ON IV.ItemMasterId = CIM.ItemMasterId
WHERE 
	IV.CarVersionId = @newCarVersionId
AND
	(CIM.NodeCode = (select NodeCode from CD.CategoryMaster where CategoryMasterId = @categoryMasterId) OR @categoryMasterId = 0)
)
BEGIN

INSERT INTO CD.ItemValues
	(
	CarVersionId,
	ItemMasterId,
	DataTypeId,
	ItemValue,
	UserDefinedId,
	CustomText,
	UpdatedBy
	)
SELECT
	@newCarVersionId,
	IV.ItemMasterId,
	IV.DataTypeId,
	IV.ItemValue,
	IV.UserDefinedId,
	IV.CustomText,
	@userName
FROM 
	CD.ItemValues IV WITH(NOLOCK)
	INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
		ON IV.ItemMasterId = CIM.ItemMasterId
WHERE 
	IV.CarVersionId = @oldCarVersionId
AND
	(CIM.NodeCode = (select NodeCode from CD.CategoryMaster where CategoryMasterId = @categoryMasterId) OR @categoryMasterId = 0)
AND
	IV.ItemMasterId NOT IN 
	(SELECT ItemMasterId FROM CD.ItemValues WITH(NOLOCK) WHERE CarVersionId = @newCarVersionId )
--group item logic start	(added by amit v)
--DECLARE @ItemMasterIds TABLE 
--  ( 
--     id        INT IDENTITY, 
--     ItemMasterId INT 
--  ) 

--INSERT INTO @ItemMasterIds 
--SELECT IV.ItemMasterId FROM CD.ItemValues IV
--LEFT JOIN CD.CategoryItemMapping CIM ON IV.ItemMasterId = CIM.ItemMasterId
--WHERE IV.CarVersionId = @newCarVersionId AND
--(CIM.NodeCode = (SELECT NodeCode FROM CD.CategoryMaster WHERE CategoryMasterId = @categoryMasterId) OR @categoryMasterId = 0)

--SELECT * 
--FROM   @ItemMasterIds 

--DECLARE @ItemMasterId INT = -1 
--DECLARE @rowCount INT = (SELECT COUNT(id) 
--   FROM   @ItemMasterIds) 

--WHILE ( @rowCount > 0 ) 
--  BEGIN 
--      SET @ItemMasterId = (SELECT TOP 1 ItemMasterId 
--						  FROM   @ItemMasterIds 
--						  WHERE  id = @rowCount) 

--      EXEC [CD].[FillGroupItemValues] 
--        @newCarVersionId, 
--        @ItemMasterID 

--      SELECT @rowCount 

--      SET @rowCount -=1 
--  END
--group item logic end

--fill dbo.NewCarSpecifications start	(added by amit v on 16 aug 2013)

--Amit Verma		        Aug 21, 2013               	Added logic to generate spec summary on bulk copy (start)

UPDATE CD.ItemValues SET ItemValue = ItemValue WHERE
ItemMasterId = ( SELECT TOP 1 ItemMasterId FROM cd.ItemValues WHERE ItemMasterId in(12,14,26,29) and CarVersionId = @newCarVersionId)
and CarVersionId = @newCarVersionId
--Amit Verma		        Aug 21, 2013               	Added logic to generate spec summary on bulk copy (end)

DECLARE @ItemMasterIds TABLE 
  ( 
     id        INT IDENTITY, 
     ItemMasterId INT 
  ) 

INSERT INTO @ItemMasterIds 
SELECT IV.ItemMasterId FROM CD.ItemValues IV
WHERE IV.CarVersionId = @newCarVersionId AND
IV.ItemMasterId IN (SELECT DISTINCT ItemMasterID FROM New_Old_Specs_mapping)

SELECT * 
FROM   @ItemMasterIds 

DECLARE @ItemMasterId INT = -1 
DECLARE @rowCount INT = (SELECT COUNT(id) 
   FROM   @ItemMasterIds) 

WHILE ( @rowCount > 0 ) 
  BEGIN 
      SET @ItemMasterId = (SELECT TOP 1 ItemMasterId 
						  FROM   @ItemMasterIds 
						  WHERE  id = @rowCount) 

      EXEC [CD].[FillSpecValues] 
        @newCarVersionId, 
        @ItemMasterID 
	  	  
	  EXEC [CD].[FillGroupItemValues] 
			@newCarVersionId, 
			@ItemMasterID 

      SELECT @rowCount 

      SET @rowCount -=1 
  END

-------amit verma				10/14/2013					Update model summary on value change
EXEC [CD].[GenModelSummary] @newCarVersionId,14

------Amit Verma		        Oct 07, 2013               	Added logic to update recommendcars table on bulk copy
IF EXISTS (SELECT TOP 1 RecommendCarId FROM RecommendCars WITH(NOLOCK) WHERE Versionid=@newCarVersionId)
EXEC UpdateRecommendCars @newCarVersionId
------Amit Verma		        Oct 07, 2013               	Added logic to update recommendcars table on bulk copy

--fill dbo.NewCarSpecifications end
END


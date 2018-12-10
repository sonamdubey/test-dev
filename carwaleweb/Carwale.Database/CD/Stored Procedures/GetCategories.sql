IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCategories]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURN CATEGORIES OF ITEMS
	IF YOU PASS CategoryMasterId = 0, IT WILL FETCH ALL THE ACTIVE CATEGORIES

	WRITTEN BY : SHIKHAR MAHESHWARI ON 3 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Shikhar Maheshwari         22 May 2012             Modified to add the Column Copy Car Data
       --------                	-----------       	             ----------
*/

CREATE PROCEDURE [CD].[GetCategories]
	@srcCarVersionId NUMERIC = 0,
	@tarCarVersionId NUMERIC = 0,
	@categoryMasterId NUMERIC = 0
	
AS

BEGIN
IF(@srcCarVersionId = 0 AND @tarCarVersionId = 0)
BEGIN
SELECT
	CM.CategoryMasterId as CategoryMasterId,
	(select CategoryName from CD.CategoryMaster where HierId = CM.HierID.GetAncestor(1))+' - ' + CM.CategoryName as Name
FROM 
	CD.CategoryMaster CM
  WHERE
	CM.lvl = 2
--WHERE
--	(CM.CategoryMasterId = @categoryMasterId OR @categoryMasterId = 0)
--AND
--	CM.IsActive = 1

--SELECT 
--	CM.CategoryMasterId, 
--	CM.Name
--FROM 
--	CD.CategoryMaster CM	
--WHERE
--	(CM.CategoryMasterId = @categoryMasterId OR @categoryMasterId = 0)
--AND
--	CM.IsActive = 1
END
ELSE
BEGIN
SELECT --CM.CategoryMasterId, 
       --CM.Name,
       	CM.CategoryMasterId as CategoryMasterId,
		(select CategoryName from CD.CategoryMaster where HierId = CM.HierID.GetAncestor(1))+' - ' + CM.CategoryName as Name,
       ( CASE ISNULL(SCM.CategoryMasterId, 0) 
           WHEN 0 THEN 1 
           ELSE 0 
         END ) AS IsSrcVal, 
       ( CASE ISNULL(TCM.CategoryMasterId, 0) 
           WHEN 0 THEN 1 
           ELSE 0 
         END ) AS IsTarVal 
FROM   CD.CategoryMaster CM WITH(NOLOCK) 
       LEFT JOIN (SELECT CategoryMasterId as CategoryMasterId from CD.CategoryMaster where NodeCode in (SELECT DISTINCT CIM.NodeCode 
                  FROM   CD.ItemValues IV WITH(NOLOCK) 
                         INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) 
                                 ON IV.ItemMasterId = CIM.ItemMasterId 
                  WHERE  CarVersionId = @SrcCarVersionId)) AS SCM 
              ON CM.CategoryMasterId = SCM.CategoryMasterId 
       LEFT JOIN (SELECT CategoryMasterId as CategoryMasterId from CD.CategoryMaster where NodeCode in (SELECT DISTINCT CIM.NodeCode 
                  FROM   CD.ItemValues IV WITH(NOLOCK) 
                         INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) 
                                 ON IV.ItemMasterId = CIM.ItemMasterId 
                  WHERE  CarVersionId = @tarCarVersionId)) AS TCM 
              ON CM.CategoryMasterId = TCM.CategoryMasterId 
WHERE  ( CM.CategoryMasterId = @categoryMasterId 
          OR @categoryMasterId = 0 )
          AND CM.lvl = 2 
       --AND CM.IsActive = 1 
--SELECT 
--	CM.CategoryMasterId, 
--	CM.Name,
--	(CASE ISNULL(SCM.CategoryMasterId, 0) 
--		WHEN 0 THEN 1 
--		ELSE 0 
--	END) AS IsSrcVal,
--	(CASE ISNULL(TCM.CategoryMasterId, 0) 
--		WHEN 0 THEN 1
--		ELSE 0 
--	END) AS IsTarVal 
--FROM CD.CategoryMaster CM WITH(NOLOCK)
--	LEFT JOIN (SELECT DISTINCT
--	CIM.CategoryMasterId
--	FROM CD.ItemValues IV WITH(NOLOCK)
--		INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
--			ON IV.ItemMasterId = CIM.ItemMasterId
--	WHERE 
--		CarVersionId = @SrcCarVersionId) AS SCM
--	ON 
--		CM.CategoryMasterId = SCM.CategoryMasterId
--	LEFT JOIN (SELECT DISTINCT
--	CIM.CategoryMasterId
--	FROM CD.ItemValues IV WITH(NOLOCK)
--		INNER JOIN CD.CategoryItemMapping CIM WITH(NOLOCK)
--			ON IV.ItemMasterId = CIM.ItemMasterId
--	WHERE 
--		CarVersionId = @tarCarVersionId) AS TCM
--	ON 
--		CM.CategoryMasterId = TCM.CategoryMasterId
--WHERE
--	(CM.CategoryMasterId = @categoryMasterId OR @categoryMasterId = 0)
--AND
--	CM.IsActive = 1
END
END

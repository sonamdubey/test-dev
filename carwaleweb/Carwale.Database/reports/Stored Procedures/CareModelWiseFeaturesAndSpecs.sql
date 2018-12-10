IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[CareModelWiseFeaturesAndSpecs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[CareModelWiseFeaturesAndSpecs]
GO

	--Created By: Manish on 01-10-2014 
--Description: Will return all specs and features of the comma separted make and model list
CREATE PROCEDURE Reports.CareModelWiseFeaturesAndSpecs
@CarModelIds VARCHAR (MAX)
 AS
  BEGIN

		DECLARE @Items VARCHAR(MAX) ='';
		DECLARE @SQL VARCHAR(MAX);

		SELECT  @Items= COALESCE(@ITEMS ,',') + '['+NAME+'],'   
		FROM  CD.ItemMaster WITH (NOLOCK)
		WHERE  IsActive=1
		AND    IsPublished = 1
		AND   (ItemTypeId = 1 OR ItemTypeId = 3);

		SET @Items= SUBSTRING (@Items,1,LEN(@Items)-1);


			SET @SQL=
							'(SELECT  MakeId,
									   MakeName,
									   ModelId,
									   ModelName,
									CarVersionName
									,CarVersionId ,'
									+ @ITEMS
							  +' FROM
							 (SELECT CarVersionName
									,CarVersionId 
									,MakeId
									,MakeName
									 ,ModelId
									 ,ModelName
									,ItemName
							,ItemValue
							FROM  (SELECT DISTINCT IM.NAME AS ItemName
									,ISNULL(CustomText, '''') + ISNULL(CAST(ItemValue AS VARCHAR(20)), '''') + ISNULL(UDM.NAME, '''') ItemValue
									,CategoryName
									, CASE  WHEN CIM.NodeCode LIKE ''/1/%'' THEN ''Specs'' ELSE ''Features'' END AS [SpecOrFeature]
									,UT.NAME AS UnitType
									,CM.SortOrder AS CategoryOrder
									,IM.SortOrder AS ItemOrder
									,CK.Id  As MakeId
									,CK.Name AS MakeName
									,CMD.ID AS  ModelId
									,CMD.Name AS ModelName
									,CV.Name AS CarVersionName
									,CV.ID   AS CarVersionId
								FROM CD.ItemValues IV WITH(NOLOCK)
								INNER JOIN CarVersions AS CV WITH(NOLOCK)  ON CV.ID=IV.CarVersionId
								INNER JOIN CarModels AS CMD WITH(NOLOCK) ON CMD.ID=CV.CarModelId
								INNER JOIN CarMakes AS CK WITH(NOLOCK) ON CK.ID=CMD.CarMakeId
								INNER JOIN CD.ItemMaster IM WITH(NOLOCK) ON IV.ItemMasterId = IM.ItemMasterId
								LEFT JOIN CD.UserDefinedMaster UDM WITH(NOLOCK) ON IV.UserDefinedId = UDM.UserDefinedId
								LEFT JOIN CD.CategoryItemMapping CIM WITH(NOLOCK) ON IM.ItemMasterId = CIM.ItemMasterId
								LEFT JOIN CD.CategoryMaster CM WITH(NOLOCK) ON CIM.NodeCode = CM.NodeCode
								LEFT JOIN CD.UnitTypes UT WITH(NOLOCK) ON IM.UnitTypeId = UT.UnitTypeId
								WHERE 
							  CMD.id  IN ('+@CarModelIds+')
								AND
								 IM.IsPublished = 1
								AND IM.IsActive = 1 AND (IM.ItemTypeId = 1 OR IM.ItemTypeId = 3)) --added by amit v
								AS Tab
							) A
							PIVOT 
							(
							 MAX(ItemValue)
							 FOR
							 ItemName  IN ( '
							 + @Items 	
							   +')
							) B
							) ORDER BY ModelId DESC;'

					EXEC( @SQL);


      END
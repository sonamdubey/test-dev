IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelsByMakeId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelsByMakeId]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 12-07-2014
-- Description:	for getting new Car Models by makeId passed
--Approved by: Manish Chourasiya on 01-07-2014 6:10pm
--Modified by: Rohan Sapkal on 26-08-2014
--Modify Description: Added ModelCondition For Expert Review(models only with expert reviews)
-- =============================================
CREATE PROCEDURE [cw].[GetCarModelsByMakeId] --[cw].[GetCarModelsByMakeId] 7,'expert'
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT = NULL
	,@ModelCond VARCHAR(10) = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@ModelCond = 'All')
		SELECT DISTINCT cm.NAME AS MakeName
			,*
			,cmo.NAME AS ModelName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
		WHERE cmo.IsDeleted = 0
		AND CarMakeId = @MakeId
		ORDER BY cmo.NAME
	ELSE
		IF (@ModelCond = 'New')
			SELECT DISTINCT cm.NAME AS MakeName
				,*
				,cmo.NAME AS ModelName
			FROM CarModels cmo WITH (NOLOCK)
			INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
			WHERE cmo.IsDeleted = 0
				AND cmo.Futuristic = 0
				AND cmo.New = 1
				AND CarMakeId = @MakeId
			ORDER BY cmo.NAME
		ELSE
			IF (@ModelCond = 'Used')
				SELECT DISTINCT cm.NAME AS MakeName
					,*
					,cmo.NAME AS ModelName
				FROM CarModels cmo WITH (NOLOCK)
				INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
				WHERE cmo.IsDeleted = 0
					AND cmo.Futuristic = 0
					AND cmo.Used = 1
					AND CarMakeId = @MakeId
				ORDER BY cmo.NAME
		ELSE
			IF (@ModelCond = 'expert')
				SELECT DISTINCT cm.NAME AS MakeName
					,cmo.ID
					,cmo.NAME AS ModelName
					,cmo.MaskingName,cmo.*
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CB.id = CC.BasicId
				INNER JOIN CarModels cmo WITH (NOLOCK) ON CC.ModelId = cmo.ID
				INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
				WHERE CB.CategoryId IN (
						8
						,2
						)
					AND cmo.IsDeleted = 0
					AND MakeId = @MakeId
					AND CB.IsPublished=1 AND CB.IsActive=1
					AND CB.ApplicationID = 1
			ORDER BY cmo.NAME
END




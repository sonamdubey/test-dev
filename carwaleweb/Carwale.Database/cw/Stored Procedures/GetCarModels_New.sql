IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModels_New]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModels_New]
GO

	

-- =============================================
-- Author:		Ashish Verma
-- Create date: 24-03-2014
-- Description:	for getting new Car Models for xml site map generation
-- Modified by : Ashwini Todkar on 13-10-2015 added condition of new make while retrieving models
-- Modified by : Ashwini Todkar on 21-10-2015 retrived upcoming models
-- =============================================
CREATE PROCEDURE [cw].[GetCarModels_New]
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT = NULL
	,@ModelCond VARCHAR(10) = NULL
	,@ID SMALLINT = NULL
	-- @carYear smallint = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@ModelCond = 'All')
		SELECT DISTINCT cm.Id AS MakeId
			,cm.NAME AS MakeName
			,cmo.NAME AS ModelName
			,cmo.ID ModelId
			,cmo.MaskingName AS MaskingName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
		WHERE cmo.IsDeleted = 0
		ORDER BY cmo.NAME
	ELSE
		IF (@ModelCond = 'New')
			SELECT DISTINCT cm.Id AS MakeId
				,cm.NAME AS MakeName
				,cmo.NAME AS ModelName
				,cmo.ID ModelId
				,cmo.MaskingName AS MaskingName
			FROM CarModels cmo WITH (NOLOCK)
			INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
			WHERE cmo.IsDeleted = 0
				AND cmo.Futuristic = 0
				AND cmo.New = 1 AND cm.New = 1 -- added by Ashwini Todkar
			ORDER BY cmo.NAME
		ELSE
			IF (@ModelCond = 'Used')
				SELECT DISTINCT cm.Id AS MakeId
					,cm.NAME AS MakeName
					,cmo.NAME AS ModelName
					,cmo.ID ModelId
					,cmo.MaskingName AS MaskingName
				FROM CarModels cmo WITH (NOLOCK)
				INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
				WHERE cmo.IsDeleted = 0
					AND cmo.Futuristic = 0
					AND cmo.Used = 1
				ORDER BY cmo.NAME
			ELSE 
				IF (@ModelCond = 'Upcoming')
					SELECT DISTINCT cm.Id AS MakeId
						,cm.NAME AS MakeName
						,cmo.NAME AS ModelName
						,cmo.ID ModelId
						,cmo.MaskingName AS MaskingName
					FROM CarModels cmo WITH (NOLOCK)
					INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
					WHERE cmo.IsDeleted = 0
						AND cmo.Futuristic = 1
						AND cmo.New = 0 AND cm.New = 1 -- added by Ashwini Todkar
					ORDER BY cmo.NAME
	IF (@ModelCond = 'id')
		SELECT cm.Id AS MakeId
			,cm.NAME AS MakeName
			,cmo.NAME AS ModelName
			,cmo.ID ModelId
			,cmo.MaskingName AS MaskingName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN CarMakes cm WITH (NOLOCK) ON cmo.IsDeleted = cm.IsDeleted
		WHERE cm.IsDeleted = 0
			AND cmo.ID = @ID
			AND cmo.Futuristic = 0
		ORDER BY cmo.NAME
	ELSE
		SELECT cm.Id AS MakeId
			,cm.NAME AS MakeName
			,cmo.NAME AS ModelName
			,cmo.ID ModelId
			,cmo.MaskingName AS MaskingName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN CarMakes cm WITH (NOLOCK) ON cmo.IsDeleted = cm.IsDeleted
		WHERE cmo.IsDeleted = 0
			AND CarMakeId = @MakeId
			AND cmo.Futuristic = 0
		ORDER BY cmo.NAME
END


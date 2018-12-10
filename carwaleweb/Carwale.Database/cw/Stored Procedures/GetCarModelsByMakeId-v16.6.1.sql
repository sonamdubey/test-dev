IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelsByMakeId-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelsByMakeId-v16]
GO

	

-- =============================================
-- Author:		Piyush Sahu
-- Create date: 6/20/2016
-- Description:	for getting  All Car Models except futuristic
-- Modified by Piyush Added nonfuturistic condition to fetch all models except deleted and futuristic to be used in insurance section
-- =============================================
CREATE PROCEDURE [cw].[GetCarModelsByMakeId-v16.6.1] 
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT = NULL
	,@ModelCond VARCHAR(30) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (lower(@ModelCond) = 'all')
		SELECT DISTINCT cm.NAME AS MakeName
			,cmo.ID
			,cmo.NAME AS ModelName
			,cmo.MaskingName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
		WHERE cmo.IsDeleted = 0
			AND CarMakeId = @MakeId
		ORDER BY cmo.NAME

	ELSE
		IF (lower(@ModelCond) = 'nonfuturistic')   -- added by piyush
		SELECT DISTINCT cm.NAME AS MakeName
			,cmo.ID
			,cmo.NAME AS ModelName
			,cmo.MaskingName
		FROM CarModels cmo WITH (NOLOCK)
		INNER JOIN carmakes cm WITH (NOLOCK) ON cmo.CarMakeId = cm.ID
		WHERE cmo.IsDeleted = 0
			AND cmo.Futuristic = 0
			AND CarMakeId = @MakeId
		ORDER BY cmo.NAME
	ELSE
		IF (@ModelCond = 'New')
			SELECT DISTINCT cm.NAME AS MakeName
				,cmo.ID
				,cmo.NAME AS ModelName
				,cmo.MaskingName
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
					,cmo.ID
					,cmo.NAME AS ModelName
					,cmo.MaskingName
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
						,cmo.MaskingName
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
						AND CB.IsPublished = 1
						AND CB.IsActive = 1
						AND CB.ApplicationID = 1
					ORDER BY cmo.NAME
END


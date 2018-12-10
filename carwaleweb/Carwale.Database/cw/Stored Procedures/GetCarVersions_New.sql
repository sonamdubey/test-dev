IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarVersions_New]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarVersions_New]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,17/04/2014>
-- Description:	<Description,to geting car Versions based on version Condition>
-- Modified By : Supriya K on 25/8/2014 to add version condition for compare
-- Modified By : Vikas J on 7/3/2016 order by price for new versions
-- =============================================
CREATE PROCEDURE [cw].[GetCarVersions_New]
	-- Add the parameters for the stored procedure here
	@VersionCond VARCHAR(10)
	,@modelId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	IF (@VersionCond = 'New')
	BEGIN
	   ;WITH CTE
		AS (
			SELECT CV.ID AS Value
				,CV.NAME AS TEXT
				,PQ_CategoryItemValue
				,ROW_NUMBER() OVER (
					PARTITION BY NCP.CarVersionId ORDER BY PQ_CategoryItemValue
					) RowNum
			FROM CarVersions CV WITH (NOLOCK)
			INNER JOIN CW_NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
			WHERE IsDeleted = 0
				AND Futuristic = 0
				AND New = 1
				AND CarModelId = @modelId
				AND NCP.CityId = 10
				AND NCP.PQ_CategoryItem = 2
			)
		SELECT Value
			,TEXT
		FROM CTE WITH (NOLOCK)
		WHERE RowNum = 1
		ORDER BY PQ_CategoryItemValue
	END
	ELSE
		IF (@VersionCond = 'Compare')
			SELECT *
				,Id AS Value
				,NAME AS TEXT
			FROM CarVersions WITH (NOLOCK)
			WHERE IsDeleted = 0
				AND Futuristic = 0
				AND New = 1
				AND IsSpecsAvailable = 1
				AND CarModelId = @modelId
			ORDER BY NAME
		ELSE
			IF (@VersionCond = 'Used')
				SELECT *
					,Id AS Value
					,NAME AS TEXT
				FROM CarVersions WITH (NOLOCK)
				WHERE IsDeleted = 0
					AND Futuristic = 0
					AND Used = 1
					AND CarModelId = @modelId
				ORDER BY NAME
			ELSE
				IF (@VersionCond = 'All')
					SELECT *
						,Id AS Value
						,NAME AS TEXT
					FROM CarVersions WITH (NOLOCK)
					WHERE IsDeleted = 0
						AND CarModelId = @modelId
					ORDER BY NAME
END

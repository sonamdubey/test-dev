IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQByModelCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQByModelCity]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 23-OCT-2015
-- Description:	Get prices based on model and city
-- =============================================
-- exec GetPQByModelCity 862,1
CREATE PROCEDURE [dbo].[GetPQByModelCity] @ModelId INT
	,@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CV.ID AS VersionId
		,CV.NAME AS VersionName
		,PQC.CategoryId
		,Ci.Id AS CategoryItemId
		,CI.CategoryName AS CategoryItemName
		,ISNULL(PQN.PQ_CategoryItemValue,0) AS CategoryItemValue
		,PQN.isMetallic
	FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
	INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = PQN.CarVersionId AND CV.IsDeleted = 0 AND CV.New = 1
	WHERE CV.CarModelId = @ModelId
		AND PQN.CityId = @CityId
		AND PQN.PQ_CategoryItemValue IS NOT NULL -- filtered Null Values
		AND PQC.CategoryId NOT IN (
			1
			,2
			,7
			) -- not retrieving 'Price before local tax', 'Local Tax', 'Optional Charges'(categoryId= 1, 2, 7) respectively
	ORDER BY CV.ID
		,PQC.SortOrder ASC
END

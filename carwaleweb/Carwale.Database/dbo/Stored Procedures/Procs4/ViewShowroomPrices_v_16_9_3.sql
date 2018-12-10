IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewShowroomPrices_v_16_9_3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewShowroomPrices_v_16_9_3]
GO

	-- =============================================
-- Author:		<Raghupathy>
-- Create date: <10/5/2013>
-- Description:	<This Sp is used to get LocalTax Name and Rate, IsTaxonTax field>
-- Modified by Raghu : <10/24/2013> added order by Categoryid 
-- Modified by Raghu : <10/31/2013> checked CV.IsDeleted Flag
-- Modified by Jitendra : <12/10/2015> change sp to grouped metallic and non metallic prices
-- Modified by Chetan : <28/06/2016> getting prices if the rule is active
-- Modified by Anuj : <09/16/2016> Reduced joins and introduced new sort order
-- Modified by Anuj : <14/10/2016> Introduced OrderColumn to keep TCS column in last
-- [dbo].[ViewShowroomPrices_v_16_9_3] 10,493
-- =============================================
CREATE PROCEDURE [dbo].[ViewShowroomPrices_v_16_9_3]
	@CityId INT
	,@ModelId INT
	,@OnlyNew BIT
	,@SolidInd BIT
	,@MetallicInd BIT
AS
DECLARE @SortOrder BIT

BEGIN
	SET @SortOrder = 0

	SELECT @SortOrder = ToggleFlag
	FROM ModelCitySortOrder WITH (NOLOCK)
	WHERE ModelId = @ModelId
		AND CityId = @CityId

	SELECT VersionId
		,VersionName
		,ItemValue
		,CategoryName
		,LastUpdated
		,IsMetallic
		,Color
		,ModelId
		,IsDeleted
		,New
		,RowOrder
	INTO #SolidVersionsDetails
	FROM (
		SELECT CV.ID AS VersionId
			,CV.NAME VersionName
			,NP.PQ_CategoryItemValue AS ItemValue
			,CI.CategoryName
			,DATEDIFF(dd, NP.LastUpdated, GETDATE()) AS LastUpdated
			,0 AS IsMetallic
			,'SOLID' AS Color
			,CV.CarModelId AS ModelId
			,CV.IsDeleted
			,CV.New
			,ROW_NUMBER() OVER (
				ORDER BY NP.PQ_CategoryItemValue
				) RowOrder
		FROM CarVersions CV WITH (NOLOCK)
		LEFT JOIN CW_NewCarShowroomPrices NP WITH (NOLOCK) ON CV.ID = NP.CarVersionId
			AND NP.CityId = @CityId
			AND isMetallic = 0
			AND NP.PQ_CategoryItem = 2
		LEFT JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = NP.PQ_CategoryItem
		LEFT JOIN PQ_Category PC WITH (NOLOCK) ON PC.CategoryId = CI.CategoryId
		WHERE CV.CarModelId = @ModelId
			AND CV.IsDeleted = 0
			AND CV.New = @OnlyNew
		) AS X

	SELECT VersionId
		,VersionName
		,ItemValue
		,CategoryName
		,LastUpdated
		,IsMetallic
		,Color
		,RowOrder
	INTO #MetallicVersionsDetails
	FROM (
		SELECT SVD.VersionId
			,SVD.VersionName
			,NP.PQ_CategoryItemValue AS ItemValue
			,CI.CategoryName
			,DATEDIFF(dd, NP.LastUpdated, GETDATE()) AS LastUpdated
			,1 AS IsMetallic
			,'METALLIC' AS Color
			,ROW_NUMBER() OVER (
				ORDER BY SVD.RowOrder
				) RowOrder
		FROM #SolidVersionsDetails SVD WITH (NOLOCK)
		LEFT JOIN CW_NewCarShowroomPrices NP WITH (NOLOCK) ON SVD.VersionId = NP.CarVersionId
			AND NP.CityId = @CityId
			AND NP.IsMetallic = 1
			AND NP.PQ_CategoryItem = 2
		LEFT JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = NP.PQ_CategoryItem
		LEFT JOIN PQ_Category PC WITH (NOLOCK) ON PC.CategoryId = CI.CategoryId
		WHERE SVD.ModelId = @ModelId
			AND SVD.IsDeleted = 0
			AND SVD.New = @OnlyNew
		) AS X

	IF @SolidInd = 1
		AND @MetallicInd = 1
	BEGIN
		SELECT VersionId
			,VersionName
			,PQ_CategoryItemValue
			,CategoryName
			,UpdatedBeforeDays
			,isMetallic
			,Color
		FROM (
			SELECT VersionId
				,VersionName
				,ItemValue AS PQ_CategoryItemValue
				,CategoryName
				,LastUpdated AS UpdatedBeforeDays
				,isMetallic
				,Color
				,RowOrder
			FROM #SolidVersionsDetails
			
			UNION ALL
			
			SELECT VersionId
				,VersionName
				,ItemValue AS PQ_CategoryItemValue
				,CategoryName
				,LastUpdated AS UpdatedBeforeDays
				,isMetallic
				,Color
				,RowOrder
			FROM #MetallicVersionsDetails
			) AS TEMP
		ORDER BY CASE 
				WHEN @SortOrder = 0
					THEN Color
				END DESC
			,CASE 
				WHEN @SortOrder = 0
					THEN PQ_CategoryItemValue
				END ASC
			,CASE 
				WHEN @SortOrder = 1
					THEN RowOrder
				END ASC
			,CASE 
				WHEN @SortOrder = 1
					THEN Color
				END DESC
	END
	ELSE
	BEGIN
		DECLARE @ColorInd BIT
		DECLARE @ColorName VARCHAR(12)

		IF (@SolidInd = 1)
		BEGIN
			SELECT VersionId
				,VersionName
				,ItemValue AS PQ_CategoryItemValue
				,CategoryName
				,LastUpdated AS UpdatedBeforeDays
				,isMetallic
				,Color
			FROM #SolidVersionsDetails
			ORDER BY PQ_CategoryItemValue ASC
		END
		ELSE
		BEGIN
			SELECT VersionId
				,VersionName
				,ItemValue AS PQ_CategoryItemValue
				,CategoryName
				,LastUpdated AS UpdatedBeforeDays
				,isMetallic
				,Color
			FROM #MetallicVersionsDetails
			ORDER BY PQ_CategoryItemValue ASC
		END
	END

	SELECT NP.CarVersionId AS VersionId
		,CI.CategoryName
		,Ci.Id AS ItemId
		,Np.PQ_CategoryItemValue AS ItemValue
		,CI.CategoryId
		,PC.SortOrder
		,NP.isMetallic
		,(
			CASE CI.CategoryName
				WHEN 'Tax Collected at Source (TCS)'
					THEN '1'
				ELSE '0'
				END
			) AS OrderColumn
	FROM CW_NewCarShowroomPrices NP WITH (NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = NP.CarVersionId
		AND CV.CarModelId = @ModelId
		AND CV.IsDeleted = 0
	INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = NP.PQ_CategoryItem
	INNER JOIN PQ_Category PC WITH (NOLOCK) ON PC.CategoryId = CI.CategoryId
	WHERE NP.CityId = @CityId
		AND CV.New = @OnlyNew --AND --CI.IsActive = 1
	--ORDER BY CI.CategoryId --Added by Raghu
	ORDER BY OrderColumn
		,PC.SortOrder
		,isMetallic

	SELECT @SortOrder AS ToggleFlag

	DROP TABLE #SolidVersionsDetails

	DROP TABLE #MetallicVersionsDetails
END

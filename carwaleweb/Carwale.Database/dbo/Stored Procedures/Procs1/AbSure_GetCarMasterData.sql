IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarMasterData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarMasterData]
GO

	-- =================================================================================================================
-- Author:		Vinay Kumar Prajapati
-- Create date: 12/12/2014
-- Description:	Get Make model, Version,  State, city, area data on the basis of  user desires.
-- EXEC [AbSure_GetCarMasterData] NULL,NULL,NULL,NULL,NULL,0,0,0,0,1,0
-- Modified By: Ashwini Dhamankar on March 2,2015 , Added join with Absure_EligibleCities
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- Modified By : Suresh Prajapati on 26th May, 2015
-- Description : To get car makes based on product type (if '1' i.e. Warranty only select eligible makes)
-- Modified BY : Chetan Navin - 7 Jul 2015 (Changed service tax to 14%)
-- exec AbSure_GetCarMasterData 10,NULL,NULL,NULL,NULL,NULL,0,1,0,0,0,0,3
-- Modified BY : Tejashree Patil on 3 Aug 2015, Made @EligibleModelFor = 2 i.e certification by default.
-- Modified By : Vinay Kumar Prajapati 4th Aug 2015 Added 'OR' Condition for  @EligibleModelFor=3
-- Modified By : Deepak Tripathi on 10th Aug, 2015
-- Description : 
-- =================================================================================================================
CREATE PROCEDURE [dbo].[AbSure_GetCarMasterData] @MakeId INT = NULL
	,@ModelId INT = NULL
	,@VersionId INT = NULL
	,@StateId INT = NULL
	,@CityId INT = NULL
	,@ProductType TINYINT = NULL --1 for Warranty (only eligible makes) 
	,@IsMake BIT = 0
	,@IsModel BIT = 0
	,@IsVersion BIT = 0
	,@IsState BIT = 0
	,@IsCity BIT = 0
	,@IsArea BIT = 0
	,@EligibleModelFor TINYINT = 2 -- 1: Warranty, 2: By default Inspection
AS
BEGIN
	-- select Car Make 
	IF @IsMake <> 0
	BEGIN
		IF @ProductType = 1 --For Warranty
		BEGIN
			--SELECT CM.ID
			--	,CM.NAME
			--FROM CarMakes AS CM WITH (NOLOCK)
			--WHERE CM.IsDeleted = 0
			--	AND CM.Futuristic = 0
			--	AND CM.New = 1
			--ORDER BY CM.NAME
			SELECT DISTINCT CMA.NAME
				,CMA.Id
			FROM AbSure_EligibleModels AM WITH (NOLOCK)
			INNER JOIN CarModels CMO WITH (NOLOCK) ON AM.ModelId = CMO.ID
			INNER JOIN CarMakes CMA WITH (NOLOCK) ON CMA.ID = CMO.CarMakeId
			WHERE AM.IsEligibleWarranty = 1
				AND AM.IsActive = 1
				AND CMA.ID NOT IN (
					5
					,7
					,8
					,9
					,10
					,16
					,17
					)
			ORDER BY CMA.NAME
		END
		ELSE
		BEGIN
			SELECT CM.ID
				,CM.NAME
			FROM CarMakes AS CM WITH (NOLOCK)
			WHERE CM.IsDeleted = 0
				AND CM.Futuristic = 0
			ORDER BY CM.NAME
		END
	END

	-- select Car Model 
	IF @IsModel <> 0
	BEGIN
		SELECT CM.ID
			,CM.NAME
			,CASE 
				WHEN EM.ModelId IS NOT NULL
					AND EM.IsActive = 1
					THEN 1
				ELSE 0
				END IsEligible
			,CAST(ISNULL(EM.SilverPrice, 0) + (ISNULL(EM.SilverPrice, 0) * 14 / 100) AS DECIMAL(10, 2)) SilverPrice
			,CAST(ISNULL(EM.GoldPrice, 0) + (ISNULL(EM.GoldPrice, 0) * 14 / 100) AS DECIMAL(10, 2)) GoldPrice
		FROM CarModels AS CM WITH (NOLOCK)
		LEFT JOIN AbSure_EligibleModels EM WITH (NOLOCK) ON EM.ModelId = CM.ID
		WHERE CM.IsDeleted = 0
			AND CM.CarMakeId = @MakeId
			AND CM.Futuristic = 0
			AND (
				CM.Used = 1
				OR CM.New = 1
				)
			AND (
				CASE @EligibleModelFor
					WHEN 1
						THEN ISNULL(EM.IsEligibleWarranty, 0) -- Warranty
					WHEN 2
						THEN ISNULL(EM.IsEligibleCertification, 0) -- Certificate
					WHEN 3
						THEN 1
					END = 1 -- ALL
				)
		ORDER BY CM.NAME
	END

	-- select Car Version 
	IF @IsVersion <> 0
	BEGIN
		SELECT CV.ID
			,CV.NAME
			,CV.CarFuelType
		FROM CarVersions AS CV WITH (NOLOCK)
		WHERE CV.IsDeleted = 0
			AND CV.CarModelId = @ModelId
			AND CV.Futuristic = 0
			AND (
				CV.Used = 1
				OR CV.New = 1
				)
		ORDER BY CV.NAME
	END

	-- select state
	IF @IsState <> 0
	BEGIN
		SELECT S.ID
			,S.NAME
		FROM States AS S WITH (NOLOCK)
		WHERE S.IsDeleted = 0
		ORDER BY S.NAME
	END

	-- select city 
	IF @IsCity <> 0
	BEGIN
		-- This query is used  for total city
		SELECT C.ID
			,C.NAME
			,CASE 
				WHEN EC.CityId IS NOT NULL
					AND EC.IsActive = 1
					THEN 1
				ELSE 0
				END IsEligible
		FROM Cities AS C WITH (NOLOCK)
		LEFT JOIN AbSure_EligibleCities EC WITH (NOLOCK) ON EC.CityId = C.ID
		WHERE C.IsDeleted = 0
			AND (
				@StateId IS NULL
				OR C.StateId = @StateId
				)
		ORDER BY C.NAME
			-- This  query is used for dealer City
			--SELECT DISTINCT C.Id,C.Name FROM Cities C WITH(NOLOCK) 
			--INNER JOIN Dealers D WITH(NOLOCK) ON D.CityId = C.Id AND D.IsWarranty = 1
			--WHERE C.IsDeleted = 0 ORDER BY C.Name ASC 
	END

	-- select Area  only 
	IF @IsArea <> 0
	BEGIN
		SELECT AR.ID
			,AR.NAME
		FROM Areas AS AR WITH (NOLOCK)
		WHERE AR.IsDeleted = 0
			AND AR.CityId = @CityId
		ORDER BY AR.NAME
	END
END

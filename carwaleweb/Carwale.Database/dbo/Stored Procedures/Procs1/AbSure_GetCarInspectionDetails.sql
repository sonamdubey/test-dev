IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarInspectionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarInspectionDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 2-01-2015
-- Description:	To fetch Car Inspection Data for AbSure Report
-- Modified By : Tejashree Patil on 9 Jan 2015, Selected Category and SubCategoryWise percentage and car score.
-- Modified By: Ashwini Dhamankar on Jan 16,2015, Ordered result by Sequence
-- Modified By: Ashwini Dhamankar on Jan 30,2015, Added script to get car part inspection details
-- Modified By : Ashwini Dhamankar on April 8,2015, added condition of Absure_QuestionId > 138 fofr new questions
-- Modified By : Ruchira Patil 11th Jun,2015, Modified the subcategory score according to the new logic
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarInspectionDetails]
	-- Add the parameters for the stored procedure here
	@AbSure_CarDetailsId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MaxQuestionId INT
	
	SELECT	@MaxQuestionId = MAX(I.AbSure_QuestionsId)       
	FROM	AbSure_InspectionData I  WITH(NOLOCK)
	WHERE	I.AbSure_CarDetailsId = @AbSure_CarDetailsId
	
	IF(@MaxQuestionId <= 138)
	BEGIN
		EXECUTE AbSure_GetCarInspectionDetailsOld @AbSure_CarDetailsId
	END
	ELSE
	BEGIN

		SELECT			I.AbSure_QuestionsId QuestionsId,Q.Question,Q.Type QuestionType,I.AbSure_AnswerValue AnswerValue,
						I.AnswerComments AnswerComments,Q.AbSure_QCategoryId CategoryId,C.Category, Q.AbSure_QSubCategoryId SubCategoryId, 
						S.SubCategory, NULL CategoryPercentage, NULL SubCategoryPercentage, Q.Weightage
		FROM			AbSure_InspectionData  I with (NOLOCK)
		INNER JOIN		AbSure_Questions Q with (NOLOCK) on I.AbSure_QuestionsId = Q.AbSure_QuestionsId
		INNER JOIN		AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
		INNER JOIN		AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
		WHERE			AbSure_CarDetailsId = @AbSure_CarDetailsId AND Q.AbSure_QuestionsId > 138   --Modified By : Ashwini Dhamankar on April 8,2015, added condition of Absure_QuestionId > 138 for new questions
		ORDER BY        C.Sequence, S.SubCategory
		
		DECLARE @Kilometer INT,@Age INT,@DependentScore DECIMAL(8,2),@Maxrange BIGINT

		SELECT @Maxrange = POWER(CAST(2 AS VARCHAR),(32) -1)-1 FROM SYS.TYPES WHERE name = 'Int'

		SELECT @Kilometer=Kilometer,@Age = DATEDIFF(MONTH,MakeYear,GETDATE())
		FROM AbSure_CarDetails
		WHERE ID=@AbSure_CarDetailsId

		SELECT @DependentScore = ((SUM(CAST((CAST(WeightagePercent AS decimal)/100)*MaxWeightage AS decimal(8,2))))/(SUM(MaxWeightage)))
		FROM AbSure_CarScoreParameterValues
		 WHERE	(ParameterId=1 AND @Age >= MinValue+1 AND @Age <= ISNULL(MaxValue,@Maxrange))
					OR (ParameterId=2 AND @Kilometer >= MinValue+1 AND @Kilometer <= ISNULL(MaxValue,@Maxrange))
					
		DECLARE @TempSubCategory TABLE 
		(
			Category INT,
			SubCategory INT,
			AnswerWt INT,
			TotalWt INT,
			DependencyFactor DECIMAL(8,2)
		)

		INSERT INTO @TempSubCategory(Category ,	SubCategory,AnswerWt,TotalWt,DependencyFactor)
		SELECT	C.AbSure_QCategoryId ,S.AbSure_QSubCategoryId ,
					SUM(CASE WHEN ID.AbSure_AnswerValue=1 THEN Q.Weightage
					WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 0.5 *Q.Weightage 
						ELSE 0 END),
				SUM(CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
					ELSE Q.Weightage END),
				CASE S.IsParameterDependent
				WHEN 1
				THEN 
				(
					((SUM(CASE WHEN ID.AbSure_AnswerValue=1 THEN Q.Weightage
					WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 0.5 *Q.Weightage 
						ELSE 0 END)) * 0.8)
					+ ((SUM(CASE WHEN ID.AbSure_AnswerValue=1 THEN Q.Weightage
					WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 0.5 *Q.Weightage 
						ELSE 0 END)) * 0.2 * @DependentScore)
				)
				ELSE
					(SUM(CASE WHEN ID.AbSure_AnswerValue=1 THEN Q.Weightage
					WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 0.5 *Q.Weightage 
						ELSE 0 END)) 
				END
			FROM	AbSure_Questions Q WITH (NOLOCK)
					LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
					INNER JOIN	AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
					INNER JOIN	AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
			WHERE	Q.IsActive = 1 
			GROUP BY C.AbSure_QCategoryId, S.AbSure_QSubCategoryId,S.IsParameterDependent

			SELECT	C.AbSure_QCategoryId CategoryId,S.AbSure_QSubCategoryId SubCategoryId,C.Category Category,S.SubCategory SubCategory,C.Sequence,
					CASE SC.TotalWt WHEN 0 THEN 0 
					ELSE
						(CAST(CAST((SC.DependencyFactor) As float) /
						(SC.TotalWt)  As decimal(8, 2))) *100 
					END
					AS	SubCategoryPercentage, 
					CASE SUM(SC.TotalWt) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
					ELSE
						(SELECT Parameter FROM AbSure_ConfigurableParameters WHERE (convert(int,ROUND((CAST(CAST((SC.DependencyFactor) As float) /(SC.TotalWt)  As decimal(8, 2))) *100 ,0))) BETWEEN MinValue AND MaxValue AND Category = 'Car Conditions')
					END AS CategoryCondition,
					0 CategoryPercentage
			FROM	AbSure_Questions Q WITH (NOLOCK)
					LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
					INNER JOIN	AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
					INNER JOIN	AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
					INNER JOIN @TempSubCategory SC ON S.AbSure_QSubCategoryId = SC.SubCategory
			WHERE	Q.IsActive = 1
			GROUP BY C.AbSure_QCategoryId, S.AbSure_QSubCategoryId, C.Category, S.SubCategory,C.Sequence,SC.DependencyFactor,SC.AnswerWt,SC.TotalWt

			UNION
		
			SELECT	C.AbSure_QCategoryId CategoryId,0 SubCategoryId,C.Category Category,NULL SubCategory,C.Sequence,
					0 SubCategoryPercentage,
					CASE SUM(SC.TotalWt) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
					ELSE
						(SELECT Parameter FROM AbSure_ConfigurableParameters WHERE (convert(int,ROUND(CAST((SUM(SC.DependencyFactor)) /SUM(SC.TotalWt) AS decimal(8,2))*100,0))) BETWEEN MinValue AND MaxValue AND Category = 'Car Conditions')
					END AS CategoryCondition,
					CASE SUM(SC.TotalWt) WHEN 0 THEN 0 
					ELSE
						CAST((SUM(SC.DependencyFactor)) /SUM(SC.TotalWt) AS decimal(8,2))*100
					END AS CategoryPercentage
			FROM	AbSure_Questions Q WITH (NOLOCK)
					LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
					INNER JOIN	AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
					INNER JOIN	AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
					INNER JOIN @TempSubCategory SC ON C.AbSure_QCategoryId = SC.Category
			WHERE	Q.IsActive = 1 
			GROUP BY C.AbSure_QCategoryId,C.Category,C.Sequence
			ORDER BY C.Sequence, S.SubCategory

			SELECT DISTINCT CP.AbSure_QCarPartsId,CP.PartName,dbo.[AbSure_GetPartInspectionDetails](API.AbSure_QCarPartResponsesId) AS Response,API.AbSure_QCarPartResponsesId
			FROM			AbSure_QCarParts CP WITH(NOLOCK)
			LEFT JOIN		AbSure_PartsInspectionData API WITH(NOLOCK) ON API.AbSure_QCarPartsId = CP.AbSure_QCarPartsId AND API.AbSure_CarDetailsId = @AbSure_CarDetailsId
	END	
END




-----------------------------------------------------------------------------------------------------------------





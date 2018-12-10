IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarInspectionDetailsOld]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarInspectionDetailsOld]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 2-01-2015
-- Description:	To fetch Car Inspection Data for AbSure Report
-- Modified By : Tejashree Patil on 9 Jan 2015, Selected Category and SubCategoryWise percentage and car score.
-- Modified By: Ashwini Dhamankar on Jan 16,2015, Ordered result by Sequence
-- Modified By: Ashwini Dhamankar on Jan 30,2015, Added script to get car part inspection details
-- Modified by : Ruchira Patil on 10 April 2015 , New Logic
-- Modified By : Ashwini Dhamankar on June 12,2015 , Fetched Answer comments
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarInspectionDetailsOld] 
	-- Add the parameters for the stored procedure here
	@AbSure_CarDetailsId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT			I.AbSure_QuestionsId QuestionsId,Q.Question,Q.Type QuestionType,I.AbSure_AnswerValue AnswerValue,
					I.AnswerComments AnswerComments,Q.AbSure_QCategoryId CategoryId,C.Category, Q.AbSure_QSubCategoryId SubCategoryId, 
					S.SubCategory, NULL CategoryPercentage,NULL SubCategoryPercentage, Q.Weightage
	FROM			AbSure_InspectionData  I with (NOLOCK)
	INNER JOIN		AbSure_Questions Q with (NOLOCK) on I.AbSure_QuestionsId = Q.AbSure_QuestionsId
	INNER JOIN		AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
	INNER JOIN		AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
	WHERE			AbSure_CarDetailsId = @AbSure_CarDetailsId  --AND Q.AbSure_QuestionsId > 138   --Modified By : Ashwini Dhamankar on April 8,2015, added condition of Absure_QuestionId > 138 to test scoring logic
	ORDER BY        C.Sequence, S.SubCategory
	/*
	SELECT	C.AbSure_QCategoryId CategoryId,S.AbSure_QSubCategoryId SubCategoryId,C.Category Category,S.SubCategory SubCategory,
				SUM(CASE  WHEN ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
					WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1
					WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2 
					ELSE Q.Weightage*0 END) AnsWeightage,
				SUM(CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
					WHEN Q.Type = 2 THEN 2 
					ELSE Q.Weightage END) TotalWeightage,
			CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END)
			WHEN 0
				THEN 0
			ELSE ROUND(CAST(CAST((SUM(	CASE  WHEN ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2 
										ELSE Q.Weightage*0 END)) As float) /
				 (SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
						WHEN Q.Type = 2 THEN 2 
						ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0)
			END AS SubCategoryPercentage
	FROM	AbSure_Questions Q WITH (NOLOCK)
			LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
			INNER JOIN		AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
			INNER JOIN		AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
	WHERE	Q.IsActive = 1
	GROUP BY C.AbSure_QCategoryId, S.AbSure_QSubCategoryId, C.Category, S.SubCategory
	ORDER BY C.AbSure_QCategoryId
	*/
	
	SELECT	C.AbSure_QCategoryId CategoryId,S.AbSure_QSubCategoryId SubCategoryId,C.Category Category,S.SubCategory SubCategory,C.Sequence,
				SUM(CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
				WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
				WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
				ELSE Q.Weightage*0 END) AnsWeightage,
				SUM(CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
					WHEN Q.Type = 2 THEN 2 
					ELSE Q.Weightage END) TotalWeightage,
			CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END)
			WHEN 0
				THEN 0
			ELSE ROUND(CAST(CAST((SUM(	CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
										ELSE Q.Weightage*0 END)) As float) /
				 (SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
						WHEN Q.Type = 2 THEN 2 
						ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0)
			END AS SubCategoryPercentage, 
			CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
			ELSE
				(SELECT Parameter 
				FROM AbSure_ConfigurableParameters 
				WHERE (convert(int,ROUND(CAST(CAST((SUM(	CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
										ELSE Q.Weightage*0 END)) As float) /
				 (SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
						WHEN Q.Type = 2 THEN 2 
						ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0))) BETWEEN MinValue AND MaxValue 
				AND Category = 'Car Conditions')
			END AS CategoryCondition,
			0 CategoryPercentage
	FROM	AbSure_Questions Q WITH (NOLOCK)
			LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
			INNER JOIN	AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
			INNER JOIN	AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
	WHERE	Q.IsActive = 0
	GROUP BY C.AbSure_QCategoryId, S.AbSure_QSubCategoryId, C.Category, S.SubCategory,C.Sequence
	--ORDER BY C.AbSure_QCategoryId

	UNION
	
	SELECT	C.AbSure_QCategoryId CategoryId,0 SubCategoryId,C.Category Category,NULL SubCategory,C.Sequence,
			NULL AnsWeightage,NULL TotalWeightage,
			0 SubCategoryPercentage,
			CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
			ELSE
				(SELECT Parameter 
				FROM AbSure_ConfigurableParameters 
				WHERE (convert(int, ROUND(CAST(CAST((SUM(	CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
										ELSE Q.Weightage*0 END)) As float) /
					(SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
						WHEN Q.Type = 2 THEN 2 
						ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0))) BETWEEN MinValue AND MaxValue 
				AND Category = 'Car Conditions')
			END AS CategoryCondition,
			CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END)
			WHEN 0
				THEN 0
			ELSE ROUND(CAST(CAST((SUM(	CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
										ELSE Q.Weightage*0 END)) As float) /
					(SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
						WHEN Q.Type = 2 THEN 2 
						ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0)
			END AS CategoryPercentage
	FROM	AbSure_Questions Q WITH (NOLOCK)
			LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
			INNER JOIN	AbSure_QCategory C with (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
			INNER JOIN	AbSure_QSubCategory S with (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
	WHERE	Q.IsActive = 0
	GROUP BY C.AbSure_QCategoryId,C.Category,C.Sequence
	ORDER BY C.Sequence, S.SubCategory

	SELECT DISTINCT CP.AbSure_QCarPartsId,CP.PartName,dbo.[AbSure_GetPartInspectionDetails](API.AbSure_QCarPartResponsesId) AS Response,API.AbSure_QCarPartResponsesId
	FROM			AbSure_QCarParts CP WITH(NOLOCK)
	LEFT JOIN		AbSure_PartsInspectionData API WITH(NOLOCK) ON API.AbSure_QCarPartsId = CP.AbSure_QCarPartsId AND API.AbSure_CarDetailsId = @AbSure_CarDetailsId
END


--------------------------------------------------------------------------------------------------------------------





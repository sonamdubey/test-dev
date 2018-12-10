IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCategoryPercentage_v2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCategoryPercentage_v2]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 8th Feb 2016
-- Description : To get data that is to be shown on carwale\
-- Modified By : 1. Chetan Navin on 9th Mar, 2016(Changed cartrade certification expiry days from 90 to 45)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCategoryPercentage_v2]  
	-- Add the parameters for the stored procedure here
	@StockId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AbSure_CarDetailsId BIGINT
 
	SELECT TOP 1 @AbSure_CarDetailsId = ACD.Id
	FROM AbSure_CarDetails ACD WITH(NOLOCK)
    WHERE ACD.StockId = @StockId AND ACD.IsActive = 1 AND 
		  ([status] IN (4,8,2) OR DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30) 
		  AND [status] NOT IN (1,3,5,6) AND ACD.CarScore >= 60
	ORDER BY Id DESC

	IF(@AbSure_CarDetailsId IS NOT NULL)
		BEGIN
			DECLARE @MaxQuestionId INT
			
			SELECT	@MaxQuestionId = MAX(I.AbSure_QuestionsId)       
			FROM	AbSure_InspectionData I  WITH(NOLOCK)
			WHERE	I.AbSure_CarDetailsId = @AbSure_CarDetailsId
			
			IF(@MaxQuestionId <= 138)
				BEGIN
				SELECT	C.AbSure_QCategoryId CategoryId,C.Category Category,C.Sequence,
				CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WITH (NOLOCK) WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
				ELSE
					(SELECT Parameter 
					FROM AbSure_ConfigurableParameters WITH (NOLOCK)
					WHERE (convert(int, ROUND(CAST(CAST((SUM(	CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
												WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
												WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
												ELSE Q.Weightage*0 END)) As float) /
							(SUM(	CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
								WHEN Q.Type = 2 THEN 2 
								ELSE Q.Weightage END)) * 100 As decimal(8, 2)),0))) BETWEEN MinValue AND MaxValue 
						AND Category LIKE '%Car Conditions%')
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
				INNER JOIN	AbSure_QCategory C WITH (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
				INNER JOIN	AbSure_QSubCategory S WITH (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
				WHERE	Q.IsActive = 0
				GROUP BY C.AbSure_QCategoryId,C.Category,C.Sequence
				ORDER BY C.Sequence
			END
			ELSE
				BEGIN
					DECLARE @Kilometer INT,@Age INT,@DependentScore DECIMAL(8,2),@Maxrange BIGINT

					SELECT @Maxrange = POWER(CAST(2 AS VARCHAR),(32) -1)-1 FROM SYS.TYPES WHERE name = 'Int'

					SELECT	@Kilometer=CD.Kilometer,@Age = DATEDIFF(MONTH,CD.MakeYear,GETDATE())
					FROM	AbSure_CarDetails CD WITH (NOLOCK)
					WHERE	CD.ID=@AbSure_CarDetailsId
							AND CD.IsActive=1

					SELECT @DependentScore = ((SUM(CAST((CAST(WeightagePercent AS decimal)/100)*MaxWeightage AS decimal(8,2))))/(SUM(MaxWeightage)))
					FROM AbSure_CarScoreParameterValues WITH(NOLOCK)
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
								INNER JOIN	AbSure_QCategory C WITH (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
								INNER JOIN	AbSure_QSubCategory S WITH (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
						WHERE	Q.IsActive = 1 
						GROUP BY C.AbSure_QCategoryId, S.AbSure_QSubCategoryId,S.IsParameterDependent

						SELECT	C.AbSure_QCategoryId CategoryId,C.Category Category,C.Sequence,
								CASE SUM(SC.TotalWt) WHEN 0 THEN (SELECT Parameter FROM AbSure_ConfigurableParameters WITH (NOLOCK) WHERE 0 BETWEEN MinValue AND MaxValue AND Category LIKE '%Car Conditions%')
								ELSE
									(SELECT Parameter FROM AbSure_ConfigurableParameters WITH (NOLOCK) WHERE (convert(int,ROUND(CAST((SUM(SC.DependencyFactor)) /SUM(SC.TotalWt) AS decimal(8,2))*100,0))) BETWEEN MinValue AND MaxValue AND Category = 'Car Conditions')
								END AS CategoryCondition,
								CASE SUM(SC.TotalWt) WHEN 0 THEN 0 
								ELSE
									CAST((SUM(SC.DependencyFactor)) /SUM(SC.TotalWt) AS decimal(8,2))*100
								END AS CategoryPercentage
						FROM	AbSure_Questions Q WITH (NOLOCK)
								LEFT JOIN	AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarDetailsId
								INNER JOIN	AbSure_QCategory C WITH (NOLOCK) on	Q.AbSure_QCategoryId = C.AbSure_QCategoryId
								INNER JOIN	AbSure_QSubCategory S WITH (NOLOCK) on	Q.AbSure_QSubCategoryId = S.AbSure_QSubCategoryId
								INNER JOIN @TempSubCategory SC ON C.AbSure_QCategoryId = SC.Category
						WHERE	Q.IsActive = 1 
						GROUP BY C.AbSure_QCategoryId,C.Category,C.Sequence
						ORDER BY C.Sequence
				END
				
			SELECT	@AbSure_CarDetailsId AbSure_CarDetailsId,
			ACD.CarScore CarScore,
			(
				SELECT Parameter
				FROM AbSure_ConfigurableParameters WITH (NOLOCK)
				WHERE ACD.CarScore BETWEEN MinValue
						AND MaxValue
					AND Category LIKE '%Car Conditions%'
			) CarCondition,

			CASE 
			WHEN  ACD.Status = 2 OR (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30 AND ACD.Status NOT IN(4,8)) THEN 0
			ELSE ACD.FinalWarrantyTypeId END WarrantyId,
			
			CASE 
			WHEN  ACD.Status = 2 THEN 'Rejected' 
			WHEN  DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30 AND ACD.Status <> 8 THEN 'Expired'
			ELSE AWT.Warranty END WarrantyType,
				
			CASE  
			WHEN ACD.FinalWarrantyTypeId = 1 AND (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30 OR (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30 AND ACD.Status=8)) --AND (ACD.IsRejected <> 1 AND ACD.Status <> 2)
			THEN 1 ELSE 0 END HasWarranty,

			CONVERT(VARCHAR,DATEADD(DAY,30,ACD.SurveyDate),106) CertificationExpiryDate,

			CONVERT(VARCHAR,ACD.SurveyDate,106) InspectedDate,

			CASE 
			WHEN ACD.FinalWarrantyTypeId = 1 AND (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30 OR (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30 AND ACD.Status=8)) --AND (ACD.IsRejected <> 1 AND ACD.Status <> 2)
			THEN 'Checked on 217 points. Includes 1 year comprehensive CarWale Guarantee. '
			ELSE ''
			END WarrantyContent,

			'Checked on 217 points.' CertificationContent,

			CASE  
			WHEN ACD.FinalWarrantyTypeId = 1 AND (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30 OR (DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30 AND ACD.Status = 8)) --AND (ACD.IsRejected <> 1 AND ACD.Status <> 2)
			THEN (SELECT  HostURL+'0x0'+OriginalImgPath AS URL FROM Classified_CertifiedOrg WITH(NOLOCK) WHERE id = 417)
			ELSE (SELECT  HostURL+'0x0'+OriginalImgPath AS URL FROM Classified_CertifiedOrg WITH(NOLOCK) WHERE id = 454)
			END CertifiedLogoUrl,'http://www.autobiz.in/absure/carcertificate.aspx?carid='+ CONVERT(VARCHAR,@AbSure_CarDetailsId) AS CertificateUrl 

			FROM	AbSure_CarDetails ACD WITH(NOLOCK)
					LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON ACD.FinalWarrantyTypeId = AWT.AbSure_WarrantyTypesId
			WHERE	id = @AbSure_CarDetailsId
					AND ACD.IsActive = 1	
		END
	ELSE
		BEGIN
			SELECT @StockId AS CarId
			,CASE WHEN TC.InvCertifiedDate IS NOT NULL THEN 'Checked on 120 points.' ELSE '' END AS CertificationContent
			,CASE WHEN TC.IsWarranty = 1 AND TC.InvCertifiedDate IS NOT NULL THEN 'Checked on 120 points. Includes 1 year comprehensive CarWale Guarantee. ' ELSE '' END AS WarrantyContent
			,'http://www.autobiz.in/absure/carcertificatenew.aspx?stockid='+ CONVERT(VARCHAR,@StockId) AS CertificateUrl
			,CASE WHEN ISNULL(TC.IsWarranty,0) = 1 
			THEN (SELECT  HostURL+'0x0'+OriginalImgPath AS URL FROM Classified_CertifiedOrg WITH(NOLOCK) WHERE id = 417)
			ELSE ''
			END CertifiedLogoUrl
			FROM TC_CarTradeCertificationData TC WITH(NOLOCK) 
			INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON  TL.ListingId = TC.ListingId
			INNER JOIN TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TL.TC_CarTradeCertificationRequestId = TR.TC_CarTradeCertificationRequestId
			WHERE TC.ListingId = @StockId AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45
		END
END

---------------------------------------------------------------------------------------------------------------

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEmiDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEmiDetails_15]
GO

	
-- Modified By: Supriya Bhide on 12.02.2015
-- Modified : Added 9 parameters for Absure Certificate
-- Modified By: Supriya Bhide on 17 April 2015 : Handling Old & New Questions for car Conditions for Absure

CREATE PROCEDURE [dbo].[GetEmiDetails_15.2.1]
	@InquiryId		NUMERIC(18,0),
	@StockId NUMERIC = NULL OUTPUT, 
	@EMI INT = NULL OUTPUT,
	@LoanAmount INT = NULL OUTPUT,
	@InterestRate FLOAT = NULL OUTPUT,
	@Tenure SMALLINT = NULL OUTPUT,
	@OtherCharges INT = NULL OUTPUT,
	@DownPayment INT = NULL OUTPUT,
	@LoanToValue INT = NULL OUTPUT,
	@ShowEmi BIT = NULL OUTPUT, -- Modified by Shikhar on 19.91.2015
	@Price	DECIMAL(18, 0) = NULL	OUTPUT,
	-- Added by Kirtan Shetty on 27 January 2015
	@AbsureCarScore INT = NULL OUTPUT,					
	@AbsureId INT = NULL OUTPUT,						
	@AbsureCertificateUrl VARCHAR(100) = NULL OUTPUT,			
	@DealerCertificateId INT = NULL OUTPUT,					
	@FinalWarrantyType VARCHAR(100) = NULL OUTPUT,
	-- Added by Supriya Bhide on 12 Feb 2015 for fetching AbSure certification values
	@Abs_DriveTrain DECIMAL = NULL OUTPUT,
	@Abs_Suspension DECIMAL = NULL OUTPUT,
	@Abs_CarExterior DECIMAL = NULL OUTPUT,
	@Abs_Ac DECIMAL = NULL OUTPUT,
	@Abs_LatestFeatures DECIMAL = NULL OUTPUT,
	@Abs_CarInterior DECIMAL = NULL OUTPUT,
	@Abs_Engine DECIMAL = NULL OUTPUT,
	@Abs_Tyres DECIMAL = NULL OUTPUT,
	@Abs_Breaks DECIMAL = NULL OUTPUT
 AS
	BEGIN    
		
		SELECT TOP 1 @StockId=TC_StockId,@Price=Price FROM SellInquiries WITH (NOLOCK) WHERE ID = @InquiryId
		IF @StockId IS NOT NULL
		BEGIN
			SELECT   
				@EMI = TCST.EMI
				,@LoanAmount = TCST.LoanAmount
				,@InterestRate = TCST.InterestRate
				,@Tenure = TCST.Tenure
				,@OtherCharges = TCST.OtherCharges 
				,@LoanToValue = TCST.LoanToValue
				,@DownPayment = (@Price - TCST.LoanAmount)
				,@ShowEmi = TCST.ShowEMIOnCarwale
				-- Added by Kirtan Shetty on Jan 27
				,@AbsureCarScore = ABC.CarScore																
				,@AbsureId = ABC.Id																			
				,@AbsureCertificateUrl = 'http://www.autobiz.in/absure/CarCertificate.aspx?carId=' + CONVERT(VARCHAR, ABC.Id)
				,@DealerCertificateId = TCST.CertificationId												
				,@FinalWarrantyType = ABW.Warranty	
			FROM TC_Stock TCST WITH(NOLOCK)
				LEFT JOIN AbSure_CarDetails ABC WITH(NOLOCK)
					ON ABC.StockId = TCST.Id --AND GETDATE() BETWEEN ABC.SurveyDate AND DATEADD(dd, 30, ABC.SurveyDate) AND ABC.CarScore >=60 --Changed By Deepak on 24th APR 2015
				LEFT JOIN AbSure_WarrantyTypes ABW WITH (NOLOCK)
					ON ABC.FinalWarrantyTypeId = ABW.AbSure_WarrantyTypesId
			WHERE-- Removed the condition to show EMI value by Shikhar on Feb 2, 2015
				TCST.Id = @StockId
		END

			-- Added by Shikhar on Feb 11, 2015 Fetching the Car Conditions from the Absure Table
	SELECT C.AbSure_QCategoryId CategoryId
	,CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) 
				WHEN 3 THEN 0 
				ELSE Q.Weightage 
			END)
		WHEN 0 THEN 0
		ELSE ROUND(CAST
						(CAST
							((SUM( CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1
										ELSE Q.Weightage*0 END)) AS FLOAT) /
							(SUM( CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0
							WHEN Q.Type = 2 THEN 2
							ELSE Q.Weightage END)) * 100 AS DECIMAL(8, 2)
						)
					,0)
		END AS CategoryPercentage
	INTO #tempAbsurePartCategory
	FROM AbSure_Questions Q WITH (NOLOCK)
		LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) 
			ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
		INNER JOIN AbSure_QCategory C with (NOLOCK) 
			ON Q.AbSure_QCategoryId = C.AbSure_QCategoryId
		INNER JOIN AbSure_CarDetails ABCD WITH(NOLOCK)
			ON ID.AbSure_CarDetailsId = ABCD.Id AND ABCD.StockId = @StockId
		INNER JOIN LiveListings LL ON LL.InquiryId = @InquiryID		-- Modified by Supriya Bhide on 17/4/2015
	--WHERE Q.IsActive = 1	-- Modified By Supriya Bhide on 17 April 2015
		--WHERE LL.AbsureScore IS NOT NULL AND LL.AbsureScore >= 60	-- Modified by Supriya Bhide on 17/4/2015
		WHERE ABCD.CarScore >=60 AND ABCD.IsSurveyDone = 1 --Changed By Deepak on 24th APR 2015
	GROUP BY C.AbSure_QCategoryId

	SELECT @Abs_Ac = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 1
	SELECT @Abs_Breaks = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 2
	SELECT @Abs_CarExterior = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 3
	SELECT @Abs_CarInterior = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 4
	SELECT @Abs_LatestFeatures = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 5
	SELECT @Abs_Engine = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 6
	SELECT @Abs_Suspension = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 7
	SELECT @Abs_DriveTrain = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 8
	SELECT @Abs_Tyres = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 9
	-- Finally dropping the temporary table
	DROP TABLE #tempAbsurePartCategory
	-- End of Code added by Shikhar
	END




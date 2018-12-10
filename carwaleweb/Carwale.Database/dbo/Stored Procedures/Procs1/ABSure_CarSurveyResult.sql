IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ABSure_CarSurveyResult]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ABSure_CarSurveyResult]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 26 Dec 2014
-- Description:	To save Car survey result which includes car score and warranty type given to car if score criteria matched.
-- Modified by : Ruchira Patil on 30th Dec 2014 (to find the warranty type and update accordingly in AbSure_CarDetails)
-- Modified by : Ruchira Patil on 31th Dec 2014 (to calculate and update carscore in AbSure_CarDetails)
-- Modified By: Tejashree Patil on 2 Jan 2014, Checked criteria before calculate score, and updated survey done or not.
-- Modified By Tejashree Patil on 16 Feb 2015 , 1. If score is >= 60% Change the certification logo url with absure logo url at both live listing and sell inquiries table
--				 2. if score < 60% check if the stock id already have absure logo url, replace it with dealers certification url else no changes.
-- Modified By Tejashree Patil on 22 Feb 2015 , Eligibility checked for LPG and CNG.
-- Modified By Tejashree Patil on 3 March 2015, 1.Updated CarScore and Warranty in livelisting.
-- Modified By Tejashree Patil on 10 March 2015, Avoid changing date in Post update form.
-- Modified By Tejashree Patil on 12 March 2015, Allow inspection for manufactured CNG Cars.
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- Modified By: Tejashree Patil on 3 April 2015, Commented sp execution AbSure_ChangeCertification
-- Modified by : Ruchira Patil on 10 April 2015 , New Logic
-- Modified by : Ruchira Patil on 19 May 2015 , Fetch the rejected reasons and fired the SP :AbSure_SaveRejectedCarReasons to save the reasons
-- Modified By : Tejashree Patil on 9 July 2015, Rejected Car having silver warranty.
-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 months on RegistrationDate instead of makeYear.
-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
--Modified By : Nilima More on 27 th august , when page source is 1 update status =1 and save in status change log.
--Modified by : Ashwini Dhamankar on Oct 16,2015 (Added 1 more reason of auto rejection i.e @CarFuelType (LPG))
-- ========================================================================================
CREATE PROCEDURE [dbo].[ABSure_CarSurveyResult]
	@AbSure_CarId		INT ,
	@PageSrc			INT =  NULL, -- 1 : UPDATE PAGE
	@EligibleModelFor	TINYINT = 1,  -- 1: By default Warranty
	@ModifiedBy			INT = -1 -- 1 : By default
AS
BEGIN
		
	DECLARE @StockId VARCHAR(MAX) , @BranchId INT	
	DECLARE @Criteria TABLE (Count INT,CTQTypeId INT)
	DECLARE @InspData TABLE (AnswerCount INT, AbSure_CTQTypeId INT, Gold INT, Silver INT)
	DECLARE @CriteriaRowCnt INT , @Warranty INT,@PrevStatus INT , @CurrStatus INT 

	--table which contains the total number questions for the respective Weightage
	INSERT INTO @Criteria
	SELECT	COUNT(Q.AbSure_QuestionsId) Count,AbSure_CTQTypeId CTQTypeId
	FROM	AbSure_Questions Q WITH(NOLOCK)
	WHERE	Q.Weightage IN (10) AND Q.Type IN (1,3)
	GROUP BY AbSure_CTQTypeId
	ORDER BY Q.AbSure_CTQTypeId DESC
	
	SET @CriteriaRowCnt = @@ROWCOUNT

	--table which contains the total number of questions answered 
	INSERT INTO @InspData (AnswerCount, AbSure_CTQTypeId, Gold)
	SELECT	COUNT(ID.ID) CarQCount,Q.AbSure_CTQTypeId AbSure_CTQTypeId,
			CASE WHEN COUNT(ID.ID) = C.Count AND Q.AbSure_CTQTypeId = C.CTQTypeId  THEN 1 ELSE 0 END Gold
			--CASE WHEN COUNT(ID.ID) = C.Count AND Q.Weightage = C.Weightage THEN 0 ELSE 1 END Silver
	FROM	AbSure_InspectionData ID WITH(NOLOCK)
			INNER JOIN AbSure_Questions Q WITH(NOLOCK)	ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
			INNER JOIN @Criteria  C						ON C.CTQTypeId = Q.AbSure_CTQTypeId
	WHERE	ID.AbSure_CarDetailsId = @AbSure_CarId
			AND Q.Type IN (1,3)
			AND Q.Weightage IN (10)
			AND ID.AbSure_AnswerValue IN(1,3)
			AND Q.IsActive = 1
	GROUP BY Q.AbSure_CTQTypeId, C.Count,C.CTQTypeId
	ORDER BY Q.AbSure_CTQTypeId DESC,C.CTQTypeId DESC
	 
	--to check the warranty type
	if exists (select * from @InspData IDT where IDT.AbSure_CTQTypeId = 1 AND IDT.Gold <> 0)
	BEGIN
		SELECT @Warranty = CASE 
			WHEN SUM(IDT.Gold) < @CriteriaRowCnt
				THEN 2
			ELSE 1


			END
	FROM @InspData IDT

	END
	ELSE
		SET @Warranty = NULL


	--SELECT @Warranty = CASE 
	--		WHEN SUM(IDT.Gold) <> 0
	--			THEN CASE 
	--					WHEN SUM(IDT.Gold) = @CriteriaRowCnt
	--						THEN 1
	--					ELSE 2
	--					END
	--		ELSE NULL
	--		END
	--FROM @InspData IDT

	--Update Warranty(start)
	--IF @Warranty IS NULL
	--	UPDATE AbSure_CarDetails SET IsRejected= 1 ,AbSure_WarrantyTypesId = @Warranty WHERE Id = @AbSure_CarId
	--ELSE 
	--	UPDATE AbSure_CarDetails SET IsRejected= 0 ,AbSure_WarrantyTypesId = @Warranty WHERE Id = @AbSure_CarId
	--Update Warranty(end)

	--calculate CarScore
	------DECLARE @TotalWt INT , @AnsWt INT , @CarScore INT

	--------SELECT	@TotalWt = SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END),
	--------		@AnsWt	 = SUM(CASE ID.AbSure_AnswerValue WHEN 1 THEN ID.AbSure_AnswerValue*Q.Weightage ELSE Q.Weightage*0 END)
	--------FROM	AbSure_Questions Q WITH (NOLOCK)
	--------		LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarId
	--------WHERE	Q.IsActive = 1
	------SELECT	@TotalWt = SUM(CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
	------		WHEN Q.Type = 2 THEN 2 
	------		ELSE Q.Weightage END),
	------		@AnsWt	 = SUM(CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
	------		WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
	------		WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
	------			ELSE Q.Weightage*0 END)
	------FROM	AbSure_Questions Q WITH (NOLOCK)
	------		LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarId
	------WHERE	Q.IsActive = 1

	--------Update carscore
	------SET @CarScore = ROUND(CAST(CAST(@AnsWt As float) / @TotalWt * 100 As decimal(8, 2)),0)

	/*Car Score According to the new logic*/

		DECLARE @TotalScore INT,
				@AchievedScore decimal(8,2),
				@CarScore INT

		/* PARAMETER SCORE*/

		DECLARE @Kilometer INT,
				@TotalParameterScore INT,
				@ParameterScore DECIMAL(8,2),
				@Age INT,
				@Owners INT,
				@Insurance VARCHAR(50),
				@Maxrange BIGINT

		SELECT    @Maxrange = POWER(CAST(2 AS VARCHAR),(32) -1)-1 FROM SYS.TYPES WHERE name = 'Int'
		--SET @Maxrange =  @Maxrange-1

		SELECT    @Kilometer=Kilometer,
				@Age =DATEDIFF(MONTH,MakeYear,GETDATE()),
				@Owners= CASE WHEN Owners > 3 THEN -1 ELSE Owners END,
				@Insurance=CASE WHEN Insurance = 'N/A' OR Insurance IS NULL THEN 'N/A' ELSE Insurance END
		FROM    AbSure_CarDetails WITH(NOLOCK)
		WHERE    ID=@AbSure_CarId

	   -- SELECT    @Kilometer 'Kilometer',@Age 'Age',@Owners 'Owners',@Insurance 'Insurance'

		--SELECT * FROM AbSure_CarScoreParameterValues

		SELECT    @TotalParameterScore = SUM(MaxWeightage),
				@ParameterScore = SUM(CAST((CAST(WeightagePercent AS decimal)/100)*MaxWeightage AS decimal(8,3)))
		FROM    AbSure_CarScoreParameterValues WITH(NOLOCK)
		 WHERE	(ParameterId=1 AND @Age >= MinValue+1 AND @Age <= ISNULL(MaxValue,@Maxrange))
				OR (ParameterId=2 AND @Kilometer >= MinValue+1 AND @Kilometer <= ISNULL(MaxValue,@Maxrange))
				OR (ParameterId=3 AND ConstantValue = CAST(@Owners AS VARCHAR(50)))
				OR (ParameterId=4 AND (ConstantValue = @Insurance))
       
		--SELECT  @TotalParameterScore 'TotalParameterScore', @ParameterScore 'ParameterScore'
	/* PARAMETER SCORE*/

	/*EXTERIOR SCORE*/
		DECLARE @TotalExteriorScore INT,
				@ExteriorScore decimal (8,2),
				@ExteriorFactor SMALLINT = 6

		DECLARE @TempPartsData TABLE 
		(
			AbSure_QCarPartsId INT,
			AbSure_QCarPartResponsesId VARCHAR(100)
		)

		INSERT INTO @TempPartsData (AbSure_QCarPartsId,AbSure_QCarPartResponsesId)
		SELECT		AQ.AbSure_QCarPartsId,API.AbSure_QCarPartResponsesId 
		FROM		AbSure_QCarParts AQ WITH(NOLOCK)
		LEFT JOIN	AbSure_PartsInspectionData API WITH(NOLOCK) ON API.AbSure_QCarPartsId = AQ.AbSure_QCarPartsId AND API.AbSure_CarDetailsId = @AbSure_CarId

		SELECT  @TotalExteriorScore = COUNT(AbSure_QCarPartsId) * @ExteriorFactor
		FROM    AbSure_QCarParts  WITH(NOLOCK)
	
		SELECT	@ExteriorScore = SUM(dbo.[Absure_GetCarPartsScore](AbSure_QCarPartResponsesId, @ExteriorFactor))
		FROM	@TempPartsData 
		--SELECT @ExteriorScore '@ExteriorScore' ,@TotalExteriorScore '@TotalExteriorScore'
	/*EXTERIOR SCORE*/

	/*QUESTIONS SCORE */
		DECLARE @TotalWt INT , @AnsWt DECIMAL(8,2)

		SELECT  @TotalWt    = SUM (   
									CASE    WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3    THEN 0
											ELSE Q.Weightage
									END
								  ),
				@AnsWt        = SUM (   
									CASE    WHEN ID.AbSure_AnswerValue = 1                    THEN Q.Weightage
											WHEN ID.AbSure_AnswerValue = 2 AND Q.Type = 2     THEN 0.5*Q.Weightage
											ELSE 0
									END
								  )
		FROM    AbSure_Questions Q WITH (NOLOCK)
				LEFT JOIN AbSure_InspectionData ID    WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
																	 AND ID.AbSure_CarDetailsId = @AbSure_CarId
		WHERE   Q.IsActive = 1

		--SELECT   @TotalWt 'TotalWt',@AnsWt 'AnsWt'
	/*QUESTIONS SCORE */

	/* TOTAL SCORE */

		SET @TotalScore = (ISNULL(@TotalParameterScore,0) + ISNULL(@TotalExteriorScore,0) + ISNULL(@TotalWt,0))
		SET @AchievedScore = (ISNULL(@ParameterScore,0) + ISNULL(@ExteriorScore,0) + ISNULL(@AnsWt,0))
		SET @CarScore = ROUND(CAST(CAST(@AchievedScore As float) / @TotalScore * 100 As decimal(8, 2)),0)

		--SELECT @TotalScore 'TotalScore',@AchievedScore 'AchievedScore',@CarScore 'CarScore'

	/* TOTAL SCORE */
	/*Car Score According to the new logic*/

	UPDATE	AbSure_CarDetails 
	SET		CarScore= @CarScore,
			ModifiedDate = GETDATE()
	WHERE	Id = @AbSure_CarId 
			AND IsActive=1

			 
	IF(@PageSrc = 1)
	BEGIN
		SELECT @PrevStatus= Status FROM AbSure_CarDetails WITH(NOLOCK) WHERE Id = @AbSure_CarId	
		UPDATE	AbSure_CarDetails
		SET		IsRejected = NULL, 
				RejectedDateTime = NULL,
				IsSurveyDone = 1
		WHERE   Id = @AbSure_CarId
				AND IsActive=1		
				
	END
	ELSE
	BEGIN
		UPDATE	AbSure_CarDetails
		SET		IsSurveyDone = 1,
				SurveyDate = GETDATE()
		WHERE   Id = @AbSure_CarId
				AND IsActive=1
	END
	-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
	IF EXISTS(	SELECT	TOP 1 CD.Id
				FROM	AbSure_CarDetails CD WITH(NOLOCK)
						INNER JOIN CarVersions V WITH(NOLOCK) ON  V.ID = CD.VersionId  
						INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId
				WHERE   CD.Id=@AbSure_CarId 
						AND (CD.Kilometer <= 85000 AND 
							--DATEDIFF(year,CD.MakeYear,GETDATE()) <= 7 AND
							(DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) > 24 AND DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) <= 72) AND-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 months on RegistrationDate instead of makeYear.
							/*(ISNULL(CD.IsSurveyDone,0) <> 1) AND
							(ISNULL(CD.IsRejected,0) <> 1) AND*/
							V.CarFuelType IN (1,2,3) AND-- Modified By Tejashree Patil on 12 March 2015
							ISNULL(CD.CarFittedWith,0) IN (0,4) AND --lpg and cng
							CD.Owners <> 0)
						AND AE.IsActive = 1
						AND (
							 ( @EligibleModelFor = 1 AND ISNULL(AE.IsEligibleWarranty,1) = 1) OR 
							 ( @EligibleModelFor = 2 AND AE.IsEligibleCertification = 1) OR 
							 ( @EligibleModelFor = 3 AND (AE.IsEligibleCertification = 1 AND AE.IsEligibleWarranty = 1))
							)
						AND CD.IsActive=1
					
		)
	BEGIN	
		IF (@Warranty IS NULL OR @Warranty = 2)--Tejashree Patil on 9 July 2015, Rejected Car having silver warranty.
		BEGIN
			UPDATE AbSure_CarDetails SET IsRejected= 1 , AbSure_WarrantyTypesId = NULL WHERE Id = @AbSure_CarId AND IsActive=1
			SET @CurrStatus = 2
		END
		ELSE
		BEGIN 
			UPDATE AbSure_CarDetails SET IsRejected= 0 , AbSure_WarrantyTypesId = @Warranty WHERE Id = @AbSure_CarId AND IsActive=1
			SET @CurrStatus = 1
		END

		IF @PageSrc = 1
		BEGIN			
			IF (@PrevStatus <> @CurrStatus)
			BEGIN			
				INSERT INTO AbSure_StatusChangeLog(AbSure_CarDetailsId,Status, PreviousStatus, ModifiedBy, ModifiedDate, IsModified) 
				VALUES(@AbSure_CarId, 1, @PrevStatus, @ModifiedBy, GETDATE(), 1)				
			
				UPDATE AbSure_CarDetails
				SET    Status=@CurrStatus--, RejectedDateTime = NULL
				WHERE Id = @AbSure_CarId

				--UPDATE Absure_RejectedCarReasons 
				--SET RejectedReason = NULL
				--WHERE Absure_CarDetailsId = @AbSure_CarId
			END
		END

		IF (@Warranty IS NULL AND (@PageSrc = 0 OR @PageSrc is null))
			UPDATE	AbSure_CarDetails 
			SET		RejectedDateTime = GETDATE()
			WHERE	Id = @AbSure_CarId
					AND IsActive=1
	/*************************** Modified By Tejashree Patil on 16 Feb 2015 to update Absure certification based on criteria *****************************/
		
		EXECUTE AbSure_ChangeCertification @StockId,  @CarScore, @AbSure_CarId

	/********************************************************/

	END
	ELSE
	BEGIN	
		IF(@PageSrc = 1)
		BEGIN
			SET @CurrStatus = 2
			IF (@PrevStatus <> @CurrStatus)
			BEGIN			
				INSERT INTO AbSure_StatusChangeLog(AbSure_CarDetailsId,Status, PreviousStatus, ModifiedBy, ModifiedDate, IsModified) 
				VALUES(@AbSure_CarId, 1, @PrevStatus, @ModifiedBy, GETDATE(), 1)
			
				UPDATE AbSure_CarDetails
				SET    Status=@CurrStatus--, RejectedDateTime=NULL
				WHERE Id = @AbSure_CarId

				--UPDATE Absure_RejectedCarReasons
				--SET RejectedReason = NULL
				--WHERE Id = @AbSure_CarId

			END
				
			UPDATE	AbSure_CarDetails
			SET		IsRejected = 1, RejectedDateTime = GETDATE()
			WHERE   Id = @AbSure_CarId
					AND IsActive=1
		END		
		ELSE
		BEGIN
			UPDATE	AbSure_CarDetails
			SET		IsRejected = 1,
					RejectedDateTime = GETDATE()
			WHERE   Id = @AbSure_CarId
					AND IsActive=1
		END
	END

	IF (SELECT IsRejected FROM AbSure_CarDetails WITH(NOLOCK) WHERE ID = @AbSure_CarId AND IsActive=1) = 1
	BEGIN
		DECLARE @CarFittedWith INT,
				@KM INT,
				@MakeYearGreater INT,
				@CarModelId INT,
				@MakeYearLess INT,
				@CarFuelType INT

		DECLARE @TblRejectedReasons AbSure_RejectedReasonsTblTyp
		-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
		SELECT		@CarFittedWith = CASE WHEN ISNULL(CarFittedWith,0)= 0 OR ISNULL(CarFittedWith,0) = 4 THEN 0 ELSE 1 END ,
					@KM = CASE WHEN Kilometer <= 85000 THEN 0 ELSE 2 END ,
					@MakeYearGreater = CASE WHEN DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) <= 72 THEN 0 ELSE 3 END ,-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 months on RegistrationDate instead of makeYear.
					@CarModelId = CASE WHEN V.CarModelId =( SELECT ModelId FROM AbSure_EligibleModels WITH(NOLOCK) WHERE ModelId = V.CarModelId AND IsEligibleWarranty = 1) THEN 0 ELSE 4 END ,
					@MakeYearLess = CASE WHEN DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) > 24 THEN 0 ELSE 5 END,-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 months on RegistrationDate instead of makeYear.
					@CarFuelType = CASE WHEN V.CarFuelType IN (1,2,3) THEN 0 ELSE 6 END 
		FROM		AbSure_CarDetails  CD WITH(NOLOCK)
		INNER JOIN	CarVersions V WITH(NOLOCK) ON  V.ID = CD.VersionId  
		WHERE		CD.Id=@AbSure_CarId
					AND IsActive=1

		--Type 1 is for Basic Parameters
		INSERT INTO @TblRejectedReasons(Type,Reasons)
		SELECT		1,Id from AbSure_RejectionReasons  WITH(NOLOCK)
		WHERE		Id IN (@CarFittedWith,@KM,@MakeYearGreater,@CarModelId,@MakeYearLess,@CarFuelType)

		--Type 2 is for CTQ
		INSERT INTO @TblRejectedReasons(Type,Reasons)
		SELECT		DISTINCT 2,Q.AbSure_QSubCategoryId
		FROM		AbSure_InspectionData ID WITH(NOLOCK)
					INNER JOIN AbSure_Questions Q WITH(NOLOCK)	ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
		WHERE		ID.AbSure_CarDetailsId = @AbSure_CarId
					AND Q.AbSure_CTQTypeId IN (1,2)
					AND ID.AbSure_AnswerValue IN(2)
					AND Q.IsActive = 1

	EXEC AbSure_SaveRejectedCarReasons @AbSure_CarId , 1, @RejectedReasons = @TblRejectedReasons
	END
END

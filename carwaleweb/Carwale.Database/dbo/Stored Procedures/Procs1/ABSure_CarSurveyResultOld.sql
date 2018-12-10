IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ABSure_CarSurveyResultOld]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ABSure_CarSurveyResultOld]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 26 Dec 2014
-- Description:	To save Car survey result which includes car score and warranty type given to car if score criteria matched.
-- Modified by : Ruchira Patil on 30th Dec 2014 (to find the warranty type and update accordingly in AbSure_CarDetails)
-- Modified by : Ruchira Patil on 31th Dec 2014 (to calculate and update carscore in AbSure_CarDetails)
-- Modified By: Tejashree Patil on 2 Jan 2014, Checked criteria before calculate score, and updated survey done or not.
-- Modified By Tejashree Patil on 16 Feb 2015 , 1. If score is >= 60% Change the certification logo url with absure logo url at both live listing and sell inquiries table
--				2. if score < 60% check if the stock id already have absure logo url, replace it with dealers certification url else no changes.
-- Modified By Tejashree Patil on 22 Feb 2015 , Eligibility checked for LPG and CNG.
-- Modified By Tejashree Patil on 3 March 2015, 1.Updated CarScore and Warranty in livelisting.
-- Modified By Tejashree Patil on 10 March 2015, Avoid changing date in Post update form.
-- Modified By Tejashree Patil on 12 March 2015, Allow inspection for manufactured CNG Cars.
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- Modified By: Tejashree Patil on 3 April 2015, Commented sp execution AbSure_ChangeCertification
-- Modified By: Tejashree Patil on 7 April 2015, IsActive = 0 for old questions.
-- Modified By : Tejashree Patil on 9 July 2015, Rejected Car having silver warranty.
-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 monthson RegistrationDate instead of makeYear .
-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
--Modified by :  Nilima More on 28 th august,added condition for forcefully updation of status 2.
-- =============================================
CREATE PROCEDURE [dbo].[ABSure_CarSurveyResultOld]
	@AbSure_CarId		INT ,
	@PageSrc			INT =  NULL, -- 1 : UPDATE PAGE
	@EligibleModelFor	TINYINT = 1,  -- 1: By default Warranty
	@ModifiedBy			INT	 = -1 --1 : By default
	 
AS
BEGIN
		
	DECLARE @StockId VARCHAR(MAX) , @BranchId INT	
	DECLARE @Criteria TABLE (Count INT, Weightage INT)
	DECLARE @InspData TABLE (AnswerCount INT, CarWeightage INT, Gold INT, Silver INT)
	DECLARE @CriteriaRowCnt INT , @Warranty INT , @PrevStatus INT , @CurrStatus INT

	--table which contains the total number questions for the respective Weightage
	INSERT INTO @Criteria
	SELECT	COUNT(Q.AbSure_QuestionsId) Count,Q.Weightage Weightage
	FROM	AbSure_Questions Q WITH(NOLOCK)
	WHERE	Q.Weightage IN (3,4) AND Q.Type IN (1,3) AND Q.IsActive=0
	GROUP BY Q.Weightage
	ORDER BY Q.Weightage DESC

	SET @CriteriaRowCnt = @@ROWCOUNT

	--table which contains the total number of questions answered 
	INSERT INTO @InspData (AnswerCount, CarWeightage, Gold)
	SELECT	COUNT(ID.ID) CarQCount,Q.Weightage CarWeightage,
			CASE WHEN COUNT(ID.ID) = C.Count AND Q.Weightage = C.Weightage  THEN 1 ELSE 0 END Gold
			--CASE WHEN COUNT(ID.ID) = C.Count AND Q.Weightage = C.Weightage THEN 0 ELSE 1 END Silver
	FROM	AbSure_InspectionData ID WITH(NOLOCK)
			INNER JOIN AbSure_Questions Q WITH(NOLOCK)	ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
			INNER JOIN @Criteria  C						ON C.Weightage = Q.Weightage
	WHERE	ID.AbSure_CarDetailsId = @AbSure_CarId
			AND Q.Type IN (1,3)
			AND Q.Weightage IN (3,4)
			AND ID.AbSure_AnswerValue IN(1,3)
			AND Q.IsActive = 0
	GROUP BY Q.Weightage, C.Count,C.Weightage
	ORDER BY Q.Weightage DESC,C.Weightage DESC
	 
	--to check the warranty type
	if exists (select * from @InspData IDT where IDT.CarWeightage = 4 AND IDT.Gold <> 0)
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
	DECLARE @TotalWt INT , @AnsWt INT , @CarScore INT

	--SELECT	@TotalWt = SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) WHEN 3 THEN 0 ELSE Q.Weightage END),
	--		@AnsWt	= SUM(CASE ID.AbSure_AnswerValue WHEN 1 THEN ID.AbSure_AnswerValue*Q.Weightage ELSE Q.Weightage*0 END)
	--FROM	AbSure_Questions Q WITH (NOLOCK)
	--		LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarId
	--WHERE	Q.IsActive = 1
	SELECT	@TotalWt = SUM(CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0 
			WHEN Q.Type = 2 THEN 2 
			ELSE Q.Weightage END),
			@AnsWt	= SUM(CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage 
			WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
			WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1 
				ELSE Q.Weightage*0 END)
	FROM	AbSure_Questions Q WITH (NOLOCK)
			LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId AND ID.AbSure_CarDetailsId = @AbSure_CarId
	WHERE	Q.IsActive = 0

	--Update carscore
	SET @CarScore = ROUND(CAST(CAST(@AnsWt As float) / @TotalWt * 100 As decimal(8, 2)),0)
	UPDATE	AbSure_CarDetails 
	SET		CarScore= @CarScore,
			ModifiedDate = GETDATE()
	WHERE	Id = @AbSure_CarId 


	IF(@PageSrc = 1)
	BEGIN
		SELECT @PrevStatus= Status FROM AbSure_CarDetails WITH(NOLOCK) WHERE Id = @AbSure_CarId	
		UPDATE	AbSure_CarDetails
		SET		IsRejected = NULL,
				RejectedDateTime = NULL,
				IsSurveyDone = 1
		WHERE   Id = @AbSure_CarId
	END
	ELSE
	BEGIN
		UPDATE	AbSure_CarDetails
		SET		IsSurveyDone = 1,
				SurveyDate = GETDATE()
		WHERE   Id = @AbSure_CarId AND IsActive = 1
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
							ISNULL(CD.CarFittedWith,0) IN (0,4) AND--ISNULL(CD.CarFittedWith,0) = 0 AND --lpg and cng
							CD.Owners <> 0)
						AND AE.IsActive = 1
						AND (
							( @EligibleModelFor = 1 AND ISNULL(AE.IsEligibleWarranty,1) = 1) OR 
							( @EligibleModelFor = 2 AND AE.IsEligibleCertification = 1) OR 
							( @EligibleModelFor = 3 AND (AE.IsEligibleCertification = 1 AND AE.IsEligibleWarranty = 1))
							)
		)
	BEGIN			
		IF (@Warranty IS NULL OR @Warranty = 2)--Tejashree Patil on 9 July 2015, Rejected Car having silver warranty.
		BEGIN
			UPDATE AbSure_CarDetails SET IsRejected= 1 , AbSure_WarrantyTypesId = NULL WHERE Id = @AbSure_CarId AND  IsActive=1
			SET @CurrStatus = 2
		END
		ELSE 
		BEGIN
			UPDATE AbSure_CarDetails SET IsRejected= 0 ,AbSure_WarrantyTypesId = @Warranty WHERE Id = @AbSure_CarId AND  IsActive=1
			SET @CurrStatus = 1
		END
		IF @PageSrc = 1
		BEGIN
			IF (@PrevStatus <> @CurrStatus)
			BEGIN
				INSERT INTO AbSure_StatusChangeLog(AbSure_CarDetailsId,Status, PreviousStatus, ModifiedBy, ModifiedDate, IsModified) 
				VALUES(@AbSure_CarId, 1, @PrevStatus, @ModifiedBy, GETDATE(), 1)
			
				UPDATE AbSure_CarDetails
				SET    Status=@CurrStatus
				WHERE Id = @AbSure_CarId
			END
		END

		IF (@Warranty IS NULL AND (@PageSrc = 0 OR @PageSrc is null))
			UPDATE	AbSure_CarDetails 
			SET		RejectedDateTime = GETDATE()
			WHERE	Id = @AbSure_CarId
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
				SET    Status=@CurrStatus
				WHERE  Id = @AbSure_CarId
			END
		
			UPDATE	AbSure_CarDetails
			SET		IsRejected = 1, RejectedDateTime = GETDATE()
			WHERE   Id = @AbSure_CarId
		END
		ELSE
		BEGIN
			UPDATE	AbSure_CarDetails
			SET		IsRejected = 1,
					RejectedDateTime = GETDATE()
			WHERE   Id = @AbSure_CarId
		END
	END

	IF (SELECT IsRejected FROM AbSure_CarDetails WITH(NOLOCK) WHERE ID = @AbSure_CarId) = 1
	BEGIN
		DECLARE @CarFittedWith INT,
				@KM INT,
				@MakeYearGreater INT,
				@CarModelId INT,
				@MakeYearLess INT

		DECLARE @TblRejectedReasons AbSure_RejectedReasonsTblTyp
		-- Modified By : Tejashree Patil on 14 Aug 2015, Changed eligibility criteria will be 6years & 85,000kms.
		SELECT		@CarFittedWith = CASE WHEN ISNULL(CarFittedWith,0)= 0 OR ISNULL(CarFittedWith,0) = 4 THEN 0 ELSE 1 END ,
					@KM = CASE WHEN Kilometer <= 85000 THEN 0 ELSE 2 END ,
					@MakeYearGreater = CASE WHEN DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) <= 72 THEN 0 ELSE 3 END ,-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria  of 24 months on RegistrationDate instead of makeYear.
					@CarModelId = CASE WHEN V.CarModelId =( SELECT ModelId FROM AbSure_EligibleModels WITH(NOLOCK) WHERE ModelId = V.CarModelId AND IsEligibleWarranty = 1) THEN 0 ELSE 4 END ,
					@MakeYearLess = CASE WHEN DATEDIFF(MONTH,CD.RegistrationDate,GETDATE()) > 24 THEN 0 ELSE 5 END-- Modified By : Tejashree Patil on 17 July 2015, Eligibility crieteria of 24 months on RegistrationDate instead of makeYear.
		FROM		AbSure_CarDetails  CD WITH(NOLOCK)
		INNER JOIN	CarVersions V WITH(NOLOCK) ON  V.ID = CD.VersionId  
		WHERE		CD.Id=@AbSure_CarId

		--Type 1 is for Basic Parameters
		INSERT INTO @TblRejectedReasons(Type,Reasons)
		SELECT		1,Id from AbSure_RejectionReasons WITH(NOLOCK) 
		WHERE		Id IN (@CarFittedWith,@KM,@MakeYearGreater,@CarModelId,@MakeYearLess)

		--Type 2 is for CTQ
		INSERT INTO @TblRejectedReasons(Type,Reasons)
		SELECT		DISTINCT 2,Q.AbSure_QSubCategoryId
		FROM		AbSure_InspectionData ID WITH(NOLOCK)
					INNER JOIN AbSure_Questions Q WITH(NOLOCK)	ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
		WHERE		ID.AbSure_CarDetailsId = @AbSure_CarId
					AND Q.AbSure_CTQTypeId=1 
					AND ID.AbSure_AnswerValue IN(2)
					AND Q.IsActive = 1

	EXEC AbSure_SaveRejectedCarReasons @AbSure_CarId , 1, @RejectedReasons = @TblRejectedReasons
	END
END
-------------------------------------------------------------------------------------------------------------------------------------------------------





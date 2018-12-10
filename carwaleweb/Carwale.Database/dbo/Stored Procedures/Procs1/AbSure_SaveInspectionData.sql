IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveInspectionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveInspectionData]
GO

	
-- ===================================================================
-- Author:		Vaibhav K
-- Create date: 20 Dec 2014
-- Description:	Save entire inspection data aginst the car i.e. AbSure_CarDetailsId
-- Modified By : Tejashree Patil on 26 Dec 2014, Updated IsSurveyDone.
-- Modified by : Ruchira Patil on 30th Dec 2014, called ABSure_CarSurveyResult SP
-- Modified By: Ashwini Dhamankar on April 10,2015 , to get warranty name
-- Modified by : Ruchira Patil on 10 April 2015 , New Logic
-- Modified By : Yuga Hatolkar on 27th July, Added Parameters PhoneManufacturer, PhoneModel, Phone ApiLevel, AppVersion, PhoneImei, UserId.
-- Modified By : Kartik Rathod on 30th july 2015, insert new column Timestamp(Datetime)
--Modified by : Nilima moren on 27 th august fetch data for ABSure_CarSurveyResult
-- =====================================================================

CREATE PROCEDURE [dbo].[AbSure_SaveInspectionData]
	-- Add the parameters for the stored procedure here
	
	@AbSure_CarDetailsId		NUMERIC(18, 0),
	@InspectionTbl				[dbo].[AbSure_InspectionDataTblTyp] READONLY,
	@PartsInspectionDataTbl		[dbo].[AbSure_PartsInspectionDataTblTyp] READONLY,
	@PageSrc					INT = NULL, -- 1 : UPDATE PAGE
	@UserId						INT = NULL,
	@PhoneManufacturer			VARCHAR(100) = NULL,
	@PhoneModel					VARCHAR(100) = NULL,
	@PhoneApiLevel				VARCHAR(100) = NULL,
	@AppVersion					VARCHAR(100) = NULL,
	@PhoneImei					VARCHAR(100) = NULL,
	@ModifiedBy					INT			 = NULL
	
AS
BEGIN	
    DECLARE @EntryDate DATETIME

	--get previous data for the same car and log it
	INSERT INTO AbSure_InspectionDataLog (AbSure_CarDetailsId, AbSure_QuestionsId, AbSure_AnswerValue,AnswerComments,AnswerDate,Timestamp)
	SELECT	AbSure_CarDetailsId, AbSure_QuestionsId, AbSure_AnswerValue, AnswerComments,AnswerDate,Timestamp
	FROM	AbSure_InspectionData WITH(NOLOCK)
	WHERE	AbSure_CarDetailsId = @AbSure_CarDetailsId

	--delete all previous records for the car
	DELETE 
	FROM	AbSure_InspectionData 
	WHERE	AbSure_CarDetailsId = @AbSure_CarDetailsId

	--insert fresh record of survey for the car
	INSERT INTO AbSure_InspectionData (AbSure_CarDetailsId, AbSure_QuestionsId, AbSure_AnswerValue,AnswerComments, Timestamp)
	SELECT	@AbSure_CarDetailsId, QuestionId, AnswerValue, Comments, Timestamp 
	FROM	@InspectionTbl

	INSERT INTO AbSure_PartsInspectionDataLog (AbSure_CarDetailsId, AbSure_QCarPartsId, AbSure_QCarPartResponsesId,ResponseComments,ResponseDate,Timestamp)
	SELECT	@AbSure_CarDetailsId, AbSure_QCarPartsId, AbSure_QCarPartResponsesId, ResponseComments,ResponseDate,Timestamp
	FROM	AbSure_PartsInspectionData  WITH(NOLOCK)
	WHERE	AbSure_CarDetailsId = @AbSure_CarDetailsId

	DELETE 
	FROM	AbSure_PartsInspectionData 
	WHERE	AbSure_CarDetailsId = @AbSure_CarDetailsId

	INSERT INTO AbSure_PartsInspectionData (AbSure_CarDetailsId, AbSure_QCarPartsId, AbSure_QCarPartResponsesId,ResponseComments,Timestamp)
	SELECT	@AbSure_CarDetailsId, QCarPartsId, QCarPartResponsesId, ResponseComments, Timestamp
	FROM	@PartsInspectionDataTbl
	
	SELECT	@EntryDate = EntryDate       
	FROM	AbSure_CarDetails  WITH(NOLOCK)
	WHERE	Id = @AbSure_CarDetailsId

	DECLARE @MaxQuestionId TINYINT
	SELECT	@MaxQuestionId = MAX(I.AbSure_QuestionsId)
	FROM	AbSure_InspectionData I WITH(NOLOCK)
	WHERE	I.AbSure_CarDetailsId = @AbSure_CarDetailsId

	IF(@MaxQuestionId > 138)
	BEGIN
		EXEC ABSure_CarSurveyResult @AbSure_CarDetailsId , @PageSrc,1, @ModifiedBy
	END
	ELSE
	BEGIN
		EXEC ABSure_CarSurveyResultOld @AbSure_CarDetailsId , @PageSrc,1, @ModifiedBy
	END

	--EXEC ABSure_CarSurveyResult @AbSure_CarDetailsId , @PageSrc

	UPDATE	AbSure_CarDetails
	SET		IsSurveyDone = 1,
			ModifiedDate = GETDATE()
	WHERE   Id = @AbSure_CarDetailsId

	--Modified By: Ashwini Dhamankar on April 10,2015 , To get warranty name
    SELECT	CASE WHEN ISNULL(ACD.IsRejected,0) = 1 THEN 'Rejected'
    ELSE	AWT.Warranty END AS Warranty
    FROM	AbSure_CarDetails ACD WITH(NOLOCK)
			LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON ACD.AbSure_WarrantyTypesId = AWT.AbSure_WarrantyTypesId
	WHERE   ACD.Id = @AbSure_CarDetailsId

	-- Added By Yuga Hatolkar on 27th July, 2015
	IF(@PhoneModel IS NOT NULL)
	BEGIN
		INSERT INTO AbSure_SaveSurveyorMobileDetails (AbSure_CarId, UserId, PhoneDetails, PhoneManufacturer, PhoneApiLevel, AppVersion, EntryDate, PhoneImei)
		VALUES (@AbSure_CarDetailsId, @UserId, @PhoneModel, @PhoneManufacturer, @PhoneApiLevel, @AppVersion, GETDATE(), @PhoneImei)
	End

END
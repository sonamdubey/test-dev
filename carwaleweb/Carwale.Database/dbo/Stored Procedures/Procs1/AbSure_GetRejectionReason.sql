IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetRejectionReason]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetRejectionReason]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: May 19,2015
-- Description:	To get rejection reason of a car
-- Modified By : Suresh Prajapati on 06th July, 2015
-- Description : To Get RejectionComments From Absure_CarDetails Table
-- Modified By : Ashwini Dhamankar on Oct 28,2015 (Called Function Absure_GetMasterCarId only if car is duplicate)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetRejectionReason] @AbSure_CarDetailsId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @RegNumber VARCHAR(50),@Status INT,@CancelReason VARCHAR(250),@CancelledReason VARCHAR(250)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

	SELECT	@RegNumber = ACD.RegNumber,
			@Status = ACD.Status,@CancelReason = ACD.CancelReason
	FROM	AbSure_CarDetails ACD WITH(NOLOCK)
	WHERE	ACD.ID = @AbSure_CarDetailsId

	IF(@Status = 3 AND @CancelReason = @CancelledReason)
	BEGIN
		SELECT @AbSure_CarDetailsId = dbo.Absure_GetMasterCarId(@RegNumber,@AbSure_CarDetailsId) 
	END  

	SELECT CASE ACD.RejectionMethod
			WHEN 1
				THEN 'Auto'
			ELSE 'Manual'
			END RejectionMethod
		,CONVERT(VARCHAR, ISNULL(ACD.RejectedDateTime, ACD.SurveyDate), 106) RejectionDate
		,ARCR.RejectedType RejectionType
		,CASE ARCR.RejectedType
			WHEN 1
				THEN ARR.Reason
			ELSE AQC.Category + '-' + AQS.SubCategory
			END Reason
		,ISNULL(ACD.RejectionComments, 'Not Available') AS RejectionComments
	FROM Absure_RejectedCarReasons ARCR WITH (NOLOCK)
	LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = ARCR.Absure_CarDetailsId
	LEFT JOIN AbSure_RejectionReasons ARR WITH (NOLOCK) ON ARR.Id = ARCR.RejectedReason
		AND ARCR.RejectedType = 1
	LEFT JOIN AbSure_QSubCategory AQS WITH (NOLOCK) ON AQS.AbSure_QSubCategoryId = ARCR.RejectedReason
		AND ARCR.RejectedType = 2
	LEFT JOIN AbSure_QCategory AQC WITH (NOLOCK) ON AQC.AbSure_QCategoryId = AQS.AbSure_QCategoryId
	WHERE ARCR.Absure_CarDetailsId = @AbSure_CarDetailsId

	SELECT CONVERT(VARCHAR, ACD.RejectedDateTime, 106) RejectionDate
	FROM AbSure_CarDetails ACD WITH (NOLOCK)
	WHERE ACD.Id = @AbSure_CarDetailsId
END

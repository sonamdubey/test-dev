IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetOnHoldReason]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetOnHoldReason]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: Sept 16,2015
-- Description:	To fetch on hold reasons
-- EXEC AbSure_GetOnHoldReason 88
-- Modified By : Ashwini Dhamankar on Oct 28,2015  (Called Function Absure_GetMasterCarId only if car is duplicate)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetOnHoldReason] 
@AbSure_CarDetailsId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @RegNumber VARCHAR(50),@Status INT,@CancelReason VARCHAR(250),@CancelledReason VARCHAR(250)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

	SELECT	@RegNumber = RegNumber, @Status = ACD.Status, @CancelReason = ACD.CancelReason
	FROM	AbSure_CarDetails ACD WITH(NOLOCK)
	WHERE	ACD.ID = @AbSure_CarDetailsId

	IF(@Status = 3 AND @CancelReason = @CancelledReason)
	BEGIN
		SELECT @AbSure_CarDetailsId = dbo.Absure_GetMasterCarId(@RegNumber,@AbSure_CarDetailsId) 
	END  

	SELECT CONVERT(VARCHAR,DCR.EntryDate, 106) Date,DR.Reason Reason,ISNULL(ACD.OnHoldComments, 'Not Available') AS OnHoldComments
	FROM AbSure_DoubtfulCarReasons DCR WITH (NOLOCK)
	LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = DCR.Absure_CarDetailsId
	INNER JOIN AbSure_DoubtfulReasons DR WITH (NOLOCK) ON DR.Id = DCR.DoubtfulReason
	WHERE DCR.AbSure_CarDetailsId = @AbSure_CarDetailsId AND DCR.IsActive = 1

END
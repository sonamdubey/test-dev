IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveAbsureCarMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveAbsureCarMapping]
GO

	CREATE PROCEDURE [dbo].[TC_SaveAbsureCarMapping] 
	@BranchId				BIGINT,
	@TC_UserId				BIGINT,
	@AbSure_CarDetailsId	BIGINT,
	@TC_StockId				BIGINT,
	@UpdatedBy				INT
AS

BEGIN
	SELECT AbSure_CarDetailsId = @AbSure_CarDetailsId FROM AbSure_CarSurveyorMapping WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId

	IF(@@ROWCOUNT = 0)
		BEGIN
			INSERT INTO AbSure_CarSurveyorMapping(BranchId,TC_UserId,AbSure_CarDetailsId,TC_StockId) 
			VALUES(@BranchId,@TC_UserId,@AbSure_CarDetailsId,@TC_StockId) 
		END
    
	ELSE
		BEGIN
			UPDATE AbSure_CarSurveyorMapping SET TC_UserId = @TC_UserId,UpdatedBy = @UpdatedBy,UpdatedOn = GETDATE() 
			WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId
		END
END


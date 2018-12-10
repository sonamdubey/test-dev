IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveDoubtfulCarReason]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveDoubtfulCarReason]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 15th Sept, 2015
-- Description:	To save doubtful reasons for absure car
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveDoubtfulCarReason]
@AbSure_CarDetailsId BIGINT,
@DoubtfulReason1 INT = NULL,
@DoubtfulReason2 INT = NULL,
@DoubtfulReason3 INT = NULL,
@DoubtfulReason4 INT = NULL,
@DoubtfulComments VARCHAR(1000) = NULL
AS
BEGIN

	SELECT AbSure_CarDetailsId FROM AbSure_DoubtfulCarReasons WITH(NOLOCK) WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId AND IsActive = 1
	IF @@ROWCOUNT > 0
	BEGIN
		UPDATE AbSure_DoubtfulCarReasons SET IsActive = 0 WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId AND IsActive = 1
	END

	IF @DoubtfulReason1 IS NOT NULL
	BEGIN
		INSERT INTO AbSure_DoubtfulCarReasons (AbSure_CarDetailsId, DoubtfulReason, EntryDate, IsActive) 
		VALUES(@AbSure_CarDetailsId, @DoubtfulReason1, GETDATE(),1)
	END

	IF @DoubtfulReason2 IS NOT NULL
	BEGIN
		INSERT INTO AbSure_DoubtfulCarReasons (AbSure_CarDetailsId, DoubtfulReason, EntryDate, IsActive) 
		VALUES(@AbSure_CarDetailsId, @DoubtfulReason2, GETDATE(),1)
	END

	IF @DoubtfulReason3 IS NOT NULL
	BEGIN
		INSERT INTO AbSure_DoubtfulCarReasons (AbSure_CarDetailsId, DoubtfulReason, EntryDate, IsActive) 
		VALUES(@AbSure_CarDetailsId, @DoubtfulReason3, GETDATE(),1)
	END

	IF @DoubtfulReason4 IS NOT NULL
	BEGIN
		INSERT INTO AbSure_DoubtfulCarReasons (AbSure_CarDetailsId, DoubtfulReason, EntryDate, IsActive) 
		VALUES(@AbSure_CarDetailsId, @DoubtfulReason4, GETDATE(),1)
	END

	UPDATE AbSure_CarDetails SET Status = 9, OnHoldComments = @DoubtfulComments WHERE Id = @AbSure_CarDetailsId

END


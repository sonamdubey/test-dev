IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteBikeAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteBikeAvailability]
GO

	-- =============================================
-- Author:	   : Suresh Prajapati
-- Created     : on 28th jan, 2015
-- Description : To Delete Availability Days
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteBikeAvailability] @BikeVersionId INT
	,@DealerId INT
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE BW_BikeAvailability
	SET IsActive = 0
	WHERE BikeVersionId = @BikeVersionId
		AND DealerId = @DealerId
END
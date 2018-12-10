IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdateDealerExclusion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdateDealerExclusion]
GO

	
-- =============================================
-- Author:		Chetan A. Thambad
-- Create date: 21-09-2016
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdateDealerExclusion]
	 @dealerId INT
	,@startDate DATE
	,@createdBy INT
	,@reason varchar(200)
	
AS
BEGIN
	
	IF NOT EXISTS (select DealerId from DEALEREXCLUSION WITH(NOLOCK) where DealerId = @dealerId)
	BEGIN
	INSERT INTO DEALEREXCLUSION (DealerId, ExclusionFromDate, ExclusionReason, CreatedBy, CreatedOn) Values (@dealerId, @startDate, @reason, @createdBy, GETDATE());
	END

	ELSE

	BEGIN
	UPDATE DEALEREXCLUSION SET ExclusionFromDate = @startDate, ExclusionReason = @reason, CreatedBy = @createdBy, CreatedOn = GETDATE() WHERE DealerId = @dealerId
	END

END


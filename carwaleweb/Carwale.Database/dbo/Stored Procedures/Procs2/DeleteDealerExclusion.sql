IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteDealerExclusion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteDealerExclusion]
GO

	
-- =============================================
-- Author:		Chetan A. Thambad
-- Create date: 21-09-2016
-- =============================================
create PROCEDURE [dbo].[DeleteDealerExclusion] @dealerId INT
	,@deletedBy INT
AS
BEGIN
	IF EXISTS (
			SELECT DealerId
			FROM DEALEREXCLUSION WITH (NOLOCK)
			WHERE DealerId = @dealerId
			)
	BEGIN
		INSERT INTO DealerExclusionDelete_Log (
			 ID
			,DealerId
			,ExclusionFromDate
			,ExclusionReason
			,CreatedOn
			,CreatedBy
			,DeletedBy
			,DeletedOn
			)
		SELECT ID
			,DealerId
			,ExclusionFromDate
			,ExclusionReason
			,CreatedOn
			,CreatedBy
			,@deletedBy
			,GETDATE()
		FROM DealerExclusion WITH(NOLOCK)
		WHERE DealerId = @dealerId

		DELETE
		FROM DEALEREXCLUSION
		WHERE DealerId = @dealerId
	END
END

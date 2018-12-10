IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSyndicationDealerBranchId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSyndicationDealerBranchId]
GO

	CREATE PROCEDURE [dbo].[TC_GetSyndicationDealerBranchId]
	@TC_SyndicationWebsiteId INT,
	@BranchIds VARCHAR(1000) OUTPUT
AS
BEGIN
	SELECT @BranchIds = COALESCE(@BranchIds + ',', '') + CAST(BranchId AS VARCHAR)
	FROM TC_SyndicationDealer
	WHERE TC_SyndicationWebsiteId = @TC_SyndicationWebsiteId AND IsActive = 1
END


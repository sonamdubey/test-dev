IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerStocksCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerStocksCount]
GO
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author:		Chetan Kane
-- Create date: 9th May 2012
-- Description:	Getting All STock count for dealer
-- Modified By : Suresh Prajapati on 11th Mar, 2016
-- Description : Removed Password Check and added WITH (NOLOCK)
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerStocksCount] (
	@BranchId BIGINT
	,@UserId VARCHAR(50)
	,@Password VARCHAR(50) = NULL
	)
AS
BEGIN
	-- interfering with SELECT STatements.
	SET NOCOUNT ON;

	DECLARE @DealerId BIGINT = NULL

	SELECT @DealerId = DealerId
	FROM TC_APIUsers WITH (NOLOCK)
	WHERE UserId = @UserId
		--AND Password = @Password
		AND IsActive = 1

	IF (@DealerId = @BranchId)
	BEGIN
		SELECT COUNT(Id) AS StocksCount
		FROM TC_Stock WITH (NOLOCK)
		WHERE BranchId = @DealerId
			AND IsActive = 1
			AND StatusId = 1
			AND IsApproved = 1
	END
			--SELECT COUNT ( Id ) as StocksCount FROM TC_Stock WHERE BranchId = 5 AND IsActive = 1 AND StatusId = 1 AND IsApproved=1
END


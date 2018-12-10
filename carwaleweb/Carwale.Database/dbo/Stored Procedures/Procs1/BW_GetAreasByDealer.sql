IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetAreasByDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetAreasByDealer]
GO

	-- =============================================
-- Author:		Sumit Kate
-- Create date: 10 May 2016
-- Description:	Returns the list of Areas by Commute Distance Dealer ID
--	@DealerId				:	DealerId
-- e.g. EXEC [dbo].[BW_GetAreasByDealer] 18990
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetAreasByDealer] 
	@DealerId INT
AS
BEGIN
	IF (ISNULL(@DealerId,0) > 0)
	BEGIN
		SELECT DISTINCT
			a.ID,
			a.Lattitude,
			a.Longitude
		FROM Areas a WITH(NOLOCK)
		INNER JOIN DealerAreaCommuteDIstance cd WITH(NOLOCK)
		ON a.ID = cd.AreaId AND a.IsDeleted = 0 AND a.Lattitude IS NOT NULL AND a.Longitude IS NOT NULL
		AND cd.Dealerid = @DealerId
	END
END


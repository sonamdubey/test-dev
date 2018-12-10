IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerByArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerByArea]
GO

	-- =============================================
-- Author:		Sumit Kate
-- Create date: 09 May 2016
-- Description:	Returns the list of Dealers by Commute Distance Area ID
--	@AreaId				:	AreaId
-- e.g. EXEC [dbo].[BW_GetDealerByArea] 36
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerByArea] 
	@AreaId INT
AS
BEGIN
	IF (ISNULL(@AreaId,0) > 0)
	BEGIN
		SELECT DISTINCT
			d.ID,
			d.Lattitude,
			d.Longitude
		FROM Dealers d WITH(NOLOCK)
		INNER JOIN DealerAreaCommuteDIstance cd WITH(NOLOCK)
		ON d.ID = cd.Dealerid AND d.IsDealerActive = 1 AND d.IsDealerDeleted = 0
		AND cd.AreaId = @AreaId
	END
END

 

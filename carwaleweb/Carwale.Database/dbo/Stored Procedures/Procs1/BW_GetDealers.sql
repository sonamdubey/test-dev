IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealers]
GO

	

-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 22nd Oct 2014
-- Description:	To get Number of Dealer exist
-- Modified By : Sadhana Upadhyay on 6 Nov 2014
-- Summary : Checked isDealerActive flag
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealers] 
	@AreaId INT
	,@VersionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT TOP 1 BDP.DealerId
		,BDP.Id
	FROM BW_NewBikeDealerShowroomPrices AS BDP WITH (NOLOCK)
	INNER JOIN BW_DealerAreaMapping AS DAM WITH (NOLOCK) ON DAM.DealerId = BDP.DealerId
	INNER JOIN Dealers D ON D.ID = BDP.DealerId
	WHERE BDP.BikeVersionId = @VersionId
		AND DAM.AreaId = @AreaId
		AND DAM.IsActive = 1
		AND D.IsDealerActive = 1
		AND D.IsDealerActive = 1
		AND D.ApplicationId = 2
		AND D.IsDealerDeleted = 0
	ORDER BY BDP.ID DESC
END


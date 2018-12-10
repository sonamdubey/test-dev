IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerDisclaimer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerDisclaimer]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 03rd Dec, 2014
-- Description:	Procedure to get Added Dealer Disclaimer. 
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerDisclaimer]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT DD.ID AS Id, BM.Name AS Make, BMO.Name AS Model, BV.Name AS Version, DD.Disclaimer AS Disclaimer 
	FROM BW_DealerDisclaimer AS DD
	INNER JOIN BikeVersions AS BV WITH(NOLOCK) ON BV.ID=DD.BikeVersionId
	INNER JOIN BikeModels AS BMO WITH(NOLOCK) ON BMO.ID=BV.BikeModelId
	INNER JOIN BikeMakes  AS BM WITH(NOLOCK) ON BM.ID=BMO.BikeMakeId
	WHERE DD.IsActive=1 AND DD.DealerId=@DealerId
	ORDER BY Id DESC
END


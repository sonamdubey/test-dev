IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_AP_DeleteInActiveModelOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_AP_DeleteInActiveModelOffers]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 30 Nov 2014
-- Description:	AP to run after 12 am to truncate table ModelOffers and insert all active offer models
-- =============================================
CREATE PROCEDURE [dbo].[DO_AP_DeleteInActiveModelOffers]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.	

    -- Insert statements for procedure here
	
	--TRUNCATE TABLE ModelOffers

	--INSERT INTO ModelOffers
	--SELECT DISTINCT ModelId 
	--FROM DealerOffersVersion DOV WITH (NOLOCK)
	--JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
	--WHERE DOV.ModelId <> -1
	--AND DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.ClaimedUnits < DO.OfferUnits 
	--AND CAST(DO.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(DO.EndDate AS DATE) >= CAST(GETDATE() AS DATE) AND DO.OfferType <> 2
	--UNION
	--SELECT DISTINCT CMO.ID ModelId
	--FROM DealerOffersVersion DOV WITH (NOLOCK)
	--JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
	--JOIN CarMakes CMK WITH (NOLOCK) ON DOV.MakeId = CMK.ID
	--JOIN CarModels CMO WITH (NOLOCK) ON CMK.ID = CMO.CarMakeId AND CMO.New = 1 AND CMO.Futuristic = 0 AND CMO.IsDeleted = 0
	--WHERE DOV.ModelId = -1
	--AND DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.ClaimedUnits < DO.OfferUnits
	--AND CAST(DO.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(DO.EndDate AS DATE) >= CAST(GETDATE() AS DATE) AND DO.OfferType <> 2
	SELECT 1	
END

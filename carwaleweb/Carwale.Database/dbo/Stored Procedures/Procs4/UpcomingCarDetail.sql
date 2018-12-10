IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpcomingCarDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpcomingCarDetail]
GO

	-- =============================================
-- Author:		<Shalini Nair>
-- Create date: <29/09/14>
-- Description:	<Gets the expected launchdate and estimated price of upcoming car>
-- =============================================
CREATE PROCEDURE [dbo].[UpcomingCarDetail]
	-- Add the parameters for the stored procedure here
	@ModelId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EC.ID, ExpectedLaunch, EstimatedPriceMin, EstimatedPriceMax,CS.FullDescription AS Review FROM ExpectedCarLaunches EC WITH(NOLOCK) 
	 INNER JOIN CarModels Mo WITH(NOLOCK) ON EC.CarModelId = Mo.ID 
				 INNER JOIN CarMakes MK WITH(NOLOCK) ON MK.ID = Mo.CarMakeId 
				 INNER JOIN CarSynopsis CS WITH(NOLOCK) ON MO.ID = CS.ModelId
                WHERE  EC.CarModelId=@ModelId 
					AND CS.IsActive = 1
	END


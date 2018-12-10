IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpcomingCarDetail_15_11_4]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpcomingCarDetail_15_11_4]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: <01/12/14>
-- Description:Gets the expected launchdate and estimated price of upcoming car
-- Modified to retrieve MakeName and MaskingName
-- Modified by Jitendra to get expected Price for Upcoming Cars
-- =============================================
CREATE PROCEDURE [dbo].[UpcomingCarDetail_15_11_4]
	-- Add the parameters for the stored procedure here
	@ModelId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EC.ID, ExpectedLaunch, EstimatedPriceMin, EstimatedPriceMax,CS.FullDescription AS Review,MK.Name as MakeName,Mo.MaskingName FROM ExpectedCarLaunches EC WITH(NOLOCK) 
	 INNER JOIN CarModels Mo WITH(NOLOCK) ON EC.CarModelId = Mo.ID 
				 INNER JOIN CarMakes MK WITH(NOLOCK) ON MK.ID = Mo.CarMakeId 
				 LEFT JOIN CarSynopsis CS WITH(NOLOCK) ON MO.ID = CS.ModelId AND CS.IsActive = 1 -- change inner join to left join 
                WHERE  EC.CarModelId=@ModelId 
					
END

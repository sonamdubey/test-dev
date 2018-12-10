IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCityDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCityDetails]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCityDetails]
	-- Add the parameters for the stored procedure here
	@CityId float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   Select  Bharti_city AS CityName,Bharti_state AS StateName,Zone from [RTO Location] with(nolock) where CarWaleCityId = @CityId
END


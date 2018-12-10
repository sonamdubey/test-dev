IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCityId_IPNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCityId_IPNumber]
GO

	
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <02/04/2013>
-- Description:	<Returns CityId corresponding to the IP Number> EXEC GetCityId_IPNumber 711837695,0
-- =============================================
create PROCEDURE [dbo].[GetCityId_IPNumber] 
	-- Add the parameters for the stored procedure here
	@IPNumber BIGINT,
	@CityId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @CityId=IM.CityId
	FROM IPtoLocation_CityMapping IM
	INNER JOIN IPToLocation IL ON IM.IPtoLocationCityName=IL.CITY
	WHERE @IPNumber BETWEEN IL.IP_FROM AND IP_TO
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCityId_IPNumber_v1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCityId_IPNumber_v1]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <02/04/2013>
-- Description:	<Returns CityId corresponding to the IP Number> EXEC GetCityId_IPNumber 711837695,0
-- Modified by Supriya Khartode on 18/02/2015 to fetch cityname 
-- =============================================
CREATE PROCEDURE [dbo].[GetCityId_IPNumber_v1] 
	-- Add the parameters for the stored procedure here
	@IPNumber BIGINT,
	@CityId INT OUTPUT,
	@CityName Varchar(30) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @CityId=IM.CityId,@CityName=IM.CWCityName
	FROM IPtoLocation_CityMapping IM WITH(NOLOCK)
	INNER JOIN IPToLocation IL WITH(NOLOCK) ON IM.IPtoLocationCityName=IL.CITY
	WHERE @IPNumber BETWEEN IL.IP_FROM AND IP_TO
END

/****** Object:  StoredProcedure [dbo].[InsertTempCustomerV2_15.3.1]    Script Date: 19-02-2015 16:22:34 ******/
SET ANSI_NULLS ON

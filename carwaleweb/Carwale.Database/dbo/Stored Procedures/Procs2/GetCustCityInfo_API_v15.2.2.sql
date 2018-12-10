IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustCityInfo_API_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustCityInfo_API_v15]
GO

	

 
-- =============================================
-- Author:		Ashish Verma
-- Create date: 29/09/2014
-- Description:	Get Customer City Information based on cityId and Zone Id
-- Modified By Chetan T. on <09/12/2015> Modified the @ZoneId Condition from @ZoneId >'-1' to @ZoneId > 0
-- =============================================
CREATE PROCEDURE [dbo].[GetCustCityInfo_API_v15.2.2] 
	-- Add the parameters for the stored procedure here
	@CityId int,
	@ZoneId int,
	@CityName varchar(50) OUTPUT,
	@ZoneName varchar(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ZoneId > 0) -- Modified By Chetan T.
	BEGIN
		SELECT @CityName=ct.NAME,
			@ZoneName =cz.ZoneName 
		FROM Cities ct WITH (NOLOCK)
		INNER JOIN CityZones cz WITH (NOLOCK) ON ct.ID = cz.CityId
		WHERE ct.ID = @CityId
			AND cz.Id = @ZoneId
	END
	ELSE
	BEGIN
		SELECT @CityName=ct.NAME
		FROM Cities ct WITH (NOLOCK)
		WHERE ct.ID = @CityId
	END
END


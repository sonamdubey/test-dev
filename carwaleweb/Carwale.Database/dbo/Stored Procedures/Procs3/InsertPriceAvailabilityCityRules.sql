IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilityCityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilityCityRules]
GO

	
-- =============================================
-- Author:	Shalini Nair
-- Create date: 28/06/2016
-- Description:	To add city rules for PriceAvailability
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilityCityRules]
	@PriceAvailabilityId INT
	,@StateId INT
	,@CityId INT
	,@ZoneId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailabilityCityRules(
		PriceAvailabilityId
		,StateId
		,CityId
		,ZoneId
		)
	VALUES (
		@PriceAvailabilityId
		,CASE 
			WHEN @StateId = 0
				THEN (
						SELECT ct.StateId
						FROM Cities ct WITH (NOLOCK)
						WHERE ct.ID = @CityId
						)
			ELSE @StateId
			END
		,@CityId
		,@ZoneId
		)
END


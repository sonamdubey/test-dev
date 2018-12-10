IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilityAdditionalRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilityAdditionalRules]
GO
	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/06/2016
-- Description:	To add additional rules like DisplacementMax value for Price availability rule
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilityAdditionalRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@DisplacementMax INT
	,@DisplacementMin INT
	,@ExShowroomMax INT
	,@ExShowroomMin INT
	,@GroundClearanceMax INT
	,@GroundClearanceMin INT
	,@LengthMax INT
	,@LengthMin INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailabilityAdditionalRules (
		PriceAvailabilityId
		,DisplacementMax
		,DisplacementMin
		,ExShowroomMax
		,ExShowroomMin
		,GroundClearanceMax
		,GroundClearanceMin
		,LengthMax
		,LengthMin
		)
	VALUES (
		@PriceAvailabilityId
		,@DisplacementMax
		,@DisplacementMin
		,@ExShowroomMax
		,@ExShowroomMin
		,@GroundClearanceMax
		,@GroundClearanceMin
		,@LengthMax
		,@LengthMin
		)
END


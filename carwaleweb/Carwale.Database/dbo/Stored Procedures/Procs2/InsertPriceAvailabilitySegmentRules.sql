IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilitySegmentRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilitySegmentRules]
GO
	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/06/2016
-- Description:	To add segment rules for a price availability inclusion/exclusion rule
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilitySegmentRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@SegmentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailabilitySegmentRules (
		PriceAvailabilityId
		,SegmentId
		)
	VALUES (
		@PriceAvailabilityId
		,@SegmentId
		)
END


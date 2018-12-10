IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LogPriceAvailabilityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LogPriceAvailabilityRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 29/06/2016
-- Description:	Log the changes done in price availability 
-- =============================================
CREATE PROCEDURE [dbo].[LogPriceAvailabilityRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@Changes VARCHAR(max)
	,@LogMessage VARCHAR(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailability_log (
		PriceAvailabilityId
		,NAME
		,Explanation
		,Type
		,UpdatedBy
		,UpdatedOn
		,IsActive
		,Changes
		,LogMessage
		)
	SELECT id
		,NAME
		,Explanation
		,Type
		,UpdatedBy
		,UpdatedOn
		,IsActive
		,@Changes
		,@LogMessage
	FROM PQ_PriceAvailability WITH(NOLOCK)
	WHERE id = @PriceAvailabilityId
END


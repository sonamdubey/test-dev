IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemovePriceAvailabilityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemovePriceAvailabilityRules]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 28/06/2016
-- EXEC [RemovePriceAvailabilityRules] '33'
-- =============================================
CREATE PROCEDURE [dbo].[RemovePriceAvailabilityRules] @RuleIds VARCHAR(5000)
AS
BEGIN
	UPDATE PQ_PriceAvailability
	SET IsActive = 0
	WHERE Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@RuleIds)
			)
END


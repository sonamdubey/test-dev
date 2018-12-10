IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchLeadFlowCondition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchLeadFlowCondition]
GO
	-- ===========================================
-- Author:		<Nilima More>
-- Create date: <1st June,2016>
-- Description:	<To FETCH PreventAutomaticLeadFlowForSE flag status>
-- isPreventAutomaticLeadFlow = 1(no automatic lead assignment).
-- EXEC [TC_FetchLeadFlowCondition] 5,NULL
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchLeadFlowCondition]
@BranchId INT
,@retVal BIT OUTPUT
AS
BEGIN

	SET NOCOUNT ON;


	--if entry already exist set return value as true 
	SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK)
	WHERE BranchId = @BranchId AND TC_DealerFeatureId = 8

	IF @@ROWCOUNT > 0
		SET @retVal = 1 --LeadFlowTo DP
	ELSE
		SET @retVal = 0 --LeadFlowTo SE

END
---------------------------------------------------------------------------------------------------------------------


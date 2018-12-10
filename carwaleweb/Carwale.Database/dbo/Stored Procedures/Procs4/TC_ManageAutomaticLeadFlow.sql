IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ManageAutomaticLeadFlow]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ManageAutomaticLeadFlow]
GO

	-- ===========================================
-- Author:		<Nilima More>
-- Create date: <1st June,2016>
-- Description:	<To save PreventAutomaticLeadFlowForSE flag status>
-- isPreventAutomaticLeadFlow = 1(no automatic lead assignment).
--EXEC [TC_ManageAutomaticLeadFlow] 5,1
--Modified By : Ashwini Dhamankar on Oct 4,2016 (Removed SET NOCOUNT ON and remove camelcase from input parameters)
-- =============================================
CREATE PROCEDURE [dbo].[TC_ManageAutomaticLeadFlow]
@BranchId INT,
@IsPreventAutomaticLeadFlow BIT

AS
BEGIN
	--if entry already exist delete it
	DELETE FROM TC_MappingDealerFeatures 
    WHERE BranchId = @BranchId AND TC_DealerFeatureId = 8

	--insert new entry in table
	IF @IsPreventAutomaticLeadFlow = 1
	BEGIN
		INSERT INTO TC_MappingDealerFeatures (BranchId, TC_DealerFeatureId)
		VALUES (@BranchId, 8)
	END
END

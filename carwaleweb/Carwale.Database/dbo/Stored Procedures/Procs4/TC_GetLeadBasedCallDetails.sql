IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetLeadBasedCallDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetLeadBasedCallDetails]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 23rd Nov, 2015
-- Description	: Get lead based Call Details.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_GetLeadBasedCallDetails] 
@BranchId INT

AS
BEGIN		

	SELECT LeadId, CallType, CallTime, Duration, BranchId, LeadName, PhoneNumber FROM TC_LeadBasedCallDetails WITH(NOLOCK)
	WHERE BranchId = @BranchId
	ORDER BY CallTime DESC
		
END

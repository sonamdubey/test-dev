IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckAutoLeadAssignment_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckAutoLeadAssignment_V16]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Oct 26,2016
-- Description:	To check whether auto lead assignment is stopped or not
--declare @IsPrevented BIT 
--exec [dbo].[TC_CheckAutoLeadAssignment_V16.10.1] 20553,@IsPrevented
--select @IsPrevented
-- Modified By : Nilima More On 3rd Nov,2016,set @IsPrevented to 0,instead of handling null in code.
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckAutoLeadAssignment_V16.10.1] 
	@BranchId INT,
	@IsPrevented BIT  OUTPUT 
AS
BEGIN
	SET @IsPrevented = 0	
	IF EXISTS(SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId = 8)
		SET @IsPrevented = 1	
END

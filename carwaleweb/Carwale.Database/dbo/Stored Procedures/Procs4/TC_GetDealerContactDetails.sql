IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerContactDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerContactDetails]
GO

	-- Created by: Binu
-- Create date: 31 May 2012
-- Description:	Get dealer email,mobile to send sms
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerContactDetails] 
	@BranchId NUMERIC
	
AS
BEGIN
	SELECT TOP 1 Id,Email,Mobile,PwdRecoveryEmail FROM TC_Users WHERE BranchId=@BranchId AND IsActive = 1 ORDER BY EntryDate ASC
END

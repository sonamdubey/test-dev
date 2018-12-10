IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetServiceCenter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetServiceCenter]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 14th Oct 2015
-- Description:	To get active service center based on the dealerId
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetServiceCenter]
	@BranchId BIGINT
AS
BEGIN
	SELECT TC_ServiceCenterId Value,ServiceCenterName Text 
	FROM TC_ServiceCenter WITH(NOLOCK)
	WHERE BranchId = @BranchId AND IsActive=1
END



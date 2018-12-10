IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetServiceCenterDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetServiceCenterDetails]
GO

	
-- =============================================
-- Author:	 Vicky Gupta
-- Create date: 9/11/2015
-- Description:	To get  service center details for a given serviceCenterId  (this sp will be used for sending emails)
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetServiceCenterDetails]
	@ServiceCenterId INT
AS
BEGIN
	SELECT SC.PhoneNo,SC.ServiceCenterName,DL.Organization
	FROM TC_ServiceCenter SC WITH(NOLOCK) INNER JOIN Dealers DL WITH(NOLOCK) ON SC.BranchId = DL.ID AND DL.IsDealerActive = 1
	WHERE SC.TC_ServiceCenterId = @ServiceCenterId  AND IsActive=1
END
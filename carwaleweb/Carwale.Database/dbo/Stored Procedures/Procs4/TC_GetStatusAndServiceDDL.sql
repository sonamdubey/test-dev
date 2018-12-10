IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStatusAndServiceDDL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStatusAndServiceDDL]
GO

	-- =============================================
-- Author:		Upendra Kumar
-- Create date: <15-10-2015>
-- Description:	<Get all Status And Service Center in Drop DownList for servicePage>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetStatusAndServiceDDL]
AS
BEGIN
	SELECT TSS.TC_ServiceStatusId,TSS.ServiceStatus 
	FROM TC_ServiceStatus AS TSS WITH (NOLOCK)
	WHERE TSS.IsActive=1

	SELECT TSC.TC_ServiceCenterId,TSC.ServiceCenterName
	FROM TC_ServiceCenter AS TSC WITH (NOLOCK)
	WHERE TSC.IsActive=1
END




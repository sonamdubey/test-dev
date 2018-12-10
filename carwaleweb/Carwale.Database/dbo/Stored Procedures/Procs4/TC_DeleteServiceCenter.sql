IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteServiceCenter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteServiceCenter]
GO

	-- =============================================================
-- Author		: Suresh Prajapati
-- Create date	: 13th Oct 2015
-- Description	: To Inactive/Delete an existing Service Center
-- =============================================================
CREATE PROCEDURE [dbo].[TC_DeleteServiceCenter] @TC_ServiceCenterId INT
AS
BEGIN
	UPDATE TC_ServiceCenter
	SET IsActive = 0
	WHERE TC_ServiceCenterId = @TC_ServiceCenterId
END







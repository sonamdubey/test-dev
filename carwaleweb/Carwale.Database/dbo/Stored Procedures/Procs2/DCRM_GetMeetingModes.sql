IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetMeetingModes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetMeetingModes]
GO

	-- =============================================
-- Author	:	Sachin Bharti(16th July)
-- Description	:	Get all dcrm meeting modes
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetMeetingModes]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT 
			DCM.Id,
			DCM.Name
	FROM DCRM_MeetingModes DCM(NOLOCK)
END


/****** Object:  StoredProcedure [dbo].[DCRM_UpdateSalesStatus]    Script Date: 07/18/2014 13:36:06 ******/
SET ANSI_NULLS ON

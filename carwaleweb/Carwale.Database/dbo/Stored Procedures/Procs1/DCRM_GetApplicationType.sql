IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetApplicationType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetApplicationType]
GO
	-- =============================================
-- Author:		Kritika Choudhary
-- Create date: 30-12-2015
-- Description:	Get Application Type
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetApplicationType] 
  AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT   ApplicationId AS Value, ApplicationName AS Text
	FROM     TC_Applications (NOLOCK) 
	
END









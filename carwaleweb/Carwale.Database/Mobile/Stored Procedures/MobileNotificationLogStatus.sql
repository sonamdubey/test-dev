IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[MobileNotificationLogStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[MobileNotificationLogStatus]
GO

	

-- Author		:	Kundan Dombale
-- Create date	:	11-Jan-2016 19:21
-- Description	:	This SP used to check currently Running process of GCM RegistrationId Based on Subscription type        
-- ============================================= 


CREATE  PROCEDURE [Mobile].[MobileNotificationLogStatus]
As 
BEGIN  
        SET NOCOUNT ON 
		--SELECT TOP 1  SubsMasterId, StartDate ,enddate FROM MOBILE.MobileNotificationLog WITH(NOLOCK)
		SELECT COUNT(*) AS NoOfProcesses FROM MOBILE.MobileNotificationLog WITH(NOLOCK)
		WHERE EndDate IS NULL
		--ORDER BY Id DESC 
END 

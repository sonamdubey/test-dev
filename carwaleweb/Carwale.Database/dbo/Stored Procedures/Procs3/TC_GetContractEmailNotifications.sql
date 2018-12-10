IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetContractEmailNotifications]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetContractEmailNotifications]
GO
	
-- =============================================
-- Author		: RUCHIRA PATIL
-- Created Date : 8th Feb 2016.
-- Description  : To check flag isEmailSent before sending the email to dealer when the contract has reached 85%.
-- EXEC [TC_GetContractEmailNotifications] 1453
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetContractEmailNotifications] 
@TC_ContractCampaignMappingId INT
AS
BEGIN
	DECLARE @IsEmailSent BIT = 0 ,
	@IsRenewalRequested BIT = 0 
	SELECT @IsRenewalRequested = ISNULL(IsRenewalRequested,0), @IsEmailSent = ISNULL(IsEmailSent,0)
	FROM TC_ContractsNotification WITH(NOLOCK) 
	WHERE TC_ContractCampaignMappingId = @TC_ContractCampaignMappingId
	SELECT @IsRenewalRequested IsRenewalRequested, @IsEmailSent IsEmailSent
END
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

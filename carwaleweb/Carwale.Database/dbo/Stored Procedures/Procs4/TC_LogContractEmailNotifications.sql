IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LogContractEmailNotifications]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LogContractEmailNotifications]
GO

	

-- =============================================
-- Author		: RUCHIRA PATIL
-- Created Date : 8th Feb 2016.
-- Description  : To log flag isEmailSent after sending the email to dealer when the contract has reached 85%.
-- EXEC [TC_LogContractEmailNotifications] 1453
-- Modified by : Kritika Choudhary on 9th Feb 2016, take comma seperated @TC_ContractCampaignMappingId and perform operation for each id
-- =============================================
CREATE PROCEDURE [dbo].[TC_LogContractEmailNotifications] 
@TC_ContractCampaignMappingId VARCHAR(MAX),
@IsEmailSent BIT,
@IsRenewalRequested BIT
AS
BEGIN

    DECLARE @sDelimiter CHAR= ',', @CCMID VARCHAR(20),@ContractCampMapId VARCHAR(MAX) = NULL
	
	SET @TC_ContractCampaignMappingId = @TC_ContractCampaignMappingId + ','
	
	WHILE CHARINDEX(@sDelimiter,@TC_ContractCampaignMappingId,0) <> 0
	BEGIN 
	      SET @CCMID=RTRIM(LTRIM(SUBSTRING(@TC_ContractCampaignMappingId,1,CHARINDEX(@sDelimiter,@TC_ContractCampaignMappingId,0)-1)))
		  SET @TC_ContractCampaignMappingId=RTRIM(LTRIM(SUBSTRING(@TC_ContractCampaignMappingId,CHARINDEX(@sDelimiter,@TC_ContractCampaignMappingId,0)+LEN(@sDelimiter),LEN(@TC_ContractCampaignMappingId))))
		 
       IF LEN(@CCMID) > 0
       BEGIN
			IF EXISTS(SELECT TC_ContractCampaignMappingId FROM TC_ContractsNotification WITH(NOLOCK) WHERE TC_ContractCampaignMappingId = @CCMID)
			BEGIN
				IF @IsEmailSent = 1 
					UPDATE TC_ContractsNotification SET IsEmailSent = @IsEmailSent WHERE TC_ContractCampaignMappingId = @CCMID
				IF @IsRenewalRequested = 1
					UPDATE TC_ContractsNotification SET IsRenewalRequested = @IsRenewalRequested WHERE TC_ContractCampaignMappingId = @CCMID
			END
			ELSE
				INSERT INTO TC_ContractsNotification(TC_ContractCampaignMappingId,IsEmailSent,IsRenewalRequested)
				VALUES(@CCMID,@IsEmailSent,@IsRenewalRequested)
	   END
    END
END



 

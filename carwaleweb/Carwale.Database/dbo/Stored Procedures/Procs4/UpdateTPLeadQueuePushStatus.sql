IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTPLeadQueuePushStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTPLeadQueuePushStatus]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <26/09/2012>
-- Description:	<Updates the push status field in ThirdPartyLeads for the id that is passed>
-- Mdified by Ashish Verma on 09-02-2015 --- this condition(AND PushStatus !='-1' AND PushStatus !='3' AND PushStatus !='4') is temp. fix for Bmw Lead 
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTPLeadQueuePushStatus] 
	-- Add the parameters for the stored procedure here
	@ThirdPartyLeadId NUMERIC(18,0),
	@PushStatus VARCHAR(200),
	@DNDStatus VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ThirdPartyLeadQueue
	SET PushStatus = @PushStatus,DNDStatus = @DNDStatus
	WHERE ThirdPartyLeadId = @ThirdPartyLeadId
	
    DECLARE @TPLeadSettingId INT 
    SELECT @TPLeadSettingId=TPLeadSettingId FROM ThirdPartyLeadQueue WITH (NOLOCK) WHERE ThirdPartyLeadId=@ThirdPartyLeadId
    
    IF(SELECT LeadVolume-LeadsSent FROM ThirdPartyLeadSettings WITH (NOLOCK) WHERE ThirdPartyLeadSettingId=@TPLeadSettingId)>0
		UPDATE ThirdPartyLeadSettings
		SET LeadsSent = (SELECT COUNT(ThirdPartyLeadId)
						 FROM ThirdPartyLeadQueue
						 WHERE TPLeadSettingId=@TPLeadSettingId AND PushStatus !='-1' AND PushStatus !='3' AND PushStatus !='4')-- Mdified by Ashish Verma on 09-02-2015
		WHERE ThirdPartyLeadSettingId=@TPLeadSettingId

	
END
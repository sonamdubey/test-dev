IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTPLeadQueuePushStatus_v16_8_7]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTPLeadQueuePushStatus_v16_8_7]
GO

	-- =============================================
-- Author:		<Sanjay Soni>
-- Create date: <01/03/2016>
-- Description:	<Updates the push status field in ThirdPartyLeads for the id that is passed>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTPLeadQueuePushStatus_v16_8_7]
	-- Add the parameters for the stored procedure here
	@ThirdPartyLeadId INT
	,@PushStatus VARCHAR(MAX)
	,@DNDStatus VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TPLeadSettingId INT

	SELECT @TPLeadSettingId = TPLeadSettingId
	FROM ThirdPartyLeadQueue WITH (NOLOCK)
	WHERE ThirdPartyLeadId = @ThirdPartyLeadId

	-- Insert statements for procedure here
	UPDATE ThirdPartyLeadQueue
	SET PushStatus = @PushStatus
		,DNDStatus = @DNDStatus
		,IsSuccess = (
			CASE 
				WHEN EXISTS (
						SELECT TPSP.Pattern
						FROM ThirdPartySuccessPattern TPSP WITH (NOLOCK)
						WHERE TPSP.LeadSettingId = @TPLeadSettingId
							AND RTRIM(LTRIM(@PushStatus)) LIKE '%' + TPSP.Pattern + '%'
						)
					THEN 1
				ELSE 0
				END
			)
	WHERE ThirdPartyLeadId = @ThirdPartyLeadId

	UPDATE ThirdPartyLeadSettings
	SET LeadsSent = LeadsSent + 1
	WHERE ThirdPartyLeadSettingId = @TPLeadSettingId
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteScheduledCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteScheduledCampaign]
GO
	-- ================================================
-- Author:		Tejashree Patil
-- Create date: 11 Oct 2013
-- Description: To delete scheduled campaign.
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteScheduledCampaign]
	@TC_CampaignSchedulingId INT,
	@UserId INT = NULL
AS
BEGIN
	
	BEGIN
		UPDATE	TC_CampaignScheduling 
		SET		IsActive = 0 , ModifiedBy = @UserId, ModifiedDate=GETDATE()
		WHERE	TC_CampaignSchedulingId = @TC_CampaignSchedulingId
	END

END

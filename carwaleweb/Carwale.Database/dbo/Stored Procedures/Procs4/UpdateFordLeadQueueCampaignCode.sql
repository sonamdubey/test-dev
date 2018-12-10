IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateFordLeadQueueCampaignCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateFordLeadQueueCampaignCode]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <28/09/2012>
-- Description:	<Updates the field CampaignCode in ThirdPartyLeadQueue table for Ford>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateFordLeadQueueCampaignCode] 
	-- Add the parameters for the stored procedure here
	@TPLQId NUMERIC(18,0),
	@ModelCode VARCHAR(10)='',
	@CampaignCode VARCHAR(20) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  @CampaignCode= @ModelCode+'CW'+ISNULL(ST.StateCode,'')+'ENQ'
	FROM ThirdPartyLeadQueue TP 
	INNER JOIN Cities CI ON TP.CityId=CI.ID AND CI.IsDeleted=0
	INNER JOIN States ST ON ST.ID=CI.StateId AND ST.IsDeleted=0
	WHERE TP.ThirdPartyLeadId=@TPLQId
	
	
	UPDATE ThirdPartyLeadQueue
	SET CampaignCode=@CampaignCode
	WHERE ThirdPartyLeadId=@TPLQId
END


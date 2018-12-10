IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MapCampaignToContract]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MapCampaignToContract]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 17/02/2016
-- Description:	Mapping the campaign to a contract and resuming the campaign if paused
-- Modified by : Shalini Nair on 12/04/2016 to map campaign in TC_contractcampaignmapping considering DealerId
-- =============================================
CREATE PROCEDURE [dbo].[MapCampaignToContract]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
	,@ContractId INT
	,@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	UPDATE TC_ContractCampaignMapping
	SET CampaignId = @CampaignId
	WHERE ContractId = @ContractId and DealerId = @DealerId

	UPDATE PQ_DealerSponsored
	SET IsActive = 1
	WHERE id = @CampaignId and IsActive = 0
END


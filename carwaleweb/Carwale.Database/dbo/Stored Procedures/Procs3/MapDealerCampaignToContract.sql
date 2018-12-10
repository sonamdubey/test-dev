IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MapDealerCampaignToContract]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MapDealerCampaignToContract]
GO

	-- =============================================
-- Author:		Ashish Kamble
-- Create date: 13 July 2016
-- Description:	Proc to map the dealer campaign to the contract
-- =============================================
create PROCEDURE [dbo].[MapDealerCampaignToContract]
	@CampaignId INT,
	@ContractId INT,
	@DealerId INT,
	@ApplicationId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE TC_ContractCampaignMapping
	SET ContractStatus = 1, CampaignId = @CampaignId
	WHERE ApplicationId = @ApplicationId and ContractId = @ContractId and DealerId = @DealerId
END

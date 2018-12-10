IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContractDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContractDetails]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 01/03/2016
-- Description:	Get contract details based on contract id
-- Modified By: Shalini Nair on 12/04/2016 to fetch contract details based on dealerid as well
-- =============================================
CREATE PROCEDURE [dbo].[GetContractDetails]
	-- Add the parameters for the stored procedure here
	@ContractId INT,
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT CampaignId
		,StartDate
		,EndDate
		,ContractBehaviour
		,DealerId
	FROM TC_ContractCampaignMapping WITH (NOLOCK)
	WHERE ContractId = @ContractId and DealerId = @DealerId
END

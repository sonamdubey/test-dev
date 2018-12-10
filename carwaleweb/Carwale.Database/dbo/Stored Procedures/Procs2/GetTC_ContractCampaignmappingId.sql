IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTC_ContractCampaignmappingId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTC_ContractCampaignmappingId]
GO

	

-- =============================================
-- Author:		<Komal Manjare >
-- Create date: <09 february 2016>
-- Description:	<get Tc_ContractCampaignmapping Id>
-- =============================================
CREATE PROCEDURE [dbo].[GetTC_ContractCampaignmappingId]  
	@DealerId INT,
	@ContractId INT
AS
BEGIN

	SELECT Id AS ContractCampaignMappingId
	FROM TC_ContractCampaignMapping WITH(NOLOCK)
	WHERE ContractId=@ContractId AND DealerId=@DealerId
	
END
---------------------------------AMIT YADAV-------------------------------


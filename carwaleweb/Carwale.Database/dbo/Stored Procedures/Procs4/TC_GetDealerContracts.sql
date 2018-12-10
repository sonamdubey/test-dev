IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerContracts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerContracts]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6th Oct 15>
-- Description:	<Return existing contracts for dealer>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerContracts]
@DealerId INT
AS
BEGIN
		SELECT ContractId,StartDate,EndDate, Id AS Value,
		'CId#'+ISNULL(CONVERT(varchar,ContractId),'')+'-SD-'+ISNULL(CONVERT(varchar,StartDate),'')+'-ED-'+ISNULL(CONVERT(varchar,EndDate),'') AS Text 		
		FROM TC_ContractCampaignMapping WITH(NOLOCK)
		WHERE DealerId = @DealerId AND ContractStatus = 3
END


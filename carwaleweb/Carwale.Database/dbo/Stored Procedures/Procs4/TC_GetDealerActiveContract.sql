IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerActiveContract]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerActiveContract]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 2nd June 2016
-- Description:	to get dealer's active contract to fill the contract dropdown
-- Modified By : Khushaboo On 20 jun 2016 added start and end date conditions to fetch only currently running active contracts
-- EXEC TC_GetDealerActiveContract 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerActiveContract]
	@DealerId INT
AS
BEGIN
		SELECT ContractId Value,
			CASE ContractBehaviour
			WHEN 1
				THEN 'StartDate-'+ CONVERT(VARCHAR, StartDate, 106) +'-'+ CONVERT(VARCHAR, TotalGoal) + '-Leads' 
			ELSE 'StartDate-'+ CONVERT(VARCHAR, StartDate, 106) +'-'+ CONVERT(VARCHAR, DATEDIFF(dd, CM.StartDate, EndDate)) + '-Days' 
			END Text
		FROM TC_ContractCampaignMapping CM WITH (NOLOCK)
		WHERE CM.DealerId= @DealerId AND ContractStatus = 1 
		AND CONVERT(DATE,CM.StartDate) <= CONVERT(DATE,GETDATE()) AND
		CONVERT(DATE,ISNULL(CM.EndDate,GETDATE())) >= CONVERT(DATE,GETDATE()) 
		ORDER BY CM.Id DESC
END
-----------------------------------------------------------------------------------------



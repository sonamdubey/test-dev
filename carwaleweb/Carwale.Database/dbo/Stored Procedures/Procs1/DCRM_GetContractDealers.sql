IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetContractDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetContractDealers]
GO
	

-- =============================================
-- Author: Vinay kumar Prajapati 24th Nov 2015
-- Purpose : get All Contract Dealers 
-- EXEC DCRM_GetContractDealers '-1',-1,'','0'
-- =============================================
CREATE  PROCEDURE [dbo].[DCRM_GetContractDealers]
	-- Add the parameters for the stored procedure here
	 @MakeId INT = NULL
	,@StateId INT= NULL
	,@CityIds VARCHAR(MAX) = NULL
AS
BEGIN
     -- Avoide Extra message 
	SET NOCOUNT ON 
	SELECT DISTINCT D.ID AS Value , D.Organization + ' - '+ CONVERT(VARCHAR, D.ID)  AS [Text]
	FROM Dealers AS D WITH(NOLOCK) 
	INNER JOIN  TC_ContractCampaignMapping AS CCM WITH(NOLOCK) ON CCM.DealerId=D.ID AND D.IsDealerActive=1
	AND (D.CityId IN (SELECT items FROM [SplitText](@CityIds, ',')) OR @CityIds IS NULL)
    AND (D.StateId = @StateId OR @StateId IS NULL)
	
END




IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealersForContractRenewal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealersForContractRenewal]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 9th Feb 2016
-- Description:	To fetch dealers whose contract have reached 85% mark
-- EXEC TC_GetDealersForContractRenewal 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealersForContractRenewal] 
AS
BEGIN
	SELECT DISTINCT CM.ID TC_ContractCampaignMappingID,CM.TotalGoal,CM.TotalDelivered,
	DATEDIFF(dd,StartDate,GETDATE()) ActiveSince,
	DATEDIFF(dd,StartDate,EndDate) TotalDays,
	CM.StartDate,CM.EndDate
	,CM.ContractBehaviour -- 1 is lead based and 2 is date based
	,CASE ISNULL(ContractBehaviour,0) WHEN  1 THEN  CAST(CAST(TotalGoal AS float) * (0.85) AS NUMERIC(15,0))   ELSE  0 END leadMargin
	,DATEDIFF(dd,StartDate,EndDate-7) DateMargin
	,CASE WHEN CM.ContractBehaviour = 1 
			THEN CONVERT(INT, ROUND(CAST(CAST(TotalDelivered AS FLOAT) / (TotalGoal) AS DECIMAL(8, 2)) * 100, 0)) 
		ELSE 
			 CONVERT(INT, ROUND(CAST(CAST(DATEDIFF(dd, StartDate, GETDATE()) AS FLOAT) / (DATEDIFF(dd, StartDate, EndDate)) AS DECIMAL(8, 2))* 100, 0))
	 END AS completionPercentage
	,d.ID DealersId
	,CM.Id TC_ContractCampaignMappingId,D.MobileNo Mobile
	,D.EmailId Email
	,D.FirstName DealerName
	FROM TC_ContractCampaignMapping CM WITH (NOLOCK)
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = CM.DealerId	
	LEFT JOIN TC_ContractsNotification CN WITH (NOLOCK) ON CN.TC_ContractCampaignMappingId = CM.id  
	WHERE 
	CM.ContractStatus = 1
	 AND
	ISNULL(CN.IsEmailSent,0) = 0 AND ISNULL(CN.IsRenewalRequested,0) = 0
		 AND
	((DATEDIFF(dd,StartDate,EndDate-7) <= DATEDIFF(dd,StartDate,GETDATE())) OR (CAST(CAST(TotalGoal AS float) * (0.85) AS NUMERIC(15,0)) <= TotalDelivered))
END

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

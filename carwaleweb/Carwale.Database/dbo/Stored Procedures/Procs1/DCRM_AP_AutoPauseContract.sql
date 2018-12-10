IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_AutoPauseContract]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_AutoPauseContract]
GO

	-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 15 feb 2015
-- Description:	Pause contracts 
-- EXEC DCRM_AP_AutoPauseContract
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_AP_AutoPauseContract]
	-- Add the parameters for the stored procedure here

AS
BEGIN

	-- TempTable to store TransactionId,ContractId,CampaignId,Amount
	DECLARE @TempContractDetails TABLE (TransactionId INT , ContractId INT,CampaignId INT,Amount FLOAT)

	INSERT INTO @TempContractDetails(TransactionId,ContractId,CampaignId,Amount)
	SELECT DISTINCT RDPF.TransactionId, TCC.ContractId,TCC.CampaignId,DPT.FinalAmount
	FROM TC_ContractCampaignMapping TCC WITH(NOLOCK)
	JOIN RVN_DealerPackageFeatures RDPF WITH(NOLOCK) ON TCC.ContractId = RDPF.DealerPackageFeatureID AND TCC.ContractStatus = 1 AND TCC.ContractType = 1 
	AND TCC.AutoPauseDate IS NOT NULL AND (DATEDIFF(day,TCC.AutoPauseDate,CONVERT(DATE ,GETDATE())) > 0)
	JOIN DCRM_PaymentTransaction DPT WITH(NOLOCK) ON DPT.TransactionId = RDPF.TransactionId
	JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = RDPF.TransactionId  -- consider approved case , rejected, no approval
	GROUP BY RDPF.TransactionId,TCC.ContractId,TCC.CampaignId,TCC.ContractStatus,TCC.AutoPauseDate,DPD.IsApproved,DPT.FinalAmount
	HAVING ((DPD.IsApproved = 1 AND SUM(DPD.AmountReceived) <= 0 ) OR DPD.IsApproved IS NULL OR (DPD.IsApproved = 0 AND SUM(DPD.AmountReceived) >= 0 ) ) 
	ORDER BY TCC.ContractId DESC

	--SELECT * FROM @TempContractDetails

	-- Log data in TC_ContractCampaignMappingLog table before modifying TC_ContractCampaignMapping Table
	INSERT INTO TC_ContractCampaignMappingLog (ContractId,CampaignId,DealerId,StartDate,EndDate,TotalGoal,
				TotalDelivered,ContractStatus,ContractBehaviour,CostPerLead,ContractType,ReplacementContractId,CreatedOn,AutoPauseDate)
	SELECT TCC.ContractId,TCC.CampaignId,TCC.DealerId,TCC.StartDate,TCC.EndDate,TCC.TotalGoal,
			TCC.TotalDelivered,TCC.ContractStatus,TCC.ContractBehaviour,TCC.CostPerLead,TCC.ContractType,TCC.ReplacementContractId,GETDATE(),AutoPauseDate
	FROM  TC_ContractCampaignMapping  TCC WITH(NOLOCK)
	JOIN @TempContractDetails TCD ON TCD.ContractId = TCC.ContractId


	-- Update Contract status of TC_ContractCampaignMapping
	UPDATE  TCC
	SET TCC.ContractStatus = 2 
	FROM TC_ContractCampaignMapping TCC WITH(NOLOCK)
	JOIN @TempContractDetails TCD ON TCD.ContractId = TCC.ContractId


	  --===============================================================================================================
	  -- Dump paused  Data Into Table DCRM_AutoPausedDataToSendmail for further sending mail to respective executive 
	  --===============================================================================================================
	
	INSERT INTO DCRM_AutoPausedDataToSendmail(DealerPackageFeatureId,TransactionId,DealerId,DealerName,CityId,CityName,
	L3UserId,L3Name,L3LoginId,Amount,StartDate,PausedDate,PackageId,PackageName,CreatedOn)
	
	SELECT RDPF.DealerPackageFeatureID,RDPF.TransactionId,TCC.DealerId,D.Organization,D.CityId,C.Name,OU.Id,OU.UserName,OU.LoginId,TCD.Amount,TCC.StartDate,TCC.AutoPauseDate,  
			RDPF.PackageId,P.Name,GETDATE()
	FROM TC_ContractCampaignMapping TCC WITH(NOLOCK)
	JOIN @TempContractDetails TCD ON TCC.ContractId = TCD.ContractId --AND TCC.CampaignId = TCD.CampaignId
	JOIN RVN_DealerPackageFeatures RDPF WITH(NOLOCK) ON RDPF.DealerPackageFeatureID = TCD.ContractId
	JOIN Packages P WITH(NOLOCK) ON P.Id = RDPF.PackageId
	JOIN Dealers D WITH(NOLOCK) ON D.ID = TCC.DealerId 
	JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
	LEFT JOIN DCRM_ADM_UserDealers AS DAU WITH(NOLOCK) ON DAU.DealerId= D.Id AND DAU.RoleId=3 -- Verify It 
	LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DAU.UserId 

	--SELECT * FROM DCRM_AutoPausedDataToSendmail WITH(NOLOCK)
END


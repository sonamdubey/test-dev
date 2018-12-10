IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SetAutoPauseDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SetAutoPauseDate]
GO

	-- =============================================
-- Author:		Sunil Yadav 
-- Create date: 17 feb 2016 
-- Description:	To set autoPause date in TC_contractCampaignMapping Table.
-- EXEC DCRM_SetAutoPauseDate '2016-02-20 00:00:00.000', 10862
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SetAutoPauseDate]
 @StartDate DATETIME 
, @ContractId INT
AS
BEGIN
	IF NOT EXISTS (
		SELECT TOP 1 DPD.ID FROM DCRM_PaymentDetails DPD WITH(NOLOCK)
		JOIN RVN_DealerPackageFeatures RDPF WITH(NOLOCK) ON RDPF.TransactionId = DPD.TransactionId 
		GROUP BY DPD.ID,RDPF.DealerPackageFeatureID,DPD.IsApproved
		HAVING RDPF.DealerPackageFeatureID = @ContractId 
		AND DPD.IsApproved = 1  
		AND SUM(DPD.AmountReceived) > 0
	)

	BEGIN
			UPDATE TC_ContractCampaignMapping 
			SET AutoPauseDate = DATEADD(day,10,CONVERT(DATE,@StartDate))
			WHERE ContractId = @ContractId
	END
END

-------------------------------------------------------------------------------------------------------------------------
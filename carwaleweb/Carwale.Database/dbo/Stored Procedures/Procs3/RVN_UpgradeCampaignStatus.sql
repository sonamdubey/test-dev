IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_UpgradeCampaignStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_UpgradeCampaignStatus]
GO

	-- =============================================
-- Author	:	Sachin Bharti(13th April)
-- Description	:	Upgrade Campaign Status 
-- =============================================
CREATE PROCEDURE [dbo].[RVN_UpgradeCampaignStatus]
	
AS
BEGIN
	
	--first get number of active campaign those are 
	--reach there end date
	SELECT 
		RVN.DealerPackageFeatureID
	FROM	
		RVN_DealerPackageFeatures RVN(NOLOCK)
	WHERE 
		CONVERT(VARCHAR,RVN.PackageEndDate,102) < CONVERT(VARCHAR,GETDATE(),102)
		AND RVN.PackageStatus = 2 --running campaign
	IF @@ROWCOUNT > 0
		BEGIN
			UPDATE	RVN_DealerPackageFeatures  
				SET PackageStatus = 4 , --make them delivered
					PackageStatusDate = GETDATE()
			WHERE 
				CONVERT(VARCHAR,PackageEndDate,102) < CONVERT(VARCHAR,GETDATE(),102)
				AND PackageStatus = 2 
		END

	--now get number of start delivery , suspended campaign 
	--those are reach there end date and no action is taken till date
	SELECT 
		RVN.DealerPackageFeatureID
	FROM	
		RVN_DealerPackageFeatures RVN(NOLOCK)
	WHERE 
		CONVERT(VARCHAR,RVN.PackageEndDate,102) < CONVERT(VARCHAR,GETDATE(),102)
		AND RVN.PackageStatus IN (1,3) --start delivery and suspended campaigns
	IF @@ROWCOUNT > 0
		BEGIN
			UPDATE	RVN_DealerPackageFeatures  
				SET PackageStatus = 5 , --mark them rejected
					PackageStatusDate = GETDATE()
			WHERE 
				CONVERT(VARCHAR,PackageEndDate,102) < CONVERT(VARCHAR,GETDATE(),102)
				AND PackageStatus IN (1,3)
		END

END

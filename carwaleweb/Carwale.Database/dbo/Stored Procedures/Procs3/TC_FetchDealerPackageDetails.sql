IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchDealerPackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchDealerPackageDetails]
GO
	 

-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22nd Mar 2016
-- Description:	To fetch Package Details and Top Up details for a dealer
-- EXEC TC_FetchDealerPackageDetails 5
-- Modified by Ruchira Patil on 28th April 2016 (changed the join condition on Packages table)
-- Modified by Ruchira Patil on 29th April 2016 (fetch the uploaded stock count)
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchDealerPackageDetails]
	@BranchId BIGINT
AS
BEGIN
	
	--To fetch package details
	SELECT P.Name PackageType, CCP.ExpiryDate PackageExpDate ,CCP.Points StockLimit,COUNT(S.Id) UploadedStockCount
	FROM ConsumerCreditPoints CCP WITH (NOLOCK)
	JOIN Packages P WITH (NOLOCK) ON P.Id = CCP.CustomerPackageId --Modified by Ruchira Patil on 28th April 2016 (changed the join condition on Packages table)
	LEFT JOIN TC_Stock S WITH (NOLOCK) ON S.BranchId=CCP.ConsumerId and IsSychronizedCW = 1 and S.StatusId=1 and S.IsActive=1--Modified by Ruchira Patil on 29th April 2016 (fetch the uploaded stock count)
	WHERE CCP.ConsumerId = @BranchId AND CCP.ConsumerType = 1 -- FOR DEALERS 
	GROUP BY P.Name , CCP.ExpiryDate ,CCP.Points 

	--To fetch multiple Top up details
	SELECT P.Name TopUpType,CPT.ExpiryDate TopUpExpDate
	FROM ConsumerPackageTopUp CPT WITH (NOLOCK)
	JOIN Packages P WITH (NOLOCK) ON P.Id = CPT.PackageId
	WHERE CPT.DealerId = @BranchId AND CPT.Status = 1 --Active Top ups
END

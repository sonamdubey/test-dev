IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_GetDealerBuyerAndSellerPackageLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_GetDealerBuyerAndSellerPackageLeads]
GO

	-- =============================================
-- Author	:	Sachin Bharti(10th July 2015)
-- Description	:	Get seller and buyer leads for Automated Process VerifiedListingAlerts
-- Modified By: Deepak on 17th June 2016. Added Dealers Table in Join and put dealer active constraint
-- =============================================
CREATE PROCEDURE [dbo].[AP_GetDealerBuyerAndSellerPackageLeads]
	
	@DealerId	INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		DECLARE  @ExpiryDate DATETIME = GETDATE()-1
		
		
		IF NOT EXISTS(SELECT Id FROM CWCTDealerMapping WITH (NOLOCK)  WHERE CWDealerID = @DealerId AND ISNULL(IsMigrated,0) = 1)--@@ROWCOUNT = 0 -- CarWale Dealer
			BEGIN
				--get buyer leads
				SELECT 
					TOP 1 @ExpiryDate = CP.ExpiryDate 
				From 
					ConsumerCreditPoints CP WITH (NOLOCK) INNER JOIN Dealers D WITH (NOLOCK) ON CP.ConsumerId = D.ID AND CP.ConsumerType = 1
				WHERE 
					CP.PackageType IN(19,29) AND --fro Dealer Maximizer and Dealer Premium Package 
					D.Status = 0 AND --Modified By: Deepak on 17th June 2016. Added Dealers Table in Join and put dealer active constraint
					CP.ConsumerId = @DealerId
				
				IF CONVERT(DATE,@ExpiryDate) < CONVERT(DATE,GETDATE())
					BEGIN
						--get seller leads
						SELECT 
							TOP 1 @ExpiryDate = RVN.PackageEndDate 
						FROM 
							RVN_DealerPackageFeatures RVN WITH (NOLOCK)
							INNER JOIN Packages PK WITH (NOLOCK) ON PK.Id = RVN.PackageId
							INNER JOIN Dealers D WITH (NOLOCK) ON RVN.DealerId = D.ID
						WHERE 
							RVN.DealerId = @DealerId AND
							PK.InqPtCategoryId = 23 AND --for seller leads
							RVN.PackageStatus = 2 -- should be active package
							AND D.Status = 0 -- Modified By: Deepak on 17th June 2016. Added Dealers Table in Join and put dealer active constraint
						ORDER BY 
							RVN.PackageEndDate DESC 
					END
			END
		ELSE -- CTE Mifrated Dealer
			BEGIN
				SELECT @ExpiryDate = CAT.EndDate 
				FROM CT_AddOnPackages CAT WITH (NOLOCK) 
				WHERE CAT.IsActive = 1 AND CAT.CWDealerId = @DealerId AND CAT.AddOnPackageId = 39 --Seller Inquiry Package
			END
			
		SELECT @ExpiryDate AS ExpiryDate
END

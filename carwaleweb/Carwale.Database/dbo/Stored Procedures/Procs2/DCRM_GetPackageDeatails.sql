IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetPackageDeatails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetPackageDeatails]
GO

	-- =============================================
-- Author		:	Sachin Bharti
-- Create date	:	25th Nov 2013
-- Description	:	Get all packages for DealerList
-- Modifier		:	Sachin Bharti(12th Aug 2014)
-- Purpose		:	Commented date conversion method and added new
-- Modifier		:	Vaibhav K 31 Aug 2016 changed where clause for date comparison
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetPackageDeatails]
	
AS
	BEGIN
		SELECT DISTINCT 
			(
				SELECT top 1 Pkg.Name Package FROM Packages Pkg(NOLOCK) 
				INNER JOIN ConsumerPackageRequests Cpr(NOLOCK) ON Pkg.Id = Cpr.PackageId 
				AND ConsumerType = 1 AND Cpr.IsActive = 1 AND Cpr.IsApproved = 1 
				AND Pkg.IsActive = 1 AND Cpr.ConsumerId = Ds.id  AND Pkg.InqPtCategoryId = Ccp.PackageType 
				AND Pkg.IsStockBased = 1 Order By Cpr.ID Desc
			) Package, 
			Ccp.ExpiryDate	, Ds.ID  ,
			(	SELECT PM.ModeName FROM ConsumerPackageRequests CP(NOLOCK) INNER JOIN PaymentModes PM(NOLOCK) ON
				CP.ConsumerId=Ds.ID AND CP.ID=(SELECT MAX(ID) FROM ConsumerPackageRequests(NOLOCK) WHERE ConsumerId=Ds.ID	) 
				AND CP.PaymentModeId=PM.Id
			)	AS PaymentMode 
            
		FROM Dealers Ds(NOLOCK) 
		INNER JOIN ConsumerCreditPoints Ccp(NOLOCK)  ON Ds.Id = Ccp.ConsumerId 	AND Ccp.ConsumerType = 1 AND Ds.Status = 0  
		AND Ds.Id Not IN(SELECT CarwaleDealerId From AutofriendDealerMap(NOLOCK) WHERE isActive = 1) 
		--AND CONVERT(VARCHAR, Ccp.ExpiryDate , 103) >= CONVERT(VARCHAR, GETDATE() , 103)
		--AND CONVERT(VARCHAR, Ccp.ExpiryDate , 111) >= CONVERT(VARCHAR, GETDATE() , 111) -- Vaibhav K 31 Aug 2016
		AND CONVERT(DATE,Ccp.ExpiryDate) >= CONVERT(DATE,GETDATE()) -- Vaibhav K 31 Aug 2016
		ORDER BY ExpiryDate ASC
	END


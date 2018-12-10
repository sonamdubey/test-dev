IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchPackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchPackageDetails]
GO
	CREATE PROCEDURE [dbo].[DCRM_FetchPackageDetails]
	
		@DealerId NUMERIC
	
AS
	
BEGIN

	DECLARE @SellCount NUMERIC
	DECLARE @LastUpdate DATETIME
	DECLARE @Package VARCHAR(50)
	DECLARE @ExpiryDate DATETIME
	DECLARE @EntryDate DATETIME
	DECLARE @SaleBO VARCHAR(50)
	DECLARE @SaleField VARCHAR(50)
	DECLARE @ServiceBO VARCHAR(50)
	DECLARE @ServiceField VARCHAR(50)
	DECLARE @PreSales VARCHAR(50)

	SELECT @SellCount=COUNT(ID), @LastUpdate = MAX(LASTUPDATED) FROM SellInquiries WHERE DealerId = @DealerId AND StatusId=1 AND 
	PackageExpiryDate >= GETDATE()


	SELECT 
	@Package=(SELECT top 1 Pkg.Name Package FROM Packages Pkg, ConsumerPackageRequests Cpr WHERE Pkg.Id = Cpr.PackageId  
	AND ConsumerType = 1 AND Cpr.IsActive = 1 AND Cpr.IsApproved = 1 AND 
	Cpr.ConsumerId = Ds.id AND Pkg.InqPtCategoryId = Ccp.PackageType AND Pkg.IsStockBased = 1 Order By Cpr.ID Desc) , 
	@ExpiryDate = Ccp.ExpiryDate  
	FROM Dealers Ds, ConsumerCreditPoints Ccp, Cities Ct 
	WHERE Ds.Id = Ccp.ConsumerId AND Ccp.ConsumerType = 1 AND Ds.CityId = Ct.Id AND Ds.Status = 0  
	AND Ds.Id Not IN(SELECT CarwaleDealerId From AutofriendDealerMap WHERE isActive = 1)  
	AND Ccp.ConsumerId = @DealerId AND Ccp.ExpiryDate  >= GETDATE()
	ORDER BY Ccp.ExpiryDate ASC

	SELECT @EntryDate = MAX(EntryDate) FROM ConsumerPackageRequests WHERE ConsumerId = @DealerId AND isActive = 1 

	SELECT @SaleBO = OU.UserName
	FROM 
	DCRM_ADM_UserDealers DAD  
	INNER JOIN OprUsers OU ON OU.Id = DAD.UserId
	WHERE DAD.DealerId = @DealerId AND DAD.RoleId = 2

	
	SELECT @SaleField = OU.UserName
	FROM 
	DCRM_ADM_UserDealers DAD  
	INNER JOIN OprUsers OU ON OU.Id = DAD.UserId
	WHERE DAD.DealerId = @DealerId AND DAD.RoleId = 3

	
	SELECT @ServiceBO = OU.UserName
	FROM 
	DCRM_ADM_UserDealers DAD 
	INNER JOIN OprUsers OU ON OU.Id = DAD.UserId
	WHERE DAD.DealerId = @DealerId AND DAD.RoleId = 4

	
	SELECT @ServiceField = OU.UserName
	FROM 
	DCRM_ADM_UserDealers DAD 
	INNER JOIN OprUsers OU ON OU.Id = DAD.UserId
	WHERE DAD.DealerId = @DealerId AND DAD.RoleId = 5
	
	--SELECT @PreSales = COUNT(DAU.RoleId)
	--FROM DCRM_ADM_UserRoles DAU INNER JOIN 
	--DCRM_ADM_UserDealers DAD ON DAU.UserId = DAD.UserId
	--WHERE DAD.DealerId = 5 AND DAU.RoleId = 1
	
	SELECT @PreSales = OU.UserName
	FROM 
	DCRM_ADM_UserDealers DAD 
	INNER JOIN OprUsers OU ON OU.Id = DAD.UserId
	WHERE DAD.DealerId = @DealerId AND DAD.RoleId = 1

	--SELECT @SellCount AS SellCount, 
	--@LastUpdate AS LastUpdate, @Package AS Package, @ExpiryDate AS ExpiryDate, 
	--@EntryDate AS EntryDate, @SaleBO AS SaleBO, @SaleField AS SaleField, @ServiceBO AS ServiceBO, 
	--@ServiceField AS ServiceField, @PreSales PreSales
	
	SELECT @SellCount AS SellCount,
	CAST(DAY(@LastUpdate) AS VARCHAR(2)) + ' ' + DATENAME(MM, @LastUpdate) + ',' + 
	RIGHT(CAST(YEAR(GETDATE()) AS VARCHAR(4)), 2)+ ' ' 
	+ LTRIM(RIGHT(CONVERT(VARCHAR,@LastUpdate,100),7)) As LastUpdate,
	@Package AS Package,
	CAST(DAY(@ExpiryDate) AS VARCHAR(2)) + ' ' + DATENAME(MM, @ExpiryDate) + ',' + 
	RIGHT(CAST(YEAR(@ExpiryDate) AS VARCHAR(4)), 2) AS ExpiryDate, 
	CAST(DAY(@EntryDate) AS VARCHAR(2)) + ' ' + DATENAME(MM, @EntryDate) + ',' + 
	RIGHT(CAST(YEAR(@EntryDate) AS VARCHAR(4)), 2) AS EntryDate,
	@SaleBO AS SaleBO, 
	@SaleField AS SaleField, 
	@ServiceBO AS ServiceBO, 
	@ServiceField AS ServiceField, 
	@PreSales PreSales
END



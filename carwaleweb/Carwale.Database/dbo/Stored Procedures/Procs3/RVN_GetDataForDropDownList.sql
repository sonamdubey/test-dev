IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetDataForDropDownList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetDataForDropDownList]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(8th Oct 2014)
-- Description	:	Get PackageStatus , States and Dealers list
--					for Revenue Generating System
-- Modified BY Ajay Singh(22 july 2015)
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetDataForDropDownList]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	--Get data for Package Status
	SELECT 
		RVN.PackageStatusID AS ID,RVN.Status AS Text 
	FROM 
		RVN_PackageStatus RVN(NOLOCK) 
	ORDER BY 
		RVN.Status

	--Get data for States
	SELECT 
		ID, NAME AS Text 
	FROM 
		States (NOLOCK) 
	WHERE 
		IsDeleted = 0 ORDER BY Name

	--Get data for Dealers those payments are approved once
	SELECT DISTINCT D.ID , D.Organization AS Text
	FROM RVN_DealerPackageFeatures RVN(NOLOCK) 
	INNER JOIN Dealers D(NOLOCK) ON RVN.DealerId = D.ID
	ORDER BY D.Organization
	
	--Get data for approved by
	SELECT 
		DISTINCT OU.Id AS Id,
		OU.UserName AS Text 
	FROM 
		RVN_DealerPackageFeatures RVN(NOLOCK) 
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id=RVN.ApprovedBy AND OU.IsActive=1
		ORDER BY
		Text

	--get product data
	SELECT 
		PK.Id,
		PK.Name AS Text
	FROM 
		Packages PK
	WHERE 
		PK.isActive = 1 AND PK.ForDealer = 1
		ORDER BY 
		Name

	--get dealer type
	EXECUTE [dbo].[DCRM_GetDealerType]

END
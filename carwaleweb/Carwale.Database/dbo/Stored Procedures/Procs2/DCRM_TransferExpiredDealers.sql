IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_TransferExpiredDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_TransferExpiredDealers]
GO

	
--Author	:	Sachin Bharti(4th April 2013)
--Purpose	:	Used to show Dealers whose packages are going to be 
--				expired in the selected date range
--Modifier	:	Sachin Bharti(29th Jan 2015)
--Purpose	:	Convert expiry date into DD/MM/YYYY format and commented CPR.ActualAmount <> 0
--[DCRM_TransferExpiredDealers]'1/1/2015','10/6/2015',null,3,null,29

CREATE PROCEDURE [dbo].[DCRM_TransferExpiredDealers]-- '1/29/2014','1/29/2015',
	@FromDate	DATETIME	,
	@ToDate		DATETIME	,
	@UserRoleId     INT = NULL,
	@FieldUserId	INT	= NULL	,
	@Type			INT = NULL,
	@PackageId		INT=NULL
AS
BEGIN
	SELECT DISTINCT D.Id DealerId, D.Organization, C.Name City, 
	DAR.Name AS Region , DSD.EntryDate , DSD.ClosingDate ,CCP.ExpiryDate AS ExpiryDate1,
	CONVERT(VARCHAR, CCP.ExpiryDate, 103) ExpiryDate, A.Name AS Area, S.Name AS State
	, ISNULL(DSD.Id,'0') AS SalesDealerId,ISNULL(DSD.ClosingProbability,'0') AS Stage,
	ISNULL((
		SELECT TOP 1 ISNULL(PK.Name,' ') + ':'+ CONVERT(VARCHAR(12),CPR.EntryDate,106)+':'+CONVERT(VARCHAR,CPR.ActualAmount) Package 
		FROM Packages PK WITH(NOLOCK) 
		INNER JOIN ConsumerPackageRequests CPR WITH(NOLOCK) ON PK.Id = CPR.PackageId 
		AND CPR.ConsumerType = 1 AND CPR.IsActive = 1 AND CPR.IsApproved = 1 AND PK.IsActive = 1 AND PK.InqPtCategoryId = CCP.PackageType 
		AND CPR.ConsumerId = D.ID --AND CPR.ActualAmount <> 0  Comented By Sachin Bharti(29th Jan 2015)
		Order By Cpr.ID Desc
	),'')	Package, 
	ISNULL((SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU INNER JOIN OprUsers OU ON OU.Id = DAU.UserId WHERE DAU.DealerId = D.Id AND DAU.RoleId = 3),'0') AS SalesField ,
	ISNULL((SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU INNER JOIN OprUsers OU ON OU.Id = DAU.UserId WHERE DAU.DealerId = D.ID AND DAU.RoleId = 5),'0') AS ServiceField, 
	(SELECT OU.UserName FROM DCRM_ADM_UserDealers DAU INNER JOIN OprUsers OU ON OU.Id = DAU.UserId WHERE DAU.DealerId = DSD.DealerId AND DAU.RoleId = 4) BackOffice , 
	(SELECT TOP 1 (CONVERT(VARCHAR,DC.CalledDate,101) + ':' + DC.Comments) FROM DCRM_Calls DC WITH(NOLOCK) WHERE DC.DealerId = D.ID AND ActionTakenId = 1 ORDER BY DC.CalledDate DESC) Comment 
	
	FROM Dealers D WITH(NOLOCK ) 
	INNER JOIN  ConsumerCreditPoints CCP WITH(NOLOCK )  ON D.ID = CCP.ConsumerId  AND ConsumerType = 1
	INNER JOIN  Cities C WITH(NOLOCK ) ON C.ID = D.CityId 
	INNER JOIN States AS S ON C.StateId = S.ID
	LEFT JOIN  DCRM_SalesDealer DSD WITH (NOLOCK) ON DSD.DealerId = D.ID AND DSD.PitchingProduct IN (30,31,32,33,34,-1) AND DSD.LeadStatus = 1 --'-1' used because if Dealer pushed into DCRM_SalesDealer it has package id -1 
	LEFT JOIN	DCRM_ADM_RegionCities DARC WITH(NOLOCK) ON DARC.CityId = C.ID 
	LEFT JOIN	DCRM_ADM_Regions DAR WITH(NOLOCK) ON DAR.Id = DARC.RegionId 
	LEFT JOIN	DCRM_ADM_UserDealers DAU WITH(NOLOCK) ON DAU.DealerId = D.ID 
	LEFT JOIN Areas AS A ON D.AreaId = A.ID
	
	
	WHERE (D.IsDealerActive = 1) 
    AND Ccp.ExpiryDate Between @FromDate  AND  @ToDate  
	AND (CCP.PackageType = @PackageId OR @PackageId IS NULL)
	AND (DAU.RoleId=@UserRoleId OR @UserRoleId IS NULL ) -- Added By Ajay Singh(7 OCT 2015) For User Role Purpose
	AND ( DAU.UserId = @FieldUserId OR @FieldUserId IS NULL)
    ORDER BY Organization
	
END

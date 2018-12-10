IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchCallAlertReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchCallAlertReport]
GO
	CREATE PROCEDURE [dbo].[DCRM_FetchCallAlertReport]
	
		@RegionId NUMERIC = NULL,
		@CityId NUMERIC = NULL,
		@User NUMERIC = NULL,
		@From DATETIME,
		@To DATETIME,
		@Roll NUMERIC 
AS
BEGIN

		SELECT ISNULL(tblCall.Organization, tblAlert.Organization) AS Organization,
		ISNULL(tblCall.ID, tblAlert.ID) AS DealerId,
		tblCall.TotalCall, tblCall.ProcessedCall, tblCall.PendingCall,
		tblAlert.TotalAlert, tblAlert.ProcessedAlert, tblAlert.PendingAlert FROM 
		(
		SELECT COUNT(DU.Id) AS TotalAlert, COUNT(DU.ActionDate) AS ProcessedAlert,
		SUM(CASE WHEN DU.ActionDate IS NULL THEN 1 ELSE 0 END) PendingAlert,
		D.ID, D.Organization
		FROM DCRM_UserAlerts DU
		INNER JOIN Dealers D ON DU.DealerId = D.ID
		INNER JOIN DCRM_ADM_RegionCities DAR ON D.CityId = DAR.CityId
		INNER JOIN DCRM_ADM_Regions R ON DAR.RegionId = R.Id
		INNER JOIN Cities C ON D.CityId = C.ID
		INNER JOIN DCRM_ADM_UserDealers DAU ON D.ID = DAU.DealerId AND DAU.RoleId = @Roll

		WHERE DAR.RegionId =COALESCE(@RegionId, DAR.RegionId) AND 
		D.CityId = COALESCE(@CityId, D.CityId)
		AND DU.UserId = COALESCE(@User, DU.UserId)
		--AND MONTH(DU.CreatedOn) = @Month AND YEAR(DU.CreatedOn)=@Year
		AND DU.CreatedOn BETWEEN @From AND @To
		
		GROUP BY D.ID , Organization

		) AS tblAlert
		FULL JOIN
		(
		SELECT COUNT(DC.ID) AS TotalCall, COUNT(DC.CalledDate) AS ProcessedCall,
		SUM(CASE WHEN DC.CalledDate IS NULL THEN 1 ELSE 0 END) AS PendingCall,
		D.ID, D.Organization
		FROM DCRM_Calls DC
		INNER JOIN Dealers D ON DC.DealerId = D.ID
		INNER JOIN DCRM_ADM_RegionCities DAR ON D.CityId = DAR.CityId
		INNER JOIN DCRM_ADM_Regions R ON DAR.RegionId = R.Id
		INNER JOIN Cities C ON D.CityId = C.ID
		INNER JOIN DCRM_ADM_UserDealers DAU ON D.ID = DAU.DealerId AND DAU.RoleId = @Roll

		WHERE DAR.RegionId = COALESCE(@RegionId, DAR.RegionId) AND 
		D.CityId = COALESCE(@CityId, D.CityId)
		AND DC.UserId = COALESCE(@User, DC.UserId)
		--AND MONTH(DC.CreatedOn)= @Month AND YEAR(DC.CreatedOn)=@Year 
		AND DC.CreatedOn BETWEEN @From AND @To
		
		GROUP BY D.ID , Organization

		) AS tblCall
		ON tblCall.ID = tblAlert.ID

		ORDER BY Organization

END


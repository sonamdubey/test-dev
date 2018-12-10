IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[OrphanDealerReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[OrphanDealerReport]
GO

	


--Summary							: Active and InActive Orphan Dealer
--Author							: Dilip V. 18-Jul-2012

CREATE PROCEDURE [DCRM].[OrphanDealerReport]
@IsInActive BIT
AS 
BEGIN
	SET NOCOUNT ON
	
	DECLARE @varsql	VARCHAR(MAX)
	
	SET @varsql = 'WITH CP AS
	(SELECT DISTINCT TOP 25 D.ID AS DealerId, D.Organization, C.Name AS City,D.CityId, DR.Name AS Region,
	(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU, OprUsers OU WHERE DealerId = D.ID AND RoleId = 2 AND OU.Id = DAU.UserId ORDER BY DAU.UpdatedOn DESC) AS SalesBO,
	(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU, OprUsers OU WHERE DealerId = D.ID AND RoleId = 3 AND OU.Id = DAU.UserId ORDER BY DAU.UpdatedOn DESC) AS SalesField,
	(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU, OprUsers OU WHERE DealerId = D.ID AND RoleId = 4 AND OU.Id = DAU.UserId ORDER BY DAU.UpdatedOn DESC) AS ServiceBO,
	(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU, OprUsers OU WHERE DealerId = D.ID AND RoleId = 5 AND OU.Id = DAU.UserId ORDER BY DAU.UpdatedOn DESC) AS ServiceField
	FROM ConsumerCreditPoints CCP, Dealers AS D
	LEFT JOIN  Cities AS C ON  D.CityId= C.ID
	LEFT JOIN DCRM_ADM_RegionCities AS DRC ON D.CityId = DRC.CityId
	LEFT JOIN DCRM_ADM_Regions AS DR ON DR.Id = DRC.RegionId
	WHERE CCP.ConsumerId = D.ID AND CCP.ConsumerType = 1'
	IF(@IsInActive = 0)
		SET @varsql += ' AND CCP.ExpiryDate >= Convert(VarChar, GETDATE(),101)'
	ELSE IF (@IsInActive = 1)
		SET @varsql += ' AND CCP.ExpiryDate < Convert(VarChar, GETDATE(),101)'
		
	SET @varsql += ' )SELECT DealerId, Organization,City,CityId,Region,SalesBO,SalesField,ServiceBO,ServiceField FROM CP WHERE SalesBO IS NULL OR SalesField IS NULL OR ServiceBO IS NULL OR ServiceField IS NULL'
	EXEC (@varsql);
END



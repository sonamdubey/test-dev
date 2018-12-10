IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DealerVisitsTrackerDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DealerVisitsTrackerDetail]
GO

	-- =============================================
-- Author	:	Sachin Bharti(18th Aug 2015)
-- Description	:	Field visits tracker dealer details
-- exec DCRM_DealerVisitsTrackerDetail 4,8,2015,86
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DealerVisitsTrackerDetail]
	@Type	SMALLINT,
	@Month	SMALLINT = NULL,
	@Year	SMALLINT	= NULL,
	@ExecutiveId	INT = NULL,
	@RegionId	SMALLINT = NULL,
	@StateId	INT = NULL,
	@CityId	INT = NULL,
	@AvgDelayDay	SMALLINT = NULL
AS
BEGIN
	--Get Paid Dealers
	IF @Type = 1
	BEGIN
		--first get active dealers based on running package
		SELECT   
			DAM.OprUserId,
			D.ID AS DealerId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name AS CityName,
			TD.DealerType,
			CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet
		FROM
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1 AND DAU.RoleId IN (3,5)
			INNER JOIN ConsumerCreditPoints CCP(NOLOCK) ON  CCP.ConsumerId = DAU.DealerId	AND CCP.ConsumerType=1 --for dealer
																							AND CCP.PackageType<>28 --exclude free listing
																							AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 --exclude expires dealers 
			INNER JOIN Dealers D(NOLOCK) ON D.ID = CCP.ConsumerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN DCRM_ADM_RegionCities DAR(NOLOCK) ON DAR.CityId = C.ID
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			LEFT JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.ActionTakenBy = DAM.OprUserId AND DSM.DealerId = DAU.DealerId
		WHERE
			(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
			AND(@RegionId IS NULL OR DAR.RegionId = @RegionId)
			AND(@CityId IS NULL OR DAR.CityId = @CityId)
		GROUP BY
			DAM.OprUserId,
			D.ID,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name,
			TD.DealerType

		UNION 

		--get dealers having warranty credit amount
		SELECT   
			DAM.OprUserId,
			D.ID AS DealerId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name AS CityName,
			TD.DealerType,
			CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet
		FROM
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1
																					  AND DAU.RoleId IN (3,5)
																					  AND (@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
			INNER JOIN AbSure_Trans_Credits AS AC ON AC.DealerId=DAU.DealerId AND AC.CreditAmount > 0
			INNER JOIN Dealers D(NOLOCK) ON D.ID = AC.DealerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN DCRM_ADM_RegionCities DAR(NOLOCK) ON DAR.CityId = C.ID
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			LEFT JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.ActionTakenBy = DAM.OprUserId AND DSM.DealerId = DAU.DealerId
		WHERE
			(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
		GROUP BY
			DAM.OprUserId,
			D.ID,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name,
			TD.DealerType
		ORDER BY 
			D.Organization
		
	END
	--Free Dealers
	ELSE IF @Type = 2
	BEGIN
		SELECT   
			DISTINCT D.ID AS DealerId,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name AS CityName,
			TD.DealerType,
			CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet
		FROM
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1 AND DAU.RoleId IN (3,5)
			INNER JOIN ConsumerCreditPoints CCP(NOLOCK) ON  CCP.ConsumerId = DAU.DealerId	AND CCP.ConsumerType=1 --for dealer
																							AND CCP.PackageType=28 --exclude free listing
			INNER JOIN Dealers D(NOLOCK) ON D.ID = CCP.ConsumerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN DCRM_ADM_RegionCities DAR(NOLOCK) ON DAR.CityId = C.ID
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			LEFT JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.ActionTakenBy = DAM.OprUserId AND DSM.DealerId = DAU.DealerId
		WHERE
			(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
			AND(@RegionId IS NULL OR DAR.RegionId = @RegionId)
			AND(@CityId IS NULL OR DAR.CityId = @CityId)
		GROUP BY
			D.ID,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name,
			TD.DealerType
	END
	--Meeting Dealers
	ELSE IF @Type = 3
	BEGIN
		SELECT 
			DISTINCT D.ID AS DealerId,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name AS CityName,
			TD.DealerType,
			CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet
		FROM 
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DAM.Id 
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1 AND DAU.RoleId IN (3,5)
			INNER JOIN Dealers D(NOLOCK) ON D.ID = DAU.DealerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			INNER JOIN DCRM_ADM_RegionCities DAR(NOLOCK) ON DAR.CityId = C.ID
			INNER JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.DealerId = DAU.DealerId	AND DSM.ActionTakenBy = DAU.UserId 
																					AND DAU.RoleId IN (3,5)
																					AND MONTH(DSM.MeetingDate) = @Month
																					AND YEAR(DSM.MeetingDate) = @Year
		WHERE
			(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
			AND(@RegionId IS NULL OR DAR.RegionId = @RegionId)
			AND(@CityId IS NULL OR DAR.CityId = @CityId)
		GROUP BY
			D.ID,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name,
			TD.DealerType
	END
	--unattended dealers
	ELSE IF @Type = 4
	BEGIN
		SELECT 
			DISTINCT D.ID AS DealerId,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name AS CityName,
			TD.DealerType,
			CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet
		FROM 
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN OprUsers OU(NOLOCK) ON DAM.OprUserId = OU.Id
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1 AND DAU.RoleId IN (3,5)
			INNER JOIN Dealers D(NOLOCK) ON D.ID = DAU.DealerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			INNER JOIN DCRM_ADM_RegionCities DAR(NOLOCK) ON DAR.CityId = C.ID
			LEFT JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.DealerId = DAU.DealerId	AND DSM.ActionTakenBy = DAU.UserId 
																					AND DAU.RoleId IN (3,5)
																					AND MONTH(DSM.MeetingDate) = @Month
																					AND YEAR(DSM.MeetingDate) = @Year
		WHERE
			DSM.DealerId IS NULL
			AND OU.IsActive = 1
			AND(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
			AND(@RegionId IS NULL OR DAR.RegionId = @RegionId)
			AND(@CityId IS NULL OR DAR.CityId = @CityId)
		GROUP BY
			D.ID,
			DAM.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			C.Name,
			TD.DealerType
	END
	--average delay days 
	ELSE IF @Type = 5
	BEGIN
		SELECT 
			DISTINCT T.MeetingDealer AS DealerId,
			T.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			T.CityName,
			TD.DealerType,
			T.LastMeet,
			(
			ISNULL(SUM(T.DaySinceLastMeet),0)
			/
			CASE WHEN (SUM(CASE WHEN (T.MeetingDealer IS NOT NULL) THEN 1 ELSE 0 END)) = 0 THEN 1 
			ELSE (SUM(CASE WHEN (T.MeetingDealer IS NOT NULL) THEN 1 ELSE 0 END)) END
			) AS AverageDelay
		FROM
			(SELECT 
				DISTINCT DAM.OprUserId,
				DSM.DealerId AS MeetingDealer,
				C.Name AS CityName,
				CONVERT(VARCHAR(30),MAX(DSM.MeetingDate),106) AS LastMeet,
				ISNULL(DATEDIFF(DAY,MAX(DSM.MeetingDate),GETDATE()),0) AS DaySinceLastMeet
			FROM
				DCRM_ADM_MappedUsers DAM(NOLOCK)
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId	AND DAM.IsActive = 1 
																							AND DAU.RoleId IN (3,5)--for sales and service field executives
																							AND DAM.BusinessUnitId = 1 --for UCD users only
				INNER JOIN Dealers D(NOLOCK) ON D.Id = DAU.DealerId
				INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
				INNER JOIN DCRM_ADM_RegionCities RC(NOLOCK) ON RC.CityId = C.ID
				LEFT JOIN DCRM_SalesMeeting DSM ON DSM.DealerId = DAU.DealerId AND DSM.ActionTakenBy = DAU.Userid
				LEFT JOIN DCRM_SalesMeeting DSM1 ON DSM1.DealerId = DAU.DealerId AND DSM1.ActionTakenBy = DAU.Userid AND MONTH(DSM1.MeetingDate) = @Month AND YEAR(DSM1.MeetingDate) = @Year
			WHERE
				(@ExecutiveId IS NULL OR DAM.OprUserId = @ExecutiveId)AND
				(@RegionId IS NULL OR RC.RegionId = @RegionId) AND 
				(@StateId IS NULL OR C.StateId = @StateId) AND
				(@CityId IS NULL OR C.Id = @CityId)
			GROUP BY
				DAM.OprUserId,
				DSM.DealerId,
				DAU.UserId,
				C.Name)AS T
			INNER JOIN Dealers D(NOLOCK) ON D.ID = T.MeetingDealer
			INNER JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId
			INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = T.OprUserId
			INNER JOIN DCRM_ADM_MappedUsers DA(NOLOCK) ON DA.OprUserId = T.OprUserId
		WHERE
			OU.IsActive = 1
		GROUP BY
			T.MeetingDealer,
			T.OprUserId,
			D.Organization,
			D.MobileNo,
			D.EmailId,
			T.CityName,
			T.LastMeet,
			TD.DealerType
		ORDER BY
			Organization
	END
END

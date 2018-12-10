IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealerVisitsTrackerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealerVisitsTrackerData]
GO

	
-- =============================================
-- Author	:	Sachin Bhart(5th Aug 2015)
-- Description	:	Get number of meetings done , total free and paid dealers
-- Execute [dbo].[DCRM_GetDealerVisitsTrackerData] 8,2015,null,1,null,null
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealerVisitsTrackerData]
	@Month	SMALLINT,
	@Year	SMALLINT,
	@ExecutiveId	INT = NULL,
	@RegionId	SMALLINT = NULL,
	@StateId	SMALLINT = NULL,
	@CityId		SMALLINT = NULL
AS
BEGIN
	
	--get total meetings and dealer not met for more than 10 days
	SELECT 
		T.OprUserId,
		OU.UserName,
		DA.UserLevel,
		DA.NodeCode,
		SUM(T.TotalMeeting)AS TotatMeeting,
		COUNT(T.UserDealer) - SUM(CASE WHEN T.MeetingDealer IS NULL THEN 0 ELSE 1 END) AS DealersNotMet,
		(
			SUM(T.DaySinceLastMeet)/
			CASE WHEN (SUM(CASE WHEN T.MeetingDealer IS NULL THEN 0 ELSE 1 END)) = 0 THEN 1 
			ELSE (SUM(CASE WHEN T.MeetingDealer IS NULL THEN 0 ELSE 1 END)) END
		) AS AverageDelay
	FROM
		(SELECT 
			DISTINCT DAM.OprUserId,
			DAU.DealerId AS UserDealer,
			DSM.DealerId AS MeetingDealer,
			COUNT(DISTINCT  CASE WHEN  MONTH(DSM.MeetingDate) = @Month AND YEAR(DSM.MeetingDate) = @Year  THEN DSM.ID END) TotalMeeting,
			MAX(MeetingDate) AS LastMeet,
			ISNULL(DATEDIFF(DAY,MAX(MeetingDate),GETDATE()),0) AS DaySinceLastMeet
		FROM
			DCRM_ADM_MappedUsers DAM(NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId	AND DAM.IsActive = 1 
																						AND DAU.RoleId IN (3,5)--for sales and service field executives
																						AND DAM.BusinessUnitId = 1 --for UCD users only
			INNER JOIN Dealers D(NOLOCK) ON D.Id = DAU.DealerId
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			INNER JOIN DCRM_ADM_RegionCities RC(NOLOCK) ON RC.CityId = C.ID
			LEFT JOIN DCRM_SalesMeeting DSM ON DAU.DealerId = DSM.DealerId AND DSM.ActionTakenBy = DAU.Userid
		WHERE
			(@ExecutiveId IS NULL OR DAM.OprUserId = @ExecutiveId)AND
			(@RegionId IS NULL OR RC.RegionId = @RegionId) AND 
			(@StateId IS NULL OR C.StateId = @StateId) AND
			(@CityId IS NULL OR C.Id = @CityId)
		GROUP BY
			DAM.OprUserId,
			DAU.DealerId,
			DAU.DealerId,
			DSM.DealerId,
			DAU.UserId)AS T
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = T.OprUserId
		INNER JOIN DCRM_ADM_MappedUsers DA(NOLOCK) ON DA.OprUserId = T.OprUserId
	WHERE
		T.DaySinceLastMeet < 10
	GROUP BY
		T.OprUserId,
		OU.UserName,
		DA.UserLevel,
		DA.NodeCode
	ORDER BY
		DA.NodeCode
	
	--get paid and free dealers
	SELECT   
		DAM.OprUserId,
		(COUNT(	CASE WHEN ( CCP.PackageType<>28 AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 ) THEN CCP.ID END) +COUNT(AC.ID)) AS PaidDealer,
		COUNT(	CASE WHEN CCP.PackageType = 28 THEN CCP.ID END) FreeDealer
	FROM
		DCRM_ADM_MappedUsers DAM(NOLOCK)
		INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAM.OprUserId = DAU.UserId AND DAM.IsActive = 1 AND DAU.RoleId IN (3,5)
		LEFT JOIN ConsumerCreditPoints CCP(NOLOCK) ON  CCP.ConsumerId = DAU.DealerId AND CCP.ConsumerType=1
		LEFT JOIN AbSure_Trans_Credits AS AC ON AC.DealerId=DAU.DealerId AND AC.CreditAmount > 0 AND CCP.ConsumerId<>AC.DealerId
	WHERE
		(@ExecutiveId IS NULL OR DAU.UserId =@ExecutiveId)
	GROUP BY 
		DAM.OprUserId
END


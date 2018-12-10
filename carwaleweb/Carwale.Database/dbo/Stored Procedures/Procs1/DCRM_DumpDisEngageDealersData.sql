IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DumpDisEngageDealersData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DumpDisEngageDealersData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(8th Jan 2014)
-- Description	:	Dump data of DisEngageDealers into table DCRM_DisEngagedDealers
-- Modifier		:	Sachin Bharti(3rd April 2014) Only active Dealers dump
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DumpDisEngageDealersData]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	TRUNCATE TABLE DCRM_DisEngagedDealers
	
	INSERT INTO DCRM_DisEngagedDealers(DealerId,IsGetResponse,IsCarUploaded,IsSuccessfullFieldVisit,IsCallConnected,IsTillDayRenewal,IsTillDayToNextMonthRenewal,IsLastMonthRenewal)

	SELECT DISTINCT DC.DealerId,
		ISNULL((SELECT TOP 1  1 FROM  UsedCarPurchaseInquiries  AS UCPI WITH (NOLOCK), SellInquiries SI WITH (NOLOCK)
				WHERE UCPI.SellInquiryId = SI.ID AND SI.DealerId=DC.DealerId  AND DateDiff(DAY,UCPI.RequestDateTime, GetDate()) <= 7  ),0) IsResponseReceived,
		ISNULL((SELECT TOP 1  1 FROM SellInquiries   AS SI  WITH (NOLOCK)
				WHERE SI.Dealerid=DC.DealerId AND DateDiff(DAY, SI.LastUpdated, GetDate()) <= 7  ),0) IsCarUploaded,
		ISNULL((SELECT TOP 1  1 FROM  DCRM_SalesMeeting DSM  WITH (NOLOCK)  
				WHERE DSM.Dealerid=DC.DealerId AND DateDiff(DAY, DSM.ActionTakenOn, GetDate()) <= 15 AND DSM.MeetDecisionMaker =1),0) IsMeetDealer,
		ISNULL((SELECT TOP 1  1 FROM  DCRM_Calls DC1 WITH (NOLOCK)  
				WHERE DC1.Dealerid=DC.DealerId AND DateDiff(DAY, DC1.CalledDate, GetDate()) <= 15 AND DC1.CallStatus = 1 AND DC1.ActionTakenId = 1),0) IsCallConnected,
		ISNULL((select top 1 1
				from ConsumerCreditPoints ccp(nolock)
				where ccp.ConsumerId = dc.DealerId and 0 <= DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101)) and DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101)) <= DAY(GETDATE())),0) IsEarlyRenewal,
		ISNULL((select top 1 1
				from ConsumerCreditPoints ccp(nolock)
				where ccp.ConsumerId = dc.DealerId and DAY(GETDATE()) < DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101))  and DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101)) < (DAY(GETDATE())+30)),0) IsEarlyRenewal,
		ISNULL((select top 1 1
				from ConsumerCreditPoints ccp(nolock)
				where ccp.ConsumerId = dc.DealerId and -30 < DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101)) and DATEDIFF(DAY, CONVERT(VARCHAR(10), GETDATE(), 101), CONVERT(VARCHAR(10), ccp.ExpiryDate, 101)) < 0),0) IsEarlyRenewal
	FROM	DCRM_Calls DC WITH (NOLOCK)
	INNER	JOIN Dealers D WITH (NOLOCK) ON DC.dealerid = D.ID AND D.Status = 0 -- For active Dealers only
	INNER	JOIN DCRM_ADM_UserRoles DAU WITH (NOLOCK) ON  DAU.UserId = DC.UserId
	WHERE	CONVERT(DATE,DC.ScheduleDate) <= CONVERT(DATE,GETDATE()) and DC.ActionTakenId = 2  AND DAU.RoleId = 4 --For Back office users only
END


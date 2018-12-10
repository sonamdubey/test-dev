IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsForNewCarActionsBikeWale]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsForNewCarActionsBikeWale]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-04-2015
-- Description:	Reward Calculations For New Car Actions BikeWale
-- Modified By : Nilima More on 19th OCT 2015, reward points should be given only on CarWale leads.
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- Modified By : Ashwini Dhamankar on Aug 24,2016 (Modified logic)
--exec [dbo].[TC_RewardsForNewCarActionsBikeWale] 4,2,'2016-08-29 00:00:00.000',13287
-- exec [dbo].[TC_RewardsForNewCarActionsBikeWale]  10255,2,'2016-09-11',16942
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsForNewCarActionsBikeWale] 
@BranchId INT,
@TC_DealerTypeId INT,
@Date DATETIME,
@UserId INT
AS
BEGIN
	DECLARE @EntryDate DATETIME
	SET @EntryDate = CONVERT(DATE,@Date)

	DECLARE @TotalPoints INT = 0

	CREATE TABLE #Temp
	(
		TC_LeadId INT,
		CallCount INT
	)
 

	INSERT INTO #Temp
	SELECT C.TC_LeadId, SUM(CASE WHEN C.ActionTakenOn <= BookingEventDate THEN 1 ELSE 0 END) AS CallCount
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead TCIL WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =  NCI.TC_InquiriesLeadId
		INNER JOIN TC_InquirySource S WITH(NOLOCK) ON NCI.TC_InquirySourceId = S.Id
		LEFT JOIN TC_Calls C WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId 
	WHERE 
		TCIL.BranchId = @BranchId AND TCIL.TC_UserId = @UserId
		AND DATEDIFF(dd,NCI.BookingEventDate,@Date) = 0
		AND NCI.BookingStatus = 32 
		AND (S.TC_InquiryGroupSourceId = 12 OR S.Id = 6)
		
	GROUP BY C.TC_LeadId
	
	SELECT  SUM(CASE WHEN CallCount <= 1 THEN 25 ELSE (25+((CallCount-1)*5)) END) AS TotalPoints
	INTO #RewardPoints
	FROM #Temp
	GROUP BY TC_LeadId
	
	SELECT @TotalPoints = SUM(CASE WHEN TotalPoints >= 40 THEN 40 ELSE TotalPoints END)
	FROM #RewardPoints 
	
	DROP table #TEMP
	DROP table #RewardPoints

			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = 22,
										@RewardPoints = 25,    
										@TotalRewardsFromWeb = @TotalPoints,
										@TotalRewardsFromApp = 0,
										@TotalRewardsToSM = 0,
										@UserId = @UserId

	


END

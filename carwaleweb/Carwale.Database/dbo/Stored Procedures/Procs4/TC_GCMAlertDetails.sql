IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GCMAlertDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GCMAlertDetails]
GO

	-- Created By: Umesh Ojha on 30 sep 2013
-- Description: For sending daily alert for Today and month till today.
--============================================================
CREATE PROCEDURE  [dbo].[TC_GCMAlertDetails]  

AS 
BEGIN
	
	DECLARE @UserId AS BIGINT
	DECLARE @NumberRecords AS INT
	DECLARE @RowCount AS INT
	DECLARE @GCMRegistrationId AS VARCHAR(MAX)

	--Get All Users where GCM Id is available
	DECLARE @TempGCMUsers Table(RowID INT IDENTITY(1, 1), UserId NUMERIC, GCMRegistrationId VARCHAR(MAX))
	INSERT INTO @TempGCMUsers
	SELECT Id, GCMRegistrationId FROM TC_Users WHERE IsActive = 1 AND GCMRegistrationId IS NOT NULL
	SET @NumberRecords = @@ROWCOUNT
	SET @RowCount = 1

	--Fetch Data fro each of them one by on
	WHILE @RowCount <= @NumberRecords
		BEGIN
			--Get User
			SELECT @UserId = UserId,  @GCMRegistrationId = GCMRegistrationId FROM @TempGCMUsers WHERE RowID = @RowCount

			--Get Reporting users if available
			DECLARE @TblChildUsers TABLE (UserId INT)
			INSERT INTO @TblChildUsers EXEC TC_GetALLChild @UserId -- get all users reporting to logged in user

			--Fetch Data and save
			INSERT INTO TC_GCMDailyAlertSendLog
			SELECT 
					@UserId,
					SUM (CurrentDayInquiryCount) AS CurrentDayInquiryCount ,
					SUM (CurrentMonthInquiryCount) AS CurrentMonthInquiryCount,
					SUM (CurrentDayBookingCount) AS CurrentDayBookingCount,
					Sum (CurrentMonthBookingCount) AS CurrentMonthBookingCount,
					SUM (CurrentDayLostCount) AS CurrentDayLostCount,
					SUM (CurrentMonthLostCount) AS CurrentMonthLostCount,
					SUM (PendingFollowup) AS PendingFollowup,
					SUM (TomorrowFollowup) AS TomorrowFollowup,
					TC_LeadInquiryTypeId,
					GETDATE(),
					@GCMRegistrationId

			
			FROM TC_UsedCarDealerDailyAlert
			WHERE TC_UsersId in (SELECT UserId FROM @TblChildUsers) OR TC_UsersId = @UserId
			GROUP BY TC_LeadInquiryTypeId

			SET @RowCount = @RowCount + 1
		END
		 
		SELECT CurrentDayInquiryCount,CurrentMonthInquiryCount,CurrentDayBookingCount,CurrentMonthBookingCount,
	                                      CurrentDayLostCount,CurrentMonthLostCount,PendingFollowup,TomorrowFollowup,TC_LeadInquiryTypeId, TC_UsersId, GCMRegistrationId
		FROM TC_GCMDailyAlertSendLog
		WHERE CONVERT(DATE,SendTime) = CONVERT(DATE,GETDATE())

END 

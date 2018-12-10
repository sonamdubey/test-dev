IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetUserActivitiesLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetUserActivitiesLog]
GO

	

-- =============================================
-- Author:		Satish Sharma
-- Create date: 18-12-2013
-- Description:	To get user activity log
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetUserActivitiesLog] -- exec AxisBank_GetUserActivitiesLog 31,1,'2014-1-1', '2014-1-15'
	-- Add the parameters for the stored procedure here
	@UserId INT
	,@ActivityTypeId SMALLINT
	,@ActivityFrom DATETIME
	,@ActivityTo DATETIME
	,@StartIndex INT = 1
	,@EndIndex INT = 10
	,@AllRecords Bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @AllRecords = 0
	BEGIN
	IF @ActivityTypeId = 1
	BEGIN
		WITH ActivityLogs
		AS (
			SELECT U.LoginId
				,U.FirstName + ' ' + U.LastName AS UserName
				,CV.RemoteHost AS ClientIP
				,'Valuation' AS ActivityName
				,CV.FileReferenceNumber
				,CV.RegistrationNumber
				,CV.ValueGood
				,CV.ValueFair
				,CV.ValueExcellent
				,CV.CarCondition
				,CV.RequestDateTime as ActivityDateTime
				,CV.City
				,AA.NAME
				,ROW_NUMBER() OVER (
					PARTITION BY U.LoginId ORDER BY CV.RequestDateTime ASC
					) AS Rno
			FROM AxisBank_Users U
			INNER JOIN AxisBank_CarValuations CV ON Cv.Customerid = u.userid
			INNER JOIN AxisBank_ASC AA ON CV.ASC_Id = AA.ID
			WHERE U.UserId = @UserId
				AND CV.RequestDateTime BETWEEN @ActivityFrom
					AND @ActivityTo
			)
		SELECT *
		FROM ActivityLogs
		WHERE Rno >= @StartIndex
			AND Rno <= @EndIndex
			

		--ORDER BY ActivitydateTime DESC
		-- Get total record count
		SELECT Count(*)
		FROM AxisBank_Users U
		INNER JOIN AxisBank_CarValuations CV ON Cv.Customerid = u.userid
		INNER JOIN AxisBank_ASC AA ON CV.ASC_Id = AA.ID
		WHERE U.UserId = @UserId
			AND CV.RequestDateTime BETWEEN @ActivityFrom
				AND @ActivityTo
	END
	ELSE
	BEGIN
		WITH ActivityLogs
		AS (
			SELECT AL.ActivityId
				,U.LoginId
				,U.FirstName + ' ' + U.LastName AS UserName
				,AL.ClientIP
				,AT.ActivityName
				,'' AS FileReferenceNumber
				,'' AS RegistrationNumber
				,'' AS ValueGood
				,Al.ActivityDateTime
				,'' AS City
				,'' AS NAME
				,ROW_NUMBER() OVER (
					PARTITION BY U.UserId ORDER BY Al.ActivityDateTime ASC  
					) AS Rno
			FROM AxisBank_UserActivitiesLog AL
			INNER JOIN AxisBank_Users U ON U.UserId = AL.UserId
			INNER JOIN AxisBank_UserActivityType AT ON AT.ActivityId = Al.ActivityTypeId
			WHERE U.UserId = @UserId
				AND (
					AL.ActivityTypeId = @ActivityTypeId
					--OR AL.ActivityTypeId IS NULL
					)
				AND Al.ActivityDateTime BETWEEN @ActivityFrom
					AND @ActivityTo
			)
		SELECT *
		FROM ActivityLogs
		WHERE Rno >= @StartIndex
			AND Rno <= @EndIndex

		-- Get total record count
		SELECT Count(*)
		FROM AxisBank_UserActivitiesLog AL
		INNER JOIN AxisBank_Users U ON U.UserId = AL.UserId
		INNER JOIN AxisBank_UserActivityType AT ON AT.ActivityId = Al.ActivityTypeId
		WHERE U.UserId = @UserId
			AND (
				AL.ActivityTypeId = @ActivityTypeId
				--OR AL.ActivityTypeId IS NULL
				)
			AND Al.ActivityDateTime BETWEEN @ActivityFrom
				AND @ActivityTo
	END
	END
	ELSE
	BEGIN
	IF @ActivityTypeId = 1
	BEGIN
		WITH ActivityLogs
		AS (
			SELECT U.LoginId
				,U.FirstName + ' ' + U.LastName AS UserName
				,CV.RemoteHost AS ClientIP
				,'Valuation' AS ActivityName
				,CV.FileReferenceNumber
				,CV.RegistrationNumber
				,CV.ValueGood
				,CV.ValueFair
				,CV.ValueExcellent
				,CV.CarCondition
				,CV.RequestDateTime as ActivityDateTime
				,AA.NAME 
				
			FROM AxisBank_Users U
			INNER JOIN AxisBank_CarValuations CV ON Cv.Customerid = u.userid
			INNER JOIN AxisBank_ASC AA ON CV.ASC_Id = AA.ID
			WHERE U.UserId = @UserId
				AND CV.RequestDateTime BETWEEN @ActivityFrom
					AND @ActivityTo
			)
		SELECT *
		FROM ActivityLogs
	END
	ELSE
	BEGIN
		WITH ActivityLogs
		AS (
			SELECT U.LoginId
				,U.FirstName + ' ' + U.LastName AS UserName
				,AL.ClientIP
				,AT.ActivityName
				,Al.ActivityDateTime
				
			FROM AxisBank_UserActivitiesLog AL
			INNER JOIN AxisBank_Users U ON U.UserId = AL.UserId
			INNER JOIN AxisBank_UserActivityType AT ON AT.ActivityId = Al.ActivityTypeId
			WHERE U.UserId = @UserId
				AND (
					AL.ActivityTypeId = @ActivityTypeId
					)
				AND Al.ActivityDateTime BETWEEN @ActivityFrom
					AND @ActivityTo
			)
		SELECT *
		FROM ActivityLogs
		
	END
	END
END



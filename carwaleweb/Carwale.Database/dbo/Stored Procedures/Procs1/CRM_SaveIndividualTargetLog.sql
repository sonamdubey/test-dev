IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveIndividualTargetLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveIndividualTargetLog]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 31st July 2014
-- Description:	An automated SP which will dump all the rows from CRM_IndividulaTarget in CRM_TargetLog and also update the actual leads assigned and processed for yesterday.
-- Modifier:	Vaibhav K 8 Aug 2014
--				Added updation in CRM_TargetLog for excluding target(set Value = 0) on the day of roaster i.e. first day of month without any assignment
--				Vaibhav K 14 Aug 2014 (Added date parameters to avoid cast and functions on date columns)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveIndividualTargetLog]
	
AS
BEGIN
	--First day of the current month
	DECLARE @CurrentDate DATE = CAST(DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AS DATE)
	DECLARE @TempSunday INT = DATEPART(DW, GETDATE())
	
	--Perform the insertion and updation only if its not a holiday
	IF NOT EXISTS (SELECT Holiday FROM CRM_HolidayList WITH (NOLOCK) WHERE HOLIDAY = @CurrentDate)
	BEGIN
		IF @TempSunday !=1
		BEGIN
			--Insertion in the CRM_TargetLog table for today
			INSERT INTO CRM_TargetLog(UserId,Brand,Type,Date,Value,ActionTakenBy,ActionTakenOn,ActualLeads)
			SELECT UserId,Brand,Type,GETDATE(),Value,CreatedBy,GETDATE(),0 FROM CRM_IndividualTarget 
			WHERE Date = @CurrentDate
		END
	END

	--Vaibhav K 14 Aug 2014 added dates that are used for below queries
	DECLARE @PrevDayStart DATETIME, @PrevDayEnd DATETIME
	SET @PrevDayStart = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE()-1, 120) + ' 00:00:00')
	SET @PrevDayEnd = CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE()-1,120)+ ' 23:59:59')

	--Updation of ActualLeads column for leads processed for yesterday
	UPDATE CTL
	SET CTL.ActualLeads = ISNULL(A.LeadProcessed,0)
	FROM CRM_TargetLog CTL
	LEFT JOIN 
		(SELECT OU.Id AS UserId, COUNT(DISTINCT CC.LeadId) AS LeadProcessed
		FROM CRM_Calls CC WITH (NOLOCK)
		JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CC.LeadId = CBD.LeadId
		JOIN vwMMV VM WITH (NOLOCK) ON VM.VersionId = CBD.VersionId
		JOIN OprUsers OU WITH (NOLOCK) ON CC.ActionTakenBy = OU.Id
		--JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Brand = VM.MakeId AND CIT.Type = 4 AND CIT.UserId = OU.Id
		JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Type = 4 AND CIT.UserId = OU.Id
		WHERE CC.CallType=1 AND CC.IsActionTaken = 1 AND CC.ActionTakenBy NOT IN (13,651)
		--AND CC.ActionTakenOn = GETDATE() - 1
		AND CC.ActionTakenOn BETWEEN @PrevDayStart AND @PrevDayEnd
		GROUP BY OU.Id) A 
	ON A.UserId = CTL.UserId
	WHERE 
	CTL.Date = CAST(GETDATE() - 1 AS DATE) AND 
	CTL.Type = 4 AND CTL.IsDeleted = 0

	--Updation of ActualLeads column for leads assigned for yesterday
	UPDATE CTL
	SET CTL.ActualLeads = ISNULL(A.LeadAssigned,0)
	FROM CRM_TargetLog CTL
	LEFT JOIN
		(SELECT OU.Id AS UserId, COUNT(DISTINCT CBD.LeadId) AS LeadAssigned
		FROM CRM_CarBasicData CBD WITH (NOLOCK)
		JOIN vwMMV VM WITH (NOLOCK) ON VM.VersionId = CBD.VersionId
		JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
		JOIN OprUsers OU WITH (NOLOCK) ON CDA.CreatedBy = OU.Id
		--JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Brand = VM.MakeId AND CIT.Type = 5 AND CIT.UserId = OU.Id
		JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Type = 5 AND CIT.UserId = OU.Id
		WHERE 
		--CDA.CreatedOn = GETDATE() - 1
		CDA.CreatedOn BETWEEN @PrevDayStart AND @PrevDayEnd
		GROUP BY OU.Id) A 
	ON A.UserId = CTL.UserId
	WHERE 
	CTL.Date = CAST(GETDATE() - 1 AS DATE) AND 
	CTL.Type = 5 AND CTL.IsDeleted =0

	--Update top most value(target) for the user for current month considering it as roaster
	--imp condition to be maintianed is exclude current date
	DECLARE @CurrentDateTime DATETIME = GETDATE()
	UPDATE CTL
	SET CTL.Value = 0
	FROM CRM_TargetLog CTL
	JOIN
	(
		SELECT Id CtlId,UserId,Type,--Isnull(ActualLeads,0) ActualLeads, [Date],month([Date]) as Mnth,Year([Date]) as Yr,
		row_number () over(partition by UserId,Type order by [date] asc) as RowNum
		FROM CRM_TargetLog WITH (NOLOCK) 
		where IsDeleted = 0
		and Isnull(ActualLeads, 0) = 0
		and [Date] <> CAST(@CurrentDateTime AS DATE)
		and month([Date])=month(@CurrentDateTime) 
		and Year([Date])=Year(@CurrentDateTime) 
	) A
	ON CTL.Id = A.CtlId
	WHERE A.RowNum = 1
END

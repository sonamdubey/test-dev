IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PopulateFLCScoreBoard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PopulateFLCScoreBoard]
GO

	
CREATE PROCEDURE [dbo].[CRM_PopulateFLCScoreBoard]
 AS
	
BEGIN
		DECLARE @TempScoreBoardData TABLE 
		(
			MakeId INT,
			MakeName VARCHAR(50),
			LeadCount INT,
			LeadTarget INT,
			ProcessType INT,
			UserId INT,
			UserName VARCHAR(50)
		)

		DECLARE @TempMakeTargets TABLE
		(
			MakeId INT,
			Type INT,
			Target NUMERIC
		) 

		--DECLARE @FirstDayOFMonth DATETIME, @TodayDate DATETIME
		DECLARE @FirstDayOfMonth DATETIME, @TodaysStart DATETIME, @CurrentDateTime DATETIME 
	
		SET @FirstDayOfMonth = CONVERT(DATETIME,DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0))
		SET @TodaysStart = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 00:00:00')
		SET @CurrentDateTime = GETDATE()
		
		INSERT INTO @TempMakeTargets (MakeId, Type, Target)
		SELECT CAST(Brand AS INT), Type, Value 
		FROM CRM_Targets CT 
		--WHERE MONTH(CT.Date) = MONTH(@CurrentDateTime) AND YEAR(CT.Date) = YEAR(@CurrentDateTime)
		WHERE CT.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime
		
		-- Type 1 - make wise lead proccessed 
		INSERT INTO @TempScoreBoardData(MakeId, MakeName, LeadCount, LeadTarget, ProcessType)
		SELECT VM.MakeId, VM.Make,COUNT(DISTINCT CC.LeadId) LeadsProcessed, 
		--(SELECT SUM(Value) FROM CRM_Targets WHERE Brand = VM.MakeId AND Type = 4 AND Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime),
		(SELECT SUM(TT.Target) FROM @TempMakeTargets TT WHERE TT.MakeId = VM.MakeId AND TT.Type = 4),
		'1' AS ProcessType
		FROM CRM_Calls CC WITH (NOLOCK)
			JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CC.LeadId = CBD.LeadId
			JOIN vwMMV VM ON VM.VersionId = CBD.VersionId
			--JOIN CRM_Targets CT WITH (NOLOCK) ON CT.Brand = VM.MakeId AND CT.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime AND CT.Type = 4
			JOIN @TempMakeTargets TT ON TT.MakeId = VM.MakeId AND TT.Type = 4
		WHERE 
			MONTH(CC.ActionTakenOn) = MONTH(@CurrentDateTime) AND YEAR(CC.ActionTakenOn) = YEAR(@CurrentDateTime) AND CC.ActionTakenOn >= '2014-08-25 00:00:00.000' AND
			--CC.ActionTakenOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime AND
			CC.CallType=1 AND CC.IsActionTaken = 1 AND CC.actiontakenby NOT IN (13,651)
		GROUP BY VM.MakeId, VM.Make
		
		-- Type 2 make wise lead assigned
		INSERT INTO @TempScoreBoardData(MakeId, MakeName, LeadCount, LeadTarget,ProcessType)
		SELECT VM.MakeId, VM.Make, COUNT(DISTINCT CBD.LeadId) LeadsAssign, 
		--(SELECT SUM(Value) FROM CRM_Targets WHERE Brand = VM.MakeId AND Type = 5 AND Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime),
		(SELECT SUM(TT.Target) FROM @TempMakeTargets TT WHERE TT.MakeId = VM.MakeId AND TT.Type = 5),
		'2' AS ProcessType
		FROM CRM_CarBasicData CBD
			JOIN vwMMV VM ON VM.VersionId = CBD.VersionId
			JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
			--JOIN CRM_Targets CT WITH (NOLOCK) ON CT.Brand = VM.MakeId AND CT.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime AND CT.Type = 5
			JOIN @TempMakeTargets TT ON TT.MakeId = VM.MakeId AND TT.Type = 5
		WHERE 
			MONTH(CDA.CreatedOn) = MONTH(@CurrentDateTime) AND YEAR(CDA.CreatedOn) = YEAR(@CurrentDateTime) AND CDA.CreatedOn >= '2014-08-25 00:00:00.000'
			--CDA.CreatedOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime
		GROUP BY VM.MakeId, VM.Make


		--Type 3 Get follow up leads i.e. lead that are in missed (set as followup)
		INSERT INTO @TempScoreBoardData(LeadCount, ProcessType)
		SELECT COUNT(DISTINCT CC.LeadId) Missed, '3' AS ProcessType
		FROM CRM_CallActiveList CC WITH (NOLOCK)
			JOIN CRM_CarBasicData CBD ON CBD.LeadId = CC.LeadId
			JOIN vwMMV VM ON VM.VersionId = CBD.VersionId
			--JOIN CRM_Targets CT WITH (NOLOCK) ON CT.Brand = VM.MakeId AND CT.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime
			JOIN @TempMakeTargets TT ON TT.MakeId = VM.MakeId
		WHERE CC.CallType = 2
			--AND MONTH(CC.ScheduledOn) = MONTH(@CurrentDateTime) AND YEAR(CC.ScheduledOn) = YEAR(@CurrentDateTime) 
			AND CC.ScheduledOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime

		--Type 4 Leads in pool owner -1 leadstageid 1 group is not future purchase 71
		INSERT INTO @TempScoreBoardData(LeadCount, ProcessType)
		SELECT COUNT(DISTINCT CL.ID) Pool,'4' AS ProcessType
		FROM CRM_Leads CL WITH (NOLOCK)
			JOIN CRM_CarBasicData CBD ON CBD.LeadId = CL.ID
			JOIN vwMMV VM ON VM.VersionId = CBD.VersionId
			--JOIN CRM_Targets CT WITH (NOLOCK) ON CT.Brand = VM.MakeId AND CT.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime
			JOIN @TempMakeTargets TT ON TT.MakeId = VM.MakeId
		WHERE CL.Owner = -1 AND CL.LeadStageId = 1 AND CL.GroupId <> 71
			--AND MONTH(CL.CreatedOn) = MONTH(@CurrentDateTime) AND YEAR(CL.CreatedOn) = YEAR(@CurrentDateTime)
			AND CL.CreatedOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime

		SELECT MakeId,MakeName,UserId, UserName, LeadCount,LeadTarget, ProcessType FROM @TempScoreBoardData

		IF @@ROWCOUNT > 0
		BEGIN
			TRUNCATE TABLE CRM_FLCScoreboardData

			INSERT INTO CRM_FLCScoreboardData(MakeId, MakeName,UserId, UserName, LeadCount,LeadTarget, ProcessType)
			SELECT MakeId,MakeName,UserId, UserName, LeadCount,LeadTarget,ProcessType FROM @TempScoreBoardData
		END		

		--Below code added by Vaibhav K 8 Aug 2014
		--Updation of ActualLeads column for leads processed for current todays data
		UPDATE CTL
		SET CTL.ActualLeads = ISNULL(A.LeadProcessed,0)
		FROM CRM_TargetLog CTL
		LEFT JOIN 
			(SELECT OU.Id AS UserId, COUNT(DISTINCT CC.LeadId) AS LeadProcessed
			FROM CRM_Calls CC WITH (NOLOCK)
			JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CC.LeadId = CBD.LeadId
			JOIN vwMMV VM ON VM.VersionId = CBD.VersionId
			JOIN OprUsers OU WITH (NOLOCK) ON CC.ActionTakenBy = OU.Id
			JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Brand = VM.MakeId AND CIT.Type = 4 AND CIT.UserId = OU.Id
			WHERE CC.CallType=1 AND CC.IsActionTaken = 1 AND CC.ActionTakenBy NOT IN (13,651)
			--AND CC.ActionTakenOn = GETDATE()
			AND CC.ActionTakenOn BETWEEN @TodaysStart AND @CurrentDateTime
			GROUP BY OU.Id) A 
		ON A.UserId = CTL.UserId
		WHERE CTL.Date = CAST(@CurrentDateTime AS DATE) AND CTL.Type = 4 AND CTL.IsDeleted = 0

		--Updation of ActualLeads column for leads assigned for current todays data
		UPDATE CTL
		SET CTL.ActualLeads = ISNULL(A.LeadAssigned,0)
		FROM CRM_TargetLog CTL
		LEFT JOIN
			(SELECT OU.Id AS UserId, COUNT(DISTINCT CBD.LeadId) AS LeadAssigned
			FROM CRM_CarBasicData CBD WITH (NOLOCK)
			JOIN vwMMV VM WITH (NOLOCK) ON VM.VersionId = CBD.VersionId
			JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
			JOIN OprUsers OU WITH (NOLOCK) ON CDA.CreatedBy = OU.Id
			JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON CIT.Brand = VM.MakeId AND CIT.Type = 5 AND CIT.UserId = OU.Id
			WHERE 
			--CDA.CreatedOn = GETDATE()
			CDA.CreatedOn BETWEEN @TodaysStart AND @CurrentDateTime
			GROUP BY OU.Id) A 
		ON A.UserId = CTL.UserId
		WHERE CTL.Date = CAST(@CurrentDateTime AS DATE) AND CTL.Type = 5 AND CTL.IsDeleted =0
END






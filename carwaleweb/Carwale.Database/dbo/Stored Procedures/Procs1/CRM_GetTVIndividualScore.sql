IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetTVIndividualScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetTVIndividualScore]
GO

	-- =============================================
-- Author		: Vaibhav K
-- Create date	: 01 Aug 2014
-- Description	: Individual scoreboard data multiple tables EXEC CRM_GetTVIndividualScore
--				: 1. Makes for which target is defined for the month & their respective pool,missed leads
--				: 2. CRM_TargetLog data for current month
-- Modifier		: Vaibhav K 14 Aug 2014 (Added date parameters to avoid cast and functions on date columns)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetTVIndividualScore]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @FirstDayOfMonth DATETIME, @TodaysStart DATETIME, @CurrentDateTime DATETIME 
	
	SET @FirstDayOfMonth = CONVERT(DATETIME,DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0))
	SET @TodaysStart = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 00:00:00')
	SET @CurrentDateTime = GETDATE()
	
	DECLARE @TempMakePool TABLE
	(
		MakeId	INT,
		MakeName VARCHAR(50),
		Pool INT
	)

	DECLARE @TempMakeMissed TABLE
	(
		MakeId	INT,
		MakeName VARCHAR(50),
		Missed INT
	)

	INSERT INTO @TempMakePool (MakeId, MakeName, Pool)
	SELECT DISTINCT VW.MakeId, VW.Make, COUNT(DISTINCT CL.ID) Pool
	FROM 
		CRM_Leads CL WITH (NOLOCK)
		JOIN CRM_CarBasicData CBD  WITH (NOLOCK) ON CBD.LeadId = CL.ID
		JOIN vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId
		JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON VW.MakeId = CIT.Brand
	WHERE 
		CL.Owner = -1 AND CL.LeadStageId = 1 AND CL.GroupId <> 71
		--AND MONTH(CL.CreatedOn) = MONTH(@CurrentDateTime) AND YEAR(CL.CreatedOn) = YEAR(@CurrentDateTime)
		AND CL.CreatedOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime
	GROUP BY VW.MakeId, VW.Make

	INSERT INTO @TempMakeMissed (MakeId, MakeName, Missed)
	SELECT DISTINCT VW.MakeId, VW.Make, COUNT(DISTINCT CC.LeadId) Missed
	FROM 
		--CRM_Calls CC WITH (NOLOCK)
		CRM_CallActiveList CC WITH (NOLOCK)
		--JOIN CRM_Leads CL WITH (NOLOCK) ON CC.LeadId = CL.ID
		JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.LeadId = CC.LeadId
		JOIN vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId
		JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON VW.MakeId = CIT.Brand
	WHERE 
		CC.CallType = 2
		--AND MONTH(CC.ScheduledOn) = MONTH(@CurrentDateTime) AND YEAR(CC.ScheduledOn) = YEAR(@CurrentDateTime)
		AND CC.ScheduledOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime
	GROUP BY VW.MakeId, VW.Make

	SELECT MP.MakeId, MP.MakeName, MP.Pool, MM.Missed
	FROM @TempMakePool MP
	JOIN @TempMakeMissed MM ON MP.MakeId = MM.MakeId

	--SELECT A.MakeId, A.Make MakeName, A.Pool, B.Missed 
	--FROM
	--	(SELECT DISTINCT VW.MakeId, VW.Make, COUNT(DISTINCT CL.ID) Pool
	--	FROM CRM_Leads CL WITH (NOLOCK)
	--	JOIN CRM_CarBasicData CBD  WITH (NOLOCK) ON CBD.LeadId = CL.ID
	--	JOIN vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId
	--	JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON VW.MakeId = CIT.Brand
	--	WHERE 
	--	CL.Owner = -1 AND CL.LeadStageId = 1 AND CL.GroupId <> 71
	--	--AND MONTH(CL.CreatedOn) = MONTH(@CurrentDateTime) AND YEAR(CL.CreatedOn) = YEAR(@CurrentDateTime)
	--	AND CL.CreatedOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime
	--	GROUP BY VW.MakeId, VW.Make) A 
	--JOIN	
	--	(SELECT DISTINCT VW.MakeId, VW.Make, COUNT(DISTINCT CC.LeadId) Missed
	--	FROM 
	--	--CRM_Calls CC WITH (NOLOCK)
	--	CRM_CallActiveList CC WITH (NOLOCK)
	--	--JOIN CRM_Leads CL WITH (NOLOCK) ON CC.LeadId = CL.ID
	--	JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.LeadId = CC.LeadId
	--	JOIN vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId
	--	JOIN CRM_IndividualTarget CIT WITH (NOLOCK) ON VW.MakeId = CIT.Brand
	--	WHERE 
	--	CC.CallType = 2
	--	--AND MONTH(CC.ScheduledOn) = MONTH(@CurrentDateTime) AND YEAR(CC.ScheduledOn) = YEAR(@CurrentDateTime)
	--	AND CC.ScheduledOn BETWEEN @FirstDayOfMonth AND @CurrentDateTime
	--	GROUP BY VW.MakeId, VW.Make) B
	--ON A.MakeId = B.MakeId

	--Complete data from CRM_TargetLog for the current month
	
	SELECT DISTINCT OU.Id UserId, OU.UserName, CMK.ID AS MakeId, CMK.Name AS MakeName,
	 CTL.Type, CTL.Date, CTL.Value, ISNULL(CTL.ActualLeads, 0) AS ActualLeads--, CTL.IsDeleted, CTL.ActionTakenBy, CTL.ActionTakenOn
	FROM CRM_TargetLog CTL WITH (NOLOCK)
	JOIN CarMakes CMK WITH (NOLOCK) ON CTL.Brand = CMK.ID
	JOIN OprUsers OU WITH (NOLOCK) ON CTL.UserId = OU.Id
	WHERE CTL.IsDeleted = 0
	--AND MONTH(CTL.Date) = MONTH(GETDATE()) AND YEAR(CTL.Date) = YEAR(GETDATE())
	AND CTL.Date BETWEEN @FirstDayOfMonth AND @CurrentDateTime
	ORDER BY CMK.Name,OU.UserName
END

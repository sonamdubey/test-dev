IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchActivityFeed]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchActivityFeed]
GO

	-- =============================================
-- Author:		Kritika Choudhary
-- Create date: 5th Aug 2016
-- Description:	fetch activity feed details
-- EXEC [TC_FetchActivityFeed] 7095485,6,1,NULL
-- EXEC [TC_FetchActivityFeed] 5,nULL,1,20
-- EXEC [TC_FetchActivityFeed] 30431,6,NULL,NULL
-- Modified by Ruchira Patil on 20Sept 2016 - Added ActionTakenOn when NextFollowUpDate is null (in case of new inquiry added)
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchActivityFeed] @CustomerId INT = NULL
	,@BusinessTypeId INT = 0
	,@FromIndex INT = NULL
	,@ToIndex INT = 20
AS
BEGIN
	WITH Cte
	AS (
		SELECT U.UserName AS [UserName]
			,ISNULL(TCC.ActionComments , '--') AS [ActionComments]
			,ActionTakenOn AS [ActionTakenOn]
			,TCA.NAME AS [Action]
			,1 AS [IsComment]
			,ActionTakenOn AS [date]
			,ISNULL(TCC.NextFollowUpDate,ActionTakenOn) AS [FollowUpDate]
			,NA.NextAction AS NextAction
			,B.NAME AS [BusinessType]
		FROM TC_Lead L WITH (NOLOCK)
		INNER JOIN TC_Calls AS TCC WITH (NOLOCK) ON L.TC_LeadId = TCC.TC_LeadId
		INNER JOIN TC_BusinessType B WITH (NOLOCK) ON B.Id = L.TC_BusinessTypeId
		JOIN TC_Users AS U WITH (NOLOCK) ON TCC.TC_UsersId = U.Id
		LEFT OUTER JOIN TC_CallAction AS TCA WITH (NOLOCK) ON TCA.TC_CallActionId = TCC.TC_CallActionId
			AND TCA.IsActive = 1
		LEFT JOIN TC_NextAction NA WITH (NOLOCK) ON NA.TC_NextActionId = TCC.TC_NextActionId
		WHERE L.TC_CustomerId = @CustomerId
			AND TCC.IsActionTaken = 1
			AND (
				@BusinessTypeId = 0
				OR L.TC_BusinessTypeId = @BusinessTypeId
				)
		
		UNION
		
		SELECT U.UserName AS [UserName]
			,CASE TCLD.IsClosed
				WHEN 1
					THEN 'Inquiry Closed - '
				WHEN 0
					THEN (
							CASE 
								WHEN ISNULL(TCDL.LeadOwnerId, 0) <> 0
									AND ISNULL(TCDL.NewLeadOwnerId, 0) <> 0
									THEN TCLD.NAME
								ELSE NULL
								END
							)
				END AS [ActionComments]
			,EventCreatedOn AS [ActionTakenOn]
			,CASE TCLD.IsClosed
				WHEN 1
					THEN TCLD.NAME
				WHEN 0
					THEN (
							CASE 
								WHEN ISNULL(TCDL.LeadOwnerId, 0) <> 0
									AND ISNULL(TCDL.NewLeadOwnerId, 0) <> 0
									THEN TCLD.NAME + ' from ' + U1.UserName + ' to ' + U2.UserName
								ELSE CASE 
										WHEN TCDL.TC_LeadDispositionId = 88
											THEN TCLD.NAME + ' - ' + ISNULL(TCDL.DispositionReason, '')
										ELSE TCLD.NAME
										END
								END
							)
				END AS [Action]
			,0 AS [IsComment]
			,EventCreatedOn AS [date]
			,TCDL.EventCreatedOn AS [FollowUpDate]
			,NULL AS NextAction
			,B.NAME AS [BusinessType]
		FROM TC_Lead L WITH (NOLOCK)
		INNER JOIN TC_DispositionLog AS TCDL WITH (NOLOCK) ON L.TC_LeadId = TCDL.TC_LeadId
		INNER JOIN TC_BusinessType B WITH (NOLOCK) ON B.Id = L.TC_BusinessTypeId
		JOIN TC_Users AS U WITH (NOLOCK) ON TCDL.EventOwnerId = U.Id
		LEFT JOIN TC_Users AS U1 WITH (NOLOCK) ON TCDL.LeadOwnerId = U1.Id
		LEFT JOIN TC_Users AS U2 WITH (NOLOCK) ON TCDL.NewLeadOwnerId = U2.Id
		JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON TCLD.TC_LeadDispositionId = TCDL.TC_LeadDispositionId
		WHERE L.TC_CustomerId = @CustomerId
			AND (
				@BusinessTypeId = 0
				OR L.TC_BusinessTypeId = @BusinessTypeId
				)
		)
	SELECT [UserName]
		,[ActionComments]
		,[ActionTakenOn]
		,[Action]
		,[IsComment]
		,[FollowUpDate]
		,NextAction
		,[BusinessType]
		,ROW_NUMBER() OVER (
			ORDER BY DATE DESC
			) NumberForPaging
	INTO #tmpAllData
	FROM Cte WITH(NOLOCK)
	ORDER BY [date] DESC;

	SELECT *
	FROM #tmpAllData WITH(NOLOCK)
	WHERE (
			@FromIndex IS NULL
			OR NumberForPaging >= @FromIndex
			)
		AND (
			@ToIndex IS NULL
			OR NumberForPaging <= @ToIndex
			)
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_DisposeCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_DisposeCalls]
GO

	

-- =============================================
-- Author      : <Deepak Tripathi>
-- Create date : <Create Date, 26th June, 2015>
-- Description : <Dispose all the calls where lead has been closed>
-- Modified By : Chetan Navin on 27th June 2016 (To remove calls that from tc_tasklist if leadstage is closed)
-- =============================================
CREATE PROCEDURE [dbo].[TC_AP_DisposeCalls]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- Delete unnecessary calls - Lead is closed but somehow call is still active
	SET NOCOUNT ON;
	
	SELECT TC.TC_CallsId, TC.TC_UsersId
	INTO #TempClosedCalls
	FROM TC_Lead TL WITH(NOLOCK)
	INNER JOIN TC_Calls TC WITH(NOLOCK) ON TL.TC_LeadId = TC.TC_LeadId
	WHERE TL.TC_LeadStageId = 3 AND TC.IsActionTaken = 0
	
	DECLARE @CallId INT
	DECLARE @TC_UsersId INT
	DECLARE @NextFollowupDate DATETIME = GETDATE()
	DECLARE @ApplicationId TINYINT

	WHILE EXISTS(SELECT TOP 1 TC_CallsId FROM #TempClosedCalls)
	BEGIN
		SELECT TOP 1 @CallId =  TC_CallsId, @TC_UsersId = TC_UsersId FROM #TempClosedCalls 

		-- Update Calls Data and finish all calls where lead has been closed
		EXEC TC_DisposeCall @CallId, 'Lead Closed', NULL, @NextFollowupDate, @TC_UsersId
		
		DELETE FROM #TempClosedCalls WHERE  TC_CallsId = @CallId
	END	
	
	DROP TABLE #TempClosedCalls;
	
	--Delete Duplicate Calls
	-- From TC_TasklIst Table
	WITH CTE AS 
	(
		SELECT  TC_TaskListsId, ROW_NUMBER() OVER (PARTITION BY TC_LeadId ORDER BY TC_CallsId DESC) rownum
		FROM TC_TaskLists WITH (NOLOCK)
	) 
	DELETE FROM TC_TaskLists WHERE TC_TaskListsId IN(SELECT TC_TaskListsId FROM CTE WHERE rownum > 1);
	
	
	-- From TC_ActiveCalls Table
	WITH CTEC AS 
	(
		SELECT  TC_CallsId, ROW_NUMBER() OVER (PARTITION BY TC_LeadId ORDER BY TC_CallsId DESC) rownum
		FROM TC_ActiveCalls WITH (NOLOCK)
	) 
	DELETE FROM TC_ActiveCalls WHERE TC_CallsId IN(SELECT tc_callsid FROM CTEC WHERE rownum > 1);
	
	--Delete Calls Where transaction has been roled back and there is no leadId
	SELECT TA.TC_CallsId 
	INTO #tmpNoLeadCalls
	FROM TC_ActiveCalls TA WITH (NOLOCK) LEFT JOIN TC_Lead TL WITH (NOLOCK) ON TA.TC_LeadId = TL.TC_LeadId 
	WHERE TL.TC_LeadId IS NULL
		
	DELETE FROM TC_ActiveCalls WHERE TC_CallsId IN(SELECT TC_CallsId FROM #tmpNoLeadCalls)
	DELETE FROM TC_Calls WHERE TC_CallsId IN(SELECT TC_CallsId FROM #tmpNoLeadCalls)
	DROP TABLE #tmpNoLeadCalls
		
	-- Insert Calls available in TC_Tasklist but not in TC_ActiveCalls
	INSERT INTO TC_ActiveCalls (TC_CallsId ,TC_LeadId ,CallType ,TC_UsersId ,ScheduledOn ,AlertId ,LastCallDate ,LastCallComment,TC_NextActionId,NextCallTo,TC_NextActionDate, TC_BusinessTypeId) 
	SELECT DISTINCT TT.TC_CallsId, TT.TC_LeadId, TT.TC_CallTypeId, TT.UserId, TT.ScheduledOn, NULL, NULL, TT.LastCallComment, TT.TC_NextActionId, NULL, TT.TC_NextActionDate, TT.TC_BusinessTypeId
	FROM TC_TaskLists TT WITH (NOLOCK) LEFT JOIN TC_ActiveCalls TA WITH (NOLOCK) ON TT.TC_CallsId = TA.TC_CallsId
	WHERE TA.TC_CallsId IS NULL
	
	-- Delete ActionTaken Calls
	SELECT TC.TC_CallsId 
	INTO #tmpActionTakenCalls
	FROM TC_Calls TC WITH (NOLOCK) INNER JOIN TC_ActiveCalls TCA WITH (NOLOCK) ON TC.TC_CallsId = TCA.TC_CallsId 
	WHERE IsActionTaken = 1 
	
	DELETE FROM TC_ActiveCalls WHERE TC_CallsId IN(SELECT TC_CallsId FROM #tmpActionTakenCalls)
	DELETE FROM TC_TaskLists WHERE TC_CallsId IN(SELECT TC_CallsId FROM #tmpActionTakenCalls)
	DROP TABLE #tmpActionTakenCalls
	
	-- Populate Calls in TC_Calls But not in TC_ActiveCalls
	--SELECT TC.TC_CallsId, TC.TC_LeadId, TC.CallType, TC.TC_UsersId, TC.ScheduledOn, TC.AlertId,
	--	TC.TC_NextActionId, TC.NextCallTo, D.ApplicationId, TC.TC_NextActionDate, TL.TC_BusinessTypeId
	--INTO #tmpNotScheduledCalls
	--FROM TC_Calls TC WITH (NOLOCK) 
	--INNER JOIN TC_Lead TL WITH (NOLOCK) ON TC.TC_LeadId = TL.TC_LeadId
	--INNER JOIN Dealers D WITH (NOLOCK) ON TL.BranchId = D.ID
	--LEFT JOIN TC_ActiveCalls TCA WITH (NOLOCK) ON TC.TC_LeadId = TCA.TC_LeadId 
	--WHERE ISNULL(TC.IsActionTaken,0) = 0 AND TCA.TC_LeadId IS NULL
	
	--DECLARE @TC_LeadId INT, @CallType INT, @ScheduleDate DATETIME, @AlertId TINYINT, 
	--						@TC_NextActionId TINYINT, @NextCallTo TINYINT, @TC_NextActionDate DATETIME, @TC_BusinessTypeId TINYINT
	--WHILE EXISTS(SELECT TOP 1 TC_CallsId FROM #tmpNotScheduledCalls)
	--	BEGIN
	--		SELECT TOP 1 @CallId = TC_CallsId, @TC_LeadId = TC_LeadId, @CallType = CallType, @TC_UsersId = TC_UsersId, 
	--					@ScheduleDate = ScheduledOn, @AlertId = AlertId, @TC_NextActionId = TC_NextActionId, 
	--					@NextCallTo = NextCallTo, @ApplicationId = ApplicationId, @TC_NextActionDate = TC_NextActionDate,
	--					@TC_BusinessTypeId = TC_BusinessTypeId
	--		FROM #tmpNotScheduledCalls 
			
	--		--Check again before processing
	--		SELECT TC_CallsId FROM TC_ActiveCalls WITH (NOLOCK) WHERE TC_CallsId = @CallId
	--		IF @@ROWCOUNT = 0
	--			BEGIN
	--				-- Schedule Data
	--				INSERT INTO TC_ActiveCalls(TC_CallsId, TC_LeadId, CallType, TC_UsersId, ScheduledOn, AlertId, LastCallDate,
	--								LastCallComment, TC_NextActionId, NextCallTo, TC_NextActionDate, TC_BusinessTypeId)
	--				VALUES (@CallId, @TC_LeadId, @CallType, @TC_UsersId, @ScheduleDate, @AlertId, GETDATE()
	--						,'', @TC_NextActionId, @NextCallTo, @TC_NextActionDate, @TC_BusinessTypeId
	--						)

	--				EXEC [TC_TaskListUpdate_V16.10.1] 2 ,TC_CallsId, @ApplicationId
					
	--				DELETE FROM #tmpNotScheduledCalls WHERE  TC_CallsId = @CallId
	--			END
	--	END	
	--DROP TABLE #tmpNotScheduledCalls;
	
	-- Populate Calls, already in TC_ActiveCalls but not in TC_Tasklist
	SELECT TA.TC_CallsId, D.ApplicationId 
	INTO #tmpNoTaskListCalls
	FROM TC_ActiveCalls TA WITH (NOLOCK) 
		INNER JOIN TC_Lead TL WITH (NOLOCK) ON TA.TC_LeadId = TL.TC_LeadId
		INNER JOIN Dealers D WITH (NOLOCK) ON TL.BranchId = D.ID
		LEFT JOIN TC_TaskLists TT WITH (NOLOCK) ON TA.TC_CallsId = TT.TC_CallsId
	WHERE TT.TC_CallsId IS NULL
	
	WHILE EXISTS(SELECT TOP 1 TC_CallsId FROM #tmpNoTaskListCalls)
		BEGIN
			SELECT TOP 1 @CallId =  TC_CallsId, @ApplicationId = ApplicationId FROM #tmpNoTaskListCalls 
			
			--Check again before processing
			SELECT TC_TaskListsId FROM TC_TaskLists WITH (NOLOCK) WHERE TC_CallsId = @CallId
			IF @@ROWCOUNT = 0
				BEGIN
					-- Schedule Data
					EXEC TC_TaskListUpdate 2, @CallId,@ApplicationId
					
					DELETE FROM #tmpNoTaskListCalls WHERE  TC_CallsId = @CallId
				END
		END	
	DROP TABLE #tmpNoTaskListCalls;
	
	-- Incase there is a mismatch in lead owner and calling user, update it
	EXEC TC_AP_AutoUpdateUser
	
END




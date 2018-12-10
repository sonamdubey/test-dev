IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CCTaskListLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CCTaskListLoad]
GO

	
-- Created By Deepak Tripathi on 13th Dec for CC Task List Data Pooling
---Modified by Manish on 30-09-2014 added With Recompole option in the sp.

CREATE PROCEDURE [dbo].[CRM_CCTaskListLoad] --21
	@CallerId	INT	
 WITH RECOMPILE
 AS
	
BEGIN
	DECLARE @CallId TABLE (CallID BIGINT)
	DECLARE @CallLogId NUMERIC
	
	--Log Check
	INSERT INTO CRM_CallCheckLog(CallerId) VALUES(@CallerId)
	SET @CallLogId = SCOPE_IDENTITY()
	
	--All Pq Alerts First
	--Shorted by Buy time + 10 Days is less than or equal to today
	UPDATE TOP (1) CRM_CallActiveList SET UserId = @CallerId
    OUTPUT INSERTED.CallId INTO @CallId
    WHERE CallId IN
     (
                    
		--First Bucket of All PQ Alerts Order by Highest Lead Score
		SELECT TOP 3 CL.CallId FROM CRM_CallActiveList AS CL WITH (NOLOCK)
			INNER JOIN CRM_Leads CLS WITH (NOLOCK) ON CL.LeadId = CLS.Id
		WHERE CL.IsTeam=1 AND CL.UserId IS NULL AND CL.CallType IN(3,4)
			AND CL.CallerId IN(SELECT CAT.TeamId FROM CRM_ADM_TeamMembers AS CAT WITH (NOLOCK) WHERE CAT.UserId = @CallerId) 
			AND CL.ScheduledOn <= GETDATE()  AND ISNULL(CL.AlertId, 0) = 1
		ORDER BY 	
					
			-- Schedule Date
			ScheduledOn DESC
			
			-- Short By Lead Score
			,CLS.CCLeadScore DESC
	)	
	
	UPDATE CRM_CallCheckLog SET CallId = (SELECT CallID FROM @CallId), CallEntryDate = GETDATE() WHERE ID = @CallLogId
	SELECT * FROM @CallId
	IF @@ROWCOUNT = 0
		BEGIN 
			-- PQ Alert is finished 
			-- Focus on Lead status pending data 
			-- Do shorting on lead status basis then buy time 
			UPDATE TOP (1) CRM_CallActiveList SET UserId = @CallerId
			OUTPUT INSERTED.CallId INTO @CallId
			WHERE CallId IN
			(
				-- If first Bucket is clean
				-- Second bucket - Buy time + 10 Days is less than or equal to today
				SELECT TOP 3 CL.CallId FROM CRM_CallActiveList AS CL WITH (NOLOCK)
					INNER JOIN CRM_Leads CLS WITH (NOLOCK) ON CL.LeadId = CLS.Id
					LEFT JOIN CRM_SubDisposition CSD WITH (NOLOCK) ON CLS.LastConnectedStatus = CSD.Id

				WHERE CL.IsTeam=1 AND CL.UserId IS NULL AND CL.CallType IN(3,4)
					AND CL.CallerId IN(SELECT CAT.TeamId FROM CRM_ADM_TeamMembers AS CAT WITH (NOLOCK) WHERE CAT.UserId = @CallerId) 
					AND CL.ScheduledOn <= GETDATE() 
					AND ISNULL(CL.AlertId, 0) <> 1
				ORDER BY 
					
					--Short By Lead Status
					-- 8 is Lead Assigned Dispisition and 31 is Subdisposition
					-- 2 is Car Booked 
					-- 3 is Interested to Buy,
					-- 1 is Not Connected
					-- 6 is no meaningfull discussion
					(CASE	WHEN ISNULL(CSD.DispId, 8) = 2 AND ISNULL(CSD.Id, 31) = 4  THEN 1 -- Car Booked(2) and Asked to call back later(4)
							WHEN ISNULL(CSD.DispId, 8) = 3 AND ISNULL(CSD.Id, 31) = 12 THEN 2 -- Interested to Buy(3) and Going to Book(12)
							WHEN ISNULL(CSD.DispId, 8) = 3 AND ISNULL(CSD.Id, 31) = 11 THEN 3 -- Interested to Buy(3) and Finance in process(11)
							WHEN ISNULL(CSD.DispId, 8) = 3 AND ISNULL(CSD.Id, 31) = 5 THEN 4 -- Interested to Buy(3) and Visited dealership(5)
							WHEN ISNULL(CSD.DispId, 8) = 3 AND ISNULL(CSD.Id, 31) NOT IN(12, 11, 5) THEN 5 -- Interested to Buy(3) and Remaining one
							WHEN ISNULL(CSD.DispId, 8) = 6 AND ISNULL(CSD.Id, 31) = 26 THEN 6 -- No meaningfull discussion(6) and Call back later(26)
							WHEN ISNULL(CSD.DispId, 8) = 8 AND ISNULL(CSD.Id, 31) = 31 THEN 7 -- Lead Assigned(8) and Lead Assigned(31)
							WHEN ISNULL(CSD.DispId, 8) = 1  THEN 8 -- Not Connected(1)
					ELSE 9 END) ASC,
					
					-- Short By Lead Score
					CLS.CCLeadScore DESC
					
					-- Schedule Date
					,ScheduledOn DESC
			)
					
		END 
		
		UPDATE CRM_CallCheckLog SET CallId = (SELECT CallID FROM @CallId), CallEntryDate = GETDATE() WHERE ID = @CallLogId
		SELECT * FROM @CallId
		IF @@ROWCOUNT = 0
			BEGIN
				UPDATE TOP (1) CRM_CallActiveList SET UserId = @CallerId
				OUTPUT INSERTED.CallId INTO @CallId
				WHERE CallId IN
				 (
			                    
					--Last Bucket 
					SELECT TOP 3 CL.CallId FROM CRM_CallActiveList AS CL WITH (NOLOCK)
						INNER JOIN CRM_Leads CLS WITH (NOLOCK) ON CL.LeadId = CLS.Id
					WHERE CL.IsTeam=1 AND CL.UserId IS NULL AND CL.CallType IN(3,4)
						AND CL.CallerId IN(SELECT CAT.TeamId FROM CRM_ADM_TeamMembers AS CAT WITH (NOLOCK) WHERE CAT.UserId = @CallerId) 
						AND CL.ScheduledOn <= GETDATE()
					ORDER BY 	
								
						-- Schedule Date
						ScheduledOn DESC
				)
				UPDATE CRM_CallCheckLog SET CallId = (SELECT CallID FROM @CallId), CallEntryDate = GETDATE() WHERE ID = @CallLogId
				SELECT * FROM @CallId
			END 
END

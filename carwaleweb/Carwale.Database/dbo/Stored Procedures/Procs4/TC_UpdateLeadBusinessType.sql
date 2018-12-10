IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateLeadBusinessType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateLeadBusinessType]
GO

	
-- Created By:	Deepak Tripathi
-- Create date: 26th Sep 2016
-- Description:	Adding New Lead
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateLeadBusinessType] @LeadId INT
	,@LatestBusinessTypeId TINYINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TC_LeadId
	FROM TC_Lead WITH (NOLOCK)
	WHERE TC_BusinessTypeId = @LatestBusinessTypeId
		AND TC_LeadId = @LeadId

	IF @@ROWCOUNT = 0
	BEGIN
		-- Update lead business type
		UPDATE TC_Lead
		SET TC_BusinessTypeId = @LatestBusinessTypeId
		WHERE TC_LeadId = @LeadId

		-- Change TC_BusinessTypeId in Calls Data
		DECLARE @CallId INT
			,@TC_UsersId INT
			,@CallType INT
			,@AlertId INT
			,@NextCallTo INT
		DECLARE @TC_NextActionId SMALLINT
		DECLARE @ScheduleDate DATETIME
		DECLARE @LastCallComment VARCHAR(MAX)

		--Get existing data
		SELECT @TC_UsersId = TA.TC_UsersId
			,@CallType = TA.CallType
			,@ScheduleDate = TA.ScheduledOn
			,@AlertId = TA.AlertId
			,@LastCallComment = TA.LastCallComment
			,@TC_NextActionId = TA.TC_NextActionId
			,@NextCallTo = TA.NextCallTo
			,@CallId = TA.TC_CallsId
		FROM TC_ActiveCalls TA WITH (NOLOCK)
		WHERE TC_LeadId = @LeadId

		--Dispose lead
		EXEC TC_DisposeCall @CallId
			,'New Advantage Lead'
			,@TC_NextActionId
			,@ScheduleDate
			,@TC_UsersId

		--Schedule new lead
		EXEC TC_ScheduleCall @TC_UsersId
			,@LeadId
			,@CallType
			,@ScheduleDate
			,@AlertId
			,@LastCallComment
			,@TC_NextActionId
			,@NextCallTo
			,NULL
			,NULL
			,NULL
	END
END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetScheduleDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetScheduleDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: July 27,2016
-- Description:	To fetch tc_activecalls parameters of a lead
-- =============================================

CREATE PROCEDURE [dbo].[TC_GetScheduleDetails] 
	@TC_LeadId INT 
AS
BEGIN
	DECLARE @TC_UserId INT,@AlertId INT,@NextCallTo INT,@TC_CallsId INT
	DECLARE @ScheduleDate DATETIME
	DECLARE @NextActionId SMALLINT,@CallActionId SMALLINT

	SELECT	AC.TC_UsersId AS UsersId
			,AC.TC_CallsId AS NewCallId
			,AC.ScheduledOn AS ScheduleDate
			,AC.AlertId AS AlertId
			,AC.NextCallTo AS NextCallTo
			,AC.TC_NextActionId  AS NextActionId   --fetch existing NextActionId
			,AC.TC_LeadId AS LeadId
			,AC.TC_BusinessTypeId AS BusinessTypeId

	FROM TC_ActiveCalls AC WITH (NOLOCK)
	WHERE  AC.TC_LeadId = @TC_LeadId

END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FLCSchedulePoolData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FLCSchedulePoolData]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 10 Dec 2013
-- Description:	Updates the Tasklist according to the LeadScoreLimit
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FLCSchedulePoolData] 
	@TeleCallerId INT,
	@IsAgencyUser INT
AS
	DECLARE @LeadId TABLE (CallId BIGINT)

	BEGIN
		UPDATE TOP (1) CRM_Leads SET Owner = @TeleCallerId
		OUTPUT INSERTED.Id INTO @LeadId
		WHERE LeadStageId = 1 AND Owner = -1 AND Id IN 
		(SELECT TOP 5 CL.Id FROM CRM_Leads CL WITH (NOLOCK)
		INNER JOIN CRM_ADM_FLCGroups FLC WITH (NOLOCK) ON FLC.Id = CL.GroupId
		WHERE CL.LeadStageId = 1 AND CL.Owner = -1 AND CL.GroupId  = @IsAgencyUser
		AND CL.CreatedOn <= GETDATE()
		AND CL.LeadScore >= ISNULL(FLC.LeadScoreLimit,0)
		ORDER BY CL.LeadScore DESC, CL.CreatedOn DESC)
		
		SELECT CallId FROM @LeadId
	END

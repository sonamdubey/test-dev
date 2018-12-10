IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[MapFLCUserTeamGetTeamId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[MapFLCUserTeamGetTeamId]
GO

	



--Summary							: Get Team Id for FLC User
--Author							: Dilip V. 04-Oct-2012
--Modification history				: 1.

CREATE PROCEDURE [CRM].[MapFLCUserTeamGetTeamId]
@UserId VARCHAR(200)
AS

BEGIN
	SET NOCOUNT ON
	
	SELECT TeamId 
	FROM CRM.MapFLCUserTeam WITH (NOLOCK)
	WHERE FLCUserId = @UserId AND IsActive = 1

END






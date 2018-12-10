IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[MapFLCUserTeamView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[MapFLCUserTeamView]
GO

	




--Summary							: View Map FLC User Team
--Author							: Dilip V. 03-Oct-2012
--Modification history				: 1.

CREATE PROCEDURE [CRM].[MapFLCUserTeamView]
AS

BEGIN
	SET NOCOUNT ON
	
	SELECT MFUT.Id,MFUT.FLCUserId,OU.UserName,MFUT.TeamId,CAT.Name Team,MFUT.IsActive
	FROM CRM_ADM_Teams CAT 
	INNER JOIN CRM.MapFLCUserTeam MFUT ON CAT.ID = MFUT.TeamId
	INNER JOIN OprUsers OU ON OU.Id = MFUT.FLCUserId
	WHERE MFUT.IsActive = 1

END







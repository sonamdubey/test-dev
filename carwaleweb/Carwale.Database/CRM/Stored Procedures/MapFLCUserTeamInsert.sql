IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[MapFLCUserTeamInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[MapFLCUserTeamInsert]
GO

	



--Summary							: Map FLC User Team Insert
--Author							: Dilip V. 03-Oct-2012
--Modification history				: 1.

CREATE PROCEDURE [CRM].[MapFLCUserTeamInsert]
	@FLCUserId	NUMERIC(18,0),
	@TeamId		NUMERIC(18,0),
	@IsActive	BIT,
	@CreatedBy	NUMERIC(18,0)
AS

BEGIN
	SET NOCOUNT ON
	
	BEGIN
		IF NOT EXISTS (SELECT Id FROM [CRM].[MapFLCUserTeam] WHERE FLCUserId = @FLCUserId AND IsActive = 1)
		BEGIN
			INSERT INTO [CRM].[MapFLCUserTeam] 
					(FLCUserId,TeamId,IsActive,CreatedBy,CreatedOn) 
			VALUES (@FLCUserId,@TeamId,@IsActive,@CreatedBy,GETDATE())
		END
	END

END






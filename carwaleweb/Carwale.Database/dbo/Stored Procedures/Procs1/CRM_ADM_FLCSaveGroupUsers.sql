IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_FLCSaveGroupUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_FLCSaveGroupUsers]
GO

	

CREATE PROCEDURE [dbo].[CRM_ADM_FLCSaveGroupUsers]
	@UserId				INT,
	@CreatedBy			INT,
	@Type				SMALLINT
	 
AS
	BEGIN
		----UPDATE STATMENT
		--UPDATE CRM_ADM_AgencyLeadVerifier SET 
		--		Type = @Type, CreatedBy = @CreatedBy
		--WHERE UserId = @UserId
				
		--IF @@ROWCOUNT = 0
		--	BEGIN
		--		--INSERT STATMENT
		--		INSERT INTO CRM_ADM_AgencyLeadVerifier(UserId, Type, CreatedBy)
		--		VALUES(@UserId, @Type, @CreatedBy)
				
		--	END
				INSERT INTO CRM_ADM_AgencyLeadVerifier(UserId, Type, CreatedBy)
				VALUES(@UserId, @Type, @CreatedBy)
END






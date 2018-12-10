IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveTemplateGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveTemplateGroup]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <14/10/2016>
-- Description:	<Save template group with template ids and abcookie values>
-- =============================================
CREATE PROCEDURE [dbo].[SaveTemplateGroup] 
	@GroupId INT OUT,
	@GroupName VARCHAR(20),
	@PlatformId INT,
	@TemplateId INT,
	@IsActive BIT,
	@ABCookieValues VARCHAR(30),
	@LastUpdatedOn DATETIME,
	@LastUpdatedBy INT
AS
DECLARE @NewRowCount INT,
		@ExistingRowCount INT,
		@Iterator INT,
		@CookieValue INT,
		@MappingId INT
BEGIN
	SET NOCOUNT ON

	IF (@IsActive = 0)
	BEGIN
		SET @MappingId = (SELECT Id FROM TemplateGroupMapping with (nolock) WHERE GroupId = @GroupId AND TemplateId = @TemplateId)

		DELETE FROM TemplateGroupABCookieMapping
		OUTPUT deleted.Id, deleted.TemplateGroupMappingId, deleted.ABCookieValue, @LastUpdatedOn, @LastUpdatedBy, 'D' INTO TemplateGroupABCookieMapping_Log (TemplateGroupABCookieMappingId, TemplateGroupMappingId, ABCookieValue, UpdatedOn, UpdatedBy, UpdateType)
		WHERE TemplateGroupMappingId = @MappingId

		DELETE FROM TemplateGroupMapping
		OUTPUT deleted.Id, @GroupId, @TemplateId, @LastUpdatedOn, @LastUpdatedBy, 'D' INTO TemplateGroupMapping_Log (TemplateGroupMappingId, GroupId, TemplateId, UpdatedOn, UpdatedBy, UpdateType)
		WHERE GroupId = @GroupId AND TemplateId = @TemplateId
	END
	ELSE
	BEGIN
		CREATE TABLE #TempNewCookieValues (MappingId INT, CookieValue INT)
		CREATE TABLE #TempExistingCookieValues (CookieValue INT)

		IF NOT EXISTS (SELECT Id FROM TemplateGroup WITH (NOLOCK) WHERE Id = @GroupId)
		BEGIN
			INSERT INTO TemplateGroup (Name, PlatformId, IsActive, LastUpdatedOn, LastupdatedBy)
							   VALUES (@GroupName, @PlatformId, 1, @LastUpdatedOn, @LastUpdatedBy)

			SET @GroupId = SCOPE_IDENTITY()
		END
		ELSE
		BEGIN
			UPDATE TemplateGroup
			SET LastUpdatedOn = @LastUpdatedOn,
				LastUpdatedBy = @LastUpdatedBy
			WHERE Id = @GroupId

			IF (@GroupName != (SELECT Name FROM TemplateGroup WITH (NOLOCK) WHERE Id = @GroupId))
			BEGIN
				UPDATE TemplateGroup
				SET Name = @GroupName
				WHERE Id = @GroupId
			END
		END

		SET @MappingId = (SELECT Id FROM TemplateGroupMapping WITH (NOLOCK)
						  WHERE GroupId = @GroupId AND TemplateId = @TemplateId) 

		IF (@MappingId IS NULL)
		BEGIN
			INSERT INTO TemplateGroupMapping (GroupId, TemplateId)
			OUTPUT inserted.Id, @GroupId, @TemplateId, @LastUpdatedOn, @LastUpdatedBy, 'I' INTO TemplateGroupMapping_Log (TemplateGroupMappingId, GroupId, TemplateId, UpdatedOn, UpdatedBy, UpdateType)
									  VALUES (@GroupId, @TemplateId)

			SET @MappingId = SCOPE_IDENTITY()
		END

		INSERT INTO #TempNewCookieValues(MappingId, CookieValue)
			(SELECT @MappingId AS MappingId, items AS CookieValue 
			 FROM [dbo].[SplitText](@ABCookieValues,','))

		SET @NewRowCount = @@ROWCOUNT

		INSERT INTO #TempExistingCookieValues(CookieValue)
			(SELECT ABCookieValue AS Cookievalue
			 FROM TemplateGroupABCookieMapping WITH (NOLOCK)
			 WHERE TemplateGroupMappingId = @MappingId)

		SET @ExistingRowCount = @@ROWCOUNT

		IF (@NewRowCount = 10)
		BEGIN
			DELETE FROM TemplateGroupABCookieMapping
			OUTPUT deleted.Id, deleted.TemplateGroupMappingId, deleted.ABCookieValue, @LastUpdatedOn, @LastUpdatedBy, 'D' INTO TemplateGroupABCookieMapping_Log (TemplateGroupABCookieMappingId, TemplateGroupMappingId, ABCookieValue, UpdatedOn, UpdatedBy, UpdateType)
			WHERE TemplateGroupMappingId = @MappingId AND ABCookieValue IN (SELECT CookieValue
																			FROM #TempExistingCookieValues WITH (NOLOCK))

			INSERT INTO TemplateGroupABCookieMapping (TemplateGroupMappingId, ABCookieValue)
			OUTPUT inserted.Id, @MappingId, -1, @LastUpdatedOn, @LastUpdatedBy, 'I' INTO TemplateGroupABCookieMapping_Log (TemplateGroupABCookieMappingId, TemplateGroupMappingId, ABCookieValue, UpdatedOn, UpdatedBy, UpdateType)
	   										  VALUES (@MappingId, -1)
		END
		ELSE
		BEGIN
			DELETE FROM TemplateGroupABCookieMapping
			OUTPUT deleted.Id, @MappingId, deleted.ABCookieValue, @LastUpdatedOn, @LastUpdatedBy, 'D' INTO TemplateGroupABCookieMapping_Log (TemplateGroupABCookieMappingId, TemplateGroupMappingId, ABCookieValue, UpdatedOn, UpdatedBy, UpdateType)
			WHERE TemplateGroupMappingId = @MappingId AND ABCookieValue IN (SELECT CookieValue
																			FROM #TempExistingCookieValues WITH (NOLOCK))
													AND ABCookieValue NOT IN (SELECT CookieValue
																			  FROM #TempNewCookieValues WITH (NOLOCK))

			INSERT INTO TemplateGroupABCookieMapping (TemplateGroupMappingId, ABCookieValue)
			OUTPUT inserted.Id, @MappingId, inserted.ABCookieValue, @LastUpdatedOn, @LastUpdatedBy, 'I' INTO TemplateGroupABCookieMapping_Log (TemplateGroupABCookieMappingId, TemplateGroupMappingId, ABCookieValue, UpdatedOn, UpdatedBy, UpdateType)
				(SELECT MappingId, CookieValue 
				FROM #TempNewCookieValues WITH (NOLOCK)
				WHERE CookieValue NOT IN (SELECT ABCookieValue AS CookieValue 
										  FROM TemplateGroupABCookieMapping WITH (NOLOCK) 
										  WHERE TemplateGroupMappingId = @MappingId))
		END

		DROP TABLE #TempExistingCookieValues
		DROP TABLE #TempNewCookieValues
	END
END


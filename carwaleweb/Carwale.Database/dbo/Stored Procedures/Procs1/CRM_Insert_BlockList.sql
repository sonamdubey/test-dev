IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_Insert_BlockList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_Insert_BlockList]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 08 May 2013
-- Description:	This proc inserts VersionId, UserId ,DateTime , CityId
-- Modifier 1: Ruchira Patil- 11 Dec 2013 (Added CityId)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_Insert_BlockList]
(   @versionId NUMERIC, -- List of delimited items
	@userId  BIGINT,
	@cityId INT,
	@Status INT OUTPUT
)
AS
	BEGIN
		SELECT VersionId FROM CRM_BlockList WHERE VersionId = @versionId AND CityId = @cityId
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_BlockList(VersionId,BlockedDate,BlockedBy,CityId) VALUES(@versionId,CONVERT(VARCHAR(24),GETDATE(),121),@userId,@cityId)
			SET @Status = 1
		END
		ELSE
			BEGIN
				SET @Status = 0
			END
	END	
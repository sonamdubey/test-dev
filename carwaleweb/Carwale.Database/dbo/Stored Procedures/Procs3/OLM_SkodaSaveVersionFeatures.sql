IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SkodaSaveVersionFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SkodaSaveVersionFeatures]
GO

	-- =============================================

-- Author		: Chetan Navin

-- Create date	: 30th Sep 2013

-- Description	: To save/update version features.

-- Module		: CRM/Masters

-- Modified By  : Chetan Navin - 10 Oct 2013 (Removed extra filter in update statement) 

-- =============================================

CREATE PROCEDURE [dbo].[OLM_SkodaSaveVersionFeatures] 

	@FeatureId INT,

	@VersionId INT,

	@Value     SMALLINT,

	@IsActive  BIT = 1,

	@UpdatedBy BIGINT,

	@Status    TINYINT = -1 OUTPUT 

AS

BEGIN

	SET NOCOUNT ON;

	DECLARE @Id INT

	SELECT @Id = SVF.Id FROM OLM_SkodaVersionFeatures SVF WHERE SVF.FeatureId = @FeatureId AND SVF.VersionId = @VersionId



	IF(@@ROWCOUNT = 0)

		BEGIN

			INSERT INTO OLM_SkodaVersionFeatures VALUES(@FeatureId,@VersionId,@Value,@IsActive,GETDATE(),@UpdatedBy)

			SET @Status = 1

		END



	ELSE

		BEGIN

			--IF NOT EXISTS (SELECT 1 FROM OLM_SkodaVersionFeatures SVF WHERE SVF.FeatureId = @FeatureId AND SVF.VersionId = @VersionId)

			--BEGIN

				UPDATE OLM_SkodaVersionFeatures SET Value = @Value , IsActive = @IsActive 

				WHERE Id = @Id;

				SET @Status = 1

		--	END

		END

END

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SkodaSaveFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SkodaSaveFeatures]
GO

	-- =============================================

-- Author      : Chetan Navin

-- Create date : 1 Oct 2013

-- Description : To add and update skoda model features.

-- Module		: CRM/Masters

-- Modified by  : Chetan Navin - 11 Oct 2013 (Removed constraint from insert statement.)

-- =============================================

CREATE PROCEDURE [dbo].[OLM_SkodaSaveFeatures] 

	@Id			INT = -1,

	@ModelId	INT,

	@SubGroupId	INT,

	@Feature    VARCHAR(300),

	@IsActive   BIT = 1,

	@Status     SMALLINT = -1 OUTPUT 

AS

BEGIN

	SET NOCOUNT ON;

	SET @Status = 1



	IF(@Id = -1)

		BEGIN

			INSERT INTO OLM_SkodaFeatures (Name,ModelId,SubGroupId,IsActive) VALUES (@Feature,@ModelId,@SubGroupId,@IsActive)

			--WHERE NOT EXISTS (SELECT 1 FROM OLM_SkodaFeatures WHERE ModelId =@ModelId AND Name=@Feature)

			SET @Status = 1

		END



	ELSE

		BEGIN

			--IF NOT EXISTS (SELECT 1 FROM OLM_SkodaFeatures WHERE ModelId =@ModelId AND Name=@Feature)

			--BEGIN

				UPDATE OLM_SkodaFeatures SET Name = @Feature , IsActive = @IsActive, SubGroupId = @SubGroupId

				WHERE Id = @Id AND ModelId = @ModelId

				SET @Status = 1

			--END

		END

END

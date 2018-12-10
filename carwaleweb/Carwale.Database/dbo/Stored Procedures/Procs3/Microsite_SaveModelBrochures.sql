IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveModelBrochures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveModelBrochures]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 6 Oct 2016
-- Description:	To Save/Update Brochures for Models
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_SaveModelBrochures]
	@ModelId INT=NULL,
	@UpdatedBy INT, 	
	@IsAcive BIT = NULL,	
	@ModelBrochuresId INT = NULL out
AS
BEGIN

	IF @ModelBrochuresId = 0 AND NOT EXISTS(SELECT TOP 1 Id FROM Microsite_ModelBrochures WITH(NOLOCK) WHERE ModelId = @ModelId)
	BEGIN
		INSERT INTO Microsite_ModelBrochures (ModelId,CreatedBy) VALUES (@ModelId,@UpdatedBy)

		SET @ModelBrochuresId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE	Microsite_ModelBrochures
		SET		IsActive = ISNULL(@IsAcive,IsActive),
				UpdatedBy = @UpdatedBy,
				UpdatedOn = getdate()
		WHERE	Id = @ModelBrochuresId
	END
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetModelBrochures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetModelBrochures]
GO

	
-- =============================================
-- Author:		Kartik Rathod
-- Create date: 6 Oct 2016
-- Description:	To get brochure data base on make id and modelId 
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetModelBrochures] 
@MakeId INT = NULL,
@ModelId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

		SELECT MB.Id AS ModelBrochuresId,CM.NAME AS ModelName , CA.Name AS CarName , MB.GeneratedKey AS BrochureKey, MB.IsActive ,CM.Id AS ModelId, CA.Id AS MakeId
		FROM		Microsite_ModelBrochures MB  WITH(NOLOCK) 
		iNNER JOIN	CarModels CM WITH(NOLOCK) ON MB.ModelId = CM.Id
		INNER JOIN	CarMakes CA WITH(NOLOCK) ON CA.Id = CM.CarMakeId
		WHERE 
			(@ModelId IS NOT NULL AND MB.ModelId = @ModelId) OR (@MakeId IS NOT NULL AND CA.Id = @MakeId) 
	

END


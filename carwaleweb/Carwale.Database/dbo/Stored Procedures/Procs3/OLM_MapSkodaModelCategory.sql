IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_MapSkodaModelCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_MapSkodaModelCategory]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 25th  Mar 2014
-- Description : Save sKODA Model-Category Mapping Details 
-- Modified By : Vinay Kumar Prajapati  29th July
--             : Return @Status 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_MapSkodaModelCategory]
(
@ModelId int,
@CategoryId int,
@Status int OUTPUT
)
AS
	BEGIN
  
		SELECT OCM.Id FROM OLM_SkodaAcc_ModelCategoryMapping AS OCM WITH(NOLOCK) WHERE OCM.ModelId=@ModelId AND OCM.CategoryId=@CategoryId
		IF @@ROWCOUNT <> 0
			BEGIN
				SET @Status=0
			END 
		ELSE
			BEGIN   			        
				INSERT INTO OLM_SkodaAcc_ModelCategoryMapping(ModelId,CategoryId) VALUES(@ModelId,@CategoryId)
				SET @Status=1
		    END	  
	END
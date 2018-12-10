IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddModelColors]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19 July 2013
-- Description : Save Audi Model-Colors  Details 
-- Modified By : Vinay Kumar Prajapati  29th July
--             : Return @NewRowId for add images 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddModelColors]
(
@ModelId int,
@ColorId int,
@ColorForId int,
@ColorTypeId int,
@IsUpdate BIT,
@Id int,
@Status int OUTPUT,
@NewRowId int OUTPUT
)
AS
BEGIN
      IF @IsUpdate <> 0
		  BEGIN  
		  
			   SELECT AMC.Id FROM OLM_AudiBE_ModelColors AS AMC WITH(NOLOCK) WHERE AMC.ModelId=@ModelId AND AMC.ColorId=@ColorId AND AMC.ColorForId=@ColorForId AND AMC.ColorTypeId=@ColorTypeId AND AMC.Id <> @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				  END 
				ELSE
				  BEGIN    
				UPDATE OLM_AudiBE_ModelColors SET ModelId=@ModelId,ColorId=@ColorId,ColorForId=@ColorForId,ColorTypeId=@ColorTypeId WHERE Id=@Id  
				SET @Status=2 
			  END
		  END      
      ELSE
		  BEGIN
			 SELECT AMC.Id FROM OLM_AudiBE_ModelColors AS AMC WITH(NOLOCK) WHERE AMC.ModelId=@ModelId AND AMC.ColorId=@ColorId AND AMC.ColorForId=@ColorForId AND AMC.ColorTypeId=@ColorTypeId AND AMC.Id <> @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				END 
			 ELSE
			    BEGIN   			        
				   INSERT INTO OLM_AudiBE_ModelColors(ModelId,ColorId,ColorForId,ColorTypeId) VALUES(@ModelId,@ColorId,@ColorForId,@ColorTypeId)
				   SET @NewRowId=SCOPE_IDENTITY()
				   SET @Status=1
			     END
		  END	  
 END
 
 
 

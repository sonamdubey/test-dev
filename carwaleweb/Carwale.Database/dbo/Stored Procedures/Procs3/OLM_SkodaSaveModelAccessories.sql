IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SkodaSaveModelAccessories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SkodaSaveModelAccessories]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date :  26th March 2014
-- Description : Save Skoda Model- Category Accessories Details 
-- =============================================
CREATE PROCEDURE  [dbo].[OLM_SkodaSaveModelAccessories]
(
@ModelId INT,
@CategoryId INT,
@Description VARCHAR(400),
@Price INT,
@HostUrl VARCHAR(100),
@IsUpdate BIT,
@Id INT,
@Status INT OUTPUT,
@NewRowId INT OUTPUT
)
AS
BEGIN
      IF @IsUpdate <> 0
		  BEGIN  
			   SELECT OSD.Id FROM OLM_SkodaAcc_ModelAccessoriesDetails AS OSD WITH(NOLOCK)
			   WHERE OSD.ModelId=@ModelId AND OSD.CategoryId=@CategoryId AND OSD.Description=@Description 
			         AND OSD.Price=@Price AND OSD.Id <>  @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				  END 
				ELSE
				  BEGIN    
				UPDATE OLM_SkodaAcc_ModelAccessoriesDetails 
				SET ModelId=@ModelId,CategoryId=@CategoryId,Description=@Description,Price=@Price 
				WHERE Id=@Id  
				SET @Status=2 
			  END
		  END      
      ELSE
		  BEGIN
			  SELECT OSD.Id FROM OLM_SkodaAcc_ModelAccessoriesDetails AS OSD WITH(NOLOCK)
			   WHERE OSD.ModelId=@ModelId AND OSD.CategoryId=@CategoryId AND OSD.Description=@Description 
			         AND OSD.Price=@Price AND OSD.Id <>  @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				END 
			 ELSE
			    BEGIN   			        
				   INSERT INTO OLM_SkodaAcc_ModelAccessoriesDetails(ModelId,CategoryId,Description,Price ,HostUrl)
				    VALUES(@ModelId,@CategoryId,@Description,@Price,@HostUrl)
				   SET @NewRowId=SCOPE_IDENTITY()
				   SET @Status=1
			     END
		  END	  
 END

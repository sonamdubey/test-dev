IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_ADDAudi]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_ADDAudi]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 15 July 2013
-- Description : Insert Audi Details 
-- =============================================

CREATE PROCEDURE [dbo].[OLM_ADDAudi]
(
@Name VARCHAR(150),
@IsUpdate BIT,
@Id BIGINT,
@Status int OUTPUT
)
AS
BEGIN
	 IF  @IsUpdate =1   
		  BEGIN 
		      SELECT Id  FROM  OLM_AudiBE_Features AS OAF WITH(NOLOCK)  WHERE OAF.Name=@Name AND OAF.Id <> @Id 
		     IF @@ROWCOUNT <> 0
		       BEGIN
		         SET @Status = 0
		       END     
		     ELSE 
		       BEGIN 
				 UPDATE OLM_AudiBE_Features SET Name=@Name WHERE Id=@Id 
				 SET @Status = 2
			   END	      
		  END      
	 ELSE  IF @IsUpdate =0   
		  BEGIN 
		     SELECT Id  FROM  OLM_AudiBE_Features AS OAF WITH(NOLOCK)  WHERE OAF.Name=@Name
		     IF @@ROWCOUNT <> 0
		       BEGIN
		         SET @Status = 0
		       END
		     ELSE
		       BEGIN
				  INSERT INTO OLM_AudiBE_Features(Name) VALUES(@Name)
				  SET @Status = 1
			   END
		  END
 END

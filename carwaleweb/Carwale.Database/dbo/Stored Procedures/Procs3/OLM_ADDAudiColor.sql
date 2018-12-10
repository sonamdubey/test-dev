IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_ADDAudiColor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_ADDAudiColor]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 15 July 2013
-- Description : Insert Audi Details 
-- =============================================

CREATE PROCEDURE [dbo].[OLM_ADDAudiColor]
(
@CName VARCHAR(150),
@HashCode VARCHAR(10),
@IsUpdate BIT,
@Id BIGINT,
@Status int OUTPUT
)
AS
BEGIN
	 IF  @IsUpdate =1   
		  BEGIN 
		      SELECT Id  FROM  OLM_AudiBE_colors WHERE Name=@CName AND Id <> @Id
		     IF @@ROWCOUNT <> 0
		       BEGIN
		         SET @Status = 0
		       END     
		     ELSE 
		       BEGIN 
				 UPDATE OLM_AudiBE_colors SET Name=@CName,HashCode=@HashCode WHERE Id=@Id 
				 SET @Status = 2
			   END	      
		  END      
	 ELSE  IF @IsUpdate =0   
		  BEGIN 
		     SELECT Id  FROM  OLM_AudiBE_colors WHERE Name=@CName 
		     IF @@ROWCOUNT <> 0
		       BEGIN
		         SET @Status = 0
		       END
		     ELSE
		       BEGIN
				  INSERT INTO OLM_AudiBE_colors(Name,HashCode) VALUES(@CName,@HashCode)
				  SET @Status = 1
			   END
		  END
 END

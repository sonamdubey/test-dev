IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[InsertOrUpdatePageMetaTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[InsertOrUpdatePageMetaTags]
GO

	
-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <15 May 2013>
-- Description:	<Used for inserting and updating Page Meta tags>
-- =============================================
CREATE PROCEDURE [cw].[InsertOrUpdatePageMetaTags]
	-- Add the parameters for the stored procedure here
	@PageId numeric,
	@MakeId int,
	@ModelId int,
	@Title varchar(200),
	@Description varchar(500),
	@Keywords varchar(500),
	@Heading varchar(200),
	@IsActive bit,
	@Id numeric,
	@Summary varchar(500)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    IF @Id =-1
		BEGIN
			 INSERT INTO PageMetaTags
			 (PageId,MakeId,ModelId,Title,Description,Keywords,Heading,IsActive,Summary) 
			 VALUES(@PageId,@MakeId,@ModelId,@Title,@Description,@Keywords,@Heading,@IsActive,@Summary)
		 END
     ELSE
		 BEGIN
			 UPDATE PageMetaTags SET
			 Title=@Title,
			 Description=@Description,
			 Keywords=@Keywords,
			 Heading=@Heading,
			 IsActive=@IsActive,
			 Summary=@Summary
			
			 WHERE Id=@Id
		 END

    -- Insert statements for procedure here
	
END


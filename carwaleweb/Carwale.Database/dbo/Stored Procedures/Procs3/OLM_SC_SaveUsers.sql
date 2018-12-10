IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SC_SaveUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SC_SaveUsers]
GO

	-- =============================================
-- Author      : Chetan Navin		
-- Create date : 3 Sep 2013
-- Description : To Save Service Cost users in database.
-- Module	   : OLM Panel 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SC_SaveUsers] 
	@FullName   VARCHAR(100),
	@Mobile     VARCHAR(15),
	@CityId     INT,
	@Email      VARCHAR(100),
	@IsSkodaOwn BIT,
	@ModelId    INT,	
	@Status     SMALLINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET @Status = -1
	
	DECLARE @UserId NUMERIC = -1,@VisitCount INT
	SELECT @UserId = Id,@VisitCount = VisitCount FROM OLM_SC_Users WHERE Email = @Email
	
	IF (@UserId = -1)
		BEGIN
			INSERT INTO OLM_SC_Users (FullName,Mobile,CityId,Email,IsSkodaOwn,ModelId,VisitCount) VALUES(@FullName,@Mobile,@CityId,@Email,@IsSkodaOwn,@ModelId,1)
			SET @Status = 1
		END
		
	ELSE
		BEGIN
			UPDATE OLM_SC_Users SET FullName = @FullName,Mobile = @Mobile ,CityId = @CityId ,IsSkodaOwn = @IsSkodaOwn ,
				   ModelId = @ModelId,UpdatedOn = GETDATE(),VisitCount = @VisitCount + 1
			WHERE Email = @Email 
			SET @Status = 1
		END	
END

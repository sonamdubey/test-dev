IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveOctaviaReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveOctaviaReviews]
GO

	-- =============================================
-- Author      : Chetan Navin	
-- Create date : 13 Aug 2013
-- Description : To save winners reviews of skoda octavia
-- EXEC OLM_SaveOctaviaReviews 1,'nice',GETDATE()
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveOctaviaReviews] 
	@UserId     BIGINT, 
	@Review		VARCHAR(MAX),
	@CreatedOn  DATETIME,
	@Status     VARCHAR(1) OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SELECT * FROM OLM_UE_Users WHERE IsWinner = 1 AND Id = @UserId
	
	IF @@ROWCOUNT <> 0
	BEGIN
		SELECT UserId FROM OLM_UE_UserReviews  WHERE UserId = @UserId

		IF @@ROWCOUNT = 0 
		-- Insert in to table
			BEGIN 
				INSERT INTO OLM_UE_UserReviews (UserId,Review,CreatedOn) VALUES (@UserId,@Review,@CreatedOn)
				SET @Status = 1			
			END
			
		ELSE
		-- Update table
			BEGIN 
				UPDATE OLM_UE_UserReviews SET Review = @Review ,CreatedOn = @CreatedOn WHERE UserId = @UserId
				SET @Status = 1	
			END
	END
END

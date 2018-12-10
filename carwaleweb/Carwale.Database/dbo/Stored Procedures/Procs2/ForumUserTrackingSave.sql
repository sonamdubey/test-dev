IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ForumUserTrackingSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ForumUserTrackingSave]
GO

	CREATE PROCEDURE [dbo].[ForumUserTrackingSave]  
@SessionID VARCHAR(100),  
@UserID NUMERIC(18,0),  
@ActivityId NUMERIC(18,0),  
@CategoryId NUMERIC(18,0),
@ThreadId NUMERIC(18,0)
AS   
DECLARE @ForumUserId AS NUMERIC,
		@HandleName AS VARCHAR(200),
		@UpdateRows AS SMALLINT
BEGIN  
  
	SET @HandleName = ''
	SET @UpdateRows = 0
	
	--Check for the availability of the handle name. if handlename is not there then he is not a forum user
	IF @UserID <> -1
	BEGIN
		Select @HandleName = IsNull(HandleName, '') From UserProfile Where UserId = @UserID
		
		IF @HandleName <> ''
		BEGIN
			SET @ForumUserId = @UserID
		END
		ELSE
		BEGIN
			SET @ForumUserId = -1
		END
	END
	ELSE
	BEGIN
		SET @ForumUserId = -1
	END
 
   
   --First try to update the record assuming that the record is already there. If it is not there then it will return the
   --rowcount as 0. If rowcount is 0 then inserrt the record into the database
   
	UPDATE 
		ForumUserTracking  
	SET  
		UserID = @ForumUserId,  
		ActivityId = @ActivityId,  
		CategoryId = @CategoryId,  
		ThreadId = @ThreadId,  
		ActivityDateTime = GETDATE()  
	WHERE 
		SessionID = @SessionID
		
	SET @UpdateRows	= @@ROWCOUNT
	
	IF @UpdateRows = 0
	BEGIN
		INSERT INTO 
			ForumUserTracking
				(
					SessionID,	UserID,			ActivityId,		CategoryId,		ThreadId,	ActivityDateTime
				)  
			VALUES  
				(
					@SessionID, @ForumUserId,	@ActivityId,	@CategoryId,	@ThreadId, GETDATE()
				)  
	END
END
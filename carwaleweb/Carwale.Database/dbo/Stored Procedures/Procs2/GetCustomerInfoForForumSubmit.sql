IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerInfoForForumSubmit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerInfoForForumSubmit]
GO

	/*
Author:Rakesh Yadav
Date: 04/07/2013
Desc : Fetch customer information to create new thread and reply to thread
*/

CREATE PROCEDURE [dbo].[GetCustomerInfoForForumSubmit]
@cutomerId INT,
@IsModerator BIT = NULL OUT,
@IsWaitTimeExceeded BIT = NULL OUT,
@IsEmailVerified BIT = NULL OUT,
@IsModerationRequired BIT = NULL OUT,
@IsCustomerRestricted BIT = NULL OUT
AS
BEGIN
	   DECLARE @PostCount INT
	   DECLARE @LastPostTime INT
	   DECLARE @NoOfModeratedPosts INT

	   --To check whether customer is Moderator or not And if he is not moderator then if have waited for 5 min for another post	
       SELECT @PostCount=Count(Id),  @LastPostTime=DATEDIFF(MINUTE, MAX(MsgDateTime), GETDATE() )
       FROM ForumThreads WITH (NOLOCK)
       WHERE CustomerId = @cutomerId AND IsActive = 1        
       
       IF(@PostCount<50)
		SET @IsModerator=0
       ELSE
		SET @IsModerator=1

		--To check whether customer have waited for 5 min or not
       IF(@LastPostTime<5)
		SET @IsWaitTimeExceeded=0
       ELSE
		SET @IsWaitTimeExceeded=1
		
	    --To check whether customer is restricted or not	
       SELECT @IsCustomerRestricted = IsFake From Customers Where Id = @cutomerId
       
       IF (@IsCustomerRestricted = 0)
       BEGIN
		   IF EXISTS(SELECT CustomerId FROM Forum_BannedList WHERE CustomerId = @cutomerId)
			SET @IsCustomerRestricted = 1
       END

	   --To check whether customer have verified his email address	
       IF EXISTS(SELECT  Id from Customers WHERE Id = @cutomerId AND IsEmailVerified =1 AND IsVerified = 1 )
        SET @IsEmailVerified = 1
       ELSE
        SET @IsEmailVerified = 0

		--To check whether moderation is required for post	
       SELECT @NoOfModeratedPosts = Count(CustomerId)
       FROM ForumThreads WITH (NOLOCK)
       WHERE IsModerated = 1 AND CustomerId = @cutomerId
	   
       IF(@NoOfModeratedPosts>=5)
        SET @IsModerationRequired=0
       ELSE
       SET @IsModerationRequired=1 
END






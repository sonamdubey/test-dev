IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_InsertSubscription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_InsertSubscription]
GO

		
CREATE PROCEDURE [dbo].[Forum_InsertSubscription]

	@customerId		NUMERIC,
	@subId		NUMERIC,
	@alertType		INT

AS
	
	BEGIN
		if NOT EXISTS (select ForumThreadId from ForumSubscriptions
		where CustomerId=@CustomerId and ForumThreadId=@subId)
		BEGIN
			INSERT INTO ForumSubscriptions 
			(CustomerId,ForumThreadId,EmailSubscriptionId) 
			VALUES 
			(@customerId,@subId,@alertType)
		END
	END



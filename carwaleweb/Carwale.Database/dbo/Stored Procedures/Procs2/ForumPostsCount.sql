IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ForumPostsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ForumPostsCount]
GO

	-- =============================================
-- Author:		Ravi Koshal
-- Create date: 05/13/2013
-- Description:	This SP is used to retrieve the number of posts by a user.
-- =============================================
CREATE PROCEDURE [dbo].[ForumPostsCount]
	@CustomerId BIGINT,
	@PostCount INT OUTPUT,
	@LastPostTime BIGINT OUTPUT,
	@VerifyCustomer INT OUTPUT,
	@IsFake INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 	SELECT @PostCount=Count(Id),  @LastPostTime=DATEDIFF(MINUTE, MAX(MsgDateTime), GETDATE() )
	FROM ForumThreads WITH (NOLOCK) 
	WHERE CustomerId = @CustomerId AND IsActive = 1	
	SELECT @IsFake = IsFake From Customers Where Id = @CustomerId
	IF EXISTS(SELECT  Id from Customers WHERE Id = @CustomerId AND IsEmailVerified =1 AND IsVerified = 1 )
	BEGIN
		SET @VerifyCustomer = 1
	END
	ELSE
	BEGIN
		SET @VerifyCustomer = 0
	END 
END


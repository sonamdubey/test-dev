IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddYetiUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddYetiUser]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 16-July-2012
-- Description:	Add user record for yeti site campaign
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddYetiUser]
	-- Add the parameters for the stored procedure here
	@FirstName			VARCHAR(100),
	@LastName			VARCHAR(100),
	@Email				VARCHAR(100),
	@Age				INT,
	@Mobile				VARCHAR(15),
	@City				VARCHAR(15),
	@FacebookId			VARCHAR(50) = NULL,
	@TweetId			VARCHAR(50) = NULL,
	@BlogId				VARCHAR(50) = NULL,
	@DiningPlace		VARCHAR(100) = NULL,
	@DiningLocation		VARCHAR(100) = NULL,
	@FoodPlace			VARCHAR(100) = NULL,
	@FoodLocation		VARCHAR(100) = NULL,
	@ShoppingPlace		VARCHAR(100) = NULL,
	@ShoppingLocation	VARCHAR(100) = NULL,
	@LandmarkPlace		VARCHAR(100) = NULL,
	@LandmarkLocation	VARCHAR(100) = NULL,
	@RelaxPlace			VARCHAR(100) = NULL,
	@RelaxLocation		VARCHAR(100) = NULL,
	@Comments			VARCHAR(500) = NULL,
	@IpAddress			VARCHAR(20) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OLM_YetiUserData
		(
			FirstName, LastName, Email, Mobile, City, Age, DiningPlace, DiningLocation, FoodPlace,
			FoodLocation, ShoppingPlace, ShoppingLocation, LandmarkPlace, LandmarkLocation, RelaxPlace,
			RelaxLocation, Comments, FacebookId, TweetId, BlogId, IpAddress,EntryDate
		)
	VALUES
		(
			@FirstName, @LastName, @Email, @Mobile, @City, @Age, @DiningPlace, @DiningLocation, @FoodPlace,
			@FoodLocation, @ShoppingPlace, @ShoppingLocation, @LandmarkPlace, @LandmarkLocation, @RelaxPlace,
			@RelaxLocation, @Comments, @FacebookId, @TweetId, @BlogId, @IpAddress,GETDATE()
		)
	
END


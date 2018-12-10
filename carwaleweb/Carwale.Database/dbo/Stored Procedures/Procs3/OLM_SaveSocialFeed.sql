IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveSocialFeed]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveSocialFeed]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 8 Aug 2013
-- Description : To insert and updated social feed data in database.
-- Module      : Product CRM Masters
--Modifier     : Vinay kumar prajapati 
--Purpose      : Add and update ModelId in OLM_SocialFeed Table .
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveSocialFeed]
	@Id					BIGINT , 
	@NameOfUser			VARCHAR(50),
	@SocialHandle		VARCHAR (20),
	@SourceOfChannel	SMALLINT,
	@Post				VARCHAR(1500),
	@PostDate           DATETIME,
	@UpdatedOn          DATETIME,
	@UrlOfUser			VARCHAR(200),
	@UrlOfUserPic		VARCHAR(200),
	@ModelId            INT,
	@IsActive           BIT,
	@Type				INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Id = -1)
		BEGIN
			INSERT INTO OLM_SocialFeed(NameOfUser,SocialHandle,SourceOfChannel,Post,PostDate,UpdatedOn,UrlOfUser,UrlOfUserPic,IsActive,Type,ModelId) 
			VALUES (@NameOfUser,@SocialHandle,@SourceOfChannel,@Post,@PostDate,@UpdatedOn,@UrlOfUser,@UrlOfUserPic,@IsActive,@Type,@ModelId)
		    
		    RETURN 1
		END
		
	ELSE
		BEGIN
			UPDATE OLM_SocialFeed SET NameOfUser=@NameOfUser,SocialHandle=@SocialHandle,SourceOfChannel=@SourceOfChannel,Post=@Post,PostDate=@PostDate,
			       UpdatedOn=@UpdatedOn,UrlOfUser=@UrlOfUser,UrlOfUserPic=@UrlOfUserPic,IsActive=@IsActive,Type=@Type,ModelId=@ModelId
				    WHERE Id = @Id
			       
			RETURN 2      
		END
END
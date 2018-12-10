IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveSplashScreenSettings_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveSplashScreenSettings_v15]
GO

	
--CREATED ON 10-jun-15
--Created By Vikas J
--PROCEDURE FOR Splash Screen Insert And Update
CREATE PROCEDURE [dbo].[SaveSplashScreenSettings_v15.8.1] @Id INT
	,@CampaignName VARCHAR(50)
	,@HostUrl VARCHAR(200)
	,@FileName VARCHAR(200)
	,@DirPath VARCHAR(200)
	,@PlatformId INT
	,@StartDate VARCHAR(50)
	,@EndDate VARCHAR(50)
	,@IsActive BIT
	,@ModifiedBy INT
	,@ReturnId INT OUTPUT
AS
BEGIN
	DECLARE @TempHost varchar(50),@TempDirPath varchar(50),@TempImageName varchar(50)
	SELECT  @TempHost = HostUrl,@TempDirPath =DirPath, @TempImageName=ImageName  from AppSplashScreenSetting WITH(NOLOCK) where id=@Id

	IF NOT EXISTS (
			SELECT ID
			FROM AppSplashScreenSetting WITH(NOLOCK)
			WHERE Id = @Id
			)
	BEGIN
		INSERT INTO AppSplashScreenSetting (
			CampaignName
			,HostUrl
			,OriginalImgPath
			,PlatformId
			,StartDate
			,EndDate
			,IsActive
			,ModifiedBy
			,ModifiedOn
			)
		VALUES (
			@CampaignName
			,@HostUrl
			,'/' + @DirPath + '/' + @FileName
			,@PlatformId
			,@StartDate
			,@EndDate
			,1
			,@ModifiedBy
			,GETDATE()
			)
			SET @ReturnId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE AppSplashScreenSetting
		SET CampaignName = @CampaignName 
			,HostUrl = ISNULL(@HostUrl,HostUrl)
			,OriginalImgPath = '/' + ISNULL(@DirPath,DirPath) + '/' + ISNULL(@FileName,ImageName)
			,PlatformId = @PlatformId
			,StartDate = @StartDate
			,EndDate = @EndDate
			,IsActive = @IsActive
			,ModifiedBy = @ModifiedBy
			,ModifiedOn = GETDATE()
		WHERE Id = @Id
		SET @ReturnId = @Id
	END
END


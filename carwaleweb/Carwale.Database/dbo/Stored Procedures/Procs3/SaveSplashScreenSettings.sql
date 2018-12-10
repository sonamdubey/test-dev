IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveSplashScreenSettings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveSplashScreenSettings]
GO

	
--CREATED ON 10-jun-15
--Created By Vikas J
--PROCEDURE FOR Splash Screen Insert And Update
CREATE PROCEDURE [dbo].[SaveSplashScreenSettings] @Id INT
	,@CampaignName VARCHAR(50)
	,@HostUrl VARCHAR(200)
	,@FileName VARCHAR(200)
	,@DirPath VARCHAR(200)
	,@PlatformId INT
	,@StartDate VARCHAR(50)
	,@EndDate VARCHAR(50)
	,@IsActive BIT
	,@ModifiedBy INT
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
			,ImageName
			,DirPath
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
			,@FileName
			,@DirPath
			,@PlatformId
			,@StartDate
			,@EndDate
			,1
			,@ModifiedBy
			,GETDATE()
			)
	END
	ELSE
	BEGIN
		UPDATE AppSplashScreenSetting
		SET CampaignName = @CampaignName 
			,HostUrl = ISNULL(@HostUrl,@TempHost)
			,ImageName = ISNULL(@FileName,@TempImageName)
			,DirPath = ISNULL(@DirPath,@TempDirPath)
			,PlatformId = @PlatformId
			,StartDate = @StartDate
			,EndDate = @EndDate
			,IsActive = @IsActive
			,ModifiedBy = @ModifiedBy
			,ModifiedOn = GETDATE()
		WHERE Id = @Id
	END
END


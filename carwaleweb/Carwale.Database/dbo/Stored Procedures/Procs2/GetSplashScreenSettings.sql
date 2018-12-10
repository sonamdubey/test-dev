IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSplashScreenSettings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSplashScreenSettings]
GO

	
--CREATED ON 10-jun-15
--Created By Vikas J
--PROCEDURE FOR Gettting active Splash Screen 
CREATE PROCEDURE [dbo].[GetSplashScreenSettings] @PlatformId INT--[dbo].[GetSplashScreenSettings] -1,0
,@IsActive bit 
AS
BEGIN
	SELECT Id
			,CampaignName
			,'http://'+ HostUrl +'/'+ DirPath +'/'+ ImageName as Splashurl
			,PlatformId
			,CONVERT(varchar,StartDate,111) as StartDate
			,CONVERT(varchar, EndDate,111) as EndDate
			,IsActive
			,ModifiedBy
			,ModifiedOn
	FROM AppSplashScreenSetting WITH (NOLOCK)
	WHERE (   PlatformId = @PlatformId
		      AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, StartDate)
			  AND CONVERT(DATE, EndDate)
		      AND IsActive=1
		   )
		    OR @IsActive=0
	ORDER BY ModifiedOn DESC
END



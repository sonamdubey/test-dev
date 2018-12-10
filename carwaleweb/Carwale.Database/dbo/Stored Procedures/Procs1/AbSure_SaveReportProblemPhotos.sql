IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveReportProblemPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveReportProblemPhotos]
GO

	

-- =============================================
-- Author       :  Vinay Kumar Prajapati  14th   Aug  2015
-- Description  :  To save AbSure report problem (from absureApp) photo details
-- Modified By  :  Vinay kumar prajapati 18th sept 2015 (Reduce Parameters)
-- Moified By    :  Vinay Kumar Prajapati 27th  Oct 2015( Save Image on Amazon server(S3))
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveReportProblemPhotos]
    @ImageUrlOriginal          VARCHAR(250) = NULL,
    @ImageUrlLarge             VARCHAR(250) = null,
	@ImageUrlXL		           VARCHAR(250) = NULL,  
    @DirectoryPath             VARCHAR(200) = null,
	@ImageName                 VARCHAR(200) = null,
    @HostUrl                   VARCHAR(100) = null,   
	@SaveId                    INT          = null,
	@IsImageAvailable          BIT          = 0,
    @Absure_ReportProblemPhotoId       INT  OUTPUT
   
AS

DECLARE @UrlOriginal     VARCHAR(100)

BEGIN
    -- Check saveId is Available ---
    SELECT TOP 1 ARP.Absure_ReportProblemsId  FROM Absure_ReportProblems AS ARP WITH(NOLOCK) WHERE ARP.Absure_ReportProblemsId=@SaveId
	IF @@ROWCOUNT <> 0
		BEGIN

		 SET @UrlOriginal   =  @DirectoryPath + @ImageUrlOriginal
		 INSERT INTO Absure_ReportProblemPhotos(Absure_ReportProblemsId,HostUrl,DirectoryPath,ImageUrlLarge,ImageUrlExtraLarge,ImageUrlOriginal,StatusId)
				 VALUES(@SaveId,@HostUrl,'','','',@UrlOriginal,1)

				 ---Return the ID
				 SET @Absure_ReportProblemPhotoId = SCOPE_IDENTITY()
		END
	ELSE
	    BEGIN
			SET @Absure_ReportProblemPhotoId = -1
		END

END

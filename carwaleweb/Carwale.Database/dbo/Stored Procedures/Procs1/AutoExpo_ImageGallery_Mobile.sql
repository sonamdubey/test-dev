IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_ImageGallery_Mobile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_ImageGallery_Mobile]
GO

	-- ============================================= 
-- Author:     
-- Create date:  
-- Description:   
-- ============================================= 
CREATE PROCEDURE [dbo].[AutoExpo_ImageGallery_Mobile] --Execute dbo.AutoExpo_ImageGallery 
	-- Add the parameters for the stored procedure here 
	@MakeId NUMERIC = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from 
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;

	-- Insert statements for procedure here 
	DECLARE @vidCount INT

	--SET @vidCount = (
	--		SELECT Count(*) AS VidCount
	--		FROM con_editcms_videos CV
	--		INNER JOIN con_editcms_cars CC ON CC.basicid = CV.basicid
	--		WHERE CC.makeid = @MakeId
	--			OR @MakeId IS NULL
	--		)

	--IF @vidCount > 2
	--BEGIN
	--	SET @vidCount = 2
	--END

	SELECT TOP (2) CV.id
		,videourl
	FROM con_editcms_videos CV
	INNER JOIN con_editcms_cars CC ON CC.basicid = CV.basicid
		AND (
			CC.makeid = @MakeId
			OR @MakeId IS NULL
			)
	INNER JOIN con_editcms_basic CB ON CB.id = CV.basicid
		AND CB.categoryid = 15
	ORDER BY CB.LastUpdatedTime DESC

	SELECT TOP (6) CI.id
		,CI.caption
		,CI.imagename
		,CI.hosturl
		,CI.imagepathcustom
		,CI.imagepathlarge
		,CI.imagepathoriginal
		,CI.imagepaththumbnail
	FROM con_editcms_images CI
	INNER JOIN con_editcms_cars cc ON CI.basicid = CC.basicid
		AND (
			CC.makeid = @MakeId
			OR @MakeId IS NULL
			)
	INNER JOIN con_editcms_basic CB ON CB.id = CI.basicid
		AND CB.categoryid = 15
	ORDER BY CI.lastupdatedtime DESC
END

